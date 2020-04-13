using JikanDotNet;
using PadoruLib.Padoru.Model;
using PadoruManager.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibUtil = PadoruLib.Utility.Util;

namespace PadoruManager.UI
{
    public partial class PadoruEditor : Form
    {
        /// <summary>
        /// Get/set the padoru entry representing the current ui state
        /// </summary>
        public PadoruEntry CurrentStateEntry
        {
            get
            {
                return UiToEntry();
            }
            set
            {
                UiFromEntry(value);
            }
        }

        /// <summary>
        /// The parent collection the current entry is part of
        /// </summary>
        public PadoruCollection EditingParentCollection { get; set; }

        /// <summary>
        /// The current collection manager configuration. If not set, image url prediction will be unavailable.
        /// </summary>
        public CollectionManagerConfig CurrentManagerConfig { get; set; }

        /// <summary>
        /// The unique id of the entry beign edited
        /// </summary>
        Guid editingId;

        /// <summary>
        /// Shared jikan instance for mal api requests
        /// </summary>
        readonly Jikan jikan;

        /// <summary>
        /// was the character name field changed?
        /// </summary>
        bool malQueryChanged;

        /// <summary>
        /// Was any field changed?
        /// </summary>
        bool anyFieldChanged = false;

        /// <summary>
        /// Temp variable for when a entry is loaded, so that the combobox for mal selection can be updated correctly 
        /// </summary>
        long loadedEntryMalId = -1;

        /// <summary>
        /// initialize ui with default values
        /// </summary>
        public PadoruEditor()
        {
            //init ui
            InitializeComponent();

            //create jikan instance
            jikan = new Jikan(true);
        }

        /// <summary>
        /// initialize the editor with the values from a existing entry
        /// </summary>
        /// <param name="entry">the entry whose values will be used</param>
        public PadoruEditor(PadoruEntry entry) : this()
        {
            CurrentStateEntry = entry;
        }

        /// <summary>
        /// Set the ui state according to the padoru entry
        /// </summary>
        /// <param name="entry">the padoru entry</param>
        void UiFromEntry(PadoruEntry entry)
        {
            txtImageUrl.Text = entry.ImageUrl;
            txtImagePath.Text = entry.ImageAbsolutePath;
            txtCharacterName.Text = entry.Name;
            chkCharacterFemale.Checked = entry.IsFemale;
            txtImageContributor.Text = entry.ImageContributor;
            txtImageCreator.Text = entry.ImageCreator;
            txtImageSource.Text = entry.ImageSource;

            txtSelectedMalName.Text = entry.MALName;
            txtSelectedMalId.Text = entry.MALId.HasValue ? entry.MALId.ToString() : "";

            //parse entry mal id
            if (entry.MALId.HasValue)
            {
                loadedEntryMalId = entry.MALId.Value;
            }

            //save id
            editingId = entry.UID;

            //save parent collection
            EditingParentCollection = entry.ParentCollection;
        }

        /// <summary>
        /// Create a PadoruEntry from the current ui state
        /// </summary>
        /// <returns>the padoru entry</returns>
        PadoruEntry UiToEntry()
        {
            //create entry instance
            PadoruEntry entry = new PadoruEntry()
            {
                ImageUrl = txtImageUrl.Text,
                Name = txtCharacterName.Text,
                IsFemale = chkCharacterFemale.Checked,
                MALName = txtSelectedMalName.Text,
                MALId = null,
                ImageContributor = txtImageContributor.Text,
                ImageCreator = txtImageCreator.Text,
                ImageSource = txtImageSource.Text
            };

            //parse and set mal id
            if (long.TryParse(txtSelectedMalId.Text, out long malId))
            {
                entry.MALId = malId;
            }

            //set image path and parent collection
            if (EditingParentCollection != null)
            {
                entry.ParentCollection = EditingParentCollection;
                entry.SetImagePath(txtImagePath.Text);
            }

            if (!editingId.Equals(Guid.Empty)) entry.UID = editingId;
            return entry;
        }

        /// <summary>
        /// Create and populate the auto complete lists
        /// </summary>
        /// <param name="collection">the collection to create the auto complete entries from</param>
        void PopulateAutoComplete(PadoruCollection collection)
        {
            //auto complete for contributor name
            BuildAutoCompleteCollection(txtImageContributor, collection.Entries, (p) => p.ImageContributor);

            //auto complete for creator name
            BuildAutoCompleteCollection(txtImageCreator, collection.Entries, (p) => p.ImageCreator);
        }

        /// <summary>
        /// set the autocomplete collection from the given list of paduro collection, using the given
        /// function to determine what strings to include
        /// </summary>
        /// <param name="txtBox">the textbox of which the auto complete list should be built</param>
        /// <param name="entries">the list of entries to check</param>
        /// <param name="func">the string selector function (returns string to include, string.Empty excludes the string)</param>
        void BuildAutoCompleteCollection(TextBox txtBox, List<PadoruEntry> entries, Func<PadoruEntry, string> func)
        {
            //Create collection, enumerate all entries
            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
            string entryString;
            foreach (PadoruEntry entry in entries)
            {
                //get entry string using function
                entryString = func(entry);

                //exclude if string is string.empty
                if (string.IsNullOrWhiteSpace(entryString)) continue;

                //exclude if already in list
                if (acsc.Contains(entryString)) continue;

                //add to colleciton
                acsc.Add(entryString);
            }

            //set autocomplete collection
            txtBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtBox.AutoCompleteCustomSource = acsc;
        }

        /// <summary>
        /// Update the padoru preview to show the character with the mal id in the mal id textbox
        /// </summary>
        async Task UpdatePadoruPreview()
        {
            //get mal id
            if (!long.TryParse(txtSelectedMalId.Text, out long malId)) return;

            //get character info using jikan
            Character character = await jikan.GetCharacter(malId);
            if (character == null) return;

            //Get which show the character is from
            string show = "?";
            if (character.Animeography != null && character.Animeography.Count > 0)
            {
                show = character.Animeography.First().Name;
            }
            else if (character.Mangaography != null && character.Mangaography.Count > 0)
            {
                show = character.Mangaography.First().Name;
            }

            //set preview name
            ppvMalResultPreview.DisplayName = $"{character.Name} ({show})";

            //download image and set preview image
            if (!string.IsNullOrWhiteSpace(character.ImageURL))
            {
                using (WebClient web = new WebClient())
                using (Stream imgStream = web.OpenRead(character.ImageURL))
                {
                    ppvMalResultPreview.PreviewImage = Image.FromStream(imgStream).Clone() as Image;
                }
            }
        }

        /// <summary>
        /// Get a list of all MAL characters matching the given search query
        /// </summary>
        /// <param name="searchQuery">the search query to use to find the character</param>
        /// <returns>the list of found characters</returns>
        async Task<List<MalCharacterEntry>> GetMalCharacters(string searchQuery)
        {
            //create a empty list to hold search results
            List<MalCharacterEntry> characterEntries = new List<MalCharacterEntry>();

            //check the query input
            if (string.IsNullOrWhiteSpace(searchQuery)) return characterEntries;

            //get search query in lower case and trimmed for additional operations
            string searchQueryLc = searchQuery.ToLower().Trim();

            //check if search query is searching for a mal id directly (prefix id:)
            long queryMalId = -1;
            if (searchQueryLc.StartsWith("id:") || searchQueryLc.StartsWith("id: "))
            {
                //get id string
                string queryIdSub = searchQueryLc.Substring("id:".Length).Trim();

                //parse id string as long
                if (!long.TryParse(queryIdSub, out queryMalId))
                {
                    //parse failed, fall back to queryid = -1 (default search)
                    queryMalId = -1;
                }
            }

            //perform normal search if queryMalId is -1 (not given OR parse failed), otherwise get character directly using id
            if (queryMalId == -1)
            {
                //search mal for character name
                CharacterSearchResult searchResults = await jikan.SearchCharacter(searchQuery);

                //check that we actually have search results
                if (searchResults == null || searchResults.Results == null) return characterEntries;

                //create a list of all search results
                foreach (CharacterSearchEntry character in searchResults.Results)
                {
                    //Get which show the character is from
                    string show = "?";
                    if (character.Animeography != null && character.Animeography.Count > 0)
                    {
                        show = character.Animeography.First().Name;
                    }
                    else if (character.Mangaography != null && character.Mangaography.Count > 0)
                    {
                        show = character.Mangaography.First().Name;
                    }

                    //Create entry object
                    characterEntries.Add(new MalCharacterEntry()
                    {
                        Name = character.Name,
                        Id = character.MalId,
                        Show = show
                    });
                }
            }
            else
            {
                //get character by mal id in query
                Character character = await jikan.GetCharacter(queryMalId);

                //check we actually found a character
                if (character == null) return characterEntries;

                //Get which show the character is from
                string show = "?";
                if (character.Animeography != null && character.Animeography.Count > 0)
                {
                    show = character.Animeography.First().Name;
                }
                else if (character.Mangaography != null && character.Mangaography.Count > 0)
                {
                    show = character.Mangaography.First().Name;
                }

                //add character to list of "search" results
                characterEntries.Add(new MalCharacterEntry()
                {
                    Name = character.Name,
                    Id = character.MalId,
                    Show = show
                });
            }

            //return populated list
            return characterEntries;
        }

        /// <summary>
        /// Update the Mal ID Selector Box by searching MAL for the given character name
        /// Also updates the Character Selection
        /// </summary>
        /// <param name="characterName">the character name to search for</param>
        async Task UpdateMalSelectorList(string characterName)
        {
            //lock mal combobox
            cbMalIdSelector.Enabled = false;

            //update mal combobox
            List<MalCharacterEntry> characters = await GetMalCharacters(characterName);
            cbMalIdSelector.Items.Clear();

            //add empty character
            cbMalIdSelector.Items.Add(new MalCharacterEntry()
            {
                Name = "Empty",
                Id = 0,
                Show = ""
            });

            //add found characters
            cbMalIdSelector.Items.AddRange(characters.ToArray());

            //set selection index
            if (loadedEntryMalId != -1)
            {
                //update selection to loaded entry
                for (int i = 0; i < cbMalIdSelector.Items.Count; i++)
                {
                    if ((cbMalIdSelector.Items[i] as MalCharacterEntry).Id == loadedEntryMalId)
                    {
                        cbMalIdSelector.SelectedIndex = i;
                        break;
                    }
                }

                loadedEntryMalId = -1;
            }
            else
            {
                if (cbMalIdSelector.Items.Count == 1)
                {
                    //only have "empty" entry, select that
                    cbMalIdSelector.SelectedIndex = 0;
                }
                else if (cbMalIdSelector.Items.Count > 1)
                {
                    //have "empty" + character entrys, select first character
                    cbMalIdSelector.SelectedIndex = 1;
                }
            }

            //re- enable mal comboobx
            cbMalIdSelector.Enabled = true;
        }

        /// <summary>
        /// Check if all required fields are filled. Also highlights the fields that need to be filled
        /// </summary>
        /// <remarks>Required are: URL, PATH, Name, Creator Name, Contributor Name</remarks>
        /// <returns>are all required fields filled?</returns>
        bool CheckRequiredFields()
        {
            //setup shared variables
            bool anyMissing = false;
            Color bgOk = SystemColors.Window;
            Color bgBd = Color.Red;

            //image url
            if (string.IsNullOrWhiteSpace(txtImageUrl.Text))
            {
                anyMissing = true;
                txtImageUrl.BackColor = bgBd;
            }
            else
            {
                txtImageUrl.BackColor = bgOk;
            }

            //image path is special because it also has to be possible to be made a relative path
            if (EditingParentCollection != null)
            {
                string collectionRoot = Path.GetDirectoryName(EditingParentCollection.LoadedFrom);
                if (string.IsNullOrWhiteSpace(txtImagePath.Text) || string.IsNullOrWhiteSpace(LibUtil.MakeRelativePath(collectionRoot, txtImagePath.Text)))
                {
                    anyMissing = true;
                    txtImagePath.BackColor = bgBd;
                }
                else
                {
                    txtImagePath.BackColor = bgOk;
                }
            }

            //character name
            if (string.IsNullOrWhiteSpace(txtCharacterName.Text))
            {
                anyMissing = true;
                txtCharacterName.BackColor = bgBd;
            }
            else
            {
                txtCharacterName.BackColor = bgOk;
            }

            //contributor name
            if (string.IsNullOrWhiteSpace(txtImageContributor.Text))
            {
                anyMissing = true;
                txtImageContributor.BackColor = bgBd;
            }
            else
            {
                txtImageContributor.BackColor = bgOk;
            }

            //creator name
            if (string.IsNullOrWhiteSpace(txtImageCreator.Text))
            {
                anyMissing = true;
                txtImageCreator.BackColor = bgBd;
            }
            else
            {
                txtImageCreator.BackColor = bgOk;
            }

            return !anyMissing;
        }

        #region UI Events
        void OnCancelClick(object sender, EventArgs e)
        {
            if (anyFieldChanged)
            {
                //user has unsaved changes, ask if he wants to cancel anyways
                if (MessageBox.Show(this, "If you Cancel, all unsaved changes will be lost.\nDo you want to Cancel anyway?", "Cancel", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    //does not want to cancel
                    return;
                }
            }

            DialogResult = DialogResult.Cancel;
            Close();
        }

        void OnFinishClick(object sender, EventArgs e)
        {
            if (!CheckRequiredFields())
            {
                //not all required fields are set!
                MessageBox.Show(this, "There are required values missing!\n(All fields marked with * are reqiured)", "Fields Missing", MessageBoxButtons.OK);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        void OnBrowseImagePathClick(object sender, EventArgs e)
        {
            //select file with dialog
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "PNG Files|*.png",
                CheckFileExists = true,
                Multiselect = false
            };

            //show dialog
            if (ofd.ShowDialog() != DialogResult.OK) return;

            //get path
            txtImagePath.Text = ofd.FileName;

            #region Image URL prediction
            //dont try to predict if there is no manager config
            if (CurrentManagerConfig == null) return;

            //exit if already have url
            if (!string.IsNullOrWhiteSpace(txtImageUrl.Text))
            {
                if (MessageBox.Show(this, "The image has already a URL! Do you want to override it with a prediction based on the current file path?", "Replace Image URL?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    //user said no
                    return;
                }
            }

            //make relative path
            string collectionRoot = Path.GetDirectoryName(EditingParentCollection.LoadedFrom);
            string relPath = LibUtil.MakeRelativePath(collectionRoot, ofd.FileName);

            //try to predict url
            string imgUrl = CurrentManagerConfig.PredictImageUrl(relPath);

            //check built url is valid
            if (string.IsNullOrWhiteSpace(imgUrl) || !Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute)) return;

            //set textbox
            txtImageUrl.Text = imgUrl;
            #endregion
        }

        void OnCharacterNameChanged(object sender, EventArgs e)
        {
            //clone changes to MAL search query
            txtMalSearchQuery.Text = txtCharacterName.Text;

            //forward event
            OnRequiredFieldChange(sender, e);
        }

        void OnMalSearchQueryChanged(object sender, EventArgs e)
        {
            malQueryChanged = true;
        }

        void OnMalSearchQueryKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter pressed, end edit
                OnMalSearchQueryEditEnd(sender, e);
            }
        }

        async void OnMalSearchQueryEditEnd(object sender, EventArgs e)
        {
            //skip if flag not set
            if (!malQueryChanged) return;

            //reset flag
            malQueryChanged = false;

            //get the search query (character name)
            string name = txtMalSearchQuery.Text;

            //update selector
            await UpdateMalSelectorList(name);
        }

        async void OnMalIdSelectorChange(object sender, EventArgs e)
        {
            //Get selected item as entry
            if (!(cbMalIdSelector.SelectedItem is MalCharacterEntry entry)) return;

            //chech for "Empty" entry with MAL id 0
            if (entry.Id != 0)
            {
                //update text boxes
                txtSelectedMalId.Text = entry.Id.ToString();
                txtSelectedMalName.Text = entry.Name;
            }
            else
            {
                //clear text boxes
                txtSelectedMalId.Text = "";
                txtSelectedMalName.Text = "";
            }

            //update preview
            await UpdatePadoruPreview();
        }

        async void OnLoad(object sender, EventArgs e)
        {
            //enable "loading" cursor for form
            UseWaitCursor = true;

            //build auto complete lists
            if (EditingParentCollection != null) PopulateAutoComplete(EditingParentCollection);

            //force check required fields
            CheckRequiredFields();

            //force get current mal preview image
            await UpdatePadoruPreview();

            //force update mal character search
            malQueryChanged = true;
            OnMalSearchQueryEditEnd(sender, e);

            //disable "loading" cursor
            UseWaitCursor = false;
        }

        void OnRequiredFieldChange(object sender, EventArgs e)
        {
            //update highlighting 
            CheckRequiredFields();

            //Forward event
            OnAnyFieldChange(sender, e);
        }

        void OnAnyFieldChange(object sender, EventArgs e)
        {
            anyFieldChanged = true;
        }
        #endregion

        class MalCharacterEntry
        {
            /// <summary>
            /// The MAL name of this character
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The MAL id of this character
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Which Anime/Manga/Show this character is from
            /// </summary>
            public string Show { get; set; }

            public override string ToString()
            {
                if (!string.IsNullOrWhiteSpace(Show))
                {
                    //has a show
                    return $"{Name}({Show})";
                }
                else
                {
                    //has no show
                    return $"{Name}";
                }
            }
        }
    }
}
