using System;
using System.Drawing;
using System.Windows.Forms;

namespace PadoruManager.UI
{
    public partial class PadoruPreview : UserControl
    {
        /// <summary>
        /// how wide the borders are drawn
        /// </summary>
        const float BORDER_WIDTH = 2f;

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
        /// If true, the border of the control is drawn in double thickness
        /// </summary>
        public bool ThickBorders { get; set; }

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

        protected override void OnPaint(PaintEventArgs e)
        {
            //do normal paint
            base.OnPaint(e);

            //draw borders around control
            e.Graphics.DrawRectangle(new Pen(Color.Black, BORDER_WIDTH * (ThickBorders ? 2 : 1)), e.ClipRectangle);
        }

        void OnAnyClick(object sender, EventArgs e)
        {
            //redirect to control click handler
            OnClick(e);
        }
    }
}
