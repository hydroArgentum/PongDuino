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
        uint difficulty;
        uint cpu_hits = 0;
        bool d_x = false;
        bool d_y = false;
        bool press_up = false;
        bool press_down = false;
        byte player_score = 0;
        byte cpu_score = 0;
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
                        button1.Enabled = false;
                        pictureBox2.Location = new Point(pictureBox2.Location.X, 270);
                        pictureBox3.Location = new Point(pictureBox3.Location.X, 270);
                        difficulty = 1;
                        Random rand_gen = new Random();
                        int random_number = rand_gen.Next(1, 5);
                        player_score = 0;
                        cpu_score = 0;
                        switch (random_number)
                        {
                            case 1:
                                d_x = false;
                                d_y = false;
                                break;
                            case 2:
                                d_x = false;
                                d_y = true;
                                break;
                            case 3:
                                d_x = true;
                                d_y = false;
                                break;
                            case 4:
                                d_x = true;
                                d_y = true;
                                break;
                        }
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (port_found)
            {
                if (d_x || d_y)
                {
                    pictureBox5.Location = new Point(pictureBox5.Location.X + 1, pictureBox5.Location.Y + 1);
                }
                else if (d_x || !d_y)
                {
                    pictureBox5.Location = new Point(pictureBox5.Location.X + 1, pictureBox5.Location.Y - 1);
                }
                else if (!d_x || d_y)
                {
                    pictureBox5.Location = new Point(pictureBox5.Location.X - 1, pictureBox5.Location.Y + 1);
                }
                else
                {
                    pictureBox5.Location = new Point(pictureBox5.Location.X - 1, pictureBox5.Location.Y - 1);
                }
                //Move player.
                if (press_up)
                {
                    pictureBox2.Location = new Point(pictureBox2.Location.X, Math.Max(pictureBox2.Location.Y - 1, 43));
                }
                else if (press_down)
                {
                    pictureBox2.Location = new Point(pictureBox2.Location.X, Math.Min(pictureBox2.Location.Y + 1, 423));
                }
                //Move CPU
                if (cpu_hits < difficulty)
                {
                    pictureBox3.Location = new Point(pictureBox3.Location.X, Math.Min(Math.Max(pictureBox5.Location.Y - 40, 43), 423));
                }
                //Check collision.
                if (d_x && ((pictureBox5.Location.Y + pictureBox5.Height) > pictureBox2.Location.Y && pictureBox5.Location.Y < (pictureBox2.Location.Y + pictureBox2.Height) &&
                    pictureBox2.Location.X > (pictureBox5.Location.X + pictureBox5.Width) && (pictureBox2.Location.X + pictureBox2.Width) < pictureBox5.Location.X))
                {
                    d_x = false;
                }
                if (!d_x && ((pictureBox5.Location.Y + pictureBox5.Height) > pictureBox3.Location.Y && pictureBox5.Location.Y < (pictureBox3.Location.Y + pictureBox3.Height) &&
                    pictureBox5.Location.X > (pictureBox3.Location.X + pictureBox3.Width) && (pictureBox5.Location.X + pictureBox5.Width) < pictureBox3.Location.X))
                {
                    d_x = true;
                    cpu_hits++;
                }
                if (d_y && (pictureBox5.Location.Y + pictureBox5.Height) > (pictureBox1.Location.Y + pictureBox1.Height))
                {
                    d_y = false;
                }
                if (!d_y && (pictureBox5.Location.Y < pictureBox1.Location.Y))
                {
                    d_y = true;
                }
                //Check score.
                if (pictureBox5.Location.X < pictureBox1.Location.X)
                {
                    cpu_score++;
                    //Reset.
                    pictureBox2.Location = new Point(pictureBox2.Location.X, 270);
                    pictureBox3.Location = new Point(pictureBox3.Location.X, 270);
                    pictureBox5.Location = new Point(315, 278);
                    Random rand_gen = new Random();
                    int random_number = rand_gen.Next(1, 5);
                    switch (random_number)
                    {
                        case 1:
                            d_x = false;
                            d_y = false;
                            break;
                        case 2:
                            d_x = false;
                            d_y = true;
                            break;
                        case 3:
                            d_x = true;
                            d_y = false;
                            break;
                        case 4:
                            d_x = true;
                            d_y = true;
                            break;
                    }
                    if (difficulty != 1)
                    {
                        difficulty--;
                    }
                }
                else if ((pictureBox5.Location.X + pictureBox5.Width) > (pictureBox1.Location.X + pictureBox1.Width))
                {
                    player_score++;
                    //Reset.
                    pictureBox2.Location = new Point(pictureBox2.Location.X, 270);
                    pictureBox3.Location = new Point(pictureBox3.Location.X, 270);
                    pictureBox5.Location = new Point(315, 278);
                    Random rand_gen = new Random();
                    int random_number = rand_gen.Next(1, 5);
                    switch (random_number)
                    {
                        case 1:
                            d_x = false;
                            d_y = false;
                            break;
                        case 2:
                            d_x = false;
                            d_y = true;
                            break;
                        case 3:
                            d_x = true;
                            d_y = false;
                            break;
                        case 4:
                            d_x = true;
                            d_y = true;
                            break;
                    }
                }
                //Check end of game.
                if (cpu_score == 15 || player_score == 15)
                {
                    button1.Enabled = true;
                    port_found = false;
                }
                Application.DoEvents();
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                if (!press_down)
                {
                    press_up = true;
                }
            }
            else if (e.KeyCode == Keys.S)
            {
                if (!press_up)
                {
                    press_down = true;
                }
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                press_up = false;
            }
            else if (e.KeyCode == Keys.S)
            {
                press_down = false;
            }
        }
    }
}
