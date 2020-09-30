using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using GameCentral.Shared.Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GameCentral.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase {
        private readonly IDatabase _database;
        
        public ApiController(IDatabase database) {
            _database = database;
        }

        [HttpGet("games")]
        public async Task<ActionResult<IEnumerable<Game>>> Get(){
            try {
                var games =  await _database.GetGamesAsync();
                return Ok(games);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }
        
        [HttpGet("games/{id}")]
        public async Task<ActionResult<Game>> Get(int id) {
            try {
                var game = await _database.GetGameAsync(id);
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
                await _database.RemoveGameAsync(id);
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
                await _database.AddGameAsync(game);
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
                await _database.EditGameAsync(game);
                return Ok(game);
            }
            catch (GameNotExistsException e) {
                await _database.AddGameAsync(game);
                return CreatedAtAction("Put", game);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }
    }
}