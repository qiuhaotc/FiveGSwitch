using System;
using Android.App;
using Android.Content;
using Android.Service.QuickSettings;

namespace FiveGSwitch.Business
{
    [Service(Name = "com.qh.fivegswitch.FiveGSwitchService",
             Permission = Android.Manifest.Permission.BindQuickSettingsTile,
             Label = "@string/tile_name",
             Icon = "@drawable/five_g_switch_enable")]
    [IntentFilter(new[] { ActionQsTile })]
    public class FiveGSwitchService : TileService
    {
        bool capable;
        bool active;

        public override void OnClick()
        {
            base.OnClick();

            if (QsTile != null)
            {
                MIUISwitchProvider.Provider.Value.Toggle();

                active = MIUISwitchProvider.Provider.Value.IsEnabled;
                QsTile.State = active ? TileState.Active : TileState.Inactive;
                QsTile.UpdateTile();
            }
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(@base);
            capable = MIUISwitchProvider.Provider.Value.Capable;
        }

        public override void OnStartListening()
        {
            base.OnStartListening();

            if (capable)
            {
                if (QsTile != null)
                {
                    active = MIUISwitchProvider.Provider.Value.IsEnabled;
                    QsTile.State = active ? TileState.Active : TileState.Inactive;
                    QsTile.UpdateTile();
                }
            }
            else
            {
                if (QsTile != null)
                {
                    QsTile.State = TileState.Unavailable;
                    QsTile.UpdateTile();
                }
            }
        }
    }
}
