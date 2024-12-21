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
    public partial class Form5Güncelleme : Form
    {
        // Bağlantı dizesi
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=VeritabaniYönetimSistemleriOdev1;";

        public Form5Güncelleme()
        {
            InitializeComponent();
        }

        private void Form5Güncelleme_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Birey");
            comboBox1.Items.Add("Oyuncular");
            comboBox1.Items.Add("Baskanlar");
            comboBox1.Items.Add("Takimlar");
            comboBox1.Items.Add("Ligler");
            comboBox1.Items.Add("Maclar");
            comboBox1.Items.Add("Sehirler");
            comboBox1.Items.Add("TeknikDirektorler");
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
                    throw new Exception("Bilinmeyen tablo adı!");
            }
        }


        private List<string> GetColumnsForTable(string tableName)
        {
            switch (tableName)
            {
                case "Birey":
                    return new List<string> { "bireyAdi", "bireySoyadi", "bireyYasi", "bireyMaasi", "bireyTuru", "ulkeId", "takimId" };
                case "Oyuncular":
                    return new List<string> { "bireyId", "oyuncuMevki", "oyuncuNumarasi", "oyuncuDegeri", "oyuncuOncekitakim" };
                case "Baskanlar":
                    return new List<string> { "bireyId" };
                case "Takimlar":
                    return new List<string> { "takimAdi", "ulkeId", "takimDegeri", "takimKurulus" };
                case "Ligler":
                    return new List<string> { "ligAdi", "ulkeId", "kitaId", "ligDegeri" };
                case "Maclar":
                    return new List<string> { "macTakim1", "macTakim2", "macSaati", "macSkoru", "macKazanani", "macTarihi", "statId", "ulkeId", "sehirId", "ligId" };
                case "Sehirler":
                    return new List<string> { "sehirAdi", "ulkeId", "kitaId", "sehirNufusu" };
                case "TeknikDirektorler":
                    return new List<string> { "bireyId", "tdOncekitakm" };
                case "Statlar":
                    return new List<string> { "statAdi", "ulkeId", "statKapasitesi", "takimId" };
                default:
                    throw new Exception("Geçersiz tablo adı.");
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.Text;
            if (listBox1.SelectedItem == null) return;

            string selectedRecord = listBox1.SelectedItem.ToString();
            string recordId = selectedRecord.Split(' ')[0];

            LoadRecordDetails(selectedTable, recordId);
        }

        private void LoadRecordDetails(string tableName, string recordId)
        {
            panel1.Controls.Clear();
            List<string> columns = GetColumnsForTable(tableName);
            string primaryKeyColumn = GetPrimaryKeyColumn(tableName);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = $"SELECT * FROM {tableName} WHERE {primaryKeyColumn} = {recordId}";
                    using (var command = new NpgsqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int y = 10;
                            for (int i = 0; i < columns.Count; i++)
                            {
                                Label lbl = new Label
                                {
                                    Text = columns[i],
                                    Location = new System.Drawing.Point(10, y),
                                    Width = 150
                                };
                                panel1.Controls.Add(lbl);

                                TextBox txt = new TextBox
                                {
                                    Name = $"txt{columns[i]}",
                                    Location = new System.Drawing.Point(170, y),
                                    Width = 200,
                                    Text = reader.GetValue(i + 1).ToString()
                                };
                                panel1.Controls.Add(txt);

                                y += 30;
                            }
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
                MessageBox.Show("Lütfen güncellemek istediğiniz kaydı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedRecord = listBox1.SelectedItem.ToString();
            string recordId = selectedRecord.Split(' ')[0];

            List<string> columnNames = GetColumnsForTable(selectedTable);
            List<string> columnValues = new List<string>();

            foreach (var column in columnNames)
            {
                TextBox txt = panel1.Controls.Find($"txt{column}", true).FirstOrDefault() as TextBox;
                if (txt != null)
                {
                    string value = txt.Text.Replace("'", "''");
                    columnValues.Add($"'{value}'");
                }
            }

            string primaryKeyColumn = GetPrimaryKeyColumn(selectedTable);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = $"SELECT UpdateRecordInTable('{selectedTable}', '{primaryKeyColumn} = {recordId}', ARRAY[{string.Join(", ", columnNames.Select(c => $"'{c}'"))}], ARRAY[{string.Join(", ", columnValues)}])";
                    MessageBox.Show($"Çalıştırılacak SQL:\n{sql}", "Bilgi", MessageBoxButtons.OK);

                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Kayıt başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadRecords(selectedTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
