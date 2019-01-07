using System;
using System.Collections.Generic;
using System.IO;

using KeyboardManiac.Sdk;
using KeyboardManiac.Sdk.Search;

using log4net;

namespace KeyboardManiac.Plugins.Favorites
{
    public class FavoritesSearchPlugin : SearchPluginBase, ICommandPlugin
    {
        private const string ResultType_Favorite = "Favorite";

        private readonly static ILog Logger = LogManager.GetLogger(typeof(FavoritesSearchPlugin));

        private readonly List<SearchResultItem> m_Favorites = new List<SearchResultItem>();

        public FavoritesSearchPlugin(IPluginHost host)
            : base(host)
        {
        }

        [Setting]
        public int MaxFavoriteCount { get; set; }

        protected override void DoSearch(CommandRequest parameters)
        {
            List<SearchResultItem> results = new List<SearchResultItem>();
            foreach (SearchResultItem favorite in m_Favorites)
            {
                if (favorite.Name.ToUpperInvariant().Contains(parameters.CommandText.ToUpperInvariant()))
                {
                    results.Add(favorite);
                    Logger.InfoFormat("Match found: {0}", favorite);
                }
            }

            if (results.Count > 0)
            {
                OnResultsFound(new ItemEventArgs<List<SearchResultItem>>(results));
            }
        }

        /// <summary>
        /// Gives the plugin an opportunity to handle the command text.
        /// </summary>
        /// <param name="commandText">The command text to assess.</param>
        /// <returns>The command request.</returns>
        override public CommandRequest CanHandleCommand(string commandText)
        {
            if (File.Exists(commandText))
            {
                var item = new SearchResultItem(Path.GetFileName(commandText), ResultType_Favorite, commandText);
                m_Favorites.Insert(0, item);
                Logger.InfoFormat("Favorite stored: {0}", commandText);
                RemoveDuplicatesFavorites(item);
                RemoveExcessFavorites();
            }

            CommandRequest result = new CommandRequest();
            result.AliasCleansedCommandText = commandText;
            result.CanHandleCommand = false;
            result.CommandText = commandText;
            result.MatchingAlias = string.Empty;
            return result;
        }

        private void RemoveDuplicatesFavorites(SearchResultItem item)
        {
            for (int index = m_Favorites.Count - 1; index >= 0; index--)
            {
                SearchResultItem favorite = m_Favorites[index];
                if (favorite.Path == item.Path)
                {
                    m_Favorites.RemoveAt(index);
                }
            }
        }

        private void RemoveExcessFavorites()
        {
            while (m_Favorites.Count > MaxFavoriteCount)
            {
                m_Favorites.RemoveAt(m_Favorites.Count - 1);
            }
        }

        public CommandResult Execute(CommandRequest commandRequest)
        {
            throw new NotSupportedException("Favorites plugin does not support execution");
        }
    }
}
