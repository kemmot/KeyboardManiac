using System.Collections.Generic;
using System.Diagnostics;

using KeyboardManiac.Sdk;
using KeyboardManiac.Sdk.Search;

using log4net;

namespace KeyboardManiac.Core
{
    public class SearchThread : ThreadBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SearchThread));
        private readonly List<IPlugin> m_Plugins;
        private readonly CommandRequest m_Request;
        private List<PluginSearchThread> m_SearchPluginThreads;

        public SearchThread(List<IPlugin> plugins, CommandRequest request)
        {
            m_Plugins = plugins;
            m_Request = request;
            IsBackground = true;
        }

        protected override void InnerStart()
        {
            var pluginsToUse = FindSearchPlugins(m_Request);
            if (pluginsToUse.Count > 0)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                //SetStatus("Searching...");
                StartPluginSearch(pluginsToUse);

                foreach (PluginSearchThread thread in m_SearchPluginThreads)
                {
                    thread.Join();
                    //SetStatus("Search plugin complete: {0}", thread.Name);
                }

                stopwatch.Stop();
                //SetStatus("Search complete ({0})", stopwatch.Elapsed);
            }
        }

        private List<KeyValuePair<ISearchPluginBase, CommandRequest>> FindSearchPlugins(CommandRequest parameters)
        {
            var compatibleSearchPlugins = new List<KeyValuePair<ISearchPluginBase, CommandRequest>>();
            var matchingSearchPlugins = new List<KeyValuePair<ISearchPluginBase, CommandRequest>>();
            int compatiblePluginCount = 0;
            int matchingPluginCount = 0;

            // find which plugins are the best match
            foreach (IPlugin plugin in m_Plugins)
            {
                CommandRequest request = plugin.CanHandleCommand(parameters.CommandText);
                if (request.CanHandleCommand)
                {
                    if (string.IsNullOrEmpty(request.MatchingAlias))
                    {
                        compatiblePluginCount++;
                        var searchPlugin = plugin as ISearchPluginBase;
                        if (searchPlugin != null)
                        {
                            compatibleSearchPlugins.Add(new KeyValuePair<ISearchPluginBase, CommandRequest>(searchPlugin, request));
                        }
                    }
                    else
                    {
                        matchingPluginCount++;
                        var searchPlugin = plugin as ISearchPluginBase;
                        if (searchPlugin != null)
                        {
                            matchingSearchPlugins.Add(new KeyValuePair<ISearchPluginBase, CommandRequest>(searchPlugin, request));
                        }
                    }
                }
            }

            // decide which plugins to use, matching are better than compatible
            List<KeyValuePair<ISearchPluginBase, CommandRequest>> pluginsToUse;
            if (matchingPluginCount > 0)
            {
                pluginsToUse = matchingSearchPlugins;
            }
            else
            {
                pluginsToUse = compatibleSearchPlugins;
            }
            return pluginsToUse;
        }

        private void StartPluginSearch(List<KeyValuePair<ISearchPluginBase, CommandRequest>> pluginsToUse)
        {
            m_SearchPluginThreads = new List<PluginSearchThread>();
            for (int pluginIndex = 0; pluginIndex < pluginsToUse.Count; pluginIndex++)
            {
                KeyValuePair<ISearchPluginBase, CommandRequest> pluginRequest = pluginsToUse[pluginIndex];
                string context = string.Format(
                    "{0}/{1}: {2}",
                    pluginIndex + 1,
                    pluginsToUse.Count,
                    pluginRequest.Key);
                Logger.Debug("Starting plugin search " + context);
                PluginSearchThread pluginThread = new PluginSearchThread(pluginRequest.Key, pluginRequest.Value);
                pluginThread.Name = string.Format("PluginSearchThread-{0}", + pluginIndex + 1);
                pluginThread.Start();
                m_SearchPluginThreads.Add(pluginThread);
            }
        }

        protected override void DoPreJoin()
        {
            if (IsAlive)
            {
                foreach (IPlugin plugin in m_Plugins)
                {
                    var searchPlugin = plugin as ISearchPluginBase;
                    if (searchPlugin != null)
                    {
                        searchPlugin.Stop();
                    }
                }
            }
        }
    }
}
