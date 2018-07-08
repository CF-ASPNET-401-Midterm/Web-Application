using System;
using Xunit;
using thePlayList.Models;
using thePlayList.Data;
using thePlayList.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace XUnitTestThePlaylist
{
    public class ApiSongModelTests
    {
        [Fact]
        public void ApiSongGetterandSetters()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
.UseInMemoryDatabase("GetterSetterSongDB").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                //Arrange
                ApiSong testApiSong1 = new ApiSong();
                testApiSong1.Name = "Gonna Give You Up";
                testApiSong1.Artist = "Slick Rick";
                testApiSong1.Genre = "oldies";
                testApiSong1.PlaylistID = 37;

                testApiSong1.Name = "Just Testing API Songs";
                testApiSong1.Artist = "Ron Testmaster";
                testApiSong1.Genre = "top 40 hits";
                testApiSong1.PlaylistID = 40;
                testApiSong1.ReleaseDate = new DateTime(2018, 05, 5, 14, 1, 1, 111);

                Assert.False(testApiSong1.Equals("Gonna Give You Up"));
                Assert.Equal("Just Testing API Songs", testApiSong1.Name);
                Assert.Equal("Ron Testmaster", testApiSong1.Artist);
                Assert.Equal("top 40 hits", testApiSong1.Genre);
                Assert.Equal(40, testApiSong1.PlaylistID = 40);
                Assert.Equal(new DateTime(2018, 05, 5, 14, 1, 1, 111), testApiSong1.ReleaseDate);
            }
        }
    }
}
