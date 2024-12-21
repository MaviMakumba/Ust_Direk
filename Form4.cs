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
    public partial class Form4Silme : Form
    {
        // Bağlantı dizesi
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=VeritabaniYönetimSistemleriOdev1;";
        public Form4Silme()
        {
            InitializeComponent();
        }

        private void Form4Silme_Load(object sender, EventArgs e)
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

        private string GetPrimaryKeyColumn(string tableName)
        {
            switch (tableName)
            {
                case "Birey":
                    return "bireyId";
                case "Oyuncular":
                    return "oyuncuId";
                case "Baskanlar":
                    return "baskanId";
                case "Takimlar":
                    return "takimId";
                case "Ligler":
                    return "ligId";
                case "Maclar":
                    return "macId";
                case "Sehirler":
                    return "sehirId";
                case "TeknikDirektorler":
                    return "tdId";
                case "Statlar":
                    return "statId";
                default:
                    throw new Exception("Bilinmeyen tablo!");
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.Text;

            if (string.IsNullOrEmpty(selectedTable))
            {
                MessageBox.Show("Lütfen bir tablo seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadRecords(selectedTable);
        }

        private void LoadRecords(string tableName)
        {
            // Veritabanından kayıtları getir ve listBoxRecords'a ekle
            listBox1.Items.Clear();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = $"SELECT * FROM {tableName}";
                    using (var command = new NpgsqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string record = $"{reader[0]} - {string.Join(", ", Enumerable.Range(1, reader.FieldCount - 1).Select(i => reader.GetValue(i)))}";
                            listBox1.Items.Add(record);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.Text;
            if (string.IsNullOrEmpty(selectedTable))
            {
                MessageBox.Show("Lütfen bir tablo seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silmek istediğiniz kaydı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kayıt ID'sini ayıkla
            string selectedRecord = listBox1.SelectedItem.ToString();
            string recordId = selectedRecord.Split(' ')[0]; // İlk sütun ID olarak varsayılıyor.

            // Tabloya ait birincil anahtar sütun adını al
            string primaryKeyColumn = GetPrimaryKeyColumn(selectedTable);

            // Silme sorgusunu oluştur
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Parametreli sorgu kullanarak SQL Injection'dan korunma
                    string sql = "SELECT DeleteRecordFromTable(@TableName, @Condition)";

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", selectedTable);
                        command.Parameters.AddWithValue("@Condition", $"{primaryKeyColumn} = {recordId}");

                        // Çalıştırılacak SQL'yi ekrana yazdırmak
                        MessageBox.Show($"Çalıştırılacak SQL:\n{command.CommandText}", "Bilgi", MessageBoxButtons.OK);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Kayıt başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Kayıtları güncelle
                    LoadRecords(selectedTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    
}
