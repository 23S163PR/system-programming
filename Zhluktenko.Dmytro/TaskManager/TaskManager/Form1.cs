using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            Refresh_Grid();
          

        }
        public int GetCpuUsage()
        {
            var cpuCounter = new PerformanceCounter();
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", Process.GetCurrentProcess().MachineName);
                cpuCounter.NextValue();
                Thread.Sleep(1000);
                return (int)cpuCounter.NextValue();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "info");
                return 0;
            }
               
           
            
        }

        private void Refresh_Grid()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = ProcessToView.GetProcesses();
            this.progressBar1.Value = GetCpuUsage();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F5))
            {
                Refresh_Grid();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refresh_Grid();
        }

        private void SendPizzaMail()
        {
            MailMessage mail = new MailMessage("pizzaland.is.real@gmail.com", "manekovskiy@gmail.com");
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.googlemail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("pizzaland.is.real@gmail.com", "pizzapizza1");
            mail.Subject = "Pizza plz, homies";
            mail.Body = "hello my little pizza friend\nim going to build a pizzaland & i need your help brah\ngive me few slices so you will help me to reach my destiny! \nPizza-Land is real ! Go for it!";
            client.Send(mail);
            MessageBox.Show("Pizza inc my lord", "wow such pizza");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SendPizzaMail();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                foreach (var el in Process.GetProcesses())
                {
                    try
                    {
                        el.Kill();
                    }
                    catch (Win32Exception ex)
                    {
                        MessageBox.Show("you are not windows god soz", ex.Message);
                    }
                }
            }
        }
    }
}
