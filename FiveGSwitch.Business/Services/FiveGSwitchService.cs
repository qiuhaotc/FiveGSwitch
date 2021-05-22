using Android.App;
using Android.Service.QuickSettings;

namespace FiveGSwitch.Business
{
    [Service(Name = "com.qh.fivegswitch.FiveGSwitchService",
             Permission = Android.Manifest.Permission.BindQuickSettingsTile,
             Label = "@string/tile_name",
             Icon = "@drawable/five_g_switch")]
    [IntentFilter(new[] { ActionQsTile })]
    public class FiveGSwitchService : TileService
    {
        bool active;

        public override void OnClick()
        {
            base.OnClick();

            active = !active;
            QsTile.State = active ? TileState.Active : TileState.Inactive;
            QsTile.UpdateTile();
        }
    }
}
