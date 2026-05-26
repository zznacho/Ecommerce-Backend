using Microsoft.EntityFrameworkCore;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
using MiApp.Infrastructure.Persistence;

namespace MiApp.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _ctx;
    public OrderRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Orders
               .Include(o => o.Items) // Hace el JOIN físico en la base de datos
               .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _ctx.Orders
               .AsNoTracking()
               .Where(o => o.UserId == userId)
               .OrderByDescending(o => o.CreatedAt)
               .ToListAsync(ct);

    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await _ctx.Orders.AddAsync(order, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}