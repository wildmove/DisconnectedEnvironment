using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisconnectedEnvironment
{
    public partial class Form4 : Form
    {
        private string stringConnection = "Data Source=WILLY;" + "database=profil_mahasiswa;User ID=sa;Password=123";
        private SqlConnection koneksi;

        private void refreshform()
        {
            cbxNama.Enabled = false;
            cbxStatusMahasiswa.Enabled = false;
            cbxTahunMasuk.Enabled = false;
            cbxNama.SelectedIndex = -1;
            cbxStatusMahasiswa.SelectedIndex = -1;
            cbxTahunMasuk.SelectedIndex = -1;
            txtNIM.Visible = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button1.Enabled = true;
        }
        public Form4()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshform();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView()
        {
            koneksi.Open();
            string query = "SELECT * FROM status_mahasiswa";
            SqlDataAdapter adapter = new SqlDataAdapter(query, koneksi);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            koneksi.Close();
        }

        private void cbNama()
        {
            koneksi.Open();
            string query = "SELECT nama_mahasiswa FROM mahasiswa";
            SqlCommand command = new SqlCommand(query, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(query, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            command.ExecuteReader();
            koneksi.Close();

            cbxNama.DisplayMember = "nama_mahasiswa";
            cbxNama.ValueMember = "nim";
            cbxNama.DataSource = ds.Tables[0];
        }

        private void cbTahunMasuk()
        {
            int currentYear = DateTime.Now.Year;
            for (int i = 2010; i <= currentYear; i++)
            {
                cbxTahunMasuk.Items.Add(i.ToString());
            }
        }

        private void cbxNama_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            koneksi.Open();
            
            string query = "SELECT nim FROM mahasiswa WHERE nama_mahasiswa = @nm";
            string nim = "";
            SqlCommand command = new SqlCommand(query, koneksi);
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new SqlParameter("@nm", cbxNama.Text));
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();
            txtNIM.Text = nim;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cbxTahunMasuk.Enabled = true;
            cbxNama.Enabled = true;
            cbxStatusMahasiswa.Enabled = true;
            txtNIM.Visible = true;
            cbTahunMasuk();
            cbNama();
            button2.Enabled = true;
            button3.Enabled = true;
            button1.Enabled = false;
        }

        private void dataGridView1_CellContentClick()
        {
            koneksi.Open();
            string str = "select * From dbo.status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1_CellContentClick();
            button4.Enabled = false;
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string statusMahasiswa = cbxStatusMahasiswa.Text;
            string tahunMasuk = cbxTahunMasuk.Text;
            int count = 0;
            string tempKodeStatus = "";
            string kodeStatus = "";
            koneksi.Open();

            string s = "select count (*) from dbo.status_mahasiswa";
            SqlCommand cm = new SqlCommand(s, koneksi);
            count = (int)cm.ExecuteScalar();
            if (count == 0)
            {
                kodeStatus = "1";
            }
            else
            {
                string QueryString = "select Max(id_status) from dbo.status_mahasiswa";
                SqlCommand cmStatusMahasiswaSum = new SqlCommand(s, koneksi);
                int totalStatusMahasiswa = (int)cmStatusMahasiswaSum.ExecuteScalar();
                int finalKodeStatusInt = totalStatusMahasiswa + 1;
                kodeStatus = Convert.ToString(finalKodeStatusInt);
            }
            string queryString = "insert into dbo.status_mahasiswa(id_status, nim, status_mahasiswa, tahun_masuk)" +
                "values(@ids, @nim, @sm, @tm)";
            SqlCommand cmd = new SqlCommand(queryString, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@ids", kodeStatus));
            cmd.Parameters.Add(new SqlParameter("@nim", nim));
            cmd.Parameters.Add(new SqlParameter("@sm", statusMahasiswa));
            cmd.Parameters.Add(new SqlParameter("@tm", tahunMasuk));
            cmd.ExecuteNonQuery();
            koneksi.Close();

            MessageBox.Show("Data berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshform();
            dataGridView1_CellContentClick();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }

        private void cbxNama_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'profil_mahasiswaDataSet.mahasiswa' table. You can move, or remove it, as needed.
            this.mahasiswaTableAdapter.Fill(this.profil_mahasiswaDataSet.mahasiswa);

        }

        private void txtNIM_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNIM_Click_1(object sender, EventArgs e)
        {

        }
    }
}
