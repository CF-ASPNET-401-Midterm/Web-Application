using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int DatListEyeDee { get; set; }

        [Display(Name="Genre")]
        public int DatGenreEyeDee { get; set; }
    }
}
