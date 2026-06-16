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

namespace CRUDMahasiswaADO
{
    public partial class Form_Report : Form
    {
        static string connectionString = "Data Source=LAPTOP-BDD2S1H1\\RAIHAN_ALFAKHRI1;Initial Catalog=DBAkademikADO;User ID=sa;Password=SQLraihan1";
        DAL dbLogic = new DAL();

        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;

        Daftar_Rekap_Data_Mahasiswa  daftarrekapdatamahasiswa = new Daftar_Rekap_Data_Mahasiswa();

        string prodi {  get; set; }
        DateTime tglmasuk { get; set; }
        
        
    }
}
