using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onvif.Core.Discovery.Common;
using Onvif.Core.Discovery.Interfaces;
using Onvif.Core.Discovery.Models;

namespace Onvif.Core.Discovery
{
	public class DiscoveryService : IDiscoveryService
	{
		IWSDiscovery wsDiscovery;
		CancellationTokenSource cancellation;
		bool isRunning;

		public DiscoveryService ()
		{
			DiscoveredDevices = new ObservableCollection<DiscoveryDevice> ();
			wsDiscovery = new WSDiscovery ();
		}

		public ObservableCollection<DiscoveryDevice> DiscoveredDevices { get; }

		public async Task Start ()
		{
			if (isRunning) {
				throw new InvalidOperationException ("The discovery is already running");
			}
			isRunning = true;
			cancellation = new CancellationTokenSource ();
			try {
				while (isRunning) {
					var devicesDiscovered = await wsDiscovery.Discover (Constants.WS_TIMEOUT);
					SyncDiscoveryDevices (devicesDiscovered);
				}
			} catch (OperationCanceledException) {
				isRunning = false;
			}
		}

		public void Stop ()
		{
			isRunning = false;
			cancellation?.Cancel ();
		}

		void SyncDiscoveryDevices (IEnumerable<DiscoveryDevice> syncDevices)
		{
			//var lostDevices = DiscoveredDevices.Except (syncDevices);

			foreach (var lostDevice in syncDevices) {
				var tmp= DiscoveredDevices.Where (t => t.Address.ToString () == lostDevice.Address.ToString ()).FirstOrDefault ();
				if (tmp == null) {
					DiscoveredDevices.Add (lostDevice);
				}
			}
			 
		}
	}
}
