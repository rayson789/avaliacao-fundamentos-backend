namespace Core.Persistence.Repositories;
public class WorkLogRepository
{
    private readonly AppDbContext _context;

    public WorkLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkLog> AddAsync(WorkLog log)
    {
        _context.WorkLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task<List<WorkLog>> GetByStatusAsync(string status)
    {
        return await _context.WorkLogs
            .Where(w => w.Status == status)
            .ToListAsync();
    }

    public async Task<WorkLog?> UpdateMessageAsync(int id, string newMessage)
    {
        var log = await _context.WorkLogs.FindAsync(id);
        if (log is null) return null;

        log.Message = newMessage;
        await _context.SaveChangesAsync();

        return log;
    }
}