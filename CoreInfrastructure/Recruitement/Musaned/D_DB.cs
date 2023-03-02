using Executer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreInfrastructure.Purchase.PurchaseInvoice;

namespace CoreInfrastructure.Recruitement.Musaned
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        public List<M_RecruitementMusanedDetails> GetDetailsrowsforEdit(string CODE)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from RecruitementMusaned_Detail where  CODE= " + CODE;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            List<M_RecruitementMusanedDetails> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_RecruitementMusanedDetails>();
                foreach (DataRow row in dt.Rows)
                {
                    var fields = new M_RecruitementMusanedDetails
                    {
                        CODE = row["CODE"].ToString(),
                        COMPANY_CODE = row["COMPANY_CODE"].ToString(),
                        SEQNO = int.Parse(row["SEQNO"].ToString()),
                        CONTRACT = row["CONTRACT"].ToString(),
                        REMARKS = row["REMARKS"].ToString(),
                        STATUS = row["STATUS"].ToString(),

                    };
                    list.Add(fields);

                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return list;
        }
        public string getUpdateMasterCount( )
        {
            Console.WriteLine("HELOO I AM HERE");
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(CODE as bigint)) +1  as CODE from RecruitementMusaned_Header  ";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["CODE"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "10001";
            return no;
        }

        public void Set_Header(int Mode, M_RecruitementMusanedHeader m)
        {

            SqlCommand cmd = new SqlCommand("R_RecruitementMusaned_Header", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ActionFlag", Mode);
            cmd.Parameters.AddWithValue("@CODE", m.CODE.ToString());
            cmd.Parameters.AddWithValue("@COMPANY_CODE", m.COMPANY_CODE.ToString());
            cmd.Parameters.AddWithValue("@BANK", m.BANK.ToString());
            cmd.Parameters.AddWithValue("@AMOUNT", m.AMOUNT.ToString());
            cmd.Parameters.AddWithValue("@PAYMENTDATE", m.PAYMENTDATE.ToString());
            cmd.Parameters.AddWithValue("@NOTES", m.NOTES.ToString());
            cmd.Parameters.AddWithValue("@STATUS", m.STATUS.ToString());
            cmd.Parameters.AddWithValue("@SORT", m.SORT.ToString());
            cmd.Parameters.AddWithValue("@LOCKED", m.LOCKED.ToString());

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void Set_DetailRows(int Mode, M_RecruitementMusanedDetails m)
        {
            SqlCommand cmd = new SqlCommand("R_RecruitementMusaned_Detail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ActionFlag", Mode);
            cmd.Parameters.AddWithValue("@CODE", m.CODE.ToString());
            cmd.Parameters.AddWithValue("@SEQNO", m.SEQNO.ToString());
            cmd.Parameters.AddWithValue("@COMPANY_CODE", m.COMPANY_CODE.ToString());
            cmd.Parameters.AddWithValue("@CONTRACT", m.CONTRACT.ToString());
            cmd.Parameters.AddWithValue("@REMARKS", m.REMARKS.ToString());
            cmd.Parameters.AddWithValue("@STATUS", m.STATUS.ToString());

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void deleteData(string pera,string id)
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            if(pera == "detailrows")
            { strsql = "DELETE FROM RecruitementMusaned_Detail WHERE CODE=" + id;}
            else
            {strsql = "DELETE FROM RecruitementMusaned_Header WHERE CODE=" + id;}
            SqlCommand cmd = new SqlCommand(strsql, con);
            int i = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

    }
}
