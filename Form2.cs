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
    public partial class Form2Arama : Form
    {
        // Bağlantı dizesi
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=VeritabaniYönetimSistemleriOdev1;";

        public Form2Arama()
        {
            InitializeComponent();
            this.Load += Form2Arama_Load; // Load olayını bağlama
        }

        private void Form2Arama_Load(object sender, EventArgs e)
        {
            // comboBoxSearchType'ı doldur
            comboBoxSearchType.Items.Clear();
            comboBoxSearchType.Items.Add("Takımlar");
            comboBoxSearchType.Items.Add("Oyuncular");
            comboBoxSearchType.Items.Add("Ligler");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçilen öğeyi detaylar bölümünde göster
            if (listBoxResults.SelectedItem != null)
            {
                string selectedItem = listBoxResults.SelectedItem.ToString();
                MessageBox.Show($"Seçilen Bilgi: {selectedItem}", "Detay", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchType = comboBoxSearchType.Text;
            string searchValue = textBoxSearch.Text;

            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Lütfen arama türünü ve arama değerini girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SQL sorgusunu belirleme
            string query = searchType switch
            {
                "Takımlar" => $"SELECT * FROM TakimBilgileriniGetir('{searchValue}')",
                "Oyuncular" => $"SELECT * FROM GetPlayerDetails('{searchValue}')",
                "Ligler" => $"SELECT * FROM GetLeagueDetails('{searchValue}')",
                _ => null
            };

            if (query == null)
            {
                MessageBox.Show("Geçersiz arama türü.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Veritabanına bağlan ve sonuçları getir
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        listBoxResults.Items.Clear();

                        while (reader.Read())
                        {
                            string result = string.Join(", ", Enumerable.Range(0, reader.FieldCount)
                                .Select(i => reader.GetValue(i).ToString()));
                            listBoxResults.Items.Add(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
