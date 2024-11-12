using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Models;

public class LoginModel
{
    [Required(ErrorMessage = "El correo de usuario es requerido")]
    [MaxLength(50, ErrorMessage = "El correo de usuario no puede exceder los 50 caracteres")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MaxLength(50, ErrorMessage = "La contraseña no puede exceder los 50 caracteres")]
    public string Password { get; set; }
}