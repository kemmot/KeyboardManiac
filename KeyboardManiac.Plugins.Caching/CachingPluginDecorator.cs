using System.Collections.Generic;

using KeyboardManiac.Sdk;
using KeyboardManiac.Sdk.Search;

using log4net;

namespace KeyboardManiac.Plugins.Caching
{
    public class CachingPluginDecorator : SearchPluginDecorator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CachingPluginDecorator));

        private string m_PreviousCommandText;
        private readonly List<SearchResultItem> m_CachedResults = new List<SearchResultItem>();

        public CachingPluginDecorator(ISearchPlugin target)
            : base(target)
        {
        }

        public override void Search(CommandRequest parameters)
        {
            bool canUseCache = !string.IsNullOrEmpty(m_PreviousCommandText) && parameters.CommandText.Contains(m_PreviousCommandText);
            if (canUseCache)
            {
                Logger.DebugFormat("Can use cache");
                string upperSearchText = parameters.AliasCleansedCommandText.ToUpperInvariant();
                var results = new List<SearchResultItem>();
                lock (m_CachedResults)
                {
                    foreach (var item in m_CachedResults)
                    {
                        if (item.Name.ToUpperInvariant().Contains(upperSearchText))
                        {
                            results.Add(item);
                        }
                    }
                }

                OnResultsFound(new ItemEventArgs<List<SearchResultItem>>(results));
            }
            else
            {
                Logger.DebugFormat("Cannot use cache");
                base.Search(parameters);
            }

            m_PreviousCommandText = parameters.CommandText;
        }

        protected override void HandleResults(object sender, ItemEventArgs<List<SearchResultItem>> e)
        {
            lock (m_CachedResults)
            {
                m_CachedResults.Clear();
                m_CachedResults.AddRange(e.Item);
            }

            base.HandleResults(sender, e);
        }

        public override string ToString()
        {
            return $"Cache around {Target}";
        }
    }
}
