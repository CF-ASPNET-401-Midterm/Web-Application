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
    public class SongModelTests
    {
        [Fact]
        public async void SongGetterandSetters()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
.UseInMemoryDatabase("GetterSetterSongDB").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                //Arrange
                Song testSong1 = new Song();
                //testUser1.Id = 40;
                testSong1.Name = "Gonna Give You Up";
                testSong1.Artist = "Slick Rick";
                testSong1.Genre = "oldies";
                testSong1.ApiListId = 33;
                testSong1.OurListId = 1;

                await context.Songs.AddAsync(testSong1);
                await context.SaveChangesAsync();

                var dbTestSong = await context.Songs.FirstOrDefaultAsync(u => u.Name == "Gonna Give You Up");
                //dbTestUser.Id = 50;
                dbTestSong.Name = "Never Gonna Give You Up";
                testSong1.Artist = "Rick Astley";
                testSong1.Genre = "memerific oldies";
                testSong1.ApiListId = 34;
                testSong1.OurListId = 2;
                testSong1.ReleaseDate = new DateTime(2009, 09, 3, 14, 8, 5, 123);
                context.Update(dbTestSong);
                await context.SaveChangesAsync();

                //var result1 = await context.Users.FirstOrDefaultAsync(u => u.Id == 30);
                var result1 = await context.Songs.FirstOrDefaultAsync(s => s.Artist == "Rick Astley");
                var result2 = context.Songs.Where(s => s.Artist == "Slick Rick");

                Assert.NotNull(result1);
                Assert.Equal(0, result2.Count());
                Assert.Equal("Never Gonna Give You Up", result1.Name);
                Assert.Equal("memerific oldies", result1.Genre);
                Assert.Equal(34, result1.ApiListId);
                Assert.Equal(2, result1.OurListId);
                Assert.Equal(new DateTime(2009, 09, 3, 14, 8, 5, 123), result1.ReleaseDate);
            }
        }
    }
}
