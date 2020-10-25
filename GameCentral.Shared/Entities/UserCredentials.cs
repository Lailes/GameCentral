using System.ComponentModel.DataAnnotations;

namespace GameCentral.Shared.Entities {
    public class UserCredentials {
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}