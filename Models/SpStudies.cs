using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace USF_Health_MVC_EF.Models
{
    public class SpStudies
    {
        public int std_id { get; set; }
        public string? std_n { get; set; }
        public string? std_name { get; set; }
        public string? std_details { get; set; }


        public List<SpStudies> GetAllStudies(int? type, int? std_id)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_studies_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter sqlParameter01;
            SqlParameter sqlParameter02;

            if (type == 1)  {

                sqlParameter01 = new SqlParameter("type", 1);
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

                sqlParameter02 = new SqlParameter("std_id", 0);
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            } else if (type == 2)   {

                sqlParameter01 = new SqlParameter("type", 2);
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

                sqlParameter02 = new SqlParameter("std_id", std_id);
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            }



            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpStudies> list = new List<SpStudies>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpStudies item = new SpStudies();
                DataRow dr = dataTable.Rows[i];

                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_n = dr["std_n"].ToString();
                item.std_name = dr["std_name"].ToString();
  
                list.Add(item);
            }

            return list;
        }

        public string GetStudyNamebyId(int? std_id)
        {

            //SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_studies_select", Globals.connection);
            //dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //SqlParameter sqlParameter01 = new SqlParameter("std_id", id);
            //dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            //System.Data.DataTable dataTable = new System.Data.DataTable();

            //dataAdapter.Fill(dataTable);

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_studies_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter sqlParameter01;
            SqlParameter sqlParameter02;

            sqlParameter01 = new SqlParameter("type", 3);
            sqlParameter01.IsNullable = false;
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

            sqlParameter02 = new SqlParameter("std_id", std_id);
            sqlParameter01.IsNullable = false;
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            return dataTable.Rows[0]["std_name"].ToString();
        }


    }
}
