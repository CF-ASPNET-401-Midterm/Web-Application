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
    public class UserTests
    {
       
        [Fact]
        public async void CanCreateWithGet()
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
                var result = testUC.Get();

                // Assert
                Assert.IsType<ViewResult>(result);
                await testUC.Get(testUser1.Name);

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

        [Fact]
        public async void CanViewInfo()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
    .UseInMemoryDatabase("ViewInfoDB").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                // arrange
                User testUser1 = new User();
                testUser1.Id = 10;
                testUser1.Name = "Bob";

                UserController testUC = new UserController(context);

                await context.Users.AddAsync(testUser1);
                await context.SaveChangesAsync();

                // act
                var result1 = testUC.Info(10);
                var result2 = testUC.Info(555);

                // Assert
                Assert.IsType<ViewResult>(result1.Result);
                Assert.IsType<RedirectToActionResult>(result2.Result);
            }
        }

        [Fact]
        public async void CanEditUser()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
.UseInMemoryDatabase("CanEditDB").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                // arrange
                User testUser1 = new User();
                testUser1.Id = 20;
                testUser1.Name = "Bob";
                testUser1.GenreID = 2;
                testUser1.PlaylistID = 3;

                await context.Users.AddAsync(testUser1);
                await context.SaveChangesAsync();

                UserController testUC = new UserController(context);

                // act
                var getResult1 = context.Users.Where(u => u.Name == "Bob");

                // Assert
                Assert.Equal(1, getResult1.Count());

                var result2 = await testUC.Edit(20, "Bobby");
                var getResult2 = context.Users.Where(u => u.Name == "Bobby");
                var getResult3 = context.Users.Where(u => u.GenreID == 2);
                var getResult4 = context.Users.Where(u => u.PlaylistID == 3);

                Assert.Equal(0, getResult1.Count());
                Assert.Equal(1, getResult2.Count());
                Assert.Equal(getResult2, getResult3);
                Assert.Equal(getResult2, getResult4);
            }
        }

        [Fact]
        public async void CanDeleteUser()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
.UseInMemoryDatabase("CanDeleteDB").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                // arrange
                User testUser1 = new User();
                testUser1.Id = 30;
                testUser1.Name = "Bill";

                await context.Users.AddAsync(testUser1);
                await context.SaveChangesAsync();

                UserController testUC = new UserController(context);

                // act
                var getResult1 = context.Users.Where(u => u.Name == "Bill");

                // Assert
                Assert.Equal(1, getResult1.Count());

                var result2 = await testUC.Delete(30);

                Assert.Equal(0, getResult1.Count());
            }
        }

        [Fact]
        public async void UserGetterandSetters()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
.UseInMemoryDatabase("GetterSetterUserDB").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                //Arrange
                User testUser1 = new User();
                //testUser1.Id = 40;
                testUser1.Name = "Bob";
                testUser1.GenreID = 2;
                testUser1.PlaylistID = 3;

                await context.Users.AddAsync(testUser1);
                await context.SaveChangesAsync();

                var dbTestUser = await context.Users.FirstOrDefaultAsync(u => u.Name == "Bob");
                //dbTestUser.Id = 50;
                dbTestUser.Name = "Bobby Brown";
                dbTestUser.GenreID = 4;
                dbTestUser.PlaylistID = 5;

                context.Update(dbTestUser);
                await context.SaveChangesAsync();

                //var result1 = await context.Users.FirstOrDefaultAsync(u => u.Id == 30);
                var result1 = context.Users.Where(u => u.Name == "Bobby Brown");
                //var result3 = context.Users.Where(u => u.GenreID == 4);
                //var result4 = context.Users.Where(u => u.PlaylistID == 4);
                var result2 = context.Users.Where(u => u.Name == "Bob");

                Assert.NotNull(result1);
                Assert.Equal(0, result2.Count());
                Assert.Equal("Bobby Brown", dbTestUser.Name);
                Assert.Equal(4, dbTestUser.GenreID);
                Assert.Equal(5, dbTestUser.PlaylistID);
            }
        }
    }
}
