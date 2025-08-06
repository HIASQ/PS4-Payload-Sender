using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;

namespace Payload_Sender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static Socket _psocket;
        public static bool pDConnected;
        public static string Exception;
        public static string path;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter an IP address.");
                return;
            }

   
            bool result = Connect2PS4(textBox1.Text, textBox2.Text);

            if (result)
            {
      
                this.button1.BackColor = Color.FromArgb(0, 100, 0); 
      
             
     
            }
            else
            {

                this.button1.BackColor = Color.Red;
                MessageBox.Show("Connection failed. Please try again.");

                this.button1.BackColor = Color.FromArgb(0, 123, 255); 
            }
        }

        public static bool Connect2PS4(string ip, string port)
        {
            try
            {
                _psocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _psocket.ReceiveTimeout = 3000;
                _psocket.SendTimeout = 3000;
                _psocket.Connect(new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port)));
                pDConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                pDConnected = false;
                Exception = ex.Message;
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
             
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Please select a Payload file (.bin) first.");
                return;
            }

            try
            {
           
                SendPayload(path);

         
                MessageBox.Show("Payload sent successfully!");
            }
            catch (Exception ex)
            {

                MessageBox.Show($"An error occurred while sending the payload!\n{ex.Message}");
            }
            finally
            {
              
                DisconnectPayload();
              
             
            
                this.button1.BackColor = Color.FromArgb(0, 123, 255); 
            }
        }

        public static void SendPayload(string filename)
        {
            _psocket.SendFile(filename);
        }

        public static void DisconnectPayload()
        {
            if (_psocket != null && _psocket.Connected)
            {
                _psocket.Shutdown(SocketShutdown.Both);
                _psocket.Close();
            }
            pDConnected = false;
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}