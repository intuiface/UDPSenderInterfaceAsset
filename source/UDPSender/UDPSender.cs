using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace UDPSender
{
    public class UDPSender : INotifyPropertyChanged
    {
        private string m_strAddress = "127.0.0.1";

        private int m_iPort = 8080;

        public string Address
        {
            get
            {
                return this.m_strAddress;
            }
            set
            {
                this.m_strAddress = value;
                this.NotifyPropertyChanged("Address");
            }
        }

        public int Port
        {
            get
            {
                return this.m_iPort;
            }
            set
            {
                this.m_iPort = value;
                this.NotifyPropertyChanged("Port");
            }
        }

        public UDPSender()
        {
        }

        private void NotifyPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public void SendMessage(string strMsg_)
        {
            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress serverAddr = IPAddress.Parse(this.Address);
                IPEndPoint endPoint = new IPEndPoint(serverAddr, this.Port);
                byte[] send_buffer = Encoding.ASCII.GetBytes(strMsg_);
                sock.SendTo(send_buffer, endPoint);
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Concat("Error: ", exception.Message));
            }
        }

        public void SendHexMessage(string strMsg_)
        {
            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress serverAddr = IPAddress.Parse(this.Address);
                IPEndPoint endPoint = new IPEndPoint(serverAddr, this.Port);
                byte[] send_buffer = HexString2Bytes(strMsg_);
                sock.SendTo(send_buffer, endPoint);
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Concat("Error: ", exception.Message));
            }
        }

        private byte[] HexString2Bytes(string hexString)
        {
            //check for null
            if (hexString == null) return null;
            //get length
            int len = hexString.Length;
            if (len % 2 == 1) return null;
            int len_half = len / 2;
            //create a byte array
            byte[] bs = new byte[len_half];
            try
            {
                //convert the hexstring to bytes
                for (int i = 0; i != len_half; i++)
                {
                    bs[i] = (byte)Int32.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception : " + ex.Message);
            }
            //return the byte array
            return bs;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}