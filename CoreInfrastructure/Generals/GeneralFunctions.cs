using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
 

namespace CoreInfrastructure.Generals
{
  public  class GeneralFunctions
    {
        private SqlConnection _Con;
        public GeneralFunctions(SqlConnection V_connection)
        {
            _Con = V_connection;

        }

        public GeneralFunctions()
        {
           
        }

        public long UpdateTable(string Table, string Column, string Value, string Where)
        {
            if (_Con.State == ConnectionState.Closed)
            {
                _Con.Open();
            }

            string query = "Update " + Table + " Set " + Column + " = " + Value + " , status=1 where " + Where;
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, _Con);
            long l = cmd.ExecuteNonQuery();
            if (_Con.State == ConnectionState.Open)
            {
                _Con.Close();
            }

            return l;


        }
        public DataTable GetDataTableWithQuery(string ls_query)
        {
            if (_Con.State == ConnectionState.Closed)
            {
                _Con.Open();
            }

            string query = ls_query;

            SqlCommand cmd = new SqlCommand(query, _Con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable _dt = new DataTable();
            sda.Fill(_dt);
            if (_Con.State == ConnectionState.Open)
            {
                _Con.Close();
            }

            return _dt;


        }
        public long UpdateTable(string table, string queryUpdate, string where)
        {
            if (_Con.State == ConnectionState.Closed)
            {
                _Con.Open();
            }

            string query = "Update " + table + " Set " + queryUpdate + " where " + where;

            SqlCommand cmd = new SqlCommand(query, _Con);
            long l = cmd.ExecuteNonQuery();
            if (_Con.State == ConnectionState.Open)
            {
                _Con.Close();
            }

            return l;


        }
    }
}
