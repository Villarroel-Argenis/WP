namespace WP.Api.Endpoints;

#pragma warning disable IDE1006, ASPDEPR002 // Suppresses naming convention violations and deprecated method warnings

/// <summary>
/// Proporciona los puntos finales de la API para gestionar cuentas.
/// </summary>
public static class AccountEndpoint
{
    /// <summary>
    /// Mapea los puntos finales de cuentas al constructor de rutas de la aplicación.
    /// </summary>
    /// <param name="app">El constructor de rutas de puntos finales.</param>
    /// <returns>El constructor de rutas actualizado con los puntos finales de cuentas mapeados.</returns>
    public static IEndpointRouteBuilder MapAccountEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts", CreateAccountAsync)
            .WithName("CreateAccount")
            .WithOpenApi();

        return app;
    }

    /// <summary>
    /// Maneja la solicitud para crear una nueva cuenta.
    /// </summary>
    /// <param name="request">La solicitud con los datos de la nueva cuenta.</param>
    /// <param name="handler">El manejador del comando para crear la cuenta.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>Una respuesta HTTP 201 Created con el identificador de la cuenta creada.</returns>
    private static async Task<IResult> CreateAccountAsync(
        CreateAccountRequest request,
        ICommandHandler<CreateAccountCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(
            request.Name,
            request.InitialAmount,
            request.CurrencyCode);

        Guid id = await handler.Handle(command, cancellationToken);

        return Results.Created($"/accounts/{id}", new { id });
    }
}

/// <summary>
/// Representa la solicitud para crear una nueva cuenta.
/// </summary>
/// <param name="Name">El nombre de la cuenta.</param>
/// <param name="InitialAmount">El monto inicial de la cuenta.</param>
/// <param name="CurrencyCode">El código de la moneda de la cuenta.</param>
public sealed record CreateAccountRequest(
    string Name,
    decimal InitialAmount,
    string CurrencyCode);
