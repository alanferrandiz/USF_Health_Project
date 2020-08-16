using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;

namespace USF_Health_MVC_EF.Models
{
    public partial class IndividualSample
    {
        public int is_id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]  public string? is_barcode { get; set; }
        [DisplayName("date created")] [Column(TypeName = "date")] public DateTime? is_date_created { get; set; }
        [DisplayName("time created")] [Column(TypeName = "time")] public TimeSpan? is_time_created { get; set; }
        [DisplayName("date collected")] [Column(TypeName = "date")] public DateTime? is_date_collected { get; set; }
        [DisplayName("time collected")] [Column(TypeName = "time")] public TimeSpan? is_time_collected { get; set; }
        [DisplayName("date registered")] [Column(TypeName = "date")] public DateTime? is_date_registered { get; set; }
        [DisplayName("time registered")] [Column(TypeName = "time")] public TimeSpan? is_time_registered { get; set; }
        [DisplayName("individual id")] [Column(TypeName = "int")] public int? ind_id { get; set; }
        [DisplayName("pool id")] [Column(TypeName = "int")] public int? poo_id { get; set; }
        [DisplayName("well number")] public string? is_well_number { get; set; }
        [DisplayName("sample details")] public string? is_details { get; set; }



    }


}
