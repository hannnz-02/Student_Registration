using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Data.SqlClient;

namespace Student_Registration
{
    public class Mahasiswa
    {
        public string Nim { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public string TanggalLahir { get; set; } = string.Empty;
        public string NoTelp { get; set; } = string.Empty;
        public string Alamat { get; set; } = string.Empty;
        public string Prodi { get; set; } = string.Empty;
        public string JenisKelamin { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Nim} - {Nama} | Tgl Lahir: {TanggalLahir} | Telp: {NoTelp} | {Prodi} | {JenisKelamin} | Alamat: {Alamat}";
        }
    }

    public partial class MainWindow : Window
    {
        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

        private List<Mahasiswa> masterListMahasiswa = new List<Mahasiswa>();
        private ObservableCollection<Mahasiswa> displayedListMahasiswa = new ObservableCollection<Mahasiswa>();
        private Mahasiswa? selectedMahasiswaForEdit = null;

        public MainWindow()
        {
            InitializeComponent();
            lstMahasiswa.ItemsSource = displayedListMahasiswa;

            LoadDataFromDatabase();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                masterListMahasiswa.Clear();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Nim, Nama, TanggalLahir, NoTelp, Alamat, Prodi, JenisKelamin FROM Mahasiswa";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime dt = reader.GetDateTime(2);
                                masterListMahasiswa.Add(new Mahasiswa
                                {
                                    Nim = reader.GetString(0),
                                    Nama = reader.GetString(1),
                                    TanggalLahir = dt.ToString("dd/MM/yyyy"),
                                    NoTelp = reader.GetString(3),
                                    Alamat = reader.GetString(4),
                                    Prodi = reader.GetString(5),
                                    JenisKelamin = reader.GetString(6)
                                });
                            }
                        }
                    }
                }
                ApplyFilter();
                UpdateCounter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke Database:\n" + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCounter()
        {
            txtCounter.Text = $"Total Mahasiswa: {masterListMahasiswa.Count}";
        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {
            bool isNimEmpty = string.IsNullOrWhiteSpace(txtNim.Text);
            bool isNamaEmpty = string.IsNullOrWhiteSpace(txtNama.Text);
            bool isTglLahirEmpty = !dpTanggalLahir.SelectedDate.HasValue;
            bool isNoTelpEmpty = string.IsNullOrWhiteSpace(txtNoTelp.Text);
            bool isAlamatEmpty = string.IsNullOrWhiteSpace(txtAlamat.Text);
            bool isProdiEmpty = cmbProdi.SelectedItem == null;
            bool isGenderEmpty = (rbLaki.IsChecked != true && rbPerempuan.IsChecked != true);

            if (isNimEmpty && isNamaEmpty && isTglLahirEmpty && isNoTelpEmpty && isAlamatEmpty && isProdiEmpty && isGenderEmpty)
            {
                MessageBox.Show("Semua field formulir masih kosong! Silakan isi seluruh data terlebih dahulu.", "Validasi Gagal", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isNimEmpty) { ShowWarning("NIM tidak boleh kosong!", txtNim); return; }
            if (isNamaEmpty) { ShowWarning("Nama Mahasiswa tidak boleh kosong!", txtNama); return; }
            if (isTglLahirEmpty) { ShowWarning("Tanggal Lahir belum dipilih!", dpTanggalLahir); return; }
            if (isNoTelpEmpty) { ShowWarning("Nomor Telepon tidak boleh kosong!", txtNoTelp); return; }
            if (isAlamatEmpty) { ShowWarning("Alamat tidak boleh kosong!", txtAlamat); return; }
            if (isProdiEmpty) { ShowWarning("Program Studi belum dipilih!", cmbProdi); return; }
            if (isGenderEmpty) { MessageBox.Show("Jenis Kelamin belum dipilih!", "Validasi Jenis Kelamin", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            string prodiText = cmbProdi.SelectedItem is ComboBoxItem item ? item.Content.ToString()! : cmbProdi.SelectedItem.ToString()!;
            string genderText = rbLaki.IsChecked == true ? "Laki-laki" : "Perempuan";
            DateTime tglLahirVal = dpTanggalLahir.SelectedDate!.Value;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (selectedMahasiswaForEdit != null)
                    {
                        string updateQuery = @"UPDATE Mahasiswa 
                                               SET Nama=@Nama, TanggalLahir=@Tgl, NoTelp=@Telp, Alamat=@Alamat, Prodi=@Prodi, JenisKelamin=@JK 
                                               WHERE Nim=@Nim";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Nim", txtNim.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nama", txtNama.Text.Trim());
                            cmd.Parameters.AddWithValue("@Tgl", tglLahirVal);
                            cmd.Parameters.AddWithValue("@Telp", txtNoTelp.Text.Trim());
                            cmd.Parameters.AddWithValue("@Alamat", txtAlamat.Text.Trim());
                            cmd.Parameters.AddWithValue("@Prodi", prodiText);
                            cmd.Parameters.AddWithValue("@JK", genderText);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Data pendaftar berhasil diperbarui di Database!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string insertQuery = @"INSERT INTO Mahasiswa (Nim, Nama, TanggalLahir, NoTelp, Alamat, Prodi, JenisKelamin) 
                                               VALUES (@Nim, @Nama, @Tgl, @Telp, @Alamat, @Prodi, @JK)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Nim", txtNim.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nama", txtNama.Text.Trim());
                            cmd.Parameters.AddWithValue("@Tgl", tglLahirVal);
                            cmd.Parameters.AddWithValue("@Telp", txtNoTelp.Text.Trim());
                            cmd.Parameters.AddWithValue("@Alamat", txtAlamat.Text.Trim());
                            cmd.Parameters.AddWithValue("@Prodi", prodiText);
                            cmd.Parameters.AddWithValue("@JK", genderText);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Data pendaftar berhasil disimpan ke Database!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                LoadDataFromDatabase();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan ke Database:\n" + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowWarning(string msg, Control control)
        {
            MessageBox.Show(msg, "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
            control.Focus();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstMahasiswa.SelectedItem is Mahasiswa mhs)
            {
                selectedMahasiswaForEdit = mhs;

                txtNim.Text = mhs.Nim;
                txtNim.IsReadOnly = true;
                txtNama.Text = mhs.Nama;
                if (DateTime.TryParseExact(mhs.TanggalLahir, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dt))
                {
                    dpTanggalLahir.SelectedDate = dt;
                }
                txtNoTelp.Text = mhs.NoTelp;
                txtAlamat.Text = mhs.Alamat;

                foreach (ComboBoxItem item in cmbProdi.Items)
                {
                    if (item.Content.ToString() == mhs.Prodi)
                    {
                        cmbProdi.SelectedItem = item;
                        break;
                    }
                }

                if (mhs.JenisKelamin == "Laki-laki") rbLaki.IsChecked = true;
                else if (mhs.JenisKelamin == "Perempuan") rbPerempuan.IsChecked = true;

                btnSimpan.Content = "Update";
                MessageBox.Show("Data dimuat ke form! Silakan ubah lalu klik 'Update'.", "Mode Edit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Pilih data pada ListBox yang ingin di-edit terlebih dahulu!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnHapus_Click(object sender, RoutedEventArgs e)
        {
            if (lstMahasiswa.SelectedItem is Mahasiswa selectedMahasiswa)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Apakah Anda YAKIN ingin menghapus data mahasiswa berikut dari Database?\n\nNIM: {selectedMahasiswa.Nim}\nNama: {selectedMahasiswa.Nama}",
                    "Konfirmasi Hapus Data",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string deleteQuery = "DELETE FROM Mahasiswa WHERE Nim = @Nim";
                            using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@Nim", selectedMahasiswa.Nim);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Data pendaftar berhasil dihapus dari Database!", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadDataFromDatabase();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menghapus dari Database:\n" + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih data pada ListBox yang ingin dihapus terlebih dahulu!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            displayedListMahasiswa.Clear();

            var filtered = masterListMahasiswa.Where(m =>
                m.Nim.ToLower().Contains(keyword) ||
                m.Nama.ToLower().Contains(keyword) ||
                m.Prodi.ToLower().Contains(keyword) ||
                m.Alamat.ToLower().Contains(keyword)
            );

            foreach (var mhs in filtered)
            {
                displayedListMahasiswa.Add(mhs);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtNim.Clear();
            txtNim.IsReadOnly = false;
            txtNama.Clear();
            dpTanggalLahir.SelectedDate = null;
            txtNoTelp.Clear();
            txtAlamat.Clear();
            cmbProdi.SelectedIndex = -1;
            rbLaki.IsChecked = false;
            rbPerempuan.IsChecked = false;

            selectedMahasiswaForEdit = null;
            btnSimpan.Content = "Simpan";
            lstMahasiswa.UnselectAll();
        }

        private void LstMahasiswa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}