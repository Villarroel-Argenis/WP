namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el manejador del evento TransactionRegistered.
/// </summary>
public sealed class OnTransactionRegisteredTests
{
    private readonly OnTransactionRegistered _handler;

    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    public OnTransactionRegisteredTests()
    {
        ILogger<OnTransactionRegistered> logger = Substitute.For<ILogger<OnTransactionRegistered>>();
        _handler = new OnTransactionRegistered(logger);
    }

    /// <summary>
    /// Verifica que manejar el evento no lanza excepción.
    /// </summary>
    [Fact]
    public async Task HandleAsyncConEventoValidoNoLanzaExcepcionAsync()
    {
        var domainEvent = new TransactionRegisteredDomainEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            TransactionType.Income);

        await Should.NotThrowAsync(() =>
            _handler.HandleAsync(domainEvent, CancellationToken.None));
    }

    /// <summary>
    /// Verifica que manejar el evento completa la tarea correctamente.
    /// </summary>
    [Fact]
    public async Task HandleAsyncRetornaTaskCompletadaAsync()
    {
        var domainEvent = new TransactionRegisteredDomainEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            TransactionType.Expense);

        Task result = _handler.HandleAsync(domainEvent, CancellationToken.None);

        await result;
        result.IsCompleted.ShouldBeTrue();
    }
}
