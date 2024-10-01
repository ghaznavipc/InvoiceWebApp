namespace MyServices;

public static class InvoiceExtensions
{
	public static InvoiceDetailsVM ToInvoiceDetailsVM(this Invoice invoice)
	{
		List<RowVM> rowVMs = invoice.Rows.ToRowVMs();

		var invoiceVM = new InvoiceDetailsVM()
		{
			InvoiceNumber = invoice.InvoiceNumber,
			RowVMs = rowVMs,
			InvoiceType = invoice.InvoiceType,
			TotalPriceWithoutTax = invoice.TotalPriceWithoutTax,
			TotalOfTax = invoice.TotalOfTax,
			TotalPrice = invoice.TotalPrice,
			Debit = invoice.Debit,
			Prepayment = invoice.Prepayment,
			Description = invoice.Description,
			FinalPrice = invoice.FinalPrice,
			CurrencyType = invoice.CurrencyType,
			SubmitDate = invoice.SubmitDate,
			CustomerFullName = invoice.CustomerFullName,
			PreviousInvoiceNumber = invoice.PreviousInvoiceNumber
		};
		return invoiceVM;
	}

	public static InvoiceCreateVM ToInvoiceCreateVM(this Invoice invoice)
	{
		List<RowVM> rowVMs = invoice.Rows.ToRowVMs();

		var invoiceVM = new InvoiceCreateVM()
		{
			InvoiceNumber = invoice.InvoiceNumber,
			RowVMs = rowVMs,
			InvoiceType = invoice.InvoiceType,
			Debit = invoice.Debit,
			Prepayment = invoice.Prepayment,
			Description = invoice.Description,
			CurrencyType = invoice.CurrencyType,
			CustomerFullName = invoice.CustomerFullName,
			PreviousInvoiceNumber = invoice.PreviousInvoiceNumber
		};

		return invoiceVM;
	}

	private static List<RowVM> ToRowVMs(this IEnumerable<Row> rows)
	{
		var rowVMs = new List<RowVM>();
		foreach (var row in rows)
		{
			var rowVM = new RowVM()
			{
				Title = row.Title,
				PriceForEach = row.PriceForEach,
				Quantity = row.Quantity,
				Discount = row.Discount,
				TaxPercent = row.TaxPercent,
			};
			rowVM.CalculatePrivateFields();
			rowVMs.Add(rowVM);
		}

		return rowVMs;
	}

}
