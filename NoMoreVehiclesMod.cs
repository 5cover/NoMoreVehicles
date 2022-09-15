using System;
using System.ComponentModel;
using System.Linq;

using ColossalFramework.Plugins;
using ColossalFramework.UI;

using ICities;

namespace NoMoreVehicles
{
    public partial class NoMoreVehiclesMod : LoadingExtensionBase, IUserMod
    {
        public string Description => "Removes the More Vehicles mod from your saves.";

        public string Name => "No More Vehicles";

        public override void OnLevelLoaded(LoadMode mode)
        {
            RemoveVehicles();
            RevertPatch();

            UIView.library.ShowModal<ExceptionPanel>(nameof(ExceptionPanel)).SetMessage(Name, "The Vehicle Manager patch was reverted sucessfully. Save and exit the game, then unsubscribe from this mod and More Vehicles.", false);
        }
    }
}