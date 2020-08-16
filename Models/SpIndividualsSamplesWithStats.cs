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
    public class SpIndividualsSamplesWithStats
    {
        //public int is_id { get; set; }
        //public string? is_barcode { get; set; }
        //[Column(TypeName = "datetime")] public DateTime? is_date_time_created { get; set; }
        //public string? is_date_created_text { get; set; }
        //[Column(TypeName = "date")] public DateTime? is_date_collected { get; set; }
        //public string? ind_id { get; set; }
        //public string? ind_first_name { get; set; }
        //public string? ind_last_name { get; set; }
        //public string? first_name_last_name { get; set; }
        //public string? ind_email { get; set; }
        //public string? ind_phone { get; set; }
        //[Column(TypeName = "date")] public DateTime? ind_birthdate { get; set; }
        //public string? ind_gender { get; set; }
        //public string? ind_document { get; set; }
        //public int? indcat_id { get; set; }
        //public int std_id { get; set; }
        //public string? std_name { get; set; }
        //public string? ind_details { get; set; }
        //public string? is_date_collected_text { get; set; }
        //public string? is_date_registered_text { get; set; }
        //public string? is_well_number { get; set; }
        //public string? is_details { get; set; }
        //public int? is_count { get; set; }

        public int is_id { get; set; }
        public string? is_barcode { get; set; }
        public int? ind_id { get; set; }
        public string? ind_first_name { get; set; }
        public string? ind_last_name { get; set; }
        public string? first_name_last_name { get; set; }
        public string? ind_gender { get; set; }
        public string? ind_document { get; set; }
        public string? ind_details { get; set; }
        public int? ref_id { get; set; }
        public string? ref_name { get; set; }
        public int? std_id { get; set; }
        public string? std_name { get; set; }
        [Column(TypeName = "date")] public DateTime? is_date_created { get; set; }
        [Column(TypeName = "time")] public TimeSpan? is_time_created { get; set; }
        public string? is_date_created_text { get; set; }
        [Column(TypeName = "date")] public DateTime? is_date_collected { get; set; }
        [Column(TypeName = "time")] public TimeSpan? is_time_collected { get; set; }
        public string? is_date_collected_text { get; set; }
        [Column(TypeName = "date")] public DateTime? is_date_registered { get; set; }
        [Column(TypeName = "time")] public TimeSpan? is_time_registered { get; set; }
        public string? is_date_registered_text { get; set; }
        public string? is_details { get; set; }
        public int? poo_id { get; set; }
        public string? poo_details { get; set; }
        [Column(TypeName = "date")] public DateTime? is_date_registered_pool { get; set; }
        [Column(TypeName = "time")] public TimeSpan? is_time_registered_pool { get; set; }
        public string? is_date_registered_pool_text { get; set; }
        public int? samples_count { get; set; }
        public int? position { get; set; }
        public string? is_well_number { get; set; }
        public string? pr_result { get; set; }
        public string? pr_ct_value { get; set; }
        public int? is_count { get; set; }
    }

}