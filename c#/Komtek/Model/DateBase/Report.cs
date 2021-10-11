using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Komtek.Model.DateBase
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public string Note { get; set; }
        [Range(0, 24, ErrorMessage = "invalid count of hours")]
        public double Hour { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
