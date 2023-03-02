
using Executer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreInfrastructure.EmployeesProblems;
using System.Net.Sockets;
using System.Net;

namespace CoreInfrastructure.Hr.Setup.Attendance
{
    public class D_DB
    {
        private SqlConnection con;

        public D_DB(SqlConnection V_connection)
        {
            con = V_connection;

        }
        public List<M_HrAttendance_Detail> GetDetailsrowsforEdit(string CODE)
        {
            DataProcess dp = new DataProcess();

            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            strsql = "SELECT CompanyCode, Code, SeqNo, (Select Name from HR_EMPLOYEE where Code=EmployeeID) as EmployeeID, AttendanceDate, EntryTime, ExitTime, LateTime, ExtraTime, EarlyDismissalTime, Notes, Status, Sort, Locked FROM HrAttendance_Detail where Code= " + CODE;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            List<M_HrAttendance_Detail> list = null;
            if (dt.Rows.Count > 0)
            {
                list = new List<M_HrAttendance_Detail>();
                foreach (DataRow row in dt.Rows)
                {
                    var fields = new M_HrAttendance_Detail
                    {
                        Code = row["Code"].ToString(),
                        CompanyCode = row["CompanyCode"].ToString(),
                        SeqNo = row["SeqNo"].ToString(),
                        EmployeeID = row["EmployeeID"].ToString(),
                        AttendanceDate = DateTime.Parse(row["AttendanceDate"].ToString()),
                        EarlyDismissalTime = (TimeSpan?)row["EarlyDismissalTime"],
                        EntryTime = (TimeSpan?)row["EntryTime"],
                        ExitTime = (TimeSpan?)row["ExitTime"],
                        LateTime = (TimeSpan?)row["LateTime"],
                        ExtraTime = (TimeSpan?)row["ExtraTime"],
                        Notes = row["Notes"].ToString(),
                        Status = row["Status"].ToString(),
                        Sort = row["Sort"].ToString(),
                        Locked = row["Locked"].ToString(),


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
        public void UpdateDetailAttendence(string Code, string SeqNo, TimeSpan? ExtraTime, TimeSpan? EarlyDismissalTime)

        {
            string queryString = "UPDATE HrAttendance_Detail " +
                                "SET ExitTime = @ExitTime, " +
                                "Status = @Status ," +
                                "ExtraTime = @ExtraTime ," +
                                "EarlyDismissalTime = @EarlyDismissalTime " +
                                "WHERE Code = @Code AND SeqNo = @SeqNo";


            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            using (SqlCommand cmd = new SqlCommand(queryString, con))
            {
                cmd.Parameters.AddWithValue("@ExitTime", DateTime.Now.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Status", "0");
                cmd.Parameters.AddWithValue("@Code", Code);
                cmd.Parameters.AddWithValue("@SeqNo", SeqNo);
                cmd.Parameters.AddWithValue("@ExtraTime", ExtraTime);
                cmd.Parameters.AddWithValue("@EarlyDismissalTime", EarlyDismissalTime);

                cmd.ExecuteNonQuery();
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public M_HrAttendance_Detail FindRow(string employeeID, string fdate)
        {
            SqlDataReader row;
            M_HrAttendance_Detail obj = null;
            string queryString = "SELECT TOP (1)  CompanyCode, Code, SeqNo, EmployeeID, AttendanceDate, EntryTime, ExitTime, LateTime, ExtraTime, EarlyDismissalTime, Notes, Status, Sort, Locked " +
                     "FROM  HrAttendance_Detail " +
                     "WHERE EmployeeID = '" + employeeID + "' AND AttendanceDate = '" + fdate + "' " +
                     "ORDER BY CAST(SeqNo AS INT) DESC";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(queryString, con);
            row = cmd.ExecuteReader();
            if (row.Read())
            {
                obj = new M_HrAttendance_Detail
                {
                    Code = row["Code"].ToString(),
                    CompanyCode = row["CompanyCode"].ToString(),
                    SeqNo = row["SeqNo"].ToString(),
                    EmployeeID = row["EmployeeID"].ToString(),
                    AttendanceDate = DateTime.Parse(row["AttendanceDate"].ToString()),
                    EarlyDismissalTime = (TimeSpan?)row["EarlyDismissalTime"],
                    EntryTime = (TimeSpan?)row["EntryTime"],
                    ExitTime = (TimeSpan?)row["ExitTime"],
                    LateTime = (TimeSpan?)row["LateTime"],
                    ExtraTime = (TimeSpan?)row["ExtraTime"],
                    Notes = row["Notes"].ToString(),
                    Status = row["Status"].ToString(),
                    Sort = row["Sort"].ToString(),
                    Locked = row["Locked"].ToString(),
                };
            }
            row.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return obj;
        }

        public M_HrShiftDetails GetShiftDetails(string ShiftCode)
        {
            SqlDataReader row;
            M_HrShiftDetails obj = null;
            string queryString = "SELECT TOP (1)  * FROM  HrShiftDetails    where Code=" + ShiftCode;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(queryString, con);
            row = cmd.ExecuteReader();
            if (row.Read())
            {
                obj = new M_HrShiftDetails
                {
                    Code = row["Code"].ToString(),
                    ShiftCode = row["ShiftCode"].ToString(),
                    ShiftName = row["ShiftName"].ToString(),
                    Address = row["Address"].ToString(),
                    StartTime = row["StartTime"].ToString(),
                    EndTime = row["EndTime"].ToString(),
                    LateAllowed = row["LateAllowed"].ToString(),
                    DateAdded = row["DateAdded"].ToString(),
                    Status = row["Status"].ToString()

                };
            }
            row.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return obj;
        }
        public string GetShift(string EmpCode)
        {

            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "SELECT   ShiftId   FROM HR_EMPLOYEE where Code=" + EmpCode;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["ShiftId"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "";
            return no;
        }
        public string getUpdateMasterCount()
        {
            // Console.WriteLine("HELOO I AM HERE");
            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(Code as bigint)) +1  as Code from HrAttendance_Header  ";

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
        public string getUpdateSeqNoCount(String Code)
        {

            SqlDataReader sqldr;
            string strsql;
            string no = "";
            strsql = "select  max(  cast(SeqNo as bigint)) +1  as SeqNo from HrAttendance_Detail where Code= " + Code;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(strsql, con);
            sqldr = cmd.ExecuteReader();
            if (sqldr.Read())
            {
                no = sqldr["SeqNo"].ToString().Trim();
            }
            sqldr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (no.Trim() == "") no = "1";
            return no;
        }
        public void Set_Header(int Mode, M_HrAttendance_Header m)
        {

            SqlCommand cmd = new SqlCommand("SP_HrAttendance_Header", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ActionFlag", Mode);
            cmd.Parameters.AddWithValue("@Code", m.Code.ToString());
            cmd.Parameters.AddWithValue("@CompanyCode", m.CompanyCode.ToString() ?? "");
            cmd.Parameters.AddWithValue("@EmployeeID", m.EmployeeID.ToString() ?? "");
            cmd.Parameters.AddWithValue("@AttendanceDate", m.AttendanceDate.ToString() ?? "");
            cmd.Parameters.AddWithValue("@LastEntryTime", m.LastEntryTime.ToString() ?? "");
            cmd.Parameters.AddWithValue("@LastExitTime", m.LastExitTime.ToString() ?? "");
            cmd.Parameters.AddWithValue("@ShiftID", m.ShiftID.ToString() ?? "");
            cmd.Parameters.AddWithValue("@Notes", m.Notes.ToString() ?? "");

            cmd.Parameters.AddWithValue("@Status", m.Status.ToString() ?? "");
            cmd.Parameters.AddWithValue("@Sort", m.Sort.ToString() ?? "");
            cmd.Parameters.AddWithValue("@Locked", m.Locked.ToString() ?? "");

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void Set_DetailRows(int Mode, M_HrAttendance_Detail m)
        {
            SqlCommand cmd = new SqlCommand("SP_HrAttendance_Detail", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ActionFlag", Mode);
            cmd.Parameters.AddWithValue("@Code", m.Code.ToString());
            cmd.Parameters.AddWithValue("@CompanyCode", m.CompanyCode.ToString());
            cmd.Parameters.AddWithValue("@SeqNo", m.SeqNo.ToString());
            cmd.Parameters.AddWithValue("@EmployeeID", m.EmployeeID.ToString());
            cmd.Parameters.AddWithValue("@AttendanceDate", m.AttendanceDate.ToString());
            cmd.Parameters.AddWithValue("@EntryTime", m.EntryTime.ToString());
            cmd.Parameters.AddWithValue("@ExitTime", m.ExitTime.ToString());
            cmd.Parameters.AddWithValue("@LateTime", m.LateTime.ToString());
            cmd.Parameters.AddWithValue("@ExtraTime", m.ExtraTime.ToString());
            cmd.Parameters.AddWithValue("@EarlyDismissalTime", m.EarlyDismissalTime.ToString());
            cmd.Parameters.AddWithValue("@Notes", m.Notes.ToString());
            cmd.Parameters.AddWithValue("@Status", m.Status.ToString());
            cmd.Parameters.AddWithValue("@Sort", m.Sort.ToString());
            cmd.Parameters.AddWithValue("@Locked", m.Locked.ToString());

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void deleteData(string pera, string id)
        {
            string strsql;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            if (pera == "detailrows")
            { strsql = "DELETE FROM HrAttendance_Detail WHERE Code=" + id; }
            else
            { strsql = "DELETE FROM HrAttendance_Header WHERE Code=" + id; }
            SqlCommand cmd = new SqlCommand(strsql, con);
            int i = cmd.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}
