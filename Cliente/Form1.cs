using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets; //Using Sockets comunicação TCP/IP cliente servidor
using System.Text;
using System.Threading; // Using Threading paralelo
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Form1 : Form
    {


        private const int portNum = 4540;
        delegate void SetTexCallBack(string text);
        TcpClient cliente;
        NetworkStream ns;
        Thread t = null;
        private const string hostName = "192.168.0.2"; //change for conection

        public void execucao()
        {
            byte[] bytes = new byte[1024];
            while (true)
            {
                int bytesLidos = ns.Read(bytes, 0, bytes.Length);
                this.SetText(Encoding.ASCII.GetString(bytes, 0, bytesLidos));

            }
        }
        private void SetText(string texto)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTexCallBack d = new SetTexCallBack(SetText);
                this.Invoke(d, new object[] { texto });

            }
            else
            {
                this.textBox1.AppendText("Servidor:>"); // incrementa o Texto
                this.textBox1.AppendText(texto);
                this.textBox1.AppendText(Environment.NewLine);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cliente.Close();
            Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;
            cliente = new TcpClient(hostName, portNum); //colocar o Ip servidor
            ns = cliente.GetStream();
            string s1 = "Cliente Conectado!";
            button1.Text = s1;
            button2.Enabled = true;
            byte[] mensagem = Encoding.ASCII.GetBytes(s1);
            ns.Write(mensagem, 0, mensagem.Length);
            t = new Thread(execucao);
            t.Start();
            button1.Enabled = false;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = textBox2.Text;
            byte[] mensagem = Encoding.ASCII.GetBytes(s); //cabos passam em bytes
            ns.Write(mensagem, 0, mensagem.Length);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
