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
    public class SpPlacesSamples
    {
        public int ps_id { get; set; }
        public string? ps_barcode { get; set; }
        public int? pla_id { get; set; }
        public string? pla_name { get; set; }
        public string? pla_location_reference { get; set; }
        public string? pla_campus { get; set; }
        public string? pla_details { get; set; }
        [Column(TypeName = "date")] public DateTime? ps_date_created { get; set; }
        [Column(TypeName = "time")] public TimeSpan? ps_time_created { get; set; }
        public string? ps_date_created_text { get; set; }
        public int? usr_id_created { get; set; }
        [Column(TypeName = "date")] public DateTime? ps_date_collected { get; set; }
        [Column(TypeName = "time")] public TimeSpan? ps_time_collected { get; set; }
        public string? ps_date_collected_text { get; set; }
        public int? usr_id_collected { get; set; }
        [Column(TypeName = "date")] public DateTime? ps_date_registered { get; set; }
        [Column(TypeName = "time")] public TimeSpan? ps_time_registered { get; set; }
        public string? ps_date_registered_text { get; set; }
        public int? usr_id_registered { get; set; }
        public string? ps_well_number { get; set; }
        public string? ps_details { get; set; }
        public string? psres_result { get; set; }
        public string? psres_ct_value { get; set; }
        public int? samples_count { get; set; }
        public int? position { get; set; }
    }

}