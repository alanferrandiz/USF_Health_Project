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
    public class SpPlaces
    {
        public int pla_id { get; set; }
        [Column(TypeName = "date")] public DateTime? pla_date_created { get; set; }
        [Column(TypeName = "time")] public TimeSpan? pla_time_created { get; set; }
        public string? pla_date_created_text { get; set; }
        public int usr_id_created { get; set; }
        public string? pla_name { get; set; }
        public string? name_id { get; set; }
        public string? pla_location_reference { get; set; }
        public string? pla_campus { get; set; }
        public string? pla_details { get; set; }
        public int? ps_count { get; set; }


        public List<SpPlaces> GetAllPlaces()
        {

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpPlaces> list = new List<SpPlaces>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpPlaces item = new SpPlaces();
                DataRow dr = dataTable.Rows[i];

                item.pla_id = Int32.Parse(dr["pla_id"].ToString());
                item.pla_date_created = dr["pla_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["pla_date_created"];
                item.pla_time_created = dr["pla_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["pla_time_created"];
                item.pla_name = dr["pla_name"].ToString();
                item.name_id = dr["name_id"].ToString();
                item.pla_location_reference = dr["pla_location_reference"].ToString();
                item.pla_campus = dr["pla_campus"].ToString();
                item.pla_details = dr["pla_details"].ToString();

                list.Add(item);
            }

            return list;
        }

        public SpPlaces GetPlacebyId(int? id)
        {

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter sqlParameter01 = new SqlParameter("pla_id", id);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            SpPlaces spPlaces = new SpPlaces();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dr = dataTable.Rows[i];

                spPlaces.pla_id = Int32.Parse(dr["pla_id"].ToString());
                spPlaces.pla_date_created = dr["pla_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["pla_date_created"];
                spPlaces.pla_time_created = dr["pla_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["pla_time_created"];
                spPlaces.usr_id_created = Int32.Parse(dr["usr_id_created"].ToString());
                spPlaces.pla_name = dr["pla_name"].ToString();
                spPlaces.name_id = dr["name_id"].ToString();
                spPlaces.pla_location_reference = dr["pla_location_reference"].ToString();
                spPlaces.pla_campus = dr["pla_campus"].ToString();
                spPlaces.pla_details = dr["pla_details"].ToString();
            }

            return spPlaces;
        }






    }
}