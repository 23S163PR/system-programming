using System;

using System.Windows.Forms;

using System.Text;

using System.Net.Sockets;

using System.Threading;



namespace WindowsApplication2
{
   
    public partial class Form1 : Form
    {
        const int HOMEKEY = 36;
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        NetworkStream serverStream = default(NetworkStream);

        string readData = null;



        public Form1()
        {

            InitializeComponent();

        }



        private void getMessage()
        {

            while (true)
            {

                serverStream = clientSocket.GetStream();

                int buffSize = 0;

                byte[] inStream = new byte[10025];

                buffSize = clientSocket.ReceiveBufferSize;

                serverStream.Read(inStream, 0, buffSize);

                string returndata = System.Text.Encoding.ASCII.GetString(inStream);

                readData = "" + returndata;

                msg();

            }

        }



        private void msg()
        {

            if (this.InvokeRequired)

                this.Invoke(new MethodInvoker(msg));

            else

                ChatBox.Text += Environment.NewLine + " >> " + readData;

        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(TextMessageBox.Text + "$");

            serverStream.Write(outStream, 0, outStream.Length);

            serverStream.Flush();
            this.TextMessageBox.Clear();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {

            readData = "Conected to Chat Server ...";

            msg();

            clientSocket.Connect("127.0.0.1", 8888);

            serverStream = clientSocket.GetStream();



            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(LoginBox.Text + "$");

            serverStream.Write(outStream, 0, outStream.Length);

            serverStream.Flush();



            Thread ctThread = new Thread(getMessage);

            ctThread.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                //MessageBox.Show("", "");
                Form1 f = new Form1();
                f.Show();
            }
        }

        private void LoginBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                //MessageBox.Show("", "");
                Form1 f = new Form1();
                f.Show();
            }
        }





    }

}
