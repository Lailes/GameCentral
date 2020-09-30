using System.Threading.Tasks;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameCentral.Controllers {
    public class HomeController : Controller {

        public IDatabase Storage { get; }

        public HomeController(IDatabase storage) {
            Storage = storage;
        }

        public ViewResult Index() => View();

        [HttpGet]
        public async Task<ViewResult> ListGames() => View(await Storage.GetGamesAsync());

        [HttpPost]
        public async Task<RedirectToActionResult> ListGames(int deleteId) {
            await Storage.RemoveGameAsync(deleteId);
            return RedirectToAction("ListGames");
        }

        [HttpGet]
        public IActionResult AddGame() => View(new Game());

        [HttpPost]
        public async Task<IActionResult> AddGame(Game game) {
            if (ModelState.IsValid) {
                await Storage.AddGameAsync(game);
            }
            else {
                return AddGame();
            }
            return RedirectToAction("ListGames");
        }

        [HttpGet]
        public IActionResult EditGame(int id, string title) => View(new Game(){GameId = id, Title = title});

        [HttpPost]
        public async Task<IActionResult> EditGame(Game game) {
            if (ModelState.IsValid) {
               await Storage.EditGameAsync(game);
            }
            else {
                return EditGame(game.GameId, game.Title);
            }

            return RedirectToAction("ListGames");
        }


        public ViewResult About() => View();

        public async Task<ViewResult> GameDetails(int id) => View(await Storage.GetGameAsync(id));
        
    }
}