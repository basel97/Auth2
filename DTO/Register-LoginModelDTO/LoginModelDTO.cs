using System.ComponentModel.DataAnnotations;

namespace RP_Identity_API.DTO.Register_LoginModelDTO
{
    public class LoginModelDTO
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; init; }
        [Required,DataType(DataType.Password)]
        public string Password { get; init; }
    }
}
