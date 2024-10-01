namespace Common;

public enum InvoiceType
{
	[Display(Name = "پیش فاکتور")]
	ProformaInvoice = 0,

	[Display(Name = "فاکتور")]
	Invoice = 1,

	[Display(Name = "فاکتور رسمی")]
	OfficialInvoice = 2,
}
