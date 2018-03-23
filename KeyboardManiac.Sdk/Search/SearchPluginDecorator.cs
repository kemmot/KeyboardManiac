using System;
using System.Collections.Generic;

namespace KeyboardManiac.Sdk.Search
{
    public abstract class SearchPluginDecorator : ISearchPluginDecorator
    {
        public event EventHandler<ItemEventArgs<List<SearchResultItem>>> ResultsFound;

        private readonly ISearchPlugin m_Target;

        protected SearchPluginDecorator(ISearchPlugin plugin)
        {
            if (plugin == null) throw new ArgumentNullException(nameof(plugin));

            m_Target = plugin;
            m_Target.ResultsFound += HandleResults;
        }

        public virtual string Name
        {
            get { return m_Target.Name; }
            set { m_Target.Name = value; }
        }

        protected ISearchPlugin Target => m_Target;

        public virtual CommandRequest CanHandleCommand(string commandText)
        {
            return m_Target.CanHandleCommand(commandText);
        }

        public virtual void Initialise(Dictionary<string, string> settings)
        {
            m_Target.Initialise(settings);
        }

        public virtual void RegisterAlias(string alias)
        {
            m_Target.RegisterAlias(alias);
        }

        public virtual void Search(CommandRequest parameters)
        {
            m_Target.Search(parameters);
        }

        public virtual void Stop()
        {
            m_Target.Stop();
        }

        protected virtual void HandleResults(object sender, ItemEventArgs<List<SearchResultItem>> e)
        {
            OnResultsFound(e);
        }

        /// <summary>
        /// Fires the <see cref="ResultsFound"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnResultsFound(ItemEventArgs<List<SearchResultItem>> e)
        {
            //foreach (SearchResultItem item in e.Item)
            //{
            //    item.Score += ScoreAdjustment;
            //}

            ResultsFound?.Invoke(this, e);
        }

        public override string ToString()
        {
            return string.Format("Decorator around {0}", m_Target);
        }
    }
}
