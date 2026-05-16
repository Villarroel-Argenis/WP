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

        app.MapGet("/accounts/{id}", GetAccountByIdAsync)
            .WithName("GetAccountById")
            .WithOpenApi();

        app.MapPost("/accounts/{accountId:guid}/transactions", RegisterTransactionAsync)
            .WithName("RegisterTransaction")
            .WithOpenApi();

        app.MapGet("/accounts/{accountId:guid}/transactions", GetTransactionsByAccountIdAsync)
            .WithName("GetTransactionsByAccountId")
            .WithOpenApi();

        return app;
    }


    private static async Task<IResult> CreateAccountAsync(
        CreateAccountRequest request,
        ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(
            request.Name,
            request.InitialAmount,
            request.CurrencyCode);

        Result<Guid> result = await dispatcher.SendAsyn<CreateAccountCommand, Guid>(command, cancellationToken);

        return result.ToHttpResult(id => Results.Created($"/accounts/{id}", new { id }));
    }

    private static async Task<IResult> GetAccountByIdAsync(
        Guid id,
        IQueryHandler<GetAccountByIdQuery, AccountResponse?> handler,
        CancellationToken cancellationToken)
    {

        var query = new GetAccountByIdQuery(id);
        Result<AccountResponse?> result = await handler.Handle(query, cancellationToken);
        return result.ToHttpResult(Results.Ok);
    }

    /// <summary>
    /// Registra una nueva transacción en una cuenta.
    /// </summary>
    private static async Task<IResult> RegisterTransactionAsync(
        Guid accountId,
        RegisterTransactionRequest request,
        ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        var command = new RegisterTransactionCommand(
            accountId,
            request.Amount,
            request.CurrencyCode,
            request.Type,
            request.Description,
            request.TargetAccountId,
            request.Tags);

        Result<Guid> result = await dispatcher.SendAsyn<RegisterTransactionCommand, Guid>(command, cancellationToken);

        return result.ToHttpResult((id) => Results.Created($"/accounts/{accountId}/transactions/{id}", new { id }));
    }

    /// <summary>
    /// Obtiene las transacciones de una cuenta.
    /// </summary>
    private static async Task<IResult> GetTransactionsByAccountIdAsync(
        Guid accountId,
        IQueryHandler<GetTransactionsByAccountIdQuery, IReadOnlyList<TransactionResponse>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionsByAccountIdQuery(accountId);
        Result<IReadOnlyList<TransactionResponse>> result = await handler.Handle(query, cancellationToken);
        return result.ToHttpResult(Results.Ok);
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

/// <summary>
/// Solicitud para registrar una nueva transacción.
/// </summary>
/// <param name="Amount">Monto de la transacción.</param>
/// <param name="CurrencyCode">Código de la moneda.</param>
/// <param name="Type">Tipo de transacción: Income, Expense o Transfer.</param>
/// <param name="Description">Descripción opcional.</param>
/// <param name="TargetAccountId">Cuenta destino, solo para transferencias.</param>
/// <param name="Tags">Etiquetas opcionales.</param>
public sealed record RegisterTransactionRequest(
    decimal Amount,
    string CurrencyCode,
    string Type,
    string? Description = null,
    Guid? TargetAccountId = null,
    IEnumerable<string>? Tags = null);
