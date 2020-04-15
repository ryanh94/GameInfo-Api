using GameInfo.Core.Models;
using GameInfo.Core.Models.Requests;
using GameInfo.Core.Models.Response;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameInfo.Core.Interfaces
{
    public interface IGameService
    {
        public Task<ServiceResult<List<GameResponse>>> GetGames();
        public Task<ServiceResult<string>> UpdateGames(List<GameUpdateRequest> gamesToUpdate);
        public Task<ServiceResult<string>> UpdateGame(GameUpdateRequest gameToUpdate);
        public Task<ServiceResult<string>> InsertGames(List<GameRequest> gamesToInsert);
        public Task<ServiceResult<string>> InsertGame(GameRequest gameToInsert);
        Task<ServiceResult<string>> DeleteGame(int id);
    }
}
