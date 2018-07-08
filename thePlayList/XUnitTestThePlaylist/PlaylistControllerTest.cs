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
    public class PlaylistControllerTest
    {
        [Fact]
        public async void CheckingForRedirectWhenNewUserGetToPlaylistDisplayPage()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
               .UseInMemoryDatabase("DisplayPlaylistOne").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                User user1 = new User();
                user1.Name = "username";
                await context.Users.AddAsync(user1);
                await context.SaveChangesAsync();

                PlaylistController plc = new PlaylistController(context);

                var result = await plc.Get(user1.Id);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Null(redirectToActionResult.ControllerName);
                Assert.Equal("Create", redirectToActionResult.ActionName);
            }
        }

        [Fact]
        public async void CheckingForViewWhenFirstTimeDefaultPlaylistUser()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
              .UseInMemoryDatabase("DisplayPlaylistTwo").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                User user = new User();
                user.Name = "username";
                Playlist playlist = new Playlist();
                playlist.Id = 27;

                await context.Users.AddAsync(user);
                await context.Playlists.AddAsync(playlist);
                await context.SaveChangesAsync();

                user.GenreID = 3;
                context.Users.Update(user);
                context.Playlists.Update(playlist);
                await context.SaveChangesAsync();

                PlaylistViewModel plvm = new PlaylistViewModel();
                plvm.User = user;

                PlaylistController plc = new PlaylistController(context);
                var result = await plc.Get(user.Id) as ViewResult;
                var plVMTest = (PlaylistViewModel)result.ViewData.Model;
                var testingResult = plVMTest.User.Id;

                Assert.Equal(user.Id, testingResult);
            }
        }

        [Fact]
        public async void CheckingForViewWhenComingBackCustomPlaylistUser()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
              .UseInMemoryDatabase("DisplayPlaylistThree").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                User user = new User();
                user.Name = "username";

                Playlist playlist = new Playlist();

                await context.Users.AddAsync(user);
                await context.Playlists.AddAsync(playlist);
                await context.SaveChangesAsync();

                user.PlaylistID = playlist.Id.Value;
                playlist.UserID = user.Id;
                context.Users.Update(user);
                context.Playlists.Update(playlist);
                await context.SaveChangesAsync();

                PlaylistViewModel plvm = new PlaylistViewModel();
                plvm.User = user;

                PlaylistController plc = new PlaylistController(context);
                var result = await plc.Get(user.Id) as ViewResult;
                var plVMTest = (PlaylistViewModel)result.ViewData.Model;
                var testingResult = plVMTest.User.Id;

                Assert.Equal(user.Id, testingResult);
            }
        }


        [Fact]
        public async void CheckingForViewWhenComingBackDefaultPlaylistUser()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
              .UseInMemoryDatabase("DisplayPlaylistFour").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                User user = new User();
                user.Name = "username";
                Playlist playlist = new Playlist();
                playlist.Id = 27;

                await context.Users.AddAsync(user);
                await context.Playlists.AddAsync(playlist);
                await context.SaveChangesAsync();

                user.PlaylistID = playlist.Id.Value;
                user.GenreID = 3;
                playlist.UserID = user.Id;
                context.Users.Update(user);
                context.Playlists.Update(playlist);
                await context.SaveChangesAsync();

                PlaylistViewModel plvm = new PlaylistViewModel();
                plvm.User = user;

                PlaylistController plc = new PlaylistController(context);
                var result = await plc.Get(user.Id) as ViewResult;
                var plVMTest = (PlaylistViewModel)result.ViewData.Model;
                var testingResult = plVMTest.User.Id;

                Assert.Equal(user.Id, testingResult);
            }
        }

        [Fact]
        public async void CheckToSeeCustomPlaylistIsBeingCreated()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
             .UseInMemoryDatabase("CreateCustomPlaylist").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                User user2 = new User();
                user2.Name = "username";
                await context.Users.AddAsync(user2);
                await context.SaveChangesAsync();

                PlaylistController plc = new PlaylistController(context);

                var result = await plc.CreateCustomPlaylist(user2.Id);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                //Assert.Null(redirectToActionResult.ControllerName);
                Assert.Equal("Get", redirectToActionResult.ActionName);
            }
        }

        [Fact]
        public async void CheckToSeeIfUsersPlaylistGetsSelected()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
              .UseInMemoryDatabase("FindUserPlaylist").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                User user3 = new User();
                user3.Name = "username";
                Playlist playlist = new Playlist();
                playlist.Id = 27;
                user3.GenreID = 3;

                await context.Users.AddAsync(user3);
                await context.Playlists.AddAsync(playlist);
                await context.SaveChangesAsync();

                user3.PlaylistID = playlist.Id.Value;
                playlist.UserID = user3.Id;
                context.Users.Update(user3);
                context.Playlists.Update(playlist);
                await context.SaveChangesAsync();

                PlaylistViewModel plvm = new PlaylistViewModel();
                plvm.User = user3;

                PlaylistController plc = new PlaylistController(context);
                var result = await plc.Edit(user3);
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Get", redirectToActionResult.ActionName);
            }
        }
            [Fact]
            public async void CheckToSeeIfPlaylistDeletes()
        {
            DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
                .UseInMemoryDatabase("DeletePlaylist").Options;
            using (MusicDbContext context = new MusicDbContext(options))
            {
                User user4 = new User();
                user4.Name = "username";
                Playlist playlist = new Playlist();
                playlist.Id = 27;
                user4.PlaylistID = playlist.Id.Value;
                playlist.UserID = user4.Id;
                user4.GenreID = 3;

                await context.Users.AddAsync(user4);
                await context.Playlists.AddAsync(playlist);
                await context.SaveChangesAsync();

                PlaylistController plc = new PlaylistController(context);
                var result = await plc.Delete(user4.Id);
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Edit", redirectToActionResult.ActionName);
            }
        }
    }
}
