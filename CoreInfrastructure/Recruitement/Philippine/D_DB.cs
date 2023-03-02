using Executer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreInfrastructure.Purchase.PurchaseInvoice;

namespace CoreInfrastructure.Recruitement.Philippine
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        public List<M_RecruitementTransmittalDetail> GetDetailsrowsforEdit(string Code)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "select * from RecruitementTransmittal_Detail where  Code= " + Code;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            List<M_RecruitementTransmittalDetail> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_RecruitementTransmittalDetail>();
                foreach (DataRow row in dt.Rows)
                {
                    var fields = new M_RecruitementTransmittalDetail
                    {
                        Code = row["Code"].ToString(),
                        CompanyCode = row["CompanyCode"].ToString(),
                        SeqNo = int.Parse(row["SeqNo"].ToString()),
                        OrderID = row["OrderID"].ToString(),
                        Remarks = row["Remarks"].ToString(),
                        Status = row["Status"].ToString(),

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
            strsql = "select max(cast(Code as bigint)) +1  as Code from RecruitementTransmittal_Header  ";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["Code"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "10001";
            return no;
        }

        public void Set_Header(int Mode, M_RecruitementTransmittalHeader m)
        {

            SqlCommand cmd = new SqlCommand("R_RecruitementTransmittal_Header", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ActionFlag", Mode);
            cmd.Parameters.AddWithValue("@Code", m.Code.ToString());
            cmd.Parameters.AddWithValue("@CompanyCode", m.CompanyCode.ToString());
            cmd.Parameters.AddWithValue("@Agent", m.Agent.ToString());
            cmd.Parameters.AddWithValue("@TransmittalDate", m.TransmittalDate.ToString());
            cmd.Parameters.AddWithValue("@Notes", m.Notes.ToString());
            cmd.Parameters.AddWithValue("@Status", m.Status.ToString());
            cmd.Parameters.AddWithValue("@Sort", m.Sort.ToString());
            cmd.Parameters.AddWithValue("@Locked", m.Locked.ToString());

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void Set_DetailRows(int Mode, M_RecruitementTransmittalDetail m)
        {
            SqlCommand cmd = new SqlCommand("R_RecruitementTransmittal_Detail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ActionFlag", Mode);
            cmd.Parameters.AddWithValue("@Code", m.Code.ToString());
            cmd.Parameters.AddWithValue("@SeqNo", m.SeqNo.ToString());
            cmd.Parameters.AddWithValue("@CompanyCode", m.CompanyCode.ToString());
            cmd.Parameters.AddWithValue("@OrderID", m.OrderID.ToString());
            cmd.Parameters.AddWithValue("@Remarks", m.Remarks.ToString());
            cmd.Parameters.AddWithValue("@Status", m.Status.ToString());

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
            { strsql = "DELETE FROM RecruitementTransmittal_Detail WHERE Code=" + id;}
            else
            {strsql = "DELETE FROM RecruitementTransmittal_Header WHERE Code=" + id;}
            SqlCommand cmd = new SqlCommand(strsql, con);
            int i = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

    }
}
