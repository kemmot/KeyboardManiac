namespace KeyboardManiac.Core.Config.Xsd
{
    partial class Setting
    {
        public bool GetAsBoolean()
        {
            bool result;
            if (!bool.TryParse(value, out result))
            {
                throw new SettingsException(string.Format(
                    "Failed to parse setting as boolean {0} = {1}",
                    key,
                    value));
            }

            return result;
        }
    }
}
