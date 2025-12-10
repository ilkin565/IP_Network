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
			numericUpDownPrefix.ValueChanged += UpdateMaskFromPrefix;
			textBoxMask.TextChanged += UpdatePrefixFromMask;
			UpdatePrefixFromMask(null, EventArgs.Empty);
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

		private void UpdateMaskFromPrefix(object sender, EventArgs e)
		{
			int prefix = (int)numericUpDownPrefix.Value;
			uint mask = PrefixToMask(prefix);
			textBoxMask.TextChanged -= UpdatePrefixFromMask;
			textBoxMask.Text = UintToIP(mask);
			textBoxMask.TextChanged += UpdatePrefixFromMask;
		}

		private void UpdatePrefixFromMask(object sender, EventArgs e)
		{
			try
			{
				uint mask = IPToUint(textBoxMask.Text);
				int prefix = MaskToPrefix(mask);
				numericUpDownPrefix.ValueChanged -= UpdateMaskFromPrefix;
				numericUpDownPrefix.Value = prefix;
				numericUpDownPrefix.ValueChanged += UpdateMaskFromPrefix;
			}
			catch
			{

			}
		}

		private uint PrefixToMask(int prefix)
		{
			if (prefix < 0 || prefix > 32)
				return 0xFFFFFFFF;

			if (prefix == 0) return 0;

			uint mask = 0xFFFFFFFF << (32 - prefix);
			return mask;
		}

		private int MaskToPrefix(uint mask)
		{
			uint check = mask;
			bool valid = true;
			bool foundZero = false;

			for (int i = 31; i >= 0; i--)
			{
				uint bit = (mask >> i) & 1;
				if (bit == 0)
				{
					foundZero = true;
				}
				else if (foundZero)
				{
					valid = false;
					break;
				}
			}

			if (!valid)
			{
				MessageBox.Show("Неверная маска подсети");
				return 32;
			}

			int prefix = 0;
			while ((mask & 0x80000000) != 0)
			{
				prefix++;
				mask <<= 1;
			}
			return prefix;
		}
	}
}