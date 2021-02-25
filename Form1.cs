using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Net;
using System.IO;

namespace HTTP_Requests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textBox3.Text);
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // https://www.c-sharpcorner.com/article/create-simple-web-api-in-asp-net-mvc/
            ManagementObjectCollection collection;
            using (var finddevice = new ManagementObjectSearcher(@"Select DeviceID from Win32_USBHub"))
                collection = finddevice.Get();

            int number = 0;
            foreach (var device in collection)
            {

                if (Convert.ToString(device.GetPropertyValue("DeviceID")) == textBox2.Text)
                {
                    number = 1;
                }
                else
                {
                    number = 0;
                }
            }

            // das Zeichen \ macht Probleme
            ////string usbnumber = "USB\\VID_20EF&PID_0410\\J0000033";
            ////string bla = textBox2.Text;
            //////textbox2.Text == USB\VID_20EF&PID_0410\J0000033
            ////if (bla == usbnumber)
            ////{
            ////    label5.Text = "richtig";
            ////}
            ////else
            ////{
            ////    label5.Text = "falsch";
            ////}

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(textBox1.Text);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PATCH";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = "{\"status\":\"" + number + "\"}";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                label7.Text = result;
            }
        }
    }
}
