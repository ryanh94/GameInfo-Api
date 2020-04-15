using System.Collections.Generic;
using System.Threading.Tasks;

using GameInfo.Core.Interfaces;
using GameInfo.Core.Models.Requests;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameInfo.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var response = await _gameService.GetGames();
            if (response.Success)
            {
                return Ok(response.Value);
            }
            else if (!response.Success)
            {
                return StatusCode(500);
            }
            return NotFound("No games found");
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Add([FromBody]GameRequest insertGame)
        {
            if (ModelState.IsValid)
            {
                var response = await _gameService.InsertGame(insertGame);
                if (response.Success)
                {
                    return Ok(response.Value);
                }
                else
                {
                    return BadRequest(response.Value);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody]GameUpdateRequest gameUpdate)
        {
            if (ModelState.IsValid)
            {
                var response = await _gameService.UpdateGame(gameUpdate);
                if (response.Success)
                {
                    return Ok(response.Value);
                }
                else
                {
                    return BadRequest(response.Value);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _gameService.DeleteGame(id);
                if (response.Success)
                {
                    return Ok(response.Value);
                }
                else
                {
                    return BadRequest(response.Value);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}