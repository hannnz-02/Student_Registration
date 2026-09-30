# Student Registration Application (WPF)

**Nama** : Hidayah Nur Septiani  
**NRP**  : 5025241247  

---

## Tujuan
* Membuat project WPF.
* Memahami struktur project WPF.
* Membuat UI menggunakan XAML.
* Menggunakan TextBox, ComboBox, RadioButton, Button, dan ListBox.
* Menangani event Click.
* Menghubungkan UI dengan kode C#.
* Mengintegrasikan penyimpanan data permanen menggunakan SQL Server LocalDB.
* Membuat aplikasi desktop CRUD sederhana.

---

## Langkah-Langkah Pengerjaan

1. **Memahami Struktur File Proyek**:
   * Terdapat beberapa file bawaan template WPF, di mana file utama yang diedit adalah **`MainWindow.xaml`** (desain antarmuka UI) dan **`MainWindow.xaml.cs`** (logika program/code-behind).

2. **Merancang Antarmuka UI (`MainWindow.xaml`)**:
   * Membagi *layout* utama menjadi 2 kolom menggunakan `Grid` (sisi kiri untuk formulir input pendaftaran, sisi kanan untuk daftar pendaftar & pencarian).
   * Menambahkan kontrol komponen input: `TextBox` (NIM, Nama, No Telp, Alamat), `DatePicker` (Tanggal Lahir), `ComboBox` (Program Studi), `RadioButton` (Jenis Kelamin), `Button` (Simpan, Edit, Hapus, Reset), dan `ListBox` (menampilkan daftar pendaftar).
   * Mengintegrasikan library **FontAwesome.WPF** untuk penambahan ikon-ikon visual pada tombol dan header.

<img width="775" height="516" alt="image" src="https://github.com/user-attachments/assets/382327d0-a7f0-435f-9814-51194dfaff20" />

3. **Membuat dan Menghubungkan Database (SQL Server LocalDB)**:
   * Membuka jendela **Server Explorer** di Visual Studio (`View` > `Server Explorer`).
   * Membuat database SQL Server LocalDB baru bernama **`StudentDB`** pada instance server `(localdb)\MSSQLLocalDB`.
   * Membuat tabel **`Mahasiswa`** dengan T-SQL script yang terdiri dari kolom: `Nim` (Primary Key), `Nama`, `TanggalLahir`, `NoTelp`, `Alamat`, `Prodi`, dan `JenisKelamin`.
   * Menginstal paket driver database **`Microsoft.Data.SqlClient`** melalui **NuGet Package Manager**.

4. **Menghubungkan UI dengan Kode C# & Database (`MainWindow.xaml.cs`)**:
   * Menyiapkan *connection string* ke instance SQL Server LocalDB.
   * **READ**: Memuat data dari tabel `Mahasiswa` saat aplikasi dibuka melalui method `LoadDataFromDatabase()` dan menampilkannya di `ListBox`.
   * **CREATE / UPDATE**: Menulis penanganan event `BtnSimpan_Click` dengan validasi form lengkap untuk menambah data baru (`INSERT INTO`) atau memperbarui data lama (`UPDATE`).
   * **EDIT**: Menulis penanganan event `BtnEdit_Click` untuk memuat data pendaftar yang dipilih dari `ListBox` kembali ke dalam form input.
   * **DELETE**: Menulis penanganan event `BtnHapus_Click` untuk menghapus data pendaftar terpilih (`DELETE FROM`) dengan pop-up konfirmasi.
   * **SEARCH & COUNTER**: Menambahkan fitur filter pencarian data secara *real-time* via `TxtSearch_TextChanged` serta pencatat total pendaftar terdaftar.

5. **Pengujian Aplikasi & Upload ke GitHub**:
   * Menjalankan aplikasi (`F5`) untuk memastikan seluruh fungsi CRUD, validasi input, dan *data persistence* berjalan lancar.
   * Menyiapkan file `README.md` dan mengunggah seluruh kode sumber proyek ke repository GitHub.
