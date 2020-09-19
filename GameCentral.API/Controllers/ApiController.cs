using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using GameCentral.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GameCentral.API.Controllers {
    
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase {
        private readonly IDatabase _database;
        
        public ApiController(IDatabase database) {
            _database = database;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> Get(){
            try {
                var games =  await _database.GetGamesAsync();
                return Ok(games);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> Get(string id) {
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
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) {
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

        [HttpPost]
        public async Task<IActionResult> Post(Game game) {
            try {
                await _database.AddGameAsync(game);
                return CreatedAtAction("Post", game);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Game game) {
            try {
                game.GameId = id;
                await _database.EditGameAsync(game);
                return Ok(game);
            }
            catch (GameNotExistsException e) {
                return NotFound(id);
            }
            catch (Exception e) {
                return Problem(detail: e.Message, statusCode: 500);
            }
        }
    }
}