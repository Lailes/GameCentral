using System.ComponentModel.DataAnnotations;

namespace GameCentral.Shared.Entities {
    public class Game {
        public string GameId { get; set; }
        [Required(ErrorMessage = "Enter title")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Enter studio")]
        public string Studio { get; set; }
        [Required(ErrorMessage = "Enter genre")]
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public int Cost { get; set; }
    }
}