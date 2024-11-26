using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Models.AuthModels;

public class RegisterModel
{
    [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
    public string username { get; set; }

    [Required(ErrorMessage = "El correo del usuario es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MaxLength(20, ErrorMessage = "La contraseña no puede superar los 20 caracteres.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage =
            "La contraseña debe tener al menos 8 caracteres, incluyendo una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
    public string password { get; set; }
    // Password validation mean this: 
    // Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character
}