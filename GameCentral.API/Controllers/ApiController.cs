using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using GameCentral.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace GameCentral.API.Controllers {
    
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ApiController : ControllerBase {
        private readonly IGameService _gameService;

        public ApiController(IGameService gameService) {
            _gameService = gameService;
        }

        [HttpGet("games")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Game>>> Get(){
            try {
                var games =  await _gameService.GetGamesAsync();
                return Ok(games);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }
        
        [HttpGet("games/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Game>> Get(int id) {
            try {
                var game = await _gameService.GetGameAsync(id);
                return Ok(game);
            }
            catch (GameNotExistsException e) {
                return NotFound(id);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }
        
        [HttpDelete("games/{id}")]
        public async Task<IActionResult> Delete(int id) {
            try {
                await _gameService.RemoveGameAsync(id);
                return Ok(id);
            }
            catch (GameNotExistsException e) {
                return NotFound(id);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }

        [HttpPost("games")]
        public async Task<IActionResult> Post([FromBody] Game game) {
            try {
                await _gameService.AddGameAsync(game);
                return CreatedAtAction("Post", game);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }

        [HttpPut("games/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Game game) {
            try {
                game.GameId = id;
                await _gameService.EditGameAsync(game);
                return Ok(game);
            }
            catch (GameNotExistsException e) {
                await _gameService.AddGameAsync(game);
                return CreatedAtAction("Put", game);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }
    }
}