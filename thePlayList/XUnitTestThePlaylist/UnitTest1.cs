using System;
using Xunit;
using thePlayList.Models;
using thePlayList.Data;
using thePlayList.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace XUnitTestThePlaylist
{
    public class UnitTest1
    {
        [Fact]
        public async void CanCreateNewUserFromHome()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
                .UseInMemoryDatabase("CreateAndDeleteUserDB").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                // arrange
                User testUser1 = new User();
                testUser1.Name = "Bob";

                User testUser2 = new User();
                testUser2.Name = "Todd";

                UserController testUC = new UserController(context);

                // act
                await testUC.Get("Bob");

                var results1 = context.Users.Where(u => u.Name == "Bob");
                var results2 = context.Users.Where(u => u.Name == "Todd");

                // assert there is one instance of the results
                Assert.Equal(1, results1.Count());
                Assert.Equal(0, results2.Count());

                await testUC.Get("Bob");

                // assert that there is still one instance of results
                Assert.Equal(1, results1.Count());
            }
        }

    //    [Fact]
    //    public async void CanEditUser()
    //    {
    //        DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
    //.UseInMemoryDatabase("EditUserDB").Options;
    //        using (MusicDbContext context = new MusicDbContext(options))
    //        {
                
    //        }
    //    }
    }
}
