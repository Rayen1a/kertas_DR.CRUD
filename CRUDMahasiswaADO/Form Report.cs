using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form_Report : Form
    {
        static string connectionString = "Data Source=LAPTOP-BDD2S1H1\\RAIHAN_ALFAKHRI1;Initial Catalog=DBAkademikADO;User ID=sa;Password=SQLraihan1";

        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;

        Daftar_Rekap_Data_Mahasiswa  daftarrekapdatamahasiswa = new Daftar_Rekap_Data_Mahasiswa();

        string prodi {  get; set; }
        DateTime tglmasuk { get; set; }

        public Form_Report(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();
            prodi = Prodi;
            tglmasuk = TglMasuk;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tglmasuk.Year);

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);

                conn.Close();

                daftarrekapdatamahasiswa.SetDataSource(dtMahasiswa);
                crystalReportViewer1.ReportSource = daftarrekapdatamahasiswa;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
