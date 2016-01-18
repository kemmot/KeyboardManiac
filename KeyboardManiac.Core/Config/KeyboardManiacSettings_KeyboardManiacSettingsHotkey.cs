namespace KeyboardManiac.Core.Config
{
    partial class KeyboardManiacSettingsHotkey
    {
        public override string ToString()
        {
            return string.Format(
                "{0}{1}{2}",
                modifier == HotKeyModifier.NONE ? string.Empty : modifier.ToString(),
                modifier == HotKeyModifier.NONE ? string.Empty : "+",
                key);
        }
    }
}
