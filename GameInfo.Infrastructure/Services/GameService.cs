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
        private IDBFactory _repo;

        public GameService(IDBFactory repo)
        {
            _repo = repo;
        }
        public async Task<ServiceResult<List<GameResponse>>> GetGames(int pageSize = 20, int pageNumber = 1)
        {
            var result = new ServiceResult<List<GameResponse>>();
            result.Success = false;
            using (var repo = _repo.GetInstance())
            {
                try
                {
                    var skip = (pageNumber - 1) * pageSize;
                    var games = await repo.Get<Game>(x => x.Id > 0).Skip(skip).Select(x => new GameResponse { Id = x.Id, Name = x.Name, Rating = x.Rating, ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy"), Description = x.Description }).Take(pageSize).ToListAsync();
                    if (games.Count == 0)
                    {
                        return result;
                    }
                    result.Success = true;
                    result.Value = new List<GameResponse>(games);
                }
                catch (Exception e)
                {
                    //log exception
                }
            }
            return result;
        }
        public async Task<ServiceResult<string>> UpdateGame(GameUpdateRequest update)
        {
            var result = new ServiceResult<string>();

            using (var repo = _repo.GetInstance())
            {
                var exists = await repo.Get<Game>(game => game.Id == update.Id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    exists.Name = !String.IsNullOrEmpty(update.Name) ? update.Name : exists.Name;
                    exists.Rating = update.Rating != 0 ? update.Rating : exists.Rating;
                    exists.Description = !String.IsNullOrEmpty(update.Description) ? update.Description : exists.Description;
                    exists.ReleaseDate = update.ReleaseDate.HasValue ? update.ReleaseDate.Value : exists.ReleaseDate;
                    try
                    {
                        repo.Update(exists);
                        result.Value = new string($"Successfully updated {update.Name}");
                        result.Success = true;
                        await repo.Commit();
                    }
                    catch (Exception e)
                    {
                        //log exception
                        result.Value = new string(e.Message);
                        result.Success = false;
                        return result;
                    }
                }
            };
            return result;
        }
        public async Task<ServiceResult<string>> DeleteGame(int id)
        {
            var result = new ServiceResult<string>();

            using (var repo = _repo.GetInstance())
            {
                var exists = await repo.Get<Game>(game => game.Id == id).FirstOrDefaultAsync();
                if (exists != null)
                {
                    exists.Deleted = true;
                    try
                    {
                        repo.Update(exists);
                        result.Value = new string($"Successfully deleted {exists.Name}");
                        result.Success = true;
                        await repo.Commit();
                    }
                    catch (Exception e)
                    {
                        //log exception
                        result.Value = new string(e.Message);
                        result.Success = false;
                        return result;
                    }
                }
            };
            return result;
        }



        public async Task<ServiceResult<string>> InsertGames(List<GameRequest> gamesToInsert)
        {
            var gamesInsertCount = gamesToInsert.Count();
            var result = new ServiceResult<string>();
            var insert = new List<Game>();
            var gameExisted = new StringBuilder();

            using (var repo = _repo.GetInstance())
            {
                var exists = await repo.Get<Game>(x => gamesToInsert.Select(x => x.Name.ToLower()).Contains(x.Name.ToLower())).Select(x => x.Name).ToListAsync();
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

                try
                {
                    await repo.BulkInsert(insert);
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

                    await repo.Commit();
                }
                catch (Exception e)
                {
                    //log exception
                    result.Value = new string(e.Message);
                    result.Success = false;
                }

            }
            return result;
        }

        public async Task<ServiceResult<string>> UpdateGames(List<GameUpdateRequest> gamesToUpdate)
        {
            var gamesList = new List<Game>();
            var result = new ServiceResult<string>();

            using (var repo = _repo.GetInstance())
            {
                foreach (var update in gamesToUpdate)
                {
                    var exists = await repo.Get<Game>(game => game.Id == update.Id).FirstOrDefaultAsync();
                    if (exists != null)
                    {
                        exists.Name = !String.IsNullOrEmpty(update.Name) ? update.Name : exists.Name;
                        exists.Rating = update.Rating != 0 ? update.Rating : exists.Rating;
                        exists.Description = !String.IsNullOrEmpty(update.Description) ? update.Description : exists.Description;
                        exists.ReleaseDate = update.ReleaseDate.HasValue ? update.ReleaseDate.Value : exists.ReleaseDate;
                        try
                        {
                            repo.Update(exists);
                            result.Value = new string($"Successfully updated games");
                            result.Success = true;
                            await repo.Commit();
                        }
                        catch (Exception e)
                        {
                            //log exception
                            result.Value = new string(e.Message);
                            result.Success = false;
                            return result;
                        }
                    }
                }
            };
            return result;
        }

        public async Task<ServiceResult<string>> InsertGame(GameRequest gameToInsert)
        {
            var result = new ServiceResult<string>();
            var insert = new Game();
            using (var repo = _repo.GetInstance())
            {
                var exists = await repo.Get<Game>(x => x.Name == gameToInsert.Name).FirstOrDefaultAsync();
                if (exists == null)
                {
                    insert = new Game { Description = gameToInsert.Description, Name = gameToInsert.Name, Rating = gameToInsert.Rating, ReleaseDate = gameToInsert.ReleaseDate };
                    try
                    {
                        repo.Add(insert);
                        result.Success = true;
                        result.Value = new string($"Successfully inserted game: {gameToInsert.Name}");

                        await repo.Commit();
                    }
                    catch (Exception e)
                    {
                        //log exception
                        result.Value = new string(e.Message);
                        result.Success = false;
                    }
                }
                else
                {
                    result.Value = new string($"{gameToInsert.Name} already exists");
                    result.Success = true;
                }
            }
            return result;
        }
    }
}
