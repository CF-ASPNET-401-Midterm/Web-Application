using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace thePlayList.Data
{
    public class MusicDbContext : DbContext
    {
        public MusicDbContext(DbContextOptions<MusicDbContext>option) : base(option)
        {

        }

        //public DbSet<Playlist> Playlists { get; set; }
        //public DbSet<User> Users { Get; set; }
    }
}
