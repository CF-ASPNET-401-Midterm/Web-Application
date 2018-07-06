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
    public class HomeTests
    {
        [Fact]
        public void CanViewIndex()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
    .UseInMemoryDatabase("HomeViewIndex").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                HomeController testHC = new HomeController(context);

                var result1 = testHC.Index();
                Assert.IsType<ViewResult>(result1);

            }
        }

        [Fact]
        public async void CanViewAbout()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
    .UseInMemoryDatabase("AboutViewIndex").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                HomeController testHC = new HomeController(context);

                var result1 = testHC.AboutOut();
                Assert.IsType<ViewResult>(result1);

                User testUser1 = new User();
                testUser1.Id = 10;
                testUser1.Name = "Bob Saget";
                testUser1.GenreID = 3;
                testUser1.PlaylistID = 4;

                await context.Users.AddAsync(testUser1);
                await context.SaveChangesAsync();

                var result2 = testHC.About(10);
                Assert.IsType<ViewResult>(result2.Result);
            }
        }
    }
}
