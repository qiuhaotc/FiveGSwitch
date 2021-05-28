using System;
using Android.App;
using Android.Util;
using Android.Widget;

namespace FiveGSwitch.Business
{
    public class MIUISwitchProvider : ISwitchProvider
    {
        static Lazy<Java.Lang.Class> telephoneManager =
            new Lazy<Java.Lang.Class>(() =>
                Java.Lang.Class.ForName("miui.telephony.TelephonyManager")
            );

        Lazy<Java.Lang.Reflect.Method> getInstanceMethod = new Lazy<Java.Lang.Reflect.Method>(() => telephoneManager.Value.GetMethod("getDefault"));
        Lazy<Java.Lang.Reflect.Method> getIsFiveGCapableMethod = new Lazy<Java.Lang.Reflect.Method>(() => telephoneManager.Value.GetMethod("isFiveGCapable"));
        Lazy<Java.Lang.Reflect.Method> getIsUserFiveGEnabled = new Lazy<Java.Lang.Reflect.Method>(() => telephoneManager.Value.GetMethod("isUserFiveGEnabled"));
        Lazy<Java.Lang.Reflect.Method> setUserFiveGEnabled = new Lazy<Java.Lang.Reflect.Method>(() => telephoneManager.Value.GetMethod("setUserFiveGEnabled", Java.Lang.Boolean.Type));

        public bool IsEnabled
        {
            get
            {
                return IsEnabledCore(GetManagerInstance());
            }
        }

        public bool Capable
        {
            get
            {
                try
                {
                    var manager = GetManagerInstance();
                    return (getIsFiveGCapableMethod.Value.Invoke(manager) as Java.Lang.Boolean).BooleanValue();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, $"5G not capable", ToastLength.Short).Show();
                    Log.Warn(nameof(MIUISwitchProvider), "Error occur for capable check: " + ex);
                    return false;
                }
            }
        }

        public void Toggle()
        {
            try
            {
                var manager = GetManagerInstance();
                setUserFiveGEnabled.Value.Invoke(manager, !IsEnabledCore(manager));
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, $"Error Occur: {ex}", ToastLength.Short).Show();
            }
        }

        bool IsEnabledCore(Java.Lang.Object manager)
        {
            try
            {
                return (getIsUserFiveGEnabled.Value.Invoke(manager) as Java.Lang.Boolean).BooleanValue();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, $"Error Occur: {ex}", ToastLength.Short).Show();

                return false;
            }
        }

        Java.Lang.Object GetManagerInstance() => getInstanceMethod.Value.Invoke(telephoneManager.Value);
    }
}
