namespace KeyboardManiac.Core.Config
{
    public class GuiDetails : SettingsCollection
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
            get { return GetBoolean(SettingNames.MinimiseOnLosingFocus, DefaultValues.MinimiseOnLosingFocus); }
            set { Set(SettingNames.MinimiseOnLosingFocus, value, SettingScopes.Gui); }
        }

        public bool MinimiseToSystemTray
        {
            get { return GetBoolean(SettingNames.MinimiseToSystemTray, DefaultValues.MinimiseToSystemTray); }
            set { Set(SettingNames.MinimiseToSystemTray, value, SettingScopes.Gui); }
        }

        public bool UseClipboardForCommandText
        {
            get { return GetBoolean(SettingNames.UseClipboardForCommandText, DefaultValues.UseClipboardForCommandText); }
            set { Set(SettingNames.UseClipboardForCommandText, value, SettingScopes.Gui); }
        }
    }
}
