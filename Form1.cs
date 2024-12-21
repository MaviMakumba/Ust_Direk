using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VeritabaniYönetimSistemleri_Projesi
{
    public partial class Form1 : Form
    {

        // Bağlantı dizesi
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=VeritabaniYönetimSistemleriOdev1;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2Arama form2 = new Form2Arama();
            form2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3Ekleme form3 = new Form3Ekleme();
            form3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4Silme form4 = new Form4Silme();
            form4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5Güncelleme form5 = new Form5Güncelleme();
            form5.Show();
        }
    }
}
