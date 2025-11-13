using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP_Network
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			buttonCalculate.Click += CalculateNetworkInfo;
		}

		private void CalculateNetworkInfo(object sender, EventArgs e)
		{
			try
			{
				uint ip = IPToUint(textBoxIPAddress.Text);
				uint mask = IPToUint(textBoxMask.Text);

				uint network = ip & mask;
				uint broadcast = network | ~mask;
				uint hostCount = ~mask - 1;

				textBoxNetworkAddres.Text = UintToIP(network);
				textBoxBroadcastAddress.Text = UintToIP(broadcast);
				textBoxIPAddressCount.Text = (hostCount + 2).ToString();
				textBoxHostCount.Text = hostCount.ToString();
			}
			catch
			{
				MessageBox.Show("Неверный формат IP-адреса или маски");
			}
		}

		private uint IPToUint(string ip)
		{
			string[] octets = ip.Split('.');
			return (uint.Parse(octets[0]) << 24) |
				   (uint.Parse(octets[1]) << 16) |
				   (uint.Parse(octets[2]) << 8) |
				   uint.Parse(octets[3]);
		}

		private string UintToIP(uint ip)
		{
			return $"{(ip >> 24) & 0xFF}.{(ip >> 16) & 0xFF}.{(ip >> 8) & 0xFF}.{ip & 0xFF}";
		}
	}
}