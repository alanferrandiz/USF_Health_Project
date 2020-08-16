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
    public class SpPoolsWithStats
    {
        public int poo_id { get; set; }
        [Column(TypeName = "date")] public DateTime? poo_date_created { get; set; }
        [Column(TypeName = "time")] public TimeSpan? poo_time_created { get; set; }
        public string? poo_date_created_text { get; set; }
        public string? pr_result { get; set; }
        public string? pr_ct_value { get; set; }
        public string? poo_details { get; set; }
        public int? poo_count { get; set; }

    }
}