using System.ComponentModel.DataAnnotations;

namespace Saraha.Api.Data.DTOs.Authentication.ResetEmail
{
    public class ResetEmailObjectDto
    {
        [Required(ErrorMessage = "Please enter your old email")]
        public string OldEmail { get; set; } = null!;

        [Required(ErrorMessage = "Please enter new email")]
        public string NewEmail { get; set; } = null!;
    }
}
