using IoTHub.DeviceKeys.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHub.DeviceKeys.Business.Extensions
{
    public static class DeviceExtensions
    {
        public static Device MapFromIoTHub(this Microsoft.Azure.Devices.Device data)
        {
            Device result = new Device();

            if (data != null)
            {
                result.AuthenticationType = data.Authentication.Type.ToString();
                if (data.Authentication.Type == Microsoft.Azure.Devices.AuthenticationType.Sas)
                {
                    result.AuthenticationPrimaryKey = data.Authentication.SymmetricKey.PrimaryKey;
                    result.AuthenticationSecondaryKey = data.Authentication.SymmetricKey.SecondaryKey;
                }
                else
                {
                    result.AuthenticationPrimaryKey = data.Authentication.X509Thumbprint.PrimaryThumbprint;
                    result.AuthenticationSecondaryKey = data.Authentication.X509Thumbprint.SecondaryThumbprint;
                }

                result.CloudToDeviceMessageCount = data.CloudToDeviceMessageCount;
                result.ConnectionState = data.ConnectionState.ToString();
                result.ConnectionStateUpdatedTime = data.ConnectionStateUpdatedTime;
                result.ETag = data.ETag;
                result.GenerationId = data.GenerationId;
                result.Id = data.Id;
                result.LastActivityTime = data.LastActivityTime;
                result.Status = data.Status.ToString();
                result.StatusReason = data.StatusReason;
                result.StatusUpdatedTime = data.StatusUpdatedTime;
            }

            return result;
        }
    }
}
