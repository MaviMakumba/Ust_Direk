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
    public partial class Form3Ekleme : Form
    {
        // Bağlantı dizesi
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=VeritabaniYönetimSistemleriOdev1;";
        public Form3Ekleme()
        {
            InitializeComponent();
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form3Ekleme_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Birey");
            comboBox1.Items.Add("Oyuncular");
            comboBox1.Items.Add("Baskanlar");
            comboBox1.Items.Add("TeknikDirektorler");
            comboBox1.Items.Add("Takimlar");
            comboBox1.Items.Add("Ligler");
            comboBox1.Items.Add("Maclar");
            comboBox1.Items.Add("Mevki");
            comboBox1.Items.Add("Sehirler");
            comboBox1.Items.Add("Statlar");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.Text;
            if (string.IsNullOrEmpty(selectedTable))
            {
                MessageBox.Show("Lütfen bir tablo seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Seçilen tabloya uygun giriş alanlarını oluştur
            panel1.Controls.Clear();
            List<string> columns = GetColumnsForTable(selectedTable);

            int y = 10;
            foreach (var column in columns)
            {
                Label lbl = new Label
                {
                    Text = column,
                    Location = new System.Drawing.Point(10, y),
                    Width = 150
                };
                panel1.Controls.Add(lbl);

                TextBox txt = new TextBox
                {
                    Name = $"txt{column}",
                    Location = new System.Drawing.Point(170, y),
                    Width = 200
                };
                panel1.Controls.Add(txt);

                y += 30;
            }
        }
        private List<string> GetColumnsForTable(string tableName)
        {
            // Tablo sütunlarını döndüren yapı
            switch (tableName)
            {
                case "Birey":
                    return new List<string> { "bireyAdi", "bireySoyadi", "bireyYasi", "bireyMaasi", "bireyTuru", "ulkeId", "takimId" };
                case "Oyuncular":
                    return new List<string> { "bireyId", "oyuncuMevki", "oyuncuNumarasi", "oyuncuDegeri", "oyuncuOncekitakim" };
                case "Baskanlar":
                    return new List<string> { "bireyId" };
                case "TeknikDirektorler":
                    return new List<string> { "bireyId", "tdOncekitakm" };
                case "Takimlar":
                    return new List<string> { "takimAdi", "ulkeId", "takimDegeri", "takimKurulus" };
                case "Ligler":
                    return new List<string> { "ligAdi", "ulkeId", "kitaId", "ligDegeri" };
                case "Maclar":
                    return new List<string> { "macTakim1", "macTakim2", "macSkoru", "macSaati", "macKazanani", "macTarihi", "ulkeId", "ligId", "sehirId", "statId"  };
                case "Mevki":
                    return new List<string> { "mevkiAdi" };
                case "Sehirler":
                    return new List<string> { "sehirAdi", "ulkeId", "kitaId", "sehirNufusu" };
                case "Statlar":
                    return new List<string> { "statAdi", "ulkeId", "statKapasitesi", "takimId" };
                default:
                    return new List<string>();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.Text;
            if (string.IsNullOrEmpty(selectedTable))
            {
                MessageBox.Show("Lütfen bir tablo seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıdan alınan değerleri oku
            List<string> columnNames = GetColumnsForTable(selectedTable);
            List<string> columnValues = new List<string>();

            foreach (var column in columnNames)
            {
                TextBox txt = panel1.Controls.Find($"txt{column}", true).FirstOrDefault() as TextBox;
                if (txt != null)
                {
                    columnValues.Add($"'{txt.Text.Replace("'", "''")}'"); // Değerleri tek tırnak içine al
                }
            }

            // Sorguyu kontrol et ve çalıştır
            string sql = $"SELECT AddRecordToTable('{selectedTable}', ARRAY[{string.Join(", ", columnNames.Select(c => $"'{c}'"))}], ARRAY[{string.Join(", ", columnValues)}])";

            MessageBox.Show($"Çalıştırılacak SQL:\n{sql}", "Bilgi", MessageBoxButtons.OK);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Kayıt başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    
}
