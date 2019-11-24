using PadoruManager.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PadoruManager.UI
{
    public partial class ManagerConfigSetupWizard : Form
    {
        /// <summary>
        /// The Config that was created in this ui
        /// </summary>
        public CollectionManagerConfig ResultingConfig { get; private set; }

        /// <summary>
        /// Are both repo urls valid?
        /// </summary>
        bool repoUrlsValid = false;

        /// <summary>
        /// initialize the setup wizard
        /// </summary>
        public ManagerConfigSetupWizard()
        {
            InitializeComponent();
        }

        void OnLoad(object sender, EventArgs e)
        {
            //update repo root text boxes on load
            OnCommonRepoTextChange(sender, e);
        }

        void OnCommonRepoTextChange(object sender, EventArgs e)
        {
            //rebuild repo root and raw root text boxes
            string repoRootUrl = $"https://github.com/{txtGithubUser.Text}/{txtGithubRepo.Text}/blob/master/";
            string rawRepoRootUrl = $"https://raw.githubusercontent.com/{txtGithubUser.Text}/{txtGithubRepo.Text}/master/";

            //set urls in text fields
            txtRepoRoot.Text = repoRootUrl;
            txtRepoRootRaw.Text = rawRepoRootUrl;

            //check if (raw) repo root are valid urls
            repoUrlsValid = true;
            if (!Uri.IsWellFormedUriString(repoRootUrl, UriKind.Absolute))
            {
                txtRepoRoot.BackColor = Color.Red;
                repoUrlsValid = false;
            }
            else
            {
                txtRepoRoot.BackColor = SystemColors.Control;
            }

            if (!Uri.IsWellFormedUriString(rawRepoRootUrl, UriKind.Absolute))
            {
                txtRepoRootRaw.BackColor = Color.Red;
                repoUrlsValid = false;
            }
            else
            {
                txtRepoRootRaw.BackColor = SystemColors.Control;
            }
        }

        void OnSaveClick(object sender, EventArgs e)
        {
            //check if both repo urls are ok
            OnCommonRepoTextChange(sender, e);
            if (!repoUrlsValid)
            {
                MessageBox.Show(this, "The Repo URLs are not valid! Re- Check your github username and repo name.", "Repo URLs invalid", MessageBoxButtons.OK);
                return;
            }

            //check if table of contents path is filled
            if (string.IsNullOrWhiteSpace(txtToCRoot.Text))
            {
                MessageBox.Show(this, "You have to fill in a Table Of Contents root directory name!", "ToC Root missing", MessageBoxButtons.OK);
                return;
            }

            //ask user if settings are correct
            if (MessageBox.Show(this, "Are the settings correct?", "Check Settings", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            //create CollectionManagerConfig
            ResultingConfig = new CollectionManagerConfig()
            {
                RepoRootUrl = txtRepoRoot.Text,
                RawRepoRootUrl = txtRepoRootRaw.Text,
                TableOfContentsDirectoryName = txtToCRoot.Text,
                LastUiSize = Size.Empty
            };

            //exit with result ok
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
