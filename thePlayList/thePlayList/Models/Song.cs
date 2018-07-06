using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public int? PlaylistId { get; set; }
        public int DatListEyeDee { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
