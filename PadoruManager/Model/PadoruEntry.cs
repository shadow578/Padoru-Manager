namespace PadoruManager.Model
{
    /// <summary>
    /// A Padoru Character entry in the collection
    /// </summary>
    public class PadoruEntry
    {
        /// <summary>
        /// The Image url (in the github repo)
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The relative image path inside the local collection directory
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// This Character's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is this a (canonically) female Character?
        /// </summary>
        public bool IsFemale { get; set; }

        /// <summary>
        /// This Character's name in MAL
        /// </summary>
        /// <remarks>This field is optional and may be null/empty</remarks>
        public string MALName { get; set; }

        /// <summary>
        /// This Character's MAL character id
        /// </summary>
        /// <remarks>This field is optional and may be null/empty</remarks>
        public string MALId { get; set; }

        /// <summary>
        /// The Name of the Person that created the image
        /// </summary>
        /// <remarks>This field is optional and may be null/empty</remarks>
        public string ImageCreator { get; set; }

        /// <summary>
        /// The source this image is from (reddit post, imgur, pixiv, ...)
        /// </summary>
        /// <remarks>This field is optional and may be null/empty</remarks>
        public string ImageSource { get; set; }
    }
}
