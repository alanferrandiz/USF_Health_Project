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
    public partial class PlaceSample
    {
        public int ps_id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]  public string? ps_barcode { get; set; }
        [DisplayName("date created")] [Column(TypeName = "date")] public DateTime? ps_date_created { get; set; }
        [DisplayName("time created")] [Column(TypeName = "time")] public TimeSpan? ps_time_created { get; set; }
        [Column(TypeName = "int")] public int? usr_id_created { get; set; }
        [DisplayName("date collected")] [Column(TypeName = "date")] public DateTime? ps_date_collected { get; set; }
        [DisplayName("time collected")] [Column(TypeName = "time")] public TimeSpan? ps_time_collected { get; set; }
        [Column(TypeName = "int")] public int? usr_id_collected { get; set; }
        [DisplayName("date registered")] [Column(TypeName = "date")] public DateTime? ps_date_registered { get; set; }
        [DisplayName("time registered")] [Column(TypeName = "time")] public TimeSpan? ps_time_registered { get; set; }
        [Column(TypeName = "int")] public int? usr_id_registered { get; set; }
        [DisplayName("place id")] [Column(TypeName = "int")] public int? pla_id { get; set; }
        [DisplayName("well number")] public string? ps_well_number { get; set; }
        [DisplayName("sample details")] public string? ps_details { get; set; }

    }


}
