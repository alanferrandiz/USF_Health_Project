using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;
using System.Data;

namespace USF_Health_MVC_EF.Models
{
    public class SpIndividuals
    {
        public int ind_id { get; set; }
        [Column(TypeName = "date")] public DateTime? ind_date_created { get; set; }
        [Column(TypeName = "time")] public TimeSpan? ind_time_created { get; set; }
        public string? ind_date_created_text { get; set; }
        public string? ind_first_name { get; set; }
        public string? ind_last_name { get; set; }
        public string? first_name_last_name { get; set; }
        public string? last_name_first_name_id { get; set; }
        public string? ind_email { get; set; }
        public string? ind_phone { get; set; }
        [Column(TypeName = "date")] public DateTime? ind_birthdate { get; set; }
        public string? ind_gender { get; set; }
        public string? ind_document { get; set; }
        public int? indcat_id { get; set; }
        public int? ref_id { get; set; }
        public string? ref_name { get; set; }
        public int? std_id { get; set; }
        public string? std_name { get; set; }
        public string? ind_details { get; set; }
        public int? is_count { get; set; }






        public List<SpIndividuals> GetAllIndividuals()
        {

            //SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_select", Globals.connection);
            //dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //System.Data.DataTable dataTable = new System.Data.DataTable();

            //dataAdapter.Fill(dataTable);

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_individuals_select";

            SqlParameter sqlParameter01 = new SqlParameter("type", 4);
            sqlParameter01.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand); //I think the problem is here, how am I fixing it though? --Fixed but why can't it be put into a class?
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpIndividuals> list = new List<SpIndividuals>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpIndividuals item = new SpIndividuals();
                DataRow dr = dataTable.Rows[i];

                item.ind_id = Int32.Parse(dr["ind_id"].ToString());
                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_name = dr["ref_name"].ToString();
                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_name = dr["std_name"].ToString();
                item.last_name_first_name_id = dr["last_name_first_name_id"].ToString();

                list.Add(item);
            }

            return list;
        }

        public SpIndividuals GetIndividualbyId(int? id)
        {

            //SqlDataAdapter dataAdapter = new SqlDataAdapter("[usp_individuals_select]", Globals.connection);
            //dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //SqlParameter sqlParameter01 = new SqlParameter("ind_id", id);
            //dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            //System.Data.DataTable dataTable = new System.Data.DataTable();

            //dataAdapter.Fill(dataTable);
            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_individuals_select";

            SqlParameter sqlParameter01 = new SqlParameter("type", 1);
            sqlParameter01.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter01);


            SqlParameter sqlParameter02 = new SqlParameter("ind_id", id);
            sqlParameter02.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand); //I think the problem is here, how am I fixing it though? --Fixed but why can't it be put into a class?
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            SpIndividuals spIndividuals = new SpIndividuals();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dr = dataTable.Rows[i];

                spIndividuals.ind_id = Int32.Parse(dr["ind_id"].ToString());
                spIndividuals.ind_first_name = dr["ind_first_name"].ToString();
                spIndividuals.ind_last_name = dr["ind_last_name"].ToString();
                spIndividuals.ind_gender = dr["ind_gender"].ToString();
                spIndividuals.ind_document = dr["ind_document"].ToString();
                spIndividuals.ref_id = Int32.Parse(dr["ref_id"].ToString());
                spIndividuals.ref_name = dr["ref_name"].ToString();
                spIndividuals.std_id = Int32.Parse(dr["std_id"].ToString());
                spIndividuals.std_name = dr["std_name"].ToString();
            }

            return spIndividuals;
        }






    }
}