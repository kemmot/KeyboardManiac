using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardManiac.Sdk
{
    public interface IEngineHost
    {
        IntPtr WindowHandle { get; }

        object Invoke(Delegate method, params object[] args);
        void ToggleShown();
    }
}
