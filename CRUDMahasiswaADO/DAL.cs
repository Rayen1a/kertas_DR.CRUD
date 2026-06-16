using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public class DAL
    {
        public static string GetLocalIPAddress()
        {
            string localIP = string.Empty;
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting local IP address: " + ex.Message);
            }
            return localIP;
        }

        public string GetConnectionString()
        {
            string connectionString = $"Data Source={GetLocalIPAddress()};Initial Catalog=DBAkademikADO;User ID=sa;Password=PasswordSA";
            return connectionString;
        }
        SqlConnection conn = new SqlConnection(GetConnectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;
        public int CountMhs()
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter outputParam = new SqlParameter("@pCount", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(outputParam);
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(outputParam.Value);
        }

        public DataTable GetMhs()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public DataTable GetMhsByNIM(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public void InsertMhs(string nim, string nama, string alamat, string jeniskelamin, DateTime tanggallahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertMahasiswa", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pNIM", nim);
                cmd.Parameters.AddWithValue("pNama", nama);
                cmd.Parameters.AddWithValue("pAlamat", alamat);
                cmd.Parameters.AddWithValue("pJenisKelamin", jeniskelamin);
                cmd.Parameters.AddWithValue("pTanggalLahir", tanggallahir);
                cmd.Parameters.AddWithValue("pKodeProdi", kodeProdi);
                cmd.Parameters.AddWithValue("pFoto", foto);

                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateMhs(string nim, string nama, string alamat, string jeniskelamin, DateTime tanggallahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_UpdateMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("pNIM", nim);
            cmd.Parameters.AddWithValue("pNama", nama);
            cmd.Parameters.AddWithValue("pAlamat", alamat);
            cmd.Parameters.AddWithValue("pJenisKelamin", jeniskelamin);
            cmd.Parameters.AddWithValue("pTanggalLahir", tanggallahir);
            cmd.Parameters.AddWithValue("pKodeProdi", kodeProdi);
            cmd.Parameters.AddWithValue("pFoto", foto);

            cmd.ExecuteNonQuery();
        }

        public void DeleteMhs(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn);
            cmd.Parameters.AddWithValue("pNIM", nim);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();
        }

        public void resetData()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string deleteQuery = "DELETE FROM Mahasiswa;";
            SqlCommand cmdDelete = new SqlCommand(deleteQuery, conn);
            cmdDelete.ExecuteNonQuery();

            string insertQuery = @"INSERT INTO Mahasiswa SELECT * FROM MahasiswaBackup;";
            SqlCommand cmdInsert = new SqlCommand(insertQuery, conn);
            cmdInsert.ExecuteNonQuery();
        }

        public void testInject(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = "Update mahasiswa set nama = 'HACKED' where NIM = '" + nim;
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        public void InsertLog(string message)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_LogMessage", conn);

            cmd.Parameters.AddWithValue("@pMessage", message);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
        }

        public DataTable getProdi()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("select from namaprodi", conn);
            cmd.CommandType = CommandType.Text;
            dtProdi = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dtProdi);

            return dtProdi;
        }

        public DataTable getDataRekap(string prodi, DateTime tanggalMasuk)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_Report", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inProdi", prodi);
            cmd.Parameters.AddWithValue("@inTglMsuk", tanggalMasuk.Year.ToString());

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }
        public DataTable GetAllDataChart()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_DashBoard", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public DataTable GetDataChartByTahun(DateTime thMasuk)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("sp_DashBoardByTahun", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inTglMsuk", thMasuk.Year);
            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }
    }
}
