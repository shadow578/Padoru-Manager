using System;
using System.Collections.Generic;

namespace PadoruManager.Model
{
    /// <summary>
    /// A Collection of PadoruEntries
    /// </summary>
    public class PadoruCollection
    {
        /// <summary>
        /// The Entries in this collection
        /// </summary>
        public List<PadoruEntry> Entries { get; set; }

        /// <summary>
        /// When was the last change to this collection made?
        /// </summary>
        public DateTime LastChange { get; set; }
    }
}
