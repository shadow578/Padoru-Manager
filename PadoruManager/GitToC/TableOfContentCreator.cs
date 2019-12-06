using JikanDotNet;
using PadoruLib.Padoru.Model;
using PadoruManager.Model;
using PadoruManager.Util;
using System;
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
            ToCData tocData = await CreateData(collection);

            //create character pages links
            foreach (ToCEntry tocE in tocData.Characters)
            {
                //make character page url
                tocE.CharacterPageUrl = $"{CurrentManagerConfig.GetTableOfContentsRepoRoot()}/characters/{SanitizeForFileName(tocE.CharacterName)}.md";
            }

            //create creator pages
            foreach (ToCCreator tocC in tocData.Creators)
            {
                //make creator page url
                tocC.PageUrl = $"{CurrentManagerConfig.GetTableOfContentsRepoRoot()}/creators/{SanitizeForFileName(tocC.Name)}.md";

                //write local file
                CreateCreatorPage(tocC, Path.Combine(CurrentManagerConfig.GetTableOfContentsLocalRoot(), "creators", SanitizeForFileName(tocC.Name) + ".md"));
            }

            //create show pages
            foreach (ToCShow tocS in tocData.Shows)
            {
                //make creator page url
                tocS.PageUrl = $"{CurrentManagerConfig.GetTableOfContentsRepoRoot()}/shows/{SanitizeForFileName(tocS.Name)}.md";

                //write local file
                CreateShowPage(tocS, Path.Combine(CurrentManagerConfig.GetTableOfContentsLocalRoot(), "shows", SanitizeForFileName(tocS.Name) + ".md"));
            }

            //create character pages
            foreach (ToCEntry tocE in tocData.Characters)
            {
                //write local file
                CreateCharacterPage(tocE, Path.Combine(CurrentManagerConfig.GetTableOfContentsLocalRoot(), "characters", SanitizeForFileName(tocE.CharacterName) + ".md"));
            }

            //create characters index page
            CreateCharactersIndexPage(tocData.Characters, Path.Combine(CurrentManagerConfig.GetTableOfContentsLocalRoot(), "character-index.md"));

            //create creators index page
            CreateCreatorsIndexPage(tocData.Creators, Path.Combine(CurrentManagerConfig.GetTableOfContentsLocalRoot(), "creators-index.md"));

            //create shows index page
            CreateShowsIndexPage(tocData.Shows, Path.Combine(CurrentManagerConfig.GetTableOfContentsLocalRoot(), "shows-index.md"));
        }

        /// <summary>
        /// Create a list of table entries from a padoru collection
        /// </summary>
        /// <param name="pCollection">the padoru collection to create the list from</param>
        /// <returns>the table of contents data</returns>
        async Task<ToCData> CreateData(PadoruCollection pCollection)
        {
            //initialize jikan to resolve character names, shows, etc
            Jikan jikan = new Jikan();

            //enumerate all padorus
            List<ToCEntry> tocCharacters = new List<ToCEntry>();
            List<ToCCreator> tocCreators = new List<ToCCreator>();
            List<ToCShow> tocShows = new List<ToCShow>();
            foreach (PadoruEntry pEntry in pCollection.Entries)
            {
                //create entry with base values
                ToCEntry toc = new ToCEntry()
                {
                    CharacterName = pEntry.Name,
                    ContributorName = pEntry.ImageContributor,
                    //CreatorName = pEntry.ImageCreator,
                    SourceUrl = pEntry.ImageSource,
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
                                //check if shows list already contains this anime
                                ToCShow tocAnime = null;
                                if (tocShows.HasAnyWhere((s) => s.MalId.Equals(anime.MalId)))
                                {
                                    //show already exists, try to get it
                                    tocAnime = tocShows.Where((s) => s.MalId.Equals(anime.MalId)).FirstOrDefault();
                                }

                                //create new toc show if still null (not in list or get from list failed)
                                if (tocAnime == null)
                                {
                                    tocAnime = new ToCShow()
                                    {
                                        Name = anime.Name,
                                        MalUrl = anime.Url,
                                        MalId = anime.MalId
                                    };
                                }

                                //add current anime to list of shows
                                cShows.Add(tocAnime);
                            }
                        }

                        //get manga this character is in
                        if (charInfo.Mangaography.Count > 0)
                        {
                            foreach (MALImageSubItem manga in charInfo.Mangaography)
                            {
                                //check if shows list already contains this anime
                                ToCShow tocManga = null;
                                if (tocShows.HasAnyWhere((s) => s.MalId.Equals(manga.MalId)))
                                {
                                    //show already exists, try to get it
                                    tocManga = tocShows.Where((s) => s.MalId.Equals(manga.MalId)).FirstOrDefault();
                                }

                                //create new toc show if still null (not in list or get from list failed)
                                if (tocManga == null)
                                {
                                    tocManga = new ToCShow()
                                    {
                                        Name = manga.Name,
                                        MalUrl = manga.Url,
                                        MalId = manga.MalId
                                    };
                                }

                                //add current anime to list of shows
                                cShows.Add(tocManga);
                            }
                        }

                        //add current character to all shows
                        foreach (ToCShow show in cShows)
                        {
                            show.Characters.Add(toc);
                        }

                        //set shows of character
                        toc.CharacterShows = cShows;

                        //add all shows to global list
                        tocShows.AddRange(cShows);
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

                #region Set Creator Info
                //check if there is already a creator entry matching the current entrys creator name
                ToCCreator creator = null;
                if (tocCreators.HasAnyWhere((c) => c.Name.EqualsIgnoreCase(pEntry.ImageCreator)))
                {
                    //already has a entry, try use that
                    creator = tocCreators.Where((c) => c.Name.EqualsIgnoreCase(pEntry.ImageCreator)).FirstOrDefault();
                }

                //create new creator entry if is still null (no creator found or get failed)
                if (creator == null)
                {
                    //Create new creator
                    creator = new ToCCreator()
                    {
                        Name = pEntry.ImageCreator
                    };

                    //add it to the list of creators
                    tocCreators.Add(creator);
                }

                //add this entry to the creator and this creator to the current entry
                creator.Entries.Add(toc);
                toc.Creator = creator;
                #endregion

                //add toc entry to list
                tocCharacters.Add(toc);
            }

            return new ToCData()
            {
                Characters = tocCharacters,
                Creators = tocCreators,
                Shows = tocShows
            };
        }

        #region Page Generators
        #region Shows
        /// <summary>
        /// Create the shows index page for the given list of toc entries
        /// each toc enry must have a show page url, or it will be ignored
        /// </summary>
        /// <param name="toCEntries">the list of toc creators</param>
        /// <param name="savePath">where the shows index page should be saved to</param>
        void CreateShowsIndexPage(List<ToCShow> toCEntries, string savePath)
        {
            //sort tocs by creator name
            toCEntries.Sort((a, b) =>
            {
                //compare names
                return a.Name.CompareTo(b.Name);
            });

            //create file
            Utils.CreateFileDir(savePath);
            List<string> linkedCreatorPages = new List<string>();
            using (TextWriter page = File.CreateText(savePath))
            {
                //write header
                page.WriteLine("# Creators");

                //list characters alphabetically
                char currentChar = (char)0;
                foreach (ToCShow toc in toCEntries)
                {
                    //update current char, add header
                    char tocChar = char.ToUpper(toc.Name.First());
                    if (tocChar > currentChar)
                    {
                        currentChar = tocChar;
                        page.WriteLine();
                        page.WriteLine($"### {tocChar}");
                    }

                    //add character name and url
                    if (string.IsNullOrWhiteSpace(toc.PageUrl))
                    {
                        page.WriteLine($"* {toc.Name}");
                    }
                    else
                    {
                        //check that this page was not yet linked to
                        if (!linkedCreatorPages.Contains(toc.PageUrl))
                        {
                            //page not yet linked, add to index
                            page.WriteLine($"* [{toc.Name}]({toc.PageUrl})");
                            linkedCreatorPages.Add(toc.PageUrl);
                        }
                    }
                }

                //write footer
                WritePageFooter(page);
            }
        }

        /// <summary>
        /// Create a Show page for the given toc show
        /// </summary>
        /// <param name="toc">the toc show to create a creator page for</param>
        void CreateShowPage(ToCShow toc, string savePath)
        {
            //create title, embed show mal url if possible
            string title = $"# Padorus in {toc.Name}";
            if (!string.IsNullOrWhiteSpace(toc.MalUrl))
            {
                title = $"# Padorus in [{toc.Name}]({toc.MalUrl})";
            }

            //create page
            CreateCharactersIndexPage(toc.Characters, savePath, title);
        }
        #endregion

        #region Creators
        /// <summary>
        /// Create the creators index page for the given list of toc entries
        /// each toc enry must have a character page url, or it will be ignored
        /// </summary>
        /// <param name="toCEntries">the list of toc creators</param>
        /// <param name="savePath">where the creators index page should be saved to</param>
        void CreateCreatorsIndexPage(List<ToCCreator> toCEntries, string savePath)
        {
            //sort tocs by creator name
            toCEntries.Sort((a, b) =>
            {
                //remove u/ from the names (reddit names)
                string aN = a.Name.Replace("u/", "").Replace("U/", "");
                string bN = b.Name.Replace("u/", "").Replace("U/", "");

                //compare names
                return aN.CompareTo(bN);
            });

            //create file
            Utils.CreateFileDir(savePath);
            List<string> linkedCreatorPages = new List<string>();
            using (TextWriter page = File.CreateText(savePath))
            {
                //write header
                page.WriteLine("# Creators");

                //list characters alphabetically
                char currentChar = (char)0;
                foreach (ToCCreator toc in toCEntries)
                {
                    //get toc name without u/
                    string tocName = toc.Name.Replace("u/", "").Replace("U/", "");

                    //update current char, add header
                    char tocChar = char.ToUpper(tocName.First());
                    if (tocChar > currentChar)
                    {
                        currentChar = tocChar;
                        page.WriteLine();
                        page.WriteLine($"### {tocChar}");
                    }

                    //add character name and url
                    if (string.IsNullOrWhiteSpace(toc.PageUrl))
                    {
                        page.WriteLine($"* {tocName}");
                    }
                    else
                    {
                        //check that this page was not yet linked to
                        if (!linkedCreatorPages.Contains(toc.PageUrl))
                        {
                            //page not yet linked, add to index
                            page.WriteLine($"* [{tocName}]({toc.PageUrl})");
                            linkedCreatorPages.Add(toc.PageUrl);
                        }
                    }
                }

                //write footer
                WritePageFooter(page);
            }
        }

        /// <summary>
        /// Create a Creator page for the given toc creator
        /// </summary>
        /// <param name="toc">the toc creator to create a creator page for</param>
        void CreateCreatorPage(ToCCreator toc, string savePath)
        {
            CreateCharactersIndexPage(toc.Entries, savePath, $"# Padorus by {toc.Name}");
        }
        #endregion

        #region Characters
        /// <summary>
        /// Create the character index page for the given list of toc entries
        /// each toc enry must have a character page url, or it will be ignroed
        /// </summary>
        /// <param name="toCEntries">the list of toc entries</param>
        /// <param name="savePath">where the characters index page should be saved to</param>
        /// <param name="title">the title of the page</param>
        void CreateCharactersIndexPage(List<ToCEntry> toCEntries, string savePath, string title = "# Characters")
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
                //write header
                page.WriteLine(title);

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

                //write footer
                WritePageFooter(page);
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
                //page.WriteLine($"![padoru]({toc.ImageUrl} \"{toc.CharacterName}\")");
                page.WriteLine($"<img src=\"{toc.ImageUrl}\" height=\"300\">");

                //add image info
                page.WriteLine();
                page.WriteLine("### Image Info");
                if (!string.IsNullOrWhiteSpace(toc.SourceUrl) && Uri.TryCreate(toc.SourceUrl, UriKind.Absolute, out Uri sourceUri))
                {
                    //Get the name of the page this was posted on
                    string postedOn = sourceUri.Host;
                    postedOn = postedOn.Replace("www.", "");

                    page.WriteLine($"* **Posted on:**     [{postedOn}]({toc.SourceUrl})");
                }

                //creator with creator page linked
                if (string.IsNullOrWhiteSpace(toc.Creator.PageUrl))
                {
                    page.WriteLine($"* **Created by:**    {toc.Creator.Name}");
                }
                else
                {
                    page.WriteLine($"* **Created by:**    [{toc.Creator.Name}]({toc.Creator.PageUrl})");
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
                if (toc.CharacterShows != null && toc.CharacterShows.Count > 0)
                {
                    page.WriteLine("* **Shows:**");
                    foreach (ToCShow show in toc.CharacterShows)
                    {
                        bool hasPage = !string.IsNullOrWhiteSpace(show.PageUrl);
                        bool hasMal = !string.IsNullOrWhiteSpace(show.MalUrl);

                        if (hasPage && hasMal)
                        {
                            //has both show page and mal page, link to both
                            page.WriteLine($"  * [{show.Name}]({show.PageUrl}) - [__MAL__]({show.MalUrl})");
                        }
                        else if (hasPage && !hasMal)
                        {
                            //has a show page but no mal page, link only show page
                            page.WriteLine($"  * [{show.Name}]({show.PageUrl})");
                        }
                        else if (!hasPage && hasMal)
                        {
                            //has a mal page but no show page, link only mal page
                            page.WriteLine($"  * [{show.Name}]({show.MalUrl})");
                        }
                        else
                        {
                            //has no mal or show page, link none
                            page.WriteLine($"  * {show.Name}");
                        }
                    }
                }

                //empty lines to pad eventual additional entries
                page.WriteLine();
                page.WriteLine();
            }
        }
        #endregion
        #endregion

        #region Utility
        /// <summary>
        /// Write a footer to the given page
        /// </summary>
        /// <param name="page">the page to write to</param>
        void WritePageFooter(TextWriter page)
        {
            //empty line before footer
            page.WriteLine();

            //add notice that this page was auto- generated
            page.WriteLine("###### This page was automatically generated. If it contains errors, please open a Issue.");

            //add page generation date
            //page.WriteLine($"###### Generated on {DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")}");
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
        /// Contains data of a Table of Contents
        /// </summary>
        class ToCData
        {
            /// <summary>
            /// All character entries in the toc
            /// </summary>
            public List<ToCEntry> Characters { get; set; }

            /// <summary>
            /// All creator entries in the toc
            /// </summary>
            public List<ToCCreator> Creators { get; set; }

            /// <summary>
            /// All show entries in the toc
            /// </summary>
            public List<ToCShow> Shows { get; set; }

            public ToCData()
            {
                Characters = new List<ToCEntry>();
                Creators = new List<ToCCreator>();
                Shows = new List<ToCShow>();
            }
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
            public List<ToCShow> CharacterShows { get; set; }

            /// <summary>
            /// The image url of this character
            /// </summary>
            public string ImageUrl { get; set; }

            /// <summary>
            /// The creator of this padoru
            /// </summary>
            public ToCCreator Creator { get; set; }

            /// <summary>
            /// The source url of this padoru entry
            /// </summary>
            /// <remarks>optional field, may be empty</remarks>
            public string SourceUrl { get; set; }

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

            public ToCEntry()
            {
                CharacterShows = new List<ToCShow>();
            }

            public override string ToString()
            {
                return $"ENTRY: {CharacterName}";
            }
        }

        /// <summary>
        /// a show a character of a TOC entry is in
        /// </summary>
        class ToCShow
        {
            /// <summary>
            /// The Url to this show's overview page
            /// </summary>
            public string PageUrl { get; set; }

            /// <summary>
            /// The MAL id of this show
            /// </summary>
            public long MalId { get; set; }

            /// <summary>
            /// The name of the show
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The mal page of the show
            /// </summary>
            public string MalUrl { get; set; }

            /// <summary>
            /// The list of all characters in this show that have a entry
            /// </summary>
            public List<ToCEntry> Characters { get; set; }

            public ToCShow()
            {
                Characters = new List<ToCEntry>();
            }

            public override string ToString()
            {
                return $"SHOW: {Name}";
            }
        }

        /// <summary>
        /// a creator that created a ToC entry
        /// </summary>
        class ToCCreator
        {
            /// <summary>
            /// The url of the creators page
            /// </summary>
            public string PageUrl { get; set; }

            /// <summary>
            /// The name of this padoru's creator
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The list of entrys this creator created
            /// </summary>
            public List<ToCEntry> Entries { get; set; }

            public ToCCreator()
            {
                Entries = new List<ToCEntry>();
            }

            public override string ToString()
            {
                return $"CREATOR: {Name}";
            }
        }
        #endregion
    }
}
