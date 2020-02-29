using IoTHub.DeviceKeys.Business.Extensions;
using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace IoTHub.DeviceKeys.Business
{
    public static class ProvisioningHelper
    {
        private static string connectionString = ConfigurationManager.AppSettings["IoTHubConnectionString"];
        static RegistryManager registryManager;

        #region Public methods
        public static async Task<string> GetCertificateFromProvisionnedDeviceAsync(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
                throw new ArgumentNullException("deviceId");

            if (registryManager == null)
                registryManager = RegistryManager.CreateFromConnectionString(connectionString);


            Device device = await registryManager.GetDeviceAsync(deviceId);
            return device?.Authentication.SymmetricKey.PrimaryKey;
        }

        public static async Task<Business.Entities.Device> GetDeviceAsync(string deviceId)
        {
            var data = await GetIoTHubDeviceAsync(deviceId);

            if (data != null)
                return data.MapFromIoTHub();
            else return null;
        }

        public static async Task<Business.Entities.Device> AddDeviceAsync(string deviceId)
        {
            Device iotDevice = await AddDeviceToIoTHubAsync(deviceId);

            if (iotDevice != null)
                return iotDevice.MapFromIoTHub();
            else
                return null;
        }
        #endregion

        #region Private methods
        private static async Task<Device> GetIoTHubDeviceAsync(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
                throw new ArgumentNullException("deviceId");

            if (registryManager == null)
                registryManager = RegistryManager.CreateFromConnectionString(connectionString);


            return await registryManager.GetDeviceAsync(deviceId);
        }

        private static async Task<Device> AddDeviceToIoTHubAsync(string deviceId)
        {
            Device device;
            try
            {
                if (registryManager == null)
                    registryManager = RegistryManager.CreateFromConnectionString(connectionString);

                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            return device;
        }
        #endregion
    }
}
