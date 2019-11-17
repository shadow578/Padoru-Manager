using JikanDotNet;
using PadoruManager.Model;
using PadoruManager.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        /// The path to the root directory of the collection the current entry is part of
        /// </summary>
        public string CollectionRootPath { get; set; }

        /// <summary>
        /// The unique id of the entry beign edited
        /// </summary>
        long editingId = -1;

        /// <summary>
        /// Shared jikan instance for mal api requests
        /// </summary>
        Jikan jikan;

        /// <summary>
        /// was the character name field changed?
        /// </summary>
        bool characterNameChanged;

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
            txtImagePath.Text = !string.IsNullOrWhiteSpace(entry.ImagePath) ? Path.Combine(entry.CollectionRoot, entry.ImagePath) : "";
            txtCharacterName.Text = entry.Name;
            chkCharacterFemale.Checked = entry.IsFemale;
            txtImageCreator.Text = entry.ImageCreator;
            txtImageSource.Text = entry.ImageSource;

            txtSelectedMalName.Text = entry.MALName;
            txtSelectedMalId.Text = entry.MALId;

            //parse entry mal id
            if (long.TryParse(entry.MALId, out long malId))
            {
                loadedEntryMalId = malId;
            }

            //save id
            editingId = entry.Id;

            //save root path
            if (!string.IsNullOrWhiteSpace(entry.CollectionRoot))
            {
                CollectionRootPath = entry.CollectionRoot;
            }
        }

        /// <summary>
        /// Create a PadoruEntry from the current ui state
        /// </summary>
        /// <returns>the padoru entry</returns>
        PadoruEntry UiToEntry()
        {
            PadoruEntry entry = new PadoruEntry()
            {
                ImageUrl = txtImageUrl.Text,
                ImagePath = (!string.IsNullOrWhiteSpace(txtImagePath.Text) ? Utils.MakeRelativePath(CollectionRootPath, txtImagePath.Text) : ""),
                Name = txtCharacterName.Text,
                IsFemale = chkCharacterFemale.Checked,
                MALName = txtSelectedMalName.Text,
                MALId = txtSelectedMalId.Text,
                ImageCreator = txtImageCreator.Text,
                ImageSource = txtImageSource.Text
            };

            if (editingId != -1) entry.Id = editingId;
            if (!string.IsNullOrWhiteSpace(CollectionRootPath)) entry.CollectionRoot = CollectionRootPath;
            return entry;
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
        /// <param name="searchName">the search query to use to find the character</param>
        /// <returns>the list of found characters</returns>
        async Task<List<MalCharacterEntry>> GetMalCharacters(string searchName)
        {
            //search mal for character name
            CharacterSearchResult searchResults = await jikan.SearchCharacter(searchName);

            //create a list of all search results
            List<MalCharacterEntry> characterEntries = new List<MalCharacterEntry>();
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

            //return populated list
            return characterEntries;
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
            if (string.IsNullOrWhiteSpace(txtImagePath.Text) || string.IsNullOrWhiteSpace(Utils.MakeRelativePath(CollectionRootPath, txtImagePath.Text)))
            {
                anyMissing = true;
                txtImagePath.BackColor = bgBd;
            }
            else
            {
                txtImagePath.BackColor = bgOk;
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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Files|*.png";
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;

            //show dialog
            if (ofd.ShowDialog() != DialogResult.OK) return;

            //get path
            txtImagePath.Text = ofd.FileName;
        }

        void OnCharacterNameChange(object sender, EventArgs e)
        {
            characterNameChanged = true;

            //forward event
            OnRequiredFieldChange(sender, e);
        }

        async void OnCharacterNameEditEnd(object sender, EventArgs e)
        {
            //skip if flag not set
            if (!characterNameChanged) return;

            //skip if name is empty
            string name = txtCharacterName.Text;
            if (string.IsNullOrWhiteSpace(name)) return;

            //lock mal combobox
            cbMalIdSelector.Enabled = false;

            //update mal combobox
            List<MalCharacterEntry> characters = await GetMalCharacters(name);
            cbMalIdSelector.Items.Clear();
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
                cbMalIdSelector.SelectedIndex = 0;
            }

            //re- enable mal comboobx
            cbMalIdSelector.Enabled = true;
        }

        async void OnMalIdSelectorChange(object sender, EventArgs e)
        {
            //Get selected item as entry
            if (!(cbMalIdSelector.SelectedItem is MalCharacterEntry entry)) return;

            //update text boxes
            txtSelectedMalId.Text = entry.Id.ToString();
            txtSelectedMalName.Text = entry.Name;

            //update preview
            await UpdatePadoruPreview();
        }

        async void OnLoad(object sender, EventArgs e)
        {
            //force check required fields
            CheckRequiredFields();

            //force get current mal preview image
            await UpdatePadoruPreview();

            //force update mal character search
            characterNameChanged = true;
            OnCharacterNameEditEnd(sender, e);
        }

        void OnRequiredFieldChange(object sender, EventArgs e)
        {
            //update highlighting 
            CheckRequiredFields();
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
                return $"{Name}({Show})";
            }
        }
    }
}
