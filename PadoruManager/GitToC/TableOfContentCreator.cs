using JikanDotNet;
using PadoruLib.Padoru.Model;
using PadoruManager.Model;
using PadoruManager.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PadoruManager.GitToC
{
    /// <summary>
    /// Generator to create a ordered Table of Contents from a PadoruCollection with github markdown
    /// </summary>
    public class TableOfContentCreator
    {
        /// <summary>
        /// The current collection manager configuration. If not set, image url prediction will be unavailable.
        /// </summary>
        public CollectionManagerConfig CurrentManagerConfig { get; set; }

        /// <summary>
        /// initialize the toc creator
        /// </summary>
        /// <param name="managerConfig">the colleciton manager config</param>
        public TableOfContentCreator(CollectionManagerConfig managerConfig)
        {
            CurrentManagerConfig = managerConfig;
        }

        /// <summary>
        /// Create a Table of Contents from the given collection
        /// </summary>
        /// <param name="collection">the collection to create a table of contents from</param>
        public async Task CreateTableOfContents(PadoruCollection collection)
        {
            //create toc entries
            List<ToCEntry> toCEntries = await CreateEntries(collection);

            //create character pages
            foreach (ToCEntry toc in toCEntries)
            {
                //make character page url
                toc.CharacterPageUrl = $"{CurrentManagerConfig.GetTableOfContentsRepoRoot()}/characters/{SanitizeForFileName(toc.CharacterName)}.md";

                //write local file
                CreateCharacterPage(toc, Path.Combine(CurrentManagerConfig.GetTableOfContentsLocalRoot(), "characters", SanitizeForFileName(toc.CharacterName) + ".md"));
            }

            //create character index page
            CreateCharactersIndexPage(toCEntries, Path.Combine(CurrentManagerConfig.GetTableOfContentsLocalRoot(), "character-index.md"));
        }

        /// <summary>
        /// Create a list of table entries from a padoru collection
        /// </summary>
        /// <param name="pCollection">the padoru collection to create the list from</param>
        /// <returns>the list of table entries</returns>
        async Task<List<ToCEntry>> CreateEntries(PadoruCollection pCollection)
        {
            //initialize jikan to resolve character names, shows, etc
            Jikan jikan = new Jikan();

            //enumerate all padorus
            List<ToCEntry> tocEntries = new List<ToCEntry>();
            foreach (PadoruEntry pEntry in pCollection.Entries)
            {
                //create entry with base values
                ToCEntry toc = new ToCEntry()
                {
                    CharacterName = pEntry.Name,
                    ContributorName = pEntry.ImageContributor,
                    CreatorName = pEntry.ImageCreator,
                    CreatorUrl = pEntry.ImageSource,
                    ImageUrl = pEntry.ImageUrl
                };

                //get character info from jikan
                if (pEntry.MALId.HasValue)
                {
                    //get mal character
                    Character charInfo = await jikan.GetCharacter(pEntry.MALId.Value);

                    if (charInfo != null)
                    {
                        #region Get Character's shows
                        List<ToCShow> cShows = new List<ToCShow>();

                        //get anime this character is in
                        if (charInfo.Animeography.Count > 0)
                        {
                            foreach (MALImageSubItem anime in charInfo.Animeography)
                            {
                                cShows.Add(new ToCShow()
                                {
                                    ShowName = anime.Name,
                                    ShowMalUrl = anime.Url
                                });
                            }
                        }

                        //get manga this character is in
                        if (charInfo.Mangaography.Count > 0)
                        {
                            foreach (MALImageSubItem manga in charInfo.Mangaography)
                            {
                                cShows.Add(new ToCShow()
                                {
                                    ShowName = manga.Name,
                                    ShowMalUrl = manga.Url
                                });
                            }
                        }

                        //set show entries
                        toc.CharacterShows = cShows.ToArray();
                        #endregion

                        //override name and set mal id
                        toc.CharacterName = charInfo.Name;
                        toc.MalId = charInfo.MalId.ToString();

                        //set character nickname list
                        if (charInfo.Nicknames.Count > 0)
                        {
                            toc.CharacterNicks = charInfo.Nicknames.ToArray();
                        }
                    }
                }

                //add toc entry to list
                tocEntries.Add(toc);
            }

            return tocEntries;
        }

        /// <summary>
        /// Create the character index page for the given list of toc entries
        /// each toc enry must have a character page url, or it will be ignroed
        /// </summary>
        /// <param name="toCEntries">the list of toc entries</param>
        /// <param name="savePath">where the characters index page should be saved to</param>
        void CreateCharactersIndexPage(List<ToCEntry> toCEntries, string savePath)
        {
            //sort tocs by character name
            toCEntries.Sort((a, b) =>
            {
                //compare names
                return a.CharacterName.CompareTo(b.CharacterName);
            });

            //create file
            Utils.CreateFileDir(savePath);
            List<string> linkedCharacterPages = new List<string>();
            using (TextWriter page = File.CreateText(savePath))
            {
                //list characters alphabetically
                char currentChar = (char)0;
                foreach (ToCEntry toc in toCEntries)
                {
                    //update current char, add header
                    char tocChar = char.ToUpper(toc.CharacterName.First());
                    if (tocChar > currentChar)
                    {
                        currentChar = tocChar;
                        page.WriteLine();
                        page.WriteLine($"### {tocChar}");
                    }

                    //add character name and url
                    if (string.IsNullOrWhiteSpace(toc.CharacterPageUrl))
                    {
                        page.WriteLine($"* {toc.CharacterName}");
                    }
                    else
                    {
                        //check that this page was not yet linked to
                        if (!linkedCharacterPages.Contains(toc.CharacterPageUrl))
                        {
                            //page not yet linked, add to index
                            page.WriteLine($"* [{toc.CharacterName}]({toc.CharacterPageUrl})");
                            linkedCharacterPages.Add(toc.CharacterPageUrl);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create a Character page for the given toc entry
        /// </summary>
        /// <param name="toc">the toc entry to create a character page for</param>
        void CreateCharacterPage(ToCEntry toc, string savePath)
        {
            //create file
            Utils.CreateFileDir(savePath);
            using (TextWriter page = File.AppendText(savePath))
            {
                //add page header
                page.WriteLine($"# {toc.CharacterName}");
                page.WriteLine();

                //add inline image
                page.WriteLine($"![padoru]({toc.ImageUrl} \"{toc.CharacterName}\")");

                //add image info
                page.WriteLine();
                page.WriteLine("### Image Info");
                if (string.IsNullOrWhiteSpace(toc.CreatorUrl))
                {
                    page.WriteLine($"* **Created by:**    {toc.CreatorName}");
                }
                else
                {
                    page.WriteLine($"* **Created by:**    [{toc.CreatorName}]({toc.CreatorUrl})");
                }
                page.WriteLine($"* **Contributor:**   {toc.ContributorName}");

                //add character info
                page.WriteLine();
                page.WriteLine("### Character Info");
                if (string.IsNullOrWhiteSpace(toc.MalUrl))
                {
                    page.WriteLine($"* **Name:**   {toc.CharacterName}");
                }
                else
                {
                    page.WriteLine($"* **Name:**   [{toc.CharacterName}]({toc.MalUrl})");
                }

                //character nicknames
                if (toc.CharacterNicks != null && toc.CharacterNicks.Length > 0)
                {
                    page.WriteLine("* **Nicknames:**");
                    foreach (string nick in toc.CharacterNicks)
                    {
                        page.WriteLine($"  * {nick}");
                    }
                }

                //character shows + links
                if (toc.CharacterShows != null && toc.CharacterShows.Length > 0)
                {
                    page.WriteLine("* **Shows:**");
                    foreach (ToCShow show in toc.CharacterShows)
                    {
                        if (string.IsNullOrWhiteSpace(show.ShowMalUrl))
                        {
                            page.WriteLine($"  * {show.ShowName}");
                        }
                        else
                        {
                            page.WriteLine($"  * [{show.ShowName}]({show.ShowMalUrl})");
                        }
                    }
                }

                //empty lines to pad eventual additional entries
                page.WriteLine();
                page.WriteLine();
            }
        }

        /// <summary>
        /// Sanitize a file name so it can actually be used as file name
        /// </summary>
        /// <param name="inp">the input file name to sanitize</param>
        /// <returns>the sanitized file name</returns>
        string SanitizeForFileName(string inp)
        {
            string outp = "";
            foreach (char c in inp)
            {
                if (char.IsLetterOrDigit(c))
                {
                    outp += c;
                }
            }

            return outp;
        }

        /// <summary>
        /// A entry in a Table Of Contents for a PadoruCollection
        /// </summary>
        class ToCEntry
        {
            /// <summary>
            /// The url of the character page
            /// </summary>
            public string CharacterPageUrl { get; set; }

            /// <summary>
            /// The name of the character this entry is for
            /// </summary>
            public string CharacterName { get; set; }

            /// <summary>
            /// The nicknames of the character this entry is for
            /// </summary>
            public string[] CharacterNicks { get; set; }

            /// <summary>
            /// The show of this entry's character
            /// </summary>
            /// <remarks>taken from MAL, may be empty</remarks>
            public ToCShow[] CharacterShows { get; set; }

            /// <summary>
            /// The image url of this character
            /// </summary>
            public string ImageUrl { get; set; }

            /// <summary>
            /// The name of this padoru's creator
            /// </summary>
            public string CreatorName { get; set; }

            /// <summary>
            /// The source url of this padoru entry
            /// </summary>
            /// <remarks>optional field, may be empty</remarks>
            public string CreatorUrl { get; set; }

            /// <summary>
            /// The name of the person that contributed this entry
            /// </summary>
            public string ContributorName { get; set; }

            /// <summary>
            /// The MAL id of this entry's padoru
            /// </summary>
            /// <remarks>May be empty</remarks>
            public string MalId { get; set; }

            /// <summary>
            /// The MAL url of this entry
            /// </summary>
            /// <remarks>built from MalId, may be empty</remarks>
            public string MalUrl
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(MalId)) return string.Empty;

                    return $"https://myanimelist.net/character/{MalId}";
                }
            }
        }

        /// <summary>
        /// a show a character of a TOC entry is in
        /// </summary>
        class ToCShow
        {
            /// <summary>
            /// The name of the show
            /// </summary>
            public string ShowName { get; set; }

            /// <summary>
            /// The mal page of the show
            /// </summary>
            public string ShowMalUrl { get; set; }
        }
    }
}
