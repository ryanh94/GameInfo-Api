using GameInfo.Core.Interfaces;
using GameInfo.Core.Models;
using GameInfo.Core.Models.Entities;
using GameInfo.Core.Models.Requests;
using GameInfo.Core.Models.Response;
using GameInfo.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameInfo.Infrastructure.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository _repo;
        public GameService(IRepository repo)
        {
            _repo = repo;
        }
        public async Task<ServiceResult<List<GameResponse>>> GetGames(int pageSize, int pageNumber)
        {
            var result = new ServiceResult<List<GameResponse>>();
            var repo = _repo;
            var skip = (pageNumber - 1) * pageSize;
            var games = await repo.Get<Game>(x => x.Id > 0).Skip(skip).Select(x => new GameResponse { Id = x.Id, Name = x.Name, Rating = x.Rating, ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy"), Description = x.Description }).Take(pageSize).ToListAsync();

            result.Success = true;
            result.Value = new List<GameResponse>(games);
            return result;
        }
        public async Task<ServiceResult<string>> UpdateGame(GameUpdateRequest update)
        {
            var result = new ServiceResult<string>();
            var repo = _repo;
            var exists = await repo.Get<Game>(game => game.Id == update.Id).FirstOrDefaultAsync();
            if (exists != null)
            {
                exists.Name = !string.IsNullOrEmpty(update.Name) ? update.Name : exists.Name;
                exists.Rating = update.Rating != 0 ? update.Rating : exists.Rating;
                exists.Description = !string.IsNullOrEmpty(update.Description) ? update.Description : exists.Description;
                exists.ReleaseDate = update.ReleaseDate.HasValue ? update.ReleaseDate.Value : exists.ReleaseDate;
                repo.Update(exists);
                result.Value = new string($"Successfully updated {update.Name}");
                result.Success = true;
                await repo.Commit();
            }
            return result;
        }
        public async Task<ServiceResult<string>> DeleteGame(int id)
        {
            var result = new ServiceResult<string>();
            var exists = await _repo.Get<Game>(game => game.Id == id).FirstOrDefaultAsync();
            if (exists != null)
            {
                exists.Deleted = true;
                _repo.Update(exists);
                result.Value = new string($"Successfully deleted {exists.Name}");
                result.Success = true;
                await _repo.Commit();
            }
            return result;
        }
        public async Task<ServiceResult<string>> InsertGames(List<GameRequest> gamesToInsert)
        {
            var gamesInsertCount = gamesToInsert.Count();
            var result = new ServiceResult<string>();
            var insert = new List<Game>();
            var gameExisted = new StringBuilder();
            var exists = await _repo.Get<Game>(x => gamesToInsert.Select(x => x.Name.ToLower()).Contains(x.Name.ToLower())).Select(x => x.Name).ToListAsync();
            if (exists != null)
            {
                var removeGames = gamesToInsert.Where(x => exists.Contains(x.Name)).ToList();
                if (removeGames.Count > 0)
                {
                    gameExisted.Append("Game already exists");
                }
                foreach (var item in removeGames)
                {
                    gamesToInsert.Remove(item);
                    gameExisted.Append($" {item.Name}:");
                }
                removeGames.ForEach(x => gamesToInsert.Remove(x));
            }
            foreach (var game in gamesToInsert)
            {
                insert.Add(new Game { Description = game.Description, Name = game.Name, Rating = game.Rating, ReleaseDate = game.ReleaseDate });
            }
            await _repo.BulkInsert(insert);
            result.Success = true;
            //if games already existed
            if (exists.Count == gamesInsertCount)
            {
                result.Value = new string($"Games were not inserted: {gameExisted.ToString()}");
            }
            else
            {
                result.Value = new string($"Successfully inserted games: {gameExisted.ToString()}");
            }
            await _repo.Commit();
            return result;
        }
        public async Task<ServiceResult<string>> UpdateGames(List<GameUpdateRequest> gamesToUpdate)
        {
            var gamesList = new List<Game>();
            var result = new ServiceResult<string>();
            foreach (var update in gamesToUpdate)
            {
                var exists = await _repo.Get<Game>(game => game.Id == update.Id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    exists.Name = !string.IsNullOrEmpty(update.Name) ? update.Name : exists.Name;
                    exists.Rating = update.Rating != 0 ? update.Rating : exists.Rating;
                    exists.Description = !string.IsNullOrEmpty(update.Description) ? update.Description : exists.Description;
                    exists.ReleaseDate = update.ReleaseDate.HasValue ? update.ReleaseDate.Value : exists.ReleaseDate;
                    _repo.Update(exists);
                    result.Value = new string($"Successfully updated games");
                    result.Success = true;
                    await _repo.Commit();
                }
            }
            return result;
        }
        public async Task<ServiceResult<string>> InsertGame(GameRequest gameToInsert)
        {
            var result = new ServiceResult<string>();
            var insert = new Game();
            var exists = await _repo.Get<Game>(x => x.Name == gameToInsert.Name).FirstOrDefaultAsync();
            if (exists == null)
            {
                insert = new Game
                {
                    Description = gameToInsert.Description,
                    Name = gameToInsert.Name,
                    Rating = gameToInsert.Rating,
                    ReleaseDate = gameToInsert.ReleaseDate
                };
                _repo.Add(insert);
                result.Success = true;
                result.Value = new string($"Successfully inserted game: {gameToInsert.Name}");
                await _repo.Commit();
            }
            else
            {
                result.Value = new string($"{gameToInsert.Name} already exists");
                result.Success = true;
            }
            return result;
        }
    }
}