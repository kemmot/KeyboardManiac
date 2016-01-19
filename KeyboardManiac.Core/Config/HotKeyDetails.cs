using System.Windows.Forms;

namespace KeyboardManiac.Core.Config
{
    public class HotKeyDetails
    {
        public HotKeyDetails(Keys key, int modifier)
        {
            Key = key;
            Modifier = modifier;
        }

        public Keys Key { get; set; }
        public int Modifier { get; set; }
    }
}
