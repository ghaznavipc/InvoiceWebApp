namespace Entities;

public class User : IdentityUser<int>, IEntity
{
	public string? FullName { get; set; }
	public PersonType? PersonType { get; set; }
	public string? NationalCode { get; set; }
	public string? EconomicCode { get; set; }
	public string? Address { get; set; }
	public string? SignAndOrSeal { get; set; }
	public string? Logo { get; set; }
	public DateTimeOffset? LastLoginDate { get; set; }

	public ICollection<Invoice>? Invoices { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(p => p.UserName).IsRequired().HasMaxLength(50);
		builder.Property(p => p.Email).HasMaxLength(100);
		builder.Property(p => p.PhoneNumber).HasMaxLength(15);
		builder.Property(p => p.FullName).HasMaxLength(100);
		builder.Property(p => p.PersonType).HasMaxLength(7);
		builder.Property(p => p.NationalCode).HasMaxLength(15);
		builder.Property(p => p.EconomicCode).HasMaxLength(15);
	}
}
