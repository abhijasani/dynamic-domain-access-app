using Microsoft.EntityFrameworkCore;

public class OriginService
{
    private readonly AppDbContext _context;

    public OriginService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AllowedOrigin>> GetAllOriginsAsync()
    {
        return await _context.AllowedOrigins.ToListAsync();
    }

    public async Task<bool> IsOriginAllowedAsync(string origin)
    {
        return await _context.AllowedOrigins.AnyAsync(o => o.Origin == origin);
    }

    public async Task AddOriginAsync(string origin)
    {
        if (!await IsOriginAllowedAsync(origin))
        {
            _context.AllowedOrigins.Add(new AllowedOrigin { Origin = origin });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveOriginAsync(string origin)
    {
        var originToRemove = await _context.AllowedOrigins
            .FirstOrDefaultAsync(o => o.Origin == origin);

        if (originToRemove != null)
        {
            _context.AllowedOrigins.Remove(originToRemove);
            await _context.SaveChangesAsync();
        }
    }
}