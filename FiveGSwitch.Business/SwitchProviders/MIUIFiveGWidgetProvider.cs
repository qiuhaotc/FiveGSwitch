using System;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace FiveGSwitch.Business
{
    [BroadcastReceiver(Label = "@string/widget_name")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/five_g_widget_provider")]
    public class MIUIFiveGWidgetProvider : AppWidgetProvider
    {
        public const string ACTION_WIDGET_SWITCH = "ACTION_WIDGET_SWITCH";

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            //Update Widget layout
            //Run when create widget or meet update time
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(MIUIFiveGWidgetProvider)).Name);
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));
        }

        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            // Check if the click is from the "ACTION_WIDGET_SWITCH" button
            if (ACTION_WIDGET_SWITCH.Equals(intent.Action))
            {
                if (MIUISwitchProvider.Provider.Value.Capable)
                {
                    MIUISwitchProvider.Provider.Value.Toggle();
                    var isEnabled = MIUISwitchProvider.Provider.Value.IsEnabled;
                    Toast.MakeText(context, $"5G {(isEnabled ? "Enabled" : "Disabled")}", ToastLength.Short).Show();

                    var widgetView = new RemoteViews(context.PackageName, Resource.Layout.five_g_widget);
                    SetImages(widgetView, isEnabled);

                    AppWidgetManager.GetInstance(context).UpdateAppWidget(new ComponentName(context, Java.Lang.Class.FromType(typeof(MIUIFiveGWidgetProvider)).Name), widgetView);
                }
                else
                {
                    Toast.MakeText(context, "5G not capable", ToastLength.Short).Show();
                }
            }
        }

        RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
        {
            //Build widget layout
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.five_g_widget);

            //Update Image
            SetImages(widgetView, MIUISwitchProvider.Provider.Value.Capable && MIUISwitchProvider.Provider.Value.IsEnabled);

            //Handle click event of button on Widget
            RegisterClicks(context, appWidgetIds, widgetView);

            return widgetView;
        }

        void SetImages(RemoteViews widgetView, bool enabled)
        {
            if (enabled)
            {
                widgetView.SetInt(Resource.Id.SwitchButton, "setBackgroundResource", Resource.Drawable.five_g_switch_enable);
            }
            else
            {
                widgetView.SetInt(Resource.Id.SwitchButton, "setBackgroundResource", Resource.Drawable.five_g_switch_disable);
            }
        }

        void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
        {
            var intent = new Intent(context, typeof(MIUIFiveGWidgetProvider));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            //SwitchButton
            widgetView.SetOnClickPendingIntent(Resource.Id.SwitchButton, GetPendingSelfIntent(context, ACTION_WIDGET_SWITCH));
        }

        PendingIntent GetPendingSelfIntent(Context context, string action)
        {
            var intent = new Intent(context, typeof(MIUIFiveGWidgetProvider));
            intent.SetAction(action);
            return PendingIntent.GetBroadcast(context, 0, intent, 0);
        }
    }
}
