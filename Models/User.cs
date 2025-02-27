using System.ComponentModel.DataAnnotations;

namespace my_portfolio_backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
    }
}