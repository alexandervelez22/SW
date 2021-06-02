using System.ComponentModel.DataAnnotations;

namespace API.DTOs

{
    public class RegisterDTO

    {
        [Required]

        public string  Username { get; set; }

        public string  Password { get; set; }

    }

}