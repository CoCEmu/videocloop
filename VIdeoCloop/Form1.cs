using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VIdeoCloop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var mioo = new UsbDetector().ExistRemoveableDeviceList;
            foreach(var usbdevice in mioo)
            {
                if (usbdevice.IsReady == false)
                    return;
                FormUSB fs = new FormUSB();
                fs.Drive = usbdevice;
                fs.Show();
            }    
        }
    }
}
