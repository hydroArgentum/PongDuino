using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PongDuinoWindows
{
    public partial class Form1 : Form
    {
        //Active serial port.
        SerialPort serial_device;
        //Make sure that the port is found.
        bool port_found = false;
        public Form1()
        {
            InitializeComponent();
        }
        //Connect button.
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Enumerate all the ports.
                string[] port_names = SerialPort.GetPortNames();
                //Check the ports.
                foreach (string port in port_names)
                {
                    //Connect to the serial device.
                    serial_device = new SerialPort(port, 9600);
                    if (DetectArduino())
                    {
                        port_found = true;
                        break;
                    }
                    else
                    {
                        port_found = false;
                    }
                }
            }
            catch (Exception ex) { }
        }
        //Detect Arduino.
        private bool DetectArduino()
        {
            try
            {
                //Sent message.
                byte[] buffer = new byte[1];
                //The string allows us to piece the individual ASCII characters together.
                string return_message = "";
                //Allows us to receive data.
                int read_int;
                //Go through the data.
                int byte_count;
                buffer[0] = Convert.ToByte(0);
                serial_device.Open();
                serial_device.Write(buffer, 0, 1);
                Thread.Sleep(1000);
                byte_count = serial_device.BytesToRead;
                while (byte_count > 0)
                {
                    read_int = serial_device.ReadByte();
                    return_message = return_message + Convert.ToChar(read_int);
                    byte_count--;
                }
                serial_device.Close();
                if (return_message.Contains("THIS IS ARDUINO!"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}
