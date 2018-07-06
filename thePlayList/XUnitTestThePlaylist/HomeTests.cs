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
            }
        }
    }
}
