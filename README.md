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

2. **Merancang UI (`MainWindow.xaml`)**:
   * Membagi *layout* utama menjadi 2 kolom menggunakan `Grid` (sisi kiri untuk formulir input pendaftaran, sisi kanan untuk daftar pendaftar & pencarian).
   * Menambahkan kontrol komponen input: `TextBox` (NIM, Nama, No Telp, Alamat), `DatePicker` (Tanggal Lahir), `ComboBox` (Program Studi), `RadioButton` (Jenis Kelamin), `Button` (Simpan, Edit, Hapus, Reset), dan `ListBox` (menampilkan daftar pendaftar).
   * Mengintegrasikan library **FontAwesome.WPF** untuk penambahan ikon-ikon visual pada tombol dan header.

<img width="775" height="516" alt="image" src="https://github.com/user-attachments/assets/382327d0-a7f0-435f-9814-51194dfaff20" /><br>
<img width="780" height="514" alt="image" src="https://github.com/user-attachments/assets/3910b854-8ce8-4825-a109-896d398e6998" />


3. **Membuat dan Menghubungkan Database (SQL Server LocalDB)**:
   * Membuka jendela **Server Explorer** di Visual Studio (`View` > `Server Explorer`).
   * Membuat database SQL Server LocalDB baru bernama **`StudentDB`** pada instance server `(localdb)\MSSQLLocalDB`.
   * Membuat tabel **`Mahasiswa`** dengan T-SQL script yang terdiri dari kolom: `Nim` (Primary Key), `Nama`, `TanggalLahir`, `NoTelp`, `Alamat`, `Prodi`, dan `JenisKelamin`.
   * Menginstal paket driver database **`Microsoft.Data.SqlClient`** melalui **NuGet Package Manager**.
<img width="780" height="434" alt="image" src="https://github.com/user-attachments/assets/76c7e48f-f9a4-4f7c-9934-ce3d90b01a35" />


4. **Menghubungkan UI dengan Kode C# & Database (`MainWindow.xaml.cs`)**:
   * Menyiapkan *connection string* ke instance SQL Server LocalDB.
   * **READ**: Memuat data dari tabel `Mahasiswa` saat aplikasi dibuka melalui method `LoadDataFromDatabase()` dan menampilkannya di `ListBox`.
   * **CREATE / UPDATE**: Menulis penanganan event `BtnSimpan_Click` dengan validasi form lengkap untuk menambah data baru (`INSERT INTO`) atau memperbarui data lama (`UPDATE`).
   * **EDIT**: Menulis penanganan event `BtnEdit_Click` untuk memuat data pendaftar yang dipilih dari `ListBox` kembali ke dalam form input.
   * **DELETE**: Menulis penanganan event `BtnHapus_Click` untuk menghapus data pendaftar terpilih (`DELETE FROM`) dengan pop-up konfirmasi.
   * **SEARCH & COUNTER**: Menambahkan fitur filter pencarian data secara *real-time* via `TxtSearch_TextChanged` serta pencatat total pendaftar terdaftar.

## Dokumentasi Hasil

**Berhasil Registrasi**
<img width="741" height="477" alt="image" src="https://github.com/user-attachments/assets/3d27d8a0-154d-4e1a-86e6-38ce9b090b59" />
**Menghapus Data**
<img width="741" height="478" alt="image" src="https://github.com/user-attachments/assets/25e02700-ff01-49ee-bc66-fdc344ee6681" />
<img width="737" height="473" alt="image" src="https://github.com/user-attachments/assets/3a9b47eb-133b-4901-92da-bf889d205f0b" />
**Mengupdate Data**
<img width="777" height="517" alt="image" src="https://github.com/user-attachments/assets/9390099c-8ff9-490f-afca-5e4dcd29e43d" />
<img width="781" height="516" alt="image" src="https://github.com/user-attachments/assets/31ec489f-fc4b-4cca-abd9-cfff83f910e4" />
**Mencari Data**
<img width="784" height="517" alt="image" src="https://github.com/user-attachments/assets/aa3a5849-731d-450a-89bd-91a8d3e91abc" />
<img width="780" height="519" alt="image" src="https://github.com/user-attachments/assets/6e576bb8-542d-4532-9453-7ba0ebfe8140" />
**Bukti Data Tersimpan di Database**
<img width="602" height="152" alt="image" src="https://github.com/user-attachments/assets/7629d785-976c-4ed3-af0b-d67723476725" />





