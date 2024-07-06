namespace CongestionTaxCalculator.Application.Common.Persistence;

public interface IUnitOfWork
{
    public Task SaveChangeAsync(CancellationToken cancellationToken = default);
}