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
    public partial class SpAudit
    {
        public int aud_id { get; set; }
        public int aud_operation_id { get; set; }
        public string? aud_operation { get; set; }
        [Column(TypeName = "date")] public DateTime? aud_date { get; set; }
        [Column(TypeName = "time")] public TimeSpan? aud_time { get; set; }
        public string? aud_table { get; set; }
        public int aud_poo_id { get; set; }
        public string? aud_identifier_id { get; set; }
        public string? aud_identifier_field { get; set; }
        public string? aud_field { get; set; }
        public string? aud_before { get; set; }
        public string? aud_after { get; set; }
        public int usr_id_audit { get; set; }
        public string? usr_username { get; set; }
        public int ssn_id { get; set; }

    }

}
