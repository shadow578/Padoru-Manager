using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PadoruManager.Model;
using PadoruManager.Utils;

namespace PadoruManager.UI
{
    public partial class CollectionManager : Form
    {
        /// <summary>
        /// The file that is used to track the last opened collection file path
        /// </summary>
        const string LAST_COLLECTION_PATH_FILE = "./lastcollection.path";

        /// <summary>
        /// The currently loaded collection
        /// </summary>
        PadoruCollection currentCollection;

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
                    || (!string.IsNullOrWhiteSpace(entry.MALId) && entry.MALId.ContainsIgnoreCase(query))
                    || (!string.IsNullOrWhiteSpace(entry.ImageCreator) && entry.ImageCreator.ContainsIgnoreCase(query)))
                {
                    //something in this entry matches the query, add it to the search results
                    searchResults.Add(entry);
                }
            }

            return searchResults;
        }

        /// <summary>
        /// Show the given entrys on the Selection Panel
        /// also register click events
        /// </summary>
        /// <param name="entries">the entrys to show</param>
        void ShowOnSelectionPanel(List<PadoruEntry> entries)
        {
            //reset current selection panel
            entrySelectionPanel.Controls.Clear();

            //enumerate all entries
            PadoruPreview preview;
            foreach (PadoruEntry entry in entries)
            {
                //load image from entry (local path ONLY)
                if (string.IsNullOrWhiteSpace(entry.ImagePath) || !File.Exists(entry.ImagePath)) return;
                Image entryImg = Image.FromFile(entry.ImagePath);

                //create padoru preview control
                preview = new PadoruPreview()
                {
                    DisplayName = entry.Name,
                    PreviewImage = entryImg
                };

                //add preview to selection panel
                entrySelectionPanel.Controls.Add(preview);
            }
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
        }

        #region UI Events
        public override void Refresh()
        {
            //do normal refresh
            base.Refresh();

            //refresh state of buttons
            bool hasCollection = currentCollection != null;
            txtCollectionSearch.Enabled = hasCollection;
            btnAddNew.Enabled = hasCollection;
            btnSaveCollection.Enabled = hasCollection;

            //update search results
            UpdateSearch();
        }

        void OnLoad(object sender, EventArgs e)
        {
            //Try to open the last collection
            string lastCollection = LoadLastCollectionFilePath();
            if (!string.IsNullOrWhiteSpace(lastCollection))
            {
                LoadCollection(lastCollection);
            }

            //update ui
            Refresh();
        }

        void OnOpenCollectionClick(object sender, EventArgs e)
        {
            //select file with dialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Padoru Collection Definitions|*.json";
            ofd.CheckFileExists = false;
            ofd.Multiselect = false;

            //show dialog
            if (ofd.ShowDialog() != DialogResult.OK) return;

            //open collection
            LoadCollection(ofd.FileName);
            SaveLastCollectionFilePath(ofd.FileName);

            //update ui
            Refresh();
        }

        void OnSaveCollectionClick(object sender, EventArgs e)
        {
            //check collection exists
            if (currentCollection == null)
            {
                MessageBox.Show(this, "No Collection is currently active!", "No Collection", MessageBoxButtons.OK);
                return;
            }

            //save collection
            currentCollection.ToFile();
        }

        void OnAddNewClick(object sender, EventArgs e)
        {
            //check collection exists
            if (currentCollection == null)
            {
                MessageBox.Show(this, "No Collection is currently active!", "No Collection", MessageBoxButtons.OK);
                return;
            }

            //create Padoru Editor and let user create new entry
            PadoruEditor editor = new PadoruEditor();
            if (editor.ShowDialog() != DialogResult.OK) return;

            //Get new entry
            PadoruEntry newEntry = editor.CurrentStateEntry;

            //add new entry to list of entrys
            if (newEntry != null)
            {
                currentCollection.Entries.Add(newEntry);
            }


            //update ui
            Refresh();
        }

        void OnSearchTextChange(object sender, EventArgs e)
        {
            Refresh();
        }
        #endregion
    }
}
