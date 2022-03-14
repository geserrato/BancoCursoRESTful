using System.Reflection;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.Context;

public class ApplicationDbContext : DbContext
{
    private readonly IDateTimeService _dateTimeService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTimeService) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dateTimeService = dateTimeService;
    }

    public DbSet<Client> Clients { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (EntityEntry<AuditableBaseEntity> entry in ChangeTracker.Entries<AuditableBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.Created = _dateTimeService.NowUtc;
                    break;
                case EntityState.Added:
                    entry.Entity.LastModified = _dateTimeService.NowUtc;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}