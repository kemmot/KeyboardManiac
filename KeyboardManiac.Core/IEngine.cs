using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using KeyboardManiac.Core.Config;
using KeyboardManiac.Sdk;
using KeyboardManiac.Sdk.Search;

namespace KeyboardManiac.Core
{
    /// <summary>
    /// The interface that must be implemented to provide
    /// Keyboard Maniac engine functionality.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Raised when a command execution is complete.
        /// </summary>
        event EventHandler<ItemEventArgs<CommandResult>> CommandComplete;
        /// <summary>
        /// Raised when search results are found.
        /// </summary>
        event EventHandler<ItemEventArgs<List<SearchResultItem>>> ResultsFound;
        /// <summary>
        /// Raised when a search is complete.
        /// </summary>
        event EventHandler SearchComplete;
        /// <summary>
        /// Raised when a search is started.
        /// </summary>
        event EventHandler SearchStarted;
        /// <summary>
        /// Raised when the engine status changes.
        /// </summary>
        event EventHandler<ItemEventArgs<string>> StatusChanged;

        GlobalHotKey HotKey { get; }
        /// <summary>
        /// Gets the settings in use by this engine.
        /// Loaded during the <see cref="Initialise"/> method.
        /// </summary>
        ApplicationDetails Settings { get; set; }

        void ClearPlugins();
        /// <summary>
        /// Gets the previous command text.
        /// </summary>
        /// <returns>The previous command text.</returns>
        string GetCommandHistoryPrevious();

        ReadOnlyCollection<IPlugin> GetPlugins();
        /// <summary>
        /// Initialises logging by loading the default log config file.
        /// </summary>
        void InitialiseLogging();
        /// <summary>
        /// Initialises logging by loading the specified log config file.
        /// </summary>
        /// <param name="configFilename">
        /// The name of the file to load the logging configuration from.
        /// </param>
        void InitialiseLogging(string configFilename);
        /// <summary>
        /// Registers a command plugin with the engine.
        /// </summary>
        /// <param name="plugin">The plugin to register.</param>
        void RegisterPlugin(ICommandPlugin plugin);
        /// <summary>
        /// Registers a search plugin with the engine.
        /// </summary>
        /// <param name="plugin">The plugin to register.</param>
        void RegisterPlugin(ISearchPluginBase plugin);
        /// <summary>
        /// Runs the specified command text.
        /// </summary>
        /// <param name="commandText">The command text to run.</param>
        void RunCommand(string commandText);
        /// <summary>
        /// Runs the specified item.
        /// </summary>
        /// <param name="item">The item to run.</param>
        void RunCommand(SearchResultItem item);
        /// <summary>
        /// Starts a search.
        /// </summary>
        /// <param name="searchText">The text to search for.</param>
        void Search(string searchText);
        void WndProc(int messageId);
    }
}
