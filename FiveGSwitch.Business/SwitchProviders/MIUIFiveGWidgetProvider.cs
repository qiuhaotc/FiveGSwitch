using Android.App;
using Android.Appwidget;
using Android.Content;

namespace FiveGSwitch.Business
{
    [BroadcastReceiver(Label = "@string/widget_name")]
	[IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
	[MetaData("android.appwidget.provider", Resource = "@layout/five_g_widget")]
	public class MIUIFiveGWidgetProvider : AppWidgetProvider
	{
	}
}
