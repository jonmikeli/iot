﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHub.DeviceKeys.Business.Entities
{
    public class Device
    {
        public string Id { get; set; }

        public string GenerationId { get; set; }

        public string ETag { get; set; }

        public string ConnectionState { get; set; }

        public string Status { get; set; }

        public string StatusReason { get; set; }

        public DateTime ConnectionStateUpdatedTime { get; set; }

        public DateTime StatusUpdatedTime { get; set; }

        public DateTime LastActivityTime { get; set; }

        public int CloudToDeviceMessageCount { get; set; }

        public string AuthenticationType { get; set; }
        public string AuthenticationPrimaryKey { get; set; }
        public string AuthenticationSecondaryKey { get; set; }


    }
}
