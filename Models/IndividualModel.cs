using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USF_Health_MVC_EF.Models
{
    public partial class Individual
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int ind_id { get; set; }
        [Column(TypeName = "date")] public DateTime? ind_date_created { get; set; }
        [Column(TypeName = "time")] public TimeSpan? ind_time_created { get; set; }
        public int? usr_id_created { get; set; }
        [Required] [DisplayName("first name")]  public string? ind_first_name { get; set; }
        [Required] [DisplayName("last name")] public string? ind_last_name { get; set; }
        [DisplayName("email address")] [EmailAddress] [DataType(DataType.EmailAddress)] public string? ind_email { get; set; }
        public string? ind_phone { get; set; }
        [Column(TypeName = "date")] public DateTime? ind_birthdate { get; set; }
        [Required] public string? ind_gender { get; set; }
        public string? ind_document { get; set; }
        [DisplayName("reference id")] public int ref_id { get; set; }
        [DisplayName("study id")] public int std_id { get; set; }
        public string? ind_details { get; set; }


        public string GetNumberOfSamplesbyId(int? id)
        {

            SqlDataAdapter dataAdapter = new SqlDataAdapter("[usp_individuals_samples_select]", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter sqlParameter01 = new SqlParameter("type", 2);      //1
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            SqlParameter sqlParameter02 = new SqlParameter("ind_id", id);
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
