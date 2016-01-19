using log4net;

namespace KeyboardManiac.Core.Config.Xsd
{
    partial class KeyboardManiacSettingsGuiSettings
    {
        public class DefaultValues
        {
            public const bool MinimiseOnLosingFocus = false;
            public const bool MinimiseToSystemTray = false;
            public const bool UseClipboardForCommandText = false;
        }

        public class SettingNames
        {
            public const string MinimiseOnLosingFocus = "MinimiseOnLosingFocus";
            public const string MinimiseToSystemTray = "MinimiseToSystemTray";
            public const string UseClipboardForCommandText = "UseClipboardForCommandText";
            
        }

        public bool MinimiseOnLosingFocus
        {
            get
            {
                return GetBoolean(SettingNames.MinimiseOnLosingFocus, DefaultValues.MinimiseOnLosingFocus);
            }
        }

        public bool MinimiseToSystemTray
        {
            get
            {
                return GetBoolean(SettingNames.MinimiseToSystemTray, DefaultValues.MinimiseToSystemTray);
            }
        }

        public bool UseClipboardForCommandText
        {
            get
            {
                return GetBoolean(SettingNames.UseClipboardForCommandText, DefaultValues.UseClipboardForCommandText);
            }
        }
    }
}
