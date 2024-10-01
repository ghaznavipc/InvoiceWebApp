namespace Entities;

public class Role : IdentityRole<int>, IEntity
{
	public Role() : base()
	{

	}
	public Role(string name) : base(name)
	{

	}
	public Role(string name, string description) : base(name)
	{
		Description = description;
	}

	public string? Description { get; set; }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
		builder.Property(p => p.Description).HasMaxLength(100);

	}
}
