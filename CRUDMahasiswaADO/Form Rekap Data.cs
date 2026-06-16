using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form_Rekap_Data: Form
    {
        static string connectionString = "Data Source=LAPTOP-BDD2S1H1\\RAIHAN_ALFAKHRI1;Initial Catalog=DBAkademikADO;User ID=sa;Password=SQLraihan1";
        DAL dbLogic = new DAL();
        
        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;
        public Form_Rekap_Data()
        {
            InitializeComponent();
        }

        
    }
}
