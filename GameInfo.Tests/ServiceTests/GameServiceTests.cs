using GameInfo.Core.Models.Entities;
using GameInfo.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameInfo.Tests.ServiceTests
{
    [TestClass]
    [TestCategory("GameService")]
    public class GameServiceTests: BaseServiceTests
    {
        [TestMethod]
        public async Task GetGames_Valid_Returns_List()
        {
            // Arrange
            var games = new List<Game>();
            games.Add(new Game { Id = 1 });
            var gameList = games.AsQueryable().BuildMock();

            _Repo.Setup(x => x.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(gameList.Object);

            var gameService = new GameService(_Repo.Object);
            // Act
            var results = await gameService.GetGames(20, 1);
            var id = results.Value.Select(x => x.Id).First();
            // Assert
            Assert.IsNotNull(results.Value);
            Assert.AreEqual(id, 1);
        }
        [TestMethod]
        public async Task GetGames_Valid_Returns_EmptyList()
        {
            // Arrange
            var games = new List<Game>();
            var gameList = games.AsQueryable().BuildMock();

            _Repo.Setup(x => x.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(gameList.Object);

            var gameService = new GameService(_Repo.Object);
            // Act
            var results = await gameService.GetGames(20, 1);

            // Assert
            Assert.IsNull(results.Value);
        }
    }
}
