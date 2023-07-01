using System.ComponentModel.DataAnnotations;

namespace RP_Identity_API.DTO.Register_LoginModelDTO
{
    public class RegisterModelDTO
    {
        [Required]
        public string FirstName { get; init; }
        [Required]
        public string LastName { get; init; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; init; }
        [Required, DataType(DataType.Password)]
        public string Password { get; init; }
        [Required, DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Not Matched")]
        public string ConfirmPassword { get; init; }
    }
}
