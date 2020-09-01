using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenVPN_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           label1.Text="IP Address: "+new OpenVPN_Connector().GetRemoteHost();
           new OpenVPN_Connector().IsOpenVPNAvailable();
        }

        private void ConnectionManager_Tick(object sender, EventArgs e)
        {
            new OpenVPN_Connector().isConnected(button2, button1,ConnectionManager);
            if(new OpenVPN_Connector().ClientKill == true)
            {
                richTextBox1.AppendText("\n[" + System.DateTime.Now.ToString() + "] OpenVPN was terminated by user");
                new OpenVPN_Connector().ClientKill = false;
            }
            else
            {
                richTextBox1.AppendText("\n[" + System.DateTime.Now.ToString() + "] OpenVPN was crashed saved to logs.txt");
                File.AppendAllText("logs.txt", "\n[" + System.DateTime.Now.ToString() + "] OpenVPN Connector was crashed because of an unknown error. please try again or contact the adminstrator");

                ConnectionManager.Stop(); //emergency stop lmao.
                button2.Enabled = false;
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new OpenVPN_Connector().Disconnect(button2, button1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new OpenVPN_Connector().Connector(ConnectionManager, richTextBox1,"config.ovpn");
            new OpenVPN_Connector().Connect(button2, button1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(22, 22, 22);
            this.ForeColor = Color.White;
            tabPage1.BackColor = Color.FromArgb(22, 22, 22);
            button1.ForeColor = Color.Black;
            button2.ForeColor = Color.Black;
            button3.ForeColor = Color.Black;
            button4.ForeColor = Color.Black;
            richTextBox1.BackColor = Color.FromArgb(22, 22, 22);
            richTextBox1.ForeColor = Color.White;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.Control;
            this.ForeColor = Color.Black;
            tabPage1.BackColor = Color.White;
            button1.ForeColor = Color.Black;
            button2.ForeColor = Color.Black;
            button3.ForeColor = Color.Black;
            button4.ForeColor = Color.Black;
            richTextBox1.BackColor = Color.White;
            richTextBox1.ForeColor = Color.Black;
        }
    }
}
