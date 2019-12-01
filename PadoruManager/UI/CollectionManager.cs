using PadoruLib.Padoru.Model;
using PadoruManager.GitToC;
using PadoruManager.Model;
using PadoruManager.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PadoruManager.UI
{
    public partial class CollectionManager : Form
    {
        #region Constants
        /// <summary>
        /// The file that is used to track the last opened collection file path
        /// </summary>
        const string LAST_COLLECTION_PATH_FILE = "./lastcollection.path";

        /// <summary>
        /// the name of the save script (inside the collection path)
        /// </summary>
        const string PUSH_SCRIPT_NAME = "push-repo.bat";

        /// <summary>
        /// The name of the collection manager config file (inside the collection root path)
        /// </summary>
        const string COLLECTION_MANAGER_CONFIG_NAME = "manager.config";
        #endregion

        #region Working Variables
        /// <summary>
        /// The currently loaded collection
        /// </summary>
        PadoruCollection currentCollection;

        /// <summary>
        /// The manager configuration of the currently loaded collection
        /// </summary>
        CollectionManagerConfig currentManagerConfig;

        /// <summary>
        /// The currently selected entry
        /// </summary>
        Guid currentlySelectedEntryId = Guid.Empty;

        /// <summary>
        /// Were any changes made in the current collection that were not yet saved?
        /// </summary>
        bool hasUnsavedChanges = false;
        #endregion

        /// <summary>
        /// initialize the ui
        /// </summary>
        public CollectionManager()
        {
            InitializeComponent();
        }

        #region Collection Load & Save
        /// <summary>
        /// Load the collection represented by the given file
        /// also sets collection variable.
        /// </summary>
        /// <param name="path">the path of the collection to open</param>
        void LoadCollection(string path)
        {
            //check that path is valid
            if (string.IsNullOrWhiteSpace(path)) return;

            //check if collection file already exists
            if (File.Exists(path))
            {
                currentCollection = PadoruCollection.FromFile(path);
            }
            else
            {
                //collection file does not exist, create it?
                if (MessageBox.Show(this, $"The Collection at \"{path}\" does not exist!\nDo you want to create a new Collection?", "Create Collection", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                //create empty collection
                currentCollection = PadoruCollection.CreateEmpty();
                currentCollection.LoadedFrom = path;
            }

            //check if collection config file already exists
            string collectionConfig = Path.Combine(Path.GetDirectoryName(path), COLLECTION_MANAGER_CONFIG_NAME);
            if (File.Exists(collectionConfig))
            {
                //exists, load it
                currentManagerConfig = CollectionManagerConfig.LoadFrom(collectionConfig);
            }
            else
            {
                //does not exists, open setup wizard
                ManagerConfigSetupWizard wizard = new ManagerConfigSetupWizard();

                //have to enter a config to continue
                while (wizard.ShowDialog() != DialogResult.OK) ;

                //get configuration from wizard
                currentManagerConfig = wizard.ResultingConfig;

                //save config
                currentManagerConfig.LoadedFrom = collectionConfig;
                currentManagerConfig.SaveTo();
            }

            //set window title
            Text = $"Collection Manager - {Path.GetFileName(currentCollection.LoadedFrom)}";
        }

        /// <summary>
        /// automatically save the collection
        /// </summary>
        /// <param name="autoSave">is this a call from a autosave routine?</param>
        void SaveCollection(bool autoSave = false)
        {
            //set unsaved changes flag
            hasUnsavedChanges = true;

            //check if autosave is active
            if (autoSave && !chkAutoSave.Checked) return;

            //check collection exists
            if (currentCollection == null)
            {
                MessageBox.Show(this, "No Collection is currently active! Please Create a new one before saving.", "No Collection", MessageBoxButtons.OK);
                return;
            }

            //save collection formatted
            currentCollection.ToFile();

            //save collection minified
            string miniName = Path.GetFileNameWithoutExtension(currentCollection.LoadedFrom) + "-mini" + Path.GetExtension(currentCollection.LoadedFrom);
            string miniPath = Path.Combine(Path.GetDirectoryName(currentCollection.LoadedFrom), miniName);
            currentCollection.ToFile(miniPath, true);
            hasUnsavedChanges = false;

            //do some more if not autosaving
            if (!autoSave)
            {
                //ask user if the toc should be regenerated
                if (MessageBox.Show(this, "Collection Saved!\nDo you want to regenerate the Table of Contents to match the new data?", "Save Success", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //create toc
                    OnCreateTocClick(this, new EventArgs());
                }
            }
        }
        #endregion

        #region Selection Panel & Search
        /// <summary>
        /// Create new padoru previews for each entry and add them to the Selection Panel
        /// also register click events
        /// </summary>
        /// <param name="entries">the entrys to populate the panel with</param>
        async Task PopulateSelectionPanel(List<PadoruEntry> entries)
        {
            SuspendLayout();

            //clear old panel
            entrySelectionPanel.Controls.Clear();

            //enumerate all entries
            List<Task> imgLoadTasks = new List<Task>();
            Image imgNoPadoru = Properties.Resources.no_padoru;
            int tabIndex = 0;
            foreach (PadoruEntry entry in entries)
            {
                //get display name
                string entryName = "<NAME>";
                if (!string.IsNullOrWhiteSpace(entry.Name))
                {
                    entryName = entry.Name;
                }

                //create padoru preview control
                PadoruPreview preview = new PadoruPreview()
                {
                    DisplayName = entryName,
                    PreviewImage = imgNoPadoru,
                    EntryGuid = entry.UID,
                    TabIndex = tabIndex++
                };
                preview.MouseClick += OnAnyPreviewClick;

                //add preview to selection panel
                entrySelectionPanel.Controls.Add(preview);

                //load the image for this preview async
                imgLoadTasks.Add(GetImageFromEntry(entry).ContinueWith((t) =>
                {
                    //get result from load task
                    Image entryImg = t.Result;

                    //abort if no image loaded or there is no preview to set the image of
                    if (entryImg == null || preview == null) return;

                    //scale down loaded image to required resolution
                    using (entryImg)
                    {
                        //scale down image
                        Image entryImgScaled = entryImg.Resize(preview.PreviewImageBoxSize);

                        //set image if loaded correctly
                        preview.PreviewImage = entryImgScaled;
                    }
                }));
            }
            ResumeLayout();

            //wait for all images to finish loading
            await Task.WhenAll(imgLoadTasks);
        }

        /// <summary>
        /// Get the image for the given entry
        /// </summary>
        /// <param name="entry">the entry to get the image for</param>
        /// <returns>the loaded image</returns>
        async Task<Image> GetImageFromEntry(PadoruEntry entry)
        {
            return await Task.Run(() =>
            {
                //get image data for preview
                byte[] entryImgData = entry.GetImageDataLocal();

                //load image from data, use fallback when load failed
                if (entryImgData != null)
                {
                    using (MemoryStream entryImgDataStream = new MemoryStream(entryImgData))
                    {
                        return Image.FromStream(entryImgDataStream);
                    }
                }

                //fallback to null
                return null;
            });
        }

        /// <summary>
        /// Search the current collection for entrys matching the query string
        /// </summary>
        /// <param name="query">the query to search for</param>
        /// <returns></returns>
        List<PadoruEntry> SearchCurrentCollection(string query)
        {
            //skip search if query is empty
            if (string.IsNullOrWhiteSpace(query))
            {
                return currentCollection.Entries;
            }

            //search all entrys in the current collection
            List<PadoruEntry> searchResults = new List<PadoruEntry>();
            foreach (PadoruEntry entry in currentCollection.Entries)
            {
                if ((!string.IsNullOrWhiteSpace(entry.Name) && entry.Name.ContainsIgnoreCase(query))
                    || (!string.IsNullOrWhiteSpace(entry.MALName) && entry.MALName.ContainsIgnoreCase(query))
                    || (entry.MALId.HasValue && entry.MALId.ToString().ContainsIgnoreCase(query))
                    || (!string.IsNullOrWhiteSpace(entry.ImageContributor) && entry.ImageContributor.ContainsIgnoreCase(query))
                    || (!string.IsNullOrWhiteSpace(entry.ImageCreator) && entry.ImageCreator.ContainsIgnoreCase(query)))
                {
                    //something in this entry matches the query, add it to the search results
                    searchResults.Add(entry);
                }
            }

            return searchResults;
        }

        /// <summary>
        /// Make the given padoru previews visible on the selection panel
        /// All other entries on the panel will be hidden
        /// </summary>
        /// <param name="entries">the entrys to show on the panel</param>
        void ShowOnSelectionPanel(List<PadoruEntry> entries)
        {
            SuspendLayout();
            foreach (Control c in entrySelectionPanel.Controls)
            {
                //skip all non previews
                if (!(c is PadoruPreview preview)) continue;

                //check if preview id is in entrys to show
                bool show = entries.HasAnyWhere((e) =>
                {
                    //entry has no uid - exclude
                    if (e.UID == null) return false;

                    //bad search id - include
                    if (preview.EntryGuid == null) return true;

                    //check id
                    return e.UID.Equals(preview.EntryGuid);
                });

                //set visibility
                preview.Visible = show;
            }
            ResumeLayout();
        }

        /// <summary>
        /// Update the search results display
        /// </summary>
        void UpdateSearch()
        {
            if (currentCollection == null) return;

            //search the current collection
            List<PadoruEntry> results = SearchCurrentCollection(txtCollectionSearch.Text);

            //display results
            ShowOnSelectionPanel(results);

            //update lable at bottom
            lbEntrysCount.Text = $"Showing {results.Count} of {currentCollection.Entries.Count} entrys";
        }

        /// <summary>
        /// update the selected item
        /// </summary>
        void UpdateSelection()
        {
            SuspendLayout();
            foreach (Control ctrl in entrySelectionPanel.Controls)
            {
                //skip all that are not previews
                if (!(ctrl is PadoruPreview preview)) continue;

                //set thick border if is selected item
                if (preview.EntryGuid == null)
                {
                    preview.ThickBorders = false;
                    continue;
                }

                preview.ThickBorders = preview.EntryGuid.Equals(currentlySelectedEntryId);
            }
            ResumeLayout();
        }

        /// <summary>
        /// Get the currently selected entry
        /// </summary>
        /// <returns>the selected entry, or null if nothing is selected</returns>
        PadoruEntry GetSelection()
        {
            //check that selection and collection are ok
            if (currentCollection == null || currentlySelectedEntryId.Equals(Guid.Empty)) return null;

            //remove current selection from collection
            PadoruEntry selectedEntry = null;
            foreach (PadoruEntry entry in currentCollection.Entries)
            {
                if (entry.UID.Equals(currentlySelectedEntryId))
                {
                    selectedEntry = entry;
                    break;
                }
            }

            return selectedEntry;
        }
        #endregion

        #region Utility
        /// <summary>
        /// load the collection file path that was opened the last time
        /// </summary>
        /// <returns>the path to the last opened collection file, or string.empty</returns>
        string LoadLastCollectionFilePath()
        {
            //check that path save file exists
            if (!File.Exists(LAST_COLLECTION_PATH_FILE)) return string.Empty;

            //load path from path save file
            using (StreamReader reader = File.OpenText(LAST_COLLECTION_PATH_FILE))
            {
                return reader.ReadLine();
            }
        }

        /// <summary>
        /// Save the path as the last path that was opened
        /// </summary>
        /// <param name="path">the path to save</param>
        void SaveLastCollectionFilePath(string path)
        {
            //file has to exist
            if (!File.Exists(path)) return;

            //write path to file
            File.WriteAllText(LAST_COLLECTION_PATH_FILE, path);
        }

        /// <summary>
        /// Restore the size the ui had the last time it was opened
        /// </summary>
        void RestoreLastUISize()
        {
            //get size from config
            Size size = Size.Empty;
            if (currentManagerConfig != null)
            {
                size = currentManagerConfig.LastUiSize;
            }

            //check size is ok to use
            if (size.IsEmpty || size.Width < MinimumSize.Width || size.Height < MinimumSize.Height) return;

            //set size
            Size = size;

        }

        /// <summary>
        /// Run a script that pushes the current collection state to a server
        /// </summary>
        void RunPushScript()
        {
            //Get path of collection
            string collectionDir = Path.GetDirectoryName(currentCollection.LoadedFrom);

            //create dir if needed
            if (!Directory.Exists(collectionDir)) Directory.CreateDirectory(collectionDir);

            //build script path
            string scriptPath = Path.Combine(collectionDir, PUSH_SCRIPT_NAME);

            //create dummy push script if no script exists
            if (!File.Exists(scriptPath))
            {
                File.WriteAllText(scriptPath, @"REM Example PadoruManager Push Script.
REM use %1 to get the directory the collection is in
REM like this
echo Collection dir: %1");
            }

            //run the save script
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(scriptPath, $"\"{collectionDir}\"");
                using (Process scriptProc = Process.Start(psi))
                {
                    scriptProc.WaitForExit(30000);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Error running Push Script!", "Push Script Error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Create a Table of contents for the current collection
        /// </summary>
        /// <returns>was a table of contents created?</returns>
        async Task<bool> CreateToC()
        {
            //abort if not having a current collection
            if (currentCollection == null || !currentCollection.LoadedLocal || currentManagerConfig == null) return false;

            //get collection root
            string collectionRoot = Path.GetDirectoryName(currentCollection.LoadedFrom);
            if (string.IsNullOrWhiteSpace(collectionRoot)) return false;

            //get repo root url
            string repoRoot = currentManagerConfig.GetTableOfContentsRepoRoot();

            //get local toc root path
            string tocRoot = currentManagerConfig.GetTableOfContentsLocalRoot(collectionRoot);

            //check if both root paths are there
            if (string.IsNullOrWhiteSpace(repoRoot) || string.IsNullOrWhiteSpace(tocRoot)) return false;

            //ask user if he wants to proceed
            if (MessageBox.Show(this, $"This action will recreate the Table of Contents and delete any files in the directory {tocRoot}. Continue?", "Continue create ToC?", MessageBoxButtons.YesNo) != DialogResult.Yes) return false;

            //delete toc directory if currently exists
            if (Directory.Exists(tocRoot)) Directory.Delete(tocRoot, true);

            //enable wait cursor
            UseWaitCursor = true;

            //create instance of toc creator
            TableOfContentCreator tocCreator = new TableOfContentCreator(currentManagerConfig);

            //create table of contents from current collection
            await tocCreator.CreateTableOfContents(currentCollection);

            //disable wait cursor
            UseWaitCursor = false;

            //all ok
            return true;
        }
        #endregion

        #region UI Events
        public override void Refresh()
        {
            //refresh state of buttons
            bool hasCollection = currentCollection != null;
            txtCollectionSearch.Enabled = hasCollection;
            btnAddNew.Enabled = hasCollection;
            btnSaveCollection.Enabled = hasCollection;
            btnCreateToc.Enabled = hasCollection;
            btnRunPushScript.Enabled = hasCollection;
            btnReloadCollection.Enabled = hasCollection;
            chkAutoSave.Enabled = hasCollection;

            bool hasSelection = !currentlySelectedEntryId.Equals(Guid.Empty);
            btnEditCurrent.Enabled = hasSelection;
            btnRemoveCurrent.Enabled = hasSelection;

            //update search results
            UpdateSearch();

            //update selected preview item
            UpdateSelection();

            //do normal refresh
            base.Refresh();
        }

        async void OnLoad(object sender, EventArgs e)
        {
            //show loading cursor while loading
            UseWaitCursor = true;

            //Try to open the last collection
            string lastCollection = LoadLastCollectionFilePath();
            if (!string.IsNullOrWhiteSpace(lastCollection))
            {
                LoadCollection(lastCollection);
                currentlySelectedEntryId = Guid.Empty;
            }

            //initial populate selection panel
            if (currentCollection != null)
            {
                await PopulateSelectionPanel(currentCollection.Entries);
            }

            //try to restore last window size
            try
            {
                RestoreLastUISize();
            }
            catch (Exception)
            {
                //is ok if fails
            }

            //update ui
            Refresh();

            //disable load cursor after loading
            UseWaitCursor = false;
        }

        void OnOpenCollectionClick(object sender, EventArgs e)
        {
            //select file with dialog
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Padoru Collection Definitions|*.json",
                CheckFileExists = false,
                Multiselect = false
            };

            //show dialog
            if (ofd.ShowDialog() != DialogResult.OK) return;

            //save selected path as last collection path and re- run onLoad to initialize the ui
            SaveLastCollectionFilePath(ofd.FileName);
            OnLoad(sender, e);
        }

        void OnSaveCollectionClick(object sender, EventArgs e)
        {
            //show warning if nothing was changed
            if (!hasUnsavedChanges)
            {
                if (MessageBox.Show(this, "There are no changes! Save anyways?", "No Changes To Save", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    //user say no
                    return;
                }
            }

            //save collection
            SaveCollection(false);
        }

        void OnReloadCollectionClick(object sender, EventArgs e)
        {
            OnLoad(sender, e);
        }

        void OnRunPushScriptClick(object sender, EventArgs e)
        {
            //notify user if there are unsaved changes before pushing
            if (hasUnsavedChanges)
            {
                //ask user to save before pushing
                if (MessageBox.Show(this, "There are unsaved changes! Do you want to save before pushing?", "Save Before Push", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //user say yes, save first
                    SaveCollection(false);
                }
            }

            //push collection to server
            RunPushScript();

            //give user info that push ran
            MessageBox.Show(this, "Push Script was executed!", "Push Script", MessageBoxButtons.OK);
        }

        void OnSearchTextChange(object sender, EventArgs e)
        {
            currentlySelectedEntryId = Guid.Empty;
            Refresh();
        }

        void OnAnyPreviewClick(object sender, MouseEventArgs e)
        {
            if (!(sender is PadoruPreview preview)) return;

            //set selected id
            currentlySelectedEntryId = preview.EntryGuid;
            Refresh();

            //if is rightclick, show context menu
            if (e.Button == MouseButtons.Right)
            {
                //get click position
                Point clickPos = MousePosition;

                //open context menu at click position
                padoruPreviewContextMenu.Show(clickPos);
            }
        }

        async void OnAddNewClick(object sender, EventArgs e)
        {
            //check collection exists
            if (currentCollection == null)
            {
                MessageBox.Show(this, "No Collection is currently active!", "No Collection", MessageBoxButtons.OK);
                return;
            }

            //create Padoru Editor and let user create new entry
            PadoruEditor editor = new PadoruEditor
            {
                EditingParentCollection = currentCollection,
                CurrentManagerConfig = currentManagerConfig
            };
            if (editor.ShowDialog() != DialogResult.OK) return;

            //Get new entry
            PadoruEntry newEntry = editor.CurrentStateEntry;

            //add new entry to list of entrys
            if (newEntry != null)
            {
                currentCollection.Entries.Add(newEntry);
                currentlySelectedEntryId = newEntry.UID;
                SaveCollection(true);
            }

            //re- populate selection panel
            await PopulateSelectionPanel(currentCollection.Entries);

            //update ui
            Refresh();
        }

        async void OnEditCurrentClick(object sender, EventArgs e)
        {
            //get current selection
            PadoruEntry toEdit = GetSelection();
            if (toEdit == null) return;

            //show padoru editor
            PadoruEditor editor = new PadoruEditor
            {
                CurrentStateEntry = toEdit,
                CurrentManagerConfig = currentManagerConfig
            };
            if (editor.ShowDialog() == DialogResult.Cancel) return;

            //get edited entry
            PadoruEntry editedEntry = editor.CurrentStateEntry;
            if (editedEntry == null) return;

            //write back saved entry
            int index = currentCollection.Entries.IndexOf(toEdit);
            currentCollection.Entries.Remove(toEdit);
            currentCollection.Entries.Insert(index, editedEntry);
            SaveCollection(true);

            //re- populate selection panel
            await PopulateSelectionPanel(currentCollection.Entries);

            //update ui
            Refresh();
        }

        async void OnRemoveCurrentClick(object sender, EventArgs e)
        {
            //get current selection
            PadoruEntry toRemove = GetSelection();
            if (toRemove == null) return;

            //ask if wants to remove element
            if (MessageBox.Show(this, $"Do you want to remove \"{toRemove.ToString()}\"?", "Remove Element", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            //remove element
            currentCollection.Entries.Remove(toRemove);
            SaveCollection(true);

            //re- populate selection panel
            await PopulateSelectionPanel(currentCollection.Entries);

            //update ui
            currentlySelectedEntryId = Guid.Empty;
            Refresh();
        }

        async void OnCreateTocClick(object sender, EventArgs e)
        {
            if (await CreateToC())
            {
                //show message box that toc was created
                MessageBox.Show(this, "Table of Contents was created!", "TOC created", MessageBoxButtons.OK);
            }
            else
            {
                //show message box that toc was not created
                MessageBox.Show(this, "A Table of Contents was not created! Is a collection loaded?", "TOC not created", MessageBoxButtons.OK);
            }
        }

        void OnUiResizeEnd(object sender, EventArgs e)
        {
            if (currentManagerConfig != null)
            {
                currentManagerConfig.LastUiSize = Size;
                currentManagerConfig.SaveTo();
            }
        }

        void OnClosing(object sender, FormClosingEventArgs e)
        {
            //close if no changes are to save
            if (!hasUnsavedChanges) return;

            DialogResult res = MessageBox.Show(this, "If you close, all unsaved changes will be lost!\nDo you want to save now?", "Save now?", MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Yes)
            {
                SaveCollection(false);
            }
            if (res == DialogResult.Cancel) e.Cancel = true;
        }
        #endregion
    }
}
