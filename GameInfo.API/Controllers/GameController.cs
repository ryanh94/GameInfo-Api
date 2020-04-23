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
        public async Task<ActionResult> Add([FromBody]GameRequest insertGame)
        {
            if (ModelState.IsValid)
            {
                var response = await _gameService.InsertGame(insertGame);
                if (response.Success)
                {
                    return NoContent();
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody]GameUpdateRequest gameUpdate)
        {
            if (ModelState.IsValid)
            {
                var response = await _gameService.UpdateGame(id, gameUpdate);
                if (response.Success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await _gameService.DeleteGame(id);
                if (response.Success)
                {
                    return NoContent();
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