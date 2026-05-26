namespace MiApp.Domain.Exceptions;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(int requested, int available)
        : base($"Stock insuficiente. Solicitado: {requested}, Disponible: {available}.") { }
}