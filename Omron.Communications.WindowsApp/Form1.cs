using Omron.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Omron.Communications.WindowsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Omron.Communications.Windows.Provider provider;
            PlcConfiguration configuration;

            configuration = new PlcConfiguration()
            {
                Address = "10.146.80.90",
                Port = "9600",
                Serial = false
            };

            provider = new Windows.Provider(configuration);

            var result = await provider.ReadAreaAsync("D0001", 3);



        }
    }
}
