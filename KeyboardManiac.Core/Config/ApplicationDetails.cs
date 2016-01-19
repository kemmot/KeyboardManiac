using System.Collections.Generic;
using System.Linq;

namespace KeyboardManiac.Core.Config
{
    public class ApplicationDetails
    {
        private readonly GlobalDetails m_Global = new GlobalDetails();
        private readonly GuiDetails m_Gui = new GuiDetails();
        private readonly List<HotKeyDetails> m_HotKeys = new List<HotKeyDetails>();
        private readonly List<PluginDetails> m_Plugins = new List<PluginDetails>();
        private readonly List<PluginTypeDetails> m_PluginTypes = new List<PluginTypeDetails>();

        public GlobalDetails Global { get { return m_Global; } }
        public GuiDetails Gui { get { return m_Gui; } }
        public IList<HotKeyDetails> HotKeys { get { return m_HotKeys; } }
        public IList<PluginDetails> Plugins { get { return m_Plugins; } }
        public IList<PluginTypeDetails> PluginTypes { get { return m_PluginTypes; } }

        public bool TryGetPluginType(string pluginTypeId, out PluginTypeDetails pluginType)
        {
            pluginType = PluginTypes.FirstOrDefault(pt => pt.Id == pluginTypeId);
            return pluginType != null;
        }
    }

    public class ScoreModifier
    {
        public int ScoreAdjustment { get; set; }
    }

    public enum ScoreModifierType
    {
        FileNameMatch
    }
}
