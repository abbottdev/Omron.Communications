﻿using Omron.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Omron.WindowsApp
{
    public partial class Form1 : Form
    {
        Omron.Core.IDriver provider;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            PlcConfiguration configuration;

            configuration = new PlcConfiguration()
            {
                Address = "10.146.80.92",
                Port = "9600",
                Serial = false,
                DestinationNode = 1,
                SourceNode = 2,
                PreferTcpIp = false
            };

            provider = new Omron.Transport.Win32FinsDriver(configuration);
            this.connectedLabel.Text = provider.Connected.ToString();
        }

        private async void ReadAreaButton_Click(object sender, EventArgs e)
        {
            var result = await provider.ReadAreaAsync(txtAddress.Text, 3);

            resultLabel.Text = result.ToString();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            switch (button1.Text)
            {
                case "Disconnect":
                    button1.Text = "Diconnecting";

                    provider.Disconnect();
                    button1.Text = "Connect";

                    break;
                case "Connect":
                    button1.Text = "Connecting";

                    if (await provider.ConnectAsync())
                    {
                        button1.Text = "Disconnect";
                    }
                    break;

                default:
                    break;
            }

            connectedLabel.Text = provider.Connected.ToString();
        }
    }
}
