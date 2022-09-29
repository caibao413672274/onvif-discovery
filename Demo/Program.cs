using Onvif.Core.Client;
using Onvif.Core.Client.Device;
using OnvifDiscovery;
using OnvifDiscovery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static  void Main(string[] args) { 
        Console.WriteLine ("Starting Discover ONVIF cameras for 10 seconds, press Ctrl+C to abort\n");

			try {
				//var cts = new CancellationTokenSource ();
				//Console.CancelKeyPress += (s, e) => {
				//	e.Cancel = true;
				//	cts.Cancel ();
				//};
				//var discovery = new Discovery ();
				//discovery.Discover (5, OnNewDevice, cts.Token).Wait ();

				var cts = new CancellationTokenSource ();


				var discovery = new Discovery ();
				discovery.Discover (5, OnNewDevice, cts.Token).GetAwaiter ().GetResult ();

				cts.Cancel ();


				Console.WriteLine ("ONVIF Discovery finished");


				//var endPointAddress = new EndpointAddress ("http://192.168.8.100:8080/onvif/device_service");
				//var httpBinding = new HttpTransportBindingElement ();
				//var bind = new CustomBinding (httpBinding);
				//ServiceReference1.DeviceClient client = new ServiceReference1.DeviceClient (bind, endPointAddress);
			var client	=OnvifClientFactory.CreateDeviceClientAsync ("192.168.8.100:8080", "admin", "@zte2020bbys").Result;

				GetDeviceInformationRequest request = new GetDeviceInformationRequest ();
			 
				var re=  client.GetDeviceInformationAsync (request).Result;

			} catch(Exception ex) {
				Console.WriteLine (ex.Message);
			}
			Console.ReadKey ();
		}

		private static void OnNewDevice (DiscoveryDevice device)
		{
			// Multiple events could be received at the same time.
			// The lock is here to avoid messing the console.
			lock (Console.Out) {
				Console.WriteLine (
					$"Device model {device.Model} from manufacturer {device.Mfr} has address {device.Address}");
				Console.Write ($"Urls to device: ");
				foreach (var address in device.XAdresses) {
					Console.Write ($"{address}, ");
				}

				Console.WriteLine ("\n");
			}
		}
	}
}
