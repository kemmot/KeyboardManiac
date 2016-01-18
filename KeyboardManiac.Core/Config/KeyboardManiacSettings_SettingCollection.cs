namespace KeyboardManiac.Core.Config
{
    partial class SettingCollection
    {
        public bool TryGetValue(string key, out string value)
        {
            bool found = false;
            value = string.Empty;

            foreach (SettingCollectionSetting setting in Setting)
            {
                if (setting.key == key)
                {
                    value = setting.value;
                    found = true;
                    break;
                }
            }

            return found;
        }
    }
}
