﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using thePlayList.Models;

namespace thePlayList.Data
{
    public class MusicDbContext : DbContext
    {
        public MusicDbContext(DbContextOptions<MusicDbContext>options) : base(options)
        {

        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Song { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PlAndUserJoins> Joins { get; set; }
    }
}
