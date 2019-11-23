using PadoruLib.Padoru.Model;
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
        /// <summary>
        /// The file that is used to track the last opened collection file path
        /// </summary>
        const string LAST_COLLECTION_PATH_FILE = "./lastcollection.path";

        /// <summary>
        /// The file that is used to track the last size of the manager ui
        /// </summary>
        const string LAST_UI_SIZE_FILE = "./lastui.size";

        /// <summary>
        /// the name of the save script (inside the collection path)
        /// </summary>
        const string SAVE_SCRIPT_NAME = "onsave.bat";

        /// <summary>
        /// The currently loaded collection
        /// </summary>
        PadoruCollection currentCollection;

        /// <summary>
        /// The currently selected entry
        /// </summary>
        Guid currentlySelectedEntryId = Guid.Empty;

        /// <summary>
        /// initialize the ui
        /// </summary>
        public CollectionManager()
        {
            InitializeComponent();
        }

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
            //check file exists
            if (!File.Exists(LAST_UI_SIZE_FILE)) return;

            //get size as string from file
            string sizeStr = File.ReadAllText(LAST_UI_SIZE_FILE);

            //check size string is ok
            if (string.IsNullOrWhiteSpace(sizeStr)) return;

            //parse size
            Size size = Utils.StringToSize(sizeStr);

            //check size is ok to use
            if (size.IsEmpty || size.Width < MinimumSize.Width || size.Height < MinimumSize.Height) return;

            //set size
            Size = size;

        }

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
        }

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

        /// <summary>
        /// Run a script when saving (e.g. a git commit script)
        /// </summary>
        void RunSaveScript()
        {
            //check if script should be run
            if (!chkEnableSaveScript.Checked) return;

            //Get path of collection
            string collectionDir = Path.GetDirectoryName(currentCollection.LoadedFrom);

            //create dir if needed
            if (!Directory.Exists(collectionDir)) Directory.CreateDirectory(collectionDir);

            //build script path
            string scriptPath = Path.Combine(collectionDir, SAVE_SCRIPT_NAME);

            //create dummy save script if no script exists
            if (!File.Exists(scriptPath))
            {
                File.WriteAllText(scriptPath, @"REM Example PadoruManager SaveScript.
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
                MessageBox.Show(this, "Error running SaveScript!", "SaveScript Error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// automatically save the collection
        /// </summary>
        /// <param name="autoSave">is this a call from a autosave routine?</param>
        void SaveCollection(bool autoSave = false)
        {
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

            //run the save script 
            RunSaveScript();

            //show confirmation if not autosaving
            if (!autoSave)
                MessageBox.Show(this, "Saved Changes!", "Saved Changes", MessageBoxButtons.OK);
        }

        #region UI Events
        public override void Refresh()
        {
            //refresh state of buttons
            bool hasCollection = currentCollection != null;
            txtCollectionSearch.Enabled = hasCollection;
            btnAddNew.Enabled = hasCollection;
            btnSaveCollection.Enabled = hasCollection;

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
            await PopulateSelectionPanel(currentCollection.Entries);

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

            //open collection
            LoadCollection(ofd.FileName);
            SaveLastCollectionFilePath(ofd.FileName);

            //update ui
            currentlySelectedEntryId = Guid.Empty;
            Refresh();
        }

        void OnSaveCollectionClick(object sender, EventArgs e)
        {
            SaveCollection(false);
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
                EditingParentCollection = currentCollection
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
                CurrentStateEntry = toEdit
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

        void CollectionManager_ResizeEnd(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(LAST_UI_SIZE_FILE, Size.SizeToString());
            }
            catch (Exception)
            {
                //optional function, ignore errors
            }
        }

        void OnClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show(this, "If you close, all unchanged changes will be lost!\nDo you want to save now?", "Save now?", MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Yes)
            {
                SaveCollection(false);
            }
            if (res == DialogResult.Cancel) e.Cancel = true;
        }
        #endregion
    }
}
