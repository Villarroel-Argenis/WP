namespace WP.Application.Accounts.CreateAccount;

/// <summary>
/// Validador para el comando CreateAccountCommand. Este validador se encarga de verificar que los datos proporcionados para crear una cuenta sean válidos antes de que se ejecute la lógica de creación de la cuenta. Si los datos no son válidos, se lanzará una excepción de validación con los errores correspondientes.
/// </summary>
public class CreateAccountCommandValidator : IValidator<CreateAccountCommand>
{
    /// <summary>
    /// Valida el comando CreateAccountCommand verificando que el nombre de la cuenta no esté vacío, que no supere los 100 caracteres, que el monto inicial no sea negativo, y que el código de moneda sea válido (no esté vacío y tenga exactamente 3 caracteres). Si se encuentran errores de validación, se acumulan en un diccionario y se lanza una excepción de validación con todos los errores encontrados.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ValidationException"></exception>
    public void Validate(CreateAccountCommand value)
    {
        var errors = new Dictionary<string, List<string>>();
        if (string.IsNullOrWhiteSpace(value.Name))
        {
            errors.Add("Name", ["El nombre es requerido."]);
        }

        if (value.Name?.Length > 100)
        {
            errors.AddError("Name", "El nombre no puede superar 100 caracteres.");
        }

        if (value.InitialAmount < 0)
        {
            errors.AddError("InitialAmount", "El monto inicial no puede ser negativo.");
        }

        if (string.IsNullOrWhiteSpace(value.CurrencyCode))
        {
            errors.AddError("CurrencyCode", "El código de moneda es requerido.");
        }

        if (value.CurrencyCode?.Length != 3)
        {
            errors.AddError("CurrencyCode", "El código de moneda debe tener 3 caracteres.");
        }

        if(errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
