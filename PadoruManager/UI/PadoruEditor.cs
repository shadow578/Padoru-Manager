using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JikanDotNet;
using PadoruManager.Model;

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
        /// Shared jikan instance for mal api requests
        /// </summary>
        Jikan jikan;

        /// <summary>
        /// was the character name field changed?
        /// </summary>
        bool characterNameChanged;

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
            txtImagePath.Text = entry.ImagePath;
            txtCharacterName.Text = entry.Name;
            chkCharacterFemale.Checked = entry.IsFemale;
            //txtSelectedMalName
            //txtSelectedMalId
            txtImageCreator.Text = entry.ImageCreator;
            txtImageSource.Text = entry.ImageSource;
        }

        /// <summary>
        /// Create a PadoruEntry from the current ui state
        /// </summary>
        /// <returns>the padoru entry</returns>
        PadoruEntry UiToEntry()
        {
            return new PadoruEntry()
            {
                ImageUrl = txtImageUrl.Text,
                ImagePath = txtImagePath.Text,
                Name = txtCharacterName.Text,
                IsFemale = chkCharacterFemale.Checked,
                MALName = txtSelectedMalName.Text,
                MALId = txtSelectedMalId.Text,
                ImageCreator = txtImageCreator.Text,
                ImageSource = txtImageSource.Text
            };
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

            //set preview name
            ppvMalResultPreview.DisplayName = $"{character.Name} ({character.Animeography.First().Name})";

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
                characterEntries.Add(new MalCharacterEntry()
                {
                    Name = character.Name,
                    Id = character.MalId
                });
            }

            //return populated list
            return characterEntries;
        }

        #region UI Events
        void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        void OnFinishClick(object sender, EventArgs e)
        {
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

            //re- enable mal comboobx
            cbMalIdSelector.SelectedIndex = 0;
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

            public override string ToString()
            {
                return $"{Name}({Id})";
            }
        }
    }
}
