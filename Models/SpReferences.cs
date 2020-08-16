using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace USF_Health_MVC_EF.Models
{
    public class SpReferences
    {
        public int ref_id { get; set; }
        public string? ref_n { get; set; }
        public string? ref_name { get; set; }
        public string? ref_details { get; set; }


        public List<SpReferences> GetAllReferences(int? type, int? ref_id)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_references_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter sqlParameter01;
            SqlParameter sqlParameter02;

            if (type == 1)  {

                sqlParameter01 = new SqlParameter("type", 1);
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

                sqlParameter02 = new SqlParameter("ref_id", 0);
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            } else if (type == 2)   {

                sqlParameter01 = new SqlParameter("type", 2);
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

                sqlParameter02 = new SqlParameter("ref_id", ref_id);
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            }



            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpReferences> list = new List<SpReferences>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpReferences item = new SpReferences();
                DataRow dr = dataTable.Rows[i];

                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_n = dr["ref_n"].ToString();
                item.ref_name = dr["ref_name"].ToString();
  
                list.Add(item);
            }

            return list;
        }

        public string GetReferenceNamebyId(int? ref_id)
        {

            //SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_studies_select", Globals.connection);
            //dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //SqlParameter sqlParameter01 = new SqlParameter("std_id", id);
            //dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            //System.Data.DataTable dataTable = new System.Data.DataTable();

            //dataAdapter.Fill(dataTable);

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_references_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter sqlParameter01;
            SqlParameter sqlParameter02;

            sqlParameter01 = new SqlParameter("type", 3);
            sqlParameter01.IsNullable = false;
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

            sqlParameter02 = new SqlParameter("ref_id", ref_id);
            sqlParameter02.IsNullable = false;
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            return dataTable.Rows[0]["ref_name"].ToString();
        }


    }
}
