using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using KeyboardManiac.Core.Config;
using KeyboardManiac.Sdk;

using log4net;

namespace KeyboardManiac.Core
{
    public class GlobalHotKey : DisposableBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GlobalHotKey));

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private readonly IntPtr m_Handle;
        private readonly IEngineHost m_Host;
        private readonly List<HotKeyDetails> m_HotKeys = new List<HotKeyDetails>();


        public GlobalHotKey(IEngineHost host)
        {
            m_Host = host;
            m_Handle = m_Host.WindowHandle;
        }


        public bool IsRegistered { get; private set; }


        public void Add(HotKeyDetails hotKey)
        {
            if (IsRegistered)
            {
                throw new InvalidOperationException("Cannot alter hotkeys while existing ones are registered");
            }

            m_HotKeys.Add(hotKey);
        }

        public void Clear()
        {
            if (IsRegistered)
            {
                throw new InvalidOperationException("Cannot alter hotkeys while existing ones are registered");
            }
            m_HotKeys.Clear();
        }

        public void Register()
        {
            if (IsRegistered)
            {
                throw new InvalidOperationException("HotKeys are already registered");
            }

            ThreadStart del = delegate
            {
                m_HotKeys.ForEach(Register);
                IsRegistered = true;
            };
            m_Host.Invoke(del);
        }

        private void Register(HotKeyDetails hotKey)
        {
            int id = GetId(hotKey);
            bool succeeded = RegisterHotKey(m_Handle, id, Constants.GetCode(hotKey.Modifier.ToString()), (int)Enum.Parse(typeof(Keys), hotKey.Key.ToString()));
            if (!succeeded)
            {
                throw new Exception(string.Format(
                    "Failed to register global hotkey: {0}",
                    hotKey));
            }
        }
        
        protected override void DisposeUnmanagedResources()
        {
            base.DisposeManagedResources();

            Unregister();
        }

        public void Unregister()
        {
            if (!IsRegistered)
            {
                Logger.WarnFormat("Attempt to unregister hotkeys when they are not registered");
            }

            ThreadStart del = delegate
            {
                m_HotKeys.ForEach(Unregister);
                IsRegistered = false;
            };
            m_Host.Invoke(del);
        }

        private void Unregister(HotKeyDetails hotKey)
        {
            int id = GetId(hotKey);
            bool success = UnregisterHotKey(m_Handle, id);
            if (!success)
            {
                throw new Exception(string.Format(
                    "Failed to unregister global hotkey: {0}",
                    hotKey));
            }
        }

        private int GetId(HotKeyDetails hotKey)
        {
            return Constants.GetCode(hotKey.Modifier.ToString()) ^ (int)Enum.Parse(typeof(Keys), hotKey.Key.ToString()) ^ m_Handle.ToInt32();
        }


        public static class Constants
        {
            //modifiers
            public const int NOMOD = 0x0000;
            public const int ALT = 0x0001;
            public const int CTRL = 0x0002;
            public const int SHIFT = 0x0004;
            public const int WIN = 0x0008;
            //windows message id for hotkey
            public const int WM_HOTKEY_MSG_ID = 0x0312;

            public static int GetCode(string value)
            {
                int code;
                switch (value)
                {
                    case "ALT": code = ALT; break;
                    case "CTRL": code = CTRL; break;
                    case "SHIFT": code = SHIFT; break;
                    case "WIN": code = WIN; break;
                    default: code = NOMOD; break;
                }
                return code;
            }

            public static string ToString(int value)
            {
                string description;
                switch (value)
                {
                    case NOMOD: description = string.Empty; break;
                    case ALT: description = "ALT"; break;
                    case CTRL: description = "CTRL"; break;
                    case SHIFT: description = "SHIFT"; break;
                    case WIN: description = "WIN"; break;
                    default:
                        description = string.Format("Unknown modifier: {0}", value);
                        break;
                }
                return description;
            }
        }
    }

    //public class HotKey
    //{
    //    private readonly Keys m_Key;
    //    private readonly int m_Modifier;

    //    public HotKey(Keys key, int modifier)
    //    {
    //        m_Key = key;
    //        m_Modifier = modifier;
    //    }

    //    public Keys Key { get { return m_Key; } }
    //    public int Modifier { get { return m_Modifier; } }

    //    public override string ToString()
    //    {
    //        string modifier = GlobalHotKey.Constants.ToString(m_Modifier);
    //        return string.Format(
    //            "{0}{1}{2}",
    //            modifier,
    //            string.IsNullOrEmpty(modifier) ? string.Empty : "+",
    //            m_Key);
    //    }
    //}
}
