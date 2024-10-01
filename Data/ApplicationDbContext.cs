using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Row> Rows { get; set; }
	public DbSet<ProductOrService> ProductOrServices { get; set; }
    public DbSet<Customer> Customers { get; set; }



	public ApplicationDbContext(DbContextOptions options)
		: base(options)
	{

	}
	
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		var entityAssembly = typeof(IEntity).Assembly;
		 
		modelBuilder.RegisterEntityTypeConfiguration(entityAssembly);
		modelBuilder.AddRestrictDeleteBehaviorConvention();
		modelBuilder.AddSequentialGuidForIdConvention();
		modelBuilder.AddPluralizingTableNameConvention();
	}

	public override int SaveChanges()
	{
		_cleanString();
		return base.SaveChanges();
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		_cleanString();
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		_cleanString();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		_cleanString();
		return base.SaveChangesAsync(cancellationToken);
	}


	private void _cleanString()
	{
		var changedEntities = ChangeTracker.Entries()
			.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
		foreach (var item in changedEntities)
		{
			if (item.Entity == null)
				continue;

			var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

			foreach (var property in properties)
			{
				var propName = property.Name;
				var val = (string)property.GetValue(item.Entity, null)!;

				if (val.HasValue())
				{
					var newVal = val.Fa2En().FixPersianChars();
					if (newVal == val)
						continue;
					property.SetValue(item.Entity, newVal, null);
				}
			}
		}
	}
}
