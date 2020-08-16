using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USF_Health_MVC_EF.Models
{
    public partial class Pool
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int poo_id { get; set; }
        [Column(TypeName = "date")] public DateTime? poo_date_created { get; set; }
        [Column(TypeName = "time")] public TimeSpan? poo_time_created { get; set; }
        public string? poo_details { get; set; }

    }


}
