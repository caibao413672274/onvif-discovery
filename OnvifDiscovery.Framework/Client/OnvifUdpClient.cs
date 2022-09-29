using OnvifDiscovery.Common;
using OnvifDiscovery.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OnvifDiscovery.Client
{
	/// <summary>
	/// A simple Udp client that wrapps <see cref="System.Net.Sockets.UdpClient"/>
	/// It creates the probe messages also
	/// </summary>
	internal class OnvifUdpClient : IOnvifUdpClient
	{
		readonly UdpClient client;

		public OnvifUdpClient (IPEndPoint localpoint)
		{
			client = new UdpClient (localpoint) {
				EnableBroadcast = true
			};
		}

		public async Task<int> SendProbeAsync (Guid messageId, IPEndPoint endPoint)
		{
			try {
				var datagram = WSProbeMessageBuilder.NewProbeMessage (messageId);
				return await client.SendAsync (datagram, datagram.Length, endPoint);
			} catch (Exception ex) {
				return 0;
			}
		}

		public async Task<UdpReceiveResult> ReceiveAsync ()
		{
			try {
				return await client.ReceiveAsync ();
			} catch (Exception ex) {
				return new UdpReceiveResult ();
			}
		}

		public void Close ()
		{
			try {

				client.Close ();
			} catch { }
		}
	}
}
