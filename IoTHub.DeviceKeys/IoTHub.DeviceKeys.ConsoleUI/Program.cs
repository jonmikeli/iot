using IoTHub.DeviceKeys.Business;
using IoTHub.DeviceKeys.Business.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHub.DeviceKeys.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Getting device certificate...");

            Parameters parameters = null;

            try
            {
                ValidateInput(args);

                parameters = ExtractParameters(args);

                if (parameters != null && !string.IsNullOrEmpty(parameters.DeviceId))
                {
                    string data = string.Empty;
                    if (parameters.CreateDevice)
                    {
                        Device device = await ProvisioningHelper.AddDeviceAsync(parameters.DeviceId);
                        if (device != null)
                            data = device.AuthenticationPrimaryKey;
                    }

                    //in case no device creation has ben requested
                    if (string.IsNullOrEmpty(data))
                        data = await GetCertificateASync(parameters.DeviceId);

                    if (string.IsNullOrEmpty(parameters.CertificateDestinationPath))
                    {
                        //display the response
                        Console.WriteLine($"Device: {parameters.DeviceId}");
                        Console.WriteLine($"Certificate: {data}");
                        Console.ReadKey();
                    }
                    else
                    {
                        //store the response in a file
                        if (!string.IsNullOrEmpty(data))
                            File.WriteAllText($"{parameters.CertificateDestinationPath}\\{parameters.DeviceId}.txt", data);
                        else
                            throw new Exception($"No key for device {parameters.DeviceId}");
                    }
                }

            }
            catch (Exception ex)
            {
                if (parameters != null && !string.IsNullOrEmpty(parameters.CertificateDestinationPath))
                {
                    File.AppendAllText($"{parameters.CertificateDestinationPath}\\logs.txt", $"{Environment.NewLine}{DateTime.Now.ToString()}::ERROR::{ex.Message}");
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            }


        }

        static async Task<string> GetCertificateASync(string deviceId)
        {
            return await ProvisioningHelper.GetCertificateFromProvisionnedDeviceAsync(deviceId);
        }

        static void ValidateInput(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args", "");
        }

        static Parameters ExtractParameters(string[] args)
        {
            Parameters result = null;
            if (args != null && args.Length <= 5)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-deviceId" && (i + 1) < args.Length)
                    {
                        if (result == null)
                            result = new Parameters();

                        result.DeviceId = args[i + 1];
                    }
                    else if (args[i] == "-d" && (i + 1) < args.Length)
                    {
                        if (result == null)
                            result = new Parameters();

                        result.CertificateDestinationPath = args[i + 1];
                    }
                    else if (args[i] == "-c")
                    {
                        if (result == null)
                            result = new Parameters();

                        result.CreateDevice = true;
                    }
                }

                if (result == null)
                    throw new ApplicationException("Wrong number of parameters");

                if (string.IsNullOrEmpty(result.DeviceId))
                    throw new ApplicationException("Device Id has not been provided (-deviceId)");
            }
            else
                throw new ApplicationException("Wrong number of parameters");

            return result;
        }
    }

    internal class Parameters
    {
        public string DeviceId { get; set; }
        public string CertificateDestinationPath { get; set; }
        public bool CreateDevice { get; set; }

        public Parameters()
        {
            CreateDevice = false;
        }
    }
}
