using Common;
using System.Linq;

namespace Entities;

[PrimaryKey(nameof(UserId), nameof(InvoiceNumber))]
public class Invoice : IEntity
{
	[ForeignKey(nameof(User)), DatabaseGenerated(DatabaseGeneratedOption.None)]
	public required int UserId { get; set; }
	public int InvoiceNumber { get; set; }
	public InvoiceType InvoiceType { get; set; }
	public required ICollection<Row> Rows { get; set; }
	public long TotalPriceWithoutTax { get; private set; }
	//public long DiscountOnTotalPrice { get; set; } // not for now
	public long TotalOfTax { get; private set;	}
	public long TotalPrice { get; private set; }
	public long Debit { get; set; }
	public long Prepayment { get; set; }
	public long FinalPrice { get; private set; }
	public CurrencyType CurrencyType { get; set; }
	public string? Description { get; set; }
	public DateTime? SubmitDate { get; set; }
	public bool IsDeleted { get; set; }
	[ForeignKey(nameof(Customer))]
	public string? CustomerFullName { get; set; }
	public int? PreviousUserId { get; set; }
	public int? PreviousInvoiceNumber { get; set; }

	public User? User { get; set; }
	public Customer? Customer { get; set; }
	public Invoice? Previous { get; set; }

	public void CalculatePrivateFields()
	{
		TotalPriceWithoutTax = Rows?.Sum(r => r.PriceAfterDiscounts) ?? 0;
		TotalOfTax = Rows?.Sum(r => r.TaxPrice) ?? 0;
		TotalPrice = Rows?.Sum(r => r.FinalPriceOfRow) ?? 0;
		FinalPrice = TotalPrice + Debit - Prepayment;
	}
}

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
	public void Configure(EntityTypeBuilder<Invoice> builder)
	{
		builder.OwnsMany(i => i.Rows, row =>
		{
			row.Property(r => r.Title).HasMaxLength(200);
		});

		builder.HasQueryFilter(i => !i.IsDeleted);

		builder.HasOne(i => i.Customer).WithMany(c => c.Invoices).HasForeignKey(i => new { i.UserId, i.CustomerFullName });
	}
}
