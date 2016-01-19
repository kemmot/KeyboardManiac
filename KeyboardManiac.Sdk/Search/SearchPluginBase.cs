using System;
using System.Collections.Generic;
using System.Threading;

using log4net;

namespace KeyboardManiac.Sdk.Search
{
    /// <summary>
    /// A base class implementation of <see cref="ISearchPlugin"/> providing
    /// functionality common to all search plugins.
    /// </summary>
    abstract public class SearchPluginBase : PluginBase, ISearchPlugin
    {
        /// <summary>
        /// Fired when search results are found.
        /// </summary>
        public event EventHandler<ItemEventArgs<List<SearchResultItem>>> ResultsFound;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(SearchPluginBase));
        private readonly ManualResetEvent m_StopEvent = new ManualResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPluginBase"/> class.
        /// </summary>
        /// <param name="host">The host for this plugin.</param>
        public SearchPluginBase(IPluginHost host)
            : base(host)
        {
        }

        /// <summary>
        /// Gets whether this plugin's search thread has been signalled to stop.
        /// </summary>
        protected bool IsStopping
        {
            get
            {
                return m_StopEvent.WaitOne(0);
            }
        }
        /// <summary>
        /// Gets or sets the score adjustment to make for matches from this plugin.
        /// </summary>
        public int ScoreAdjustment { get; set; }
        /// <summary>
        /// Gets or sets whether the plugin should use short names in search results.
        /// </summary>
        public bool UseShortNames { get; set; }

        /// <summary>
        /// Fires the <see cref="ResultsFound"/> event.
        /// </summary>
        /// <param name="e"></param>
        virtual protected void OnResultsFound(ItemEventArgs<List<SearchResultItem>> e)
        {
            if (ResultsFound != null)
            {
                foreach (SearchResultItem item in e.Item)
                {
                    item.Score += ScoreAdjustment;
                }

                ResultsFound(this, e);
            }
        }
        /// <summary>
        /// Starts a search.
        /// </summary>
        /// <param name="parameters">The search parameters.</param>
        public void Search(CommandRequest parameters)
        {
            m_StopEvent.Reset();
            DoSearch(parameters);
        }
        /// <summary>
        /// Starts a search.
        /// </summary>
        /// <param name="parameters">The search parameters.</param>
        abstract protected void DoSearch(CommandRequest parameters);
        /// <summary>
        /// Stops any outstanding search.
        /// </summary>
        public void Stop()
        {
            m_StopEvent.Set();
            DoStop();
        }
        virtual protected void DoStop()
        {
        }
    }
}
