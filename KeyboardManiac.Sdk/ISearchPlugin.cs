using System;
using System.Collections.Generic;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// The interface that must be implemented to provide search plugins to the keyboard maniac engine.
    /// </summary>
    public interface ISearchPlugin : IPlugin
    {
        /// <summary>
        /// Fired when search results are found.
        /// </summary>
        event EventHandler<ItemEventArgs<List<SearchResultItem>>> ResultsFound;

        /// <summary>
        /// Starts a search.
        /// </summary>
        /// <param name="parameters">The search parameters.</param>
        void Search(CommandRequest parameters);
        /// <summary>
        /// Stops any outstanding search.
        /// </summary>
        void Stop();
    }
}
