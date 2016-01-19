using System;
using System.Collections.Generic;
using System.Diagnostics;

using KeyboardManiac.Sdk.Search;

using log4net;

using KeyboardManiac.Sdk;

namespace KeyboardManiac.Core
{
    public class PluginSearchThread : ThreadBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PluginSearchThread));
        private readonly ISearchPlugin m_Plugin;
        private readonly CommandRequest m_Request;

        public PluginSearchThread(ISearchPlugin plugin, CommandRequest request)
        {
            m_Plugin = plugin;
            m_Request = request;
            IsBackground = true;
        }

        override protected void InnerStart()
        {
            using (ThreadContext.Stacks["NDC"].Push(m_Plugin.ToString()))
            {
                Logger.DebugFormat("Starting search");
                Stopwatch stopwatch = Stopwatch.StartNew();
                m_Plugin.Search(m_Request);
                stopwatch.Stop();
                Logger.DebugFormat("Search complete: " + stopwatch.Elapsed);
            }
        }

        protected override void DoPreJoin()
        {
            m_Plugin.Stop();
        }
    }
}
