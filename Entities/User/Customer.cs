namespace Entities;

[PrimaryKey(nameof(UserId), nameof(CustomerFullName))]
public class Customer : IEntity
{
	[ForeignKey(nameof(User)), DatabaseGenerated(DatabaseGeneratedOption.None)]
	public required int UserId { get; set; }

	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public required string CustomerFullName { get; set; }
	public PersonType? PersonType { get; set; }
	public string? Address { get; set; }
	public string? NationalCode { get; set; }
	public string? EconomicCode { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? PostCode { get; set; }
	public string? FaxNumber { get; set; }

	public bool IsDeleted { get; set; }
	public ICollection<Invoice>? Invoices { get; set; }
	public User? User { get; set; }
}

public enum PersonType
{
	[Display(Name = "حقیقی")]
	Actual = 0,

	[Display(Name = "حقوقی")]
	Juridical = 1
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(EntityTypeBuilder<Customer> builder)
	{
		builder.HasMany(c => c.Invoices).WithOne(i => i.Customer).HasForeignKey(i => new { i.UserId, i.CustomerFullName });

		builder.Property(p => p.CustomerFullName).HasMaxLength(100);
		builder.Property(p => p.PersonType).HasMaxLength(7);
		builder.Property(p => p.Address).HasMaxLength(255);
		builder.Property(p => p.NationalCode).HasMaxLength(15);
		builder.Property(p => p.EconomicCode).HasMaxLength(15);
		builder.Property(p => p.Email).HasMaxLength(100);
		builder.Property(p => p.PhoneNumber).HasMaxLength(15);
		builder.Property(p => p.PostCode).HasMaxLength(15);
		builder.Property(p => p.FaxNumber).HasMaxLength(15);
	}
}