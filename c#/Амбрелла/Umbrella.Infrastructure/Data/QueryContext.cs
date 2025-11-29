using Microsoft.EntityFrameworkCore;
using Umbrella.Domain.Entities;

namespace Umbrella.Infrastructure.Data;

public sealed class QueryContext(UmbrellaDbContext dbContext)
{
    private readonly UmbrellaDbContext _dbContext = dbContext;

    public IQueryable<User> Users => _dbContext.Users.AsNoTracking().Where(u => !u.IsDeleted);
    public IQueryable<DataSource> DataSources => _dbContext.DataSources.AsNoTracking().Where(d => !d.IsDeleted);
    public IQueryable<Report> Reports => _dbContext.Reports.AsNoTracking().Where(r => !r.IsDeleted);
    public IQueryable<ScheduledReport> ScheduledReports => _dbContext.ScheduledReports.AsNoTracking().Where(s => !s.IsDeleted);
}