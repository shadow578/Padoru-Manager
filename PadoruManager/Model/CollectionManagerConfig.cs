using Newtonsoft.Json;
using PadoruManager.Util;
using System;
using System.Drawing;
using System.IO;

namespace PadoruManager.Model
{
    /// <summary>
    /// Contains information about collection paths and urls
    /// </summary>
    public class CollectionManagerConfig
    {
        #region Not Serialized Properties
        /// <summary>
        /// The Path this config was loaded from
        /// </summary>
        [JsonIgnore]
        public string LoadedFrom { get; set; }
        #endregion

        #region Serialized Properties
        /// <summary>
        /// The url to the repo root (not raw, used for e.g. table of content page linking)
        /// </summary>
        public string RepoRootUrl { get; set; }

        /// <summary>
        /// The url to the repo root (raw files, used for e.g. image url prediction)
        /// </summary>
        public string RawRepoRootUrl { get; set; }

        /// <summary>
        /// The name of the Table of Contents directory, (inside repo root directory)
        /// </summary>
        public string TableOfContentsDirectoryName { get; set; }

        /// <summary>
        /// The last known size of the CollectionManager UI
        /// </summary>
        public Size LastUiSize { get; set; }
        #endregion

        #region Functions
        /// <summary>
        /// Load the Collection Manager Info from a serialized json file
        /// </summary>
        /// <param name="filePath">the file to load from</param>
        /// <returns>the loaded collection manager info, or null if load failed</returns>
        public static CollectionManagerConfig LoadFrom(string filePath)
        {
            //read from file
            using (StreamReader reader = File.OpenText(filePath))
            {
                //read serialized object
                CollectionManagerConfig cfg = GetSerializer().Deserialize(reader, typeof(CollectionManagerConfig)) as CollectionManagerConfig;
                cfg.LoadedFrom = filePath;
                return cfg;
            }
        }

        /// <summary>
        /// Save the Collection Manager Info to a serialized json file
        /// </summary>
        /// <param name="filePath">the file to save to (default is LoadedFrom)</param>
        public void SaveTo(string filePath = null)
        {
            //default path to LoadedFrom
            if (string.IsNullOrWhiteSpace(filePath)) filePath = LoadedFrom;
            if (string.IsNullOrWhiteSpace(filePath)) return;

            //create required directorys
            Utils.CreateFileDir(filePath);

            //write to file
            using (StreamWriter writer = File.CreateText(filePath))
            {
                //write serialized object 
                GetSerializer(false).Serialize(writer, this);
            }
        }

        /// <summary>
        /// Create the Json (de) serializer used for (de) serializing this sort of object
        /// </summary>
        /// <param name="minify">should the json serializer produce indented (=pretty) json or minified json?</param>
        /// <returns>the serializer to use</returns>
        static JsonSerializer GetSerializer(bool minify = false)
        {
            return new JsonSerializer() { Formatting = minify ? Formatting.None : Formatting.Indented };
        }

        /// <summary>
        /// Get the repo url for the table of contents directory root
        /// </summary>
        /// <returns>the repo url for the toc root dir</returns>
        public string GetTableOfContentsRepoRoot()
        {
            return RepoRootUrl.TrimEnd('/') + "/" + TableOfContentsDirectoryName;
        }

        /// <summary>
        /// Get the local path for the table of contents directory root
        /// </summary>
        /// <param name="collectionRootOverride">the root path of the current colleciton, use to override default one</param>
        /// <returns>the path to the toc root dir</returns>
        public string GetTableOfContentsLocalRoot(string collectionRootOverride = "")
        {
            //set collection root if not overriden
            collectionRootOverride = Path.GetDirectoryName(LoadedFrom);

            //build path
            return Path.Combine(collectionRootOverride, TableOfContentsDirectoryName);
        }

        /// <summary>
        /// Try to predict a image url based on the given relative path (in the local file system)
        /// </summary>
        /// <param name="imageRelativePath">the relative path of the image</param>
        /// <returns>the predicted image url in the remote repo. This predicton may be incorrect (Or string.Empty if required values are missing an a prediction cannot be made)</returns>
        public string PredictImageUrl(string imageRelativePath)
        {
            //check required values for prediction
            if (string.IsNullOrWhiteSpace(imageRelativePath) || string.IsNullOrWhiteSpace(RawRepoRootUrl)) return string.Empty;

            //replace backslash with forward slash
            imageRelativePath = imageRelativePath.Replace("\\", "/");

            //try to build predicted url
            return RawRepoRootUrl.TrimEnd('/') + "/" + imageRelativePath;
        }
        #endregion
    }
}
