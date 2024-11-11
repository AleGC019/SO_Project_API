using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Models.AuthModels;

public class RegisterModel
{
    
    [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
    public string username { get; set; }
    
    [Required(ErrorMessage = "El correo del usuario es obligatorio.")]
    public string email { get; set; }
    
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MaxLength(20, ErrorMessage = "La contraseña no puede superar los 20 caracteres.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
    public string Password { get; set; }
    // Password validation mean this: 
    // Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character
    
    [Required(ErrorMessage = "Revise su fecha de nacimiento.")]
    public string BirthDate { get; set; }
}

public class MatchPasswordAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public MatchPasswordAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var password = value as string;
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            return new ValidationResult($"Property {_comparisonProperty} not found.");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance) as string;

        if (password != comparisonValue)
            return new ValidationResult("La contraseña de validación debe ser igual a la contraseña.");

        return ValidationResult.Success;
    }
}