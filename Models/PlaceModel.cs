using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USF_Health_MVC_EF.Models
{
    public partial class Place
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int pla_id { get; set; }
        [Column(TypeName = "date")] public DateTime? pla_date_created { get; set; }
        [Column(TypeName = "time")] public TimeSpan? pla_time_created { get; set; }
        public int? usr_id_created { get; set;}
        public string? pla_name { get; set; }
        public string? pla_location_reference { get; set; }
        public string? pla_campus { get; set; }
        public string? pla_details { get; set; }


        public string GetNumberOfSamplesbyId(int? id)
        {

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_samples_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter sqlParameter01 = new SqlParameter("type", 2);      //1
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            SqlParameter sqlParameter02 = new SqlParameter("pla_id", id);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            string samples_count = "0";

            if (dataTable.Rows.Count > 0)
            {
                samples_count = dataTable.Rows[0]["samples_count"].ToString() + " (all samples will be deleted)";
            }

            return samples_count.ToString();
        }

    }


}
