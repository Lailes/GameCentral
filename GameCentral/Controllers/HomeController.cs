using System;
using System.Threading.Tasks;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GameCentral.Controllers {
    public class HomeController : Controller {

        public IGameService Storage { get; }

        public HomeController(IGameService storage) {
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
        public IActionResult EditGame(
            int? id,
            string title,
            string publisher,        //TODO Refactor this crap
            string studio,
            int cost,
            string genre,
            string url) => View(new Game() {
            GameId = id, Title = title, Genre = genre, Studio = studio, Cost = cost, Publisher = publisher, PreviewImageUrl = url
        });

        [HttpPost]
        public async Task<IActionResult> EditGame(Game game) {
            if (ModelState.IsValid) {
               await Storage.EditGameAsync(game);
            }
            else {
                return EditGame(game.GameId, game.Title, game.Publisher, game.Studio, game.Cost, game.Genre, game.PreviewImageUrl);
            }

            return RedirectToAction("ListGames");
        }


        [HttpPost]
        public IActionResult SetLocale(string culture, string returnUrl) {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions {Expires = DateTime.UtcNow.AddYears(1)}
                );
            return LocalRedirect(returnUrl);
        }
        
        public ViewResult About() => View();
        
        public async Task<ViewResult> GameDetails(int id) => View(await Storage.GetGameAsync(id));
        
    }
}