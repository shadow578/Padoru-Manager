using System;
using System.Drawing;
using System.Windows.Forms;

namespace PadoruManager.UI
{
    public partial class PadoruPreview : UserControl
    {
        /// <summary>
        /// The image that is shown in this PadoruPreview
        /// </summary>
        public Image PreviewImage
        {
            get
            {
                return imgPadoru.BackgroundImage;
            }
            set
            {
                imgPadoru.BackgroundImage = value;
            }
        }

        /// <summary>
        /// The Name that is displayed below the image view
        /// </summary>
        public string DisplayName
        {
            get
            {
                return lbName.Text;
            }
            set
            {
                lbName.Text = value;
            }
        }

        /// <summary>
        /// Initialize the PadoruPreview without name and picture set
        /// </summary>
        public PadoruPreview()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the PadourPreview
        /// </summary>
        /// <param name="name">The name to show</param>
        /// <param name="image">the image to show</param>
        public PadoruPreview(string name, Image image)
        {
            DisplayName = name;
            PreviewImage = image;
        }

        void OnAnyClick(object sender, EventArgs e)
        {
            //redirect to control click handler
            OnClick(e);
        }
    }
}
