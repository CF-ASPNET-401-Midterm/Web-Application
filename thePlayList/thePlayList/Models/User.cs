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

        //public ListEnum DatGenre { get; set; }
        //public Playlist Playlist { get; set; }
    }

    //public enum ListEnum
    //{
    //    Blues = 2,
    //    Comedy = 3,
    //    [Display(Name = "Children's Music")] ChildMusic = 4,
    //    Country = 6,
    //    Holiday = 8,
    //    Opera = 9,
    //    [Display(Name = "Singer/Songwriter")] SingerSongwriter = 10,
    //    Soundtrack= 16,
    //    [Display(Name= "Hip Hop/Rap")] HipHopRap = 18,
    //    Alternative= 20,
    //    Christian & Gospel= 22,
    //    Easy Listening= 25
    //    Instrumental= 53
    //}
}
