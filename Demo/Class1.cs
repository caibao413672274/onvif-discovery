using Onvif.Core.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
	public class Class1
	{
		static void Main (string[] args)
		{
			DiscoveryService discovery = new DiscoveryService ();
			Task.Delay (5000).ContinueWith (t => {
				discovery.Stop ();
			});
			discovery.Start ().Wait();
			
			var list= discovery.DiscoveredDevices;
			Console.Read ();
		}
	}
}
