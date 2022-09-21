using System.Linq;

using ColossalFramework.Plugins;
using ColossalFramework.UI;

using ICities;

namespace NoMoreVehicles
{
    public partial class NoMoreVehiclesMod : LoadingExtensionBase, IUserMod
    {
        private const ulong _moreVehiclesModId = 1764208250;

        public string Description => "Removes the More Vehicles mod from your saves.";

        public string Name => "No More Vehicles";

        public override void OnLevelLoaded(LoadMode mode)
        {
            // Check if the More Vehicles mod is installed and enabled because we need it to initially load the save.
            if (IsModEnabled(_moreVehiclesModId))
            {
                RemoveVehicles();
                RevertPatch();
                ShowMessage("The Vehicle Manager patch was reverted sucessfully. Save and exit the game," +
                            "then unsubscribe from this mod and More Vehicles.", false);
            }
            else
            {
                ShowError("The More Vehicles patch could not be reverted, because More Vehicles is not enabled." +
                          "Make sure More Vehicles is enabled in the Content Manager, and that it is installed from the Workshop, not locally.");
            }
        }

        /// <summary>Returs whether the More Vehicles mod is installed and enabled.</summary>
        /// <remarks>This will return <see langword="false"/> if the mod is only installed locally.</remarks>
        private static bool IsModEnabled(ulong publishedFileID)
        {
            var moreVehicles = PluginManager.instance.GetPluginsInfo().FirstOrDefault(mod => mod.publishedFileID.AsUInt64 == publishedFileID);
            return moreVehicles != null && moreVehicles.isEnabled;
        }

        private void ShowError(string message) => ShowMessage(message, true);

        private void ShowMessage(string message, bool isError)
            => UIView.library.ShowModal<ExceptionPanel>(nameof(ExceptionPanel)).SetMessage(Name, message, isError);

        private void ShowWarning(string message) => ShowMessage(message, false);
    }
}