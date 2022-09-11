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

        /// <summary>Called on game startup or when this mod is enabled in the Content Manager.</summary>
        public void OnEnabled()
        {
            // Look for the More Vehicles mod in the list of installed mods.
            ulong moreVehiclesModId = 1764208250;
            var moreVehiclesMod = PluginManager.instance.GetPluginsInfo().FirstOrDefault(mod => mod.publishedFileID.AsUInt64 == moreVehiclesModId);

            // If the mod is not installed
            if (moreVehiclesMod is null)
            {
                ShowError("This mod will not work because the More Vehicles mod is not installed. Subscribe to the More Vehicles mod.");
            }
            // If the mod is disabled
            else if (!moreVehiclesMod.isEnabled)
            {
                ShowError("This mod will not work because the More Vehicles mod is disabled. Enable the More Vehicles mod.");
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame)
            {
                return;
            }

            RemoveVehicles();
            RevertPatch();

            ShowWarning("The Vehicle Manager patch was reverted sucessfully. Save and exit the game, then unsubscribe from this mod and More Vehicles.");
        }

        private void PerformShow(string message, bool error)
        {
            if (UIView.library != null)
            {
                UIView.library.ShowModal<ExceptionPanel>(nameof(ExceptionPanel)).SetMessage(Name, message, error);
            }
            else
            {
                // If the UI library is not available for some reason, fallback on throwing an exception.
                string exMessage = $"{Name}: \n{message}";
                if (error)
                {
                    throw new Exception(exMessage);
                }
                else
                {
                    throw new WarningException(exMessage);
                }
            }
        }

        private void ShowError(string message) => PerformShow(message, true);

        private void ShowWarning(string message) => PerformShow(message, false);
    }
}