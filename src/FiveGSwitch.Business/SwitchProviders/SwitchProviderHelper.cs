using System;
using Android.Util;

namespace FiveGSwitch.Business
{
    public static class SwitchProviderHelper
    {
        static Lazy<ISwitchProvider> provider;

        public static Lazy<ISwitchProvider> Provider
        {
            get
            {
                if (provider == null)
                {
                    Log.Debug(nameof(SwitchProviderHelper), "Brand: " + Android.OS.Build.Brand);

                    provider = new Lazy<ISwitchProvider>(() => new MIUISwitchProvider());
                }

                return provider;
            }
        }
    }
}
