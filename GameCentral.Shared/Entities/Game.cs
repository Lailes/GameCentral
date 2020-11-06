using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCentral.Shared.Entities {
    
    [Table("Games", Schema = "Products")]
    public class Game {
        public int? GameId { get; set; }
        [Required(ErrorMessage = "Enter title")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Enter studio")]
        public string Studio { get; set; }
        [Required(ErrorMessage = "Enter genre")]
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public int Cost { get; set; }
        public string PreviewImageUrl { get; set; }

        public override bool Equals(object? obj) {
            return (obj is Game game && (game.GameId == GameId || game.Title == Title));
        }

        public override int GetHashCode() {
            return GameId.GetHashCode();
        }
    }
}