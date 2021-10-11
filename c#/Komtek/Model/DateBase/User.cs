using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Komtek.Model.DateBase
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress(ErrorMessage = "invalid mail")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "surname cannot be empty")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "invalid surname")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "name cannot be empty")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "invalid name")]
        public string Name { get; set; }
        public string Patrunomic { get; set; }
        [JsonIgnore]
        public List<Report> Report { get; set; } = new();
    }
}
