using System.Linq;

using log4net;

namespace KeyboardManiac.Core.Config.Xsd
{
    partial class SettingCollection
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SettingCollection));

        public bool GetBoolean(string key, bool defaultValue)
        {
            bool result;

            Setting setting;
            if (TryGetSetting(key, out setting))
            {
                result = setting.GetAsBoolean();
            }
            else
            {
                result = defaultValue;
                Logger.WarnFormat("GUI setting not found: {0}, using default value of {1}", key, result);
            }

            return result;
        }

        public bool TryGetValue(string key, out string value)
        {
            Setting setting;
            bool found = TryGetSetting(key, out setting);
            value = found ? setting.value : string.Empty;
            return found;
        }

        public bool TryGetSetting(string key, out Setting setting)
        {
            setting = Setting.FirstOrDefault(s => s.key == key);
            return setting != null;
        }
    }
}
