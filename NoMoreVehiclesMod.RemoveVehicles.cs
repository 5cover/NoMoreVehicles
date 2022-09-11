using System;

using ColossalFramework;

using UnityEngine;

namespace NoMoreVehicles
{
    public partial class NoMoreVehiclesMod
    {
        // Code adapted from Loading Screen Mod (https://github.com/algernon-A/LSM-Revisited/blob/master/Code/Legacy/Safenets.cs)

        private delegate void ARef<T>(uint i, ref T item);

        private static int ForObjects<T>(T[] buffer, ARef<T> action, Predicate<T> shouldIterate)
        {
            int num = 0;
            for (uint num2 = 1u; num2 < buffer.Length; num2++)
            {
                if (shouldIterate(buffer[num2]))
                {
                    try
                    {
                        action(num2, ref buffer[num2]);
                        num++;
                    }
                    catch (Exception exception)
                    {
                        // Swallow exception.
                        Debug.LogException(exception);
                    }
                }
            }
            return num;
        }

        /// <summary>Removes all vehicle agents.</summary>
        private static void RemoveVehicles()
        {
            int num = ForObjects(Singleton<VehicleManager>.instance.m_vehicles.m_buffer,
                                 (uint i, ref Vehicle d) => Singleton<VehicleManager>.instance.ReleaseVehicle((ushort)i),
                                 v => v.m_flags != 0);
            num = ForObjects(Singleton<VehicleManager>.instance.m_parkedVehicles.m_buffer,
                                (uint i, ref VehicleParked d) => Singleton<VehicleManager>.instance.ReleaseParkedVehicle((ushort)i),
                                pv => pv.m_flags != 0);

            ushort[] vehicleGrid = Singleton<VehicleManager>.instance.m_vehicleGrid;
            ushort[] vehicleGrid2 = Singleton<VehicleManager>.instance.m_vehicleGrid2;
            ushort[] parkedGrid = Singleton<VehicleManager>.instance.m_parkedGrid;

            for (int j = 0; j < vehicleGrid.Length; j++)
            {
                vehicleGrid[j] = 0;
            }
            for (int k = 0; k < vehicleGrid2.Length; k++)
            {
                vehicleGrid2[k] = 0;
            }
            for (int l = 0; l < parkedGrid.Length; l++)
            {
                parkedGrid[l] = 0;
            }

            _ = ForObjects(Singleton<CitizenManager>.instance.m_citizens.m_buffer, (uint i, ref Citizen d) =>
                            {
                                d.SetVehicle(i, 0, 0u);
                                d.SetParkedVehicle(i, 0);
                            },
                            c => (c.m_flags & Citizen.Flags.Created) != 0);

            _ = ForObjects(Singleton<BuildingManager>.instance.m_buildings.m_buffer, (uint i, ref Building d) =>
                            {
                                d.m_ownVehicles = 0;
                                d.m_guestVehicles = 0;
                            },
                            b => b.m_flags != 0);

            _ = ForObjects(Singleton<TransportManager>.instance.m_lines.m_buffer,
                            (uint i, ref TransportLine d) => d.m_vehicles = 0,
                            l => l.m_flags != 0);
        }
    }
}