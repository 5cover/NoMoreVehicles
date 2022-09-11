using System.Reflection;

using UnityEngine;

namespace NoMoreVehicles
{
    public partial class NoMoreVehiclesMod
    {
        /// <summary>Reverts the changes made to internal structure of the game's VehicleManager by the More Vehicle mod.</summary>
        private static void RevertPatch()
        {
            const int vanillaMaxVehicleCount = 16384;
            const int vanillaMaxParkedVehicleCount = 32768;

            ChangeArray16Size<Vehicle>(nameof(VehicleManager.m_vehicles), vanillaMaxVehicleCount);
            ChangeArray16Size<VehicleParked>(nameof(VehicleManager.m_parkedVehicles), vanillaMaxParkedVehicleCount);

            ChangeArraySize("m_renderBuffer", vanillaMaxVehicleCount >> 6);
            ChangeArraySize("m_renderBuffer2", vanillaMaxParkedVehicleCount >> 6);
            ChangeArraySize(nameof(VehicleManager.m_updatedParked), vanillaMaxParkedVehicleCount >> 6);

            void ChangeArray16Size<T>(string arrayFieldName, uint size) where T : struct
            {
                var arrayField = GetField(arrayFieldName);
                if (arrayField != null)
                {
                    var newArray = new Array16<T>(size);
                    arrayField.SetValue(VehicleManager.instance, newArray);
                    _ = newArray.CreateItem(out _);
                }
            }
            void ChangeArraySize(string arrayFieldName, uint size)
            {
                var arrayField = GetField(arrayFieldName);
                if (arrayField != null)
                {
                    var newArray = new ulong[size];
                    arrayField.SetValue(VehicleManager.instance, newArray);
                }
            }
            FieldInfo GetField(string fieldName)
            {
                var field = typeof(VehicleManager).GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                Debug.Assert(field != null, $"The 'No More Vehicles' mod failed to setup the Vehicle Manager: no field '{fieldName}' in VehicleManager");

                return field;
            }
        }
    }
}