using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using VirtualRealityEscapeRooms.Data;
using VirtualRealityEscapeRooms.Models;
using VR_escape_rooms.modeli;
using VirtualRealityEscapeRooms;
using Xunit;

namespace VirtualRealityEscapeRooms.Tests
{
    public class IgrasControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private UserManager<ApplicationUser> GetMockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null).Object;
        }

        [Fact]
        public async Task Index_ReturnsViewWithGames()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();

            // ✅ Fix: Use a valid Zanr enum value (Replace 'Horor' with 'Avantura' or add 'Horor' to the Enum)
            context.Igre.Add(new Igra { ID = 1, Naziv = "Test Igra", Tezina = Tezina.Srednje, Trajanje = 60, Opis = "Opis", Zanr = Zanr.Avantura });
            context.SaveChanges();

            var controller = new IgrasController(context, userManager);

            // Act
            var result = await controller.Index() as ViewResult;
            var model = result?.Model as List<Igra>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Single(model);
            Assert.Equal("Test Igra", model.First().Naziv);
        }

        [Fact]
        public async Task Create_AddsGameToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var controller = new IgrasController(context, userManager);

            var game = new Igra { ID = 1, Naziv = "Nova Igra", Tezina = Tezina.Tesko, Trajanje = 120, Opis = "Teška igra", Zanr = Zanr.Akcija };

            // Act
            var result = await controller.Create(game) as RedirectToActionResult;

            // Assert
            Assert.Equal(nameof(controller.Index), result.ActionName);
            Assert.Single(context.Igre);
            Assert.Equal("Nova Igra", context.Igre.First().Naziv);
        }
    }
}
