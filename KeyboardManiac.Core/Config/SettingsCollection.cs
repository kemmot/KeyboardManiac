using System.Collections.Generic;
using System.Linq;

namespace KeyboardManiac.Core.Config
{
    public class SettingsCollection
    {
        private readonly List<Setting> m_Settings = new List<Setting>();

        public List<Setting> Settings { get { return m_Settings; } }

        public bool GetBoolean(string name, bool defaultValue)
        {
            bool result;
            if (!TryGetValue(name, out result))
            {
                result = defaultValue;
            }

            return result;
        }

        public void Set(string name, bool value, string scope)
        {
            Set(name, value.ToString(), scope);
        }

        public void Set(string name, string value, string scope)
        {
            Setting setting;
            if (!TryGetSetting(name, out setting))
            {
                setting = new Setting() { Name = name, Scope = scope };
                Settings.Add(setting);
            }

            setting.Value = value;
        }

        public bool TryGetValue(string name, out bool value)
        {
            string stringValue;
            bool result = TryGetValue(name, out stringValue);
            if (result)
            {
                result = bool.TryParse(stringValue, out value);
            }
            else
            {
                value = false;
            }

            return result;
        }

        public bool TryGetValue(string name, out string value)
        {
            Setting setting;
            bool result = TryGetSetting(name, out setting);
            value = result ? setting.Value : string.Empty;
            return result;
        }

        public bool TryGetSetting(string name, out Setting setting)
        {
            setting = Settings.FirstOrDefault((s) => s.Name == name);
            return setting != null;
        }
    }
}
