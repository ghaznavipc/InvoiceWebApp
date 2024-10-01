using System.Data;

namespace MyServices;

public class InvoiceService : IInvoiceService
{
	private readonly IRepository<Invoice> _invoiceRepository;
	private readonly IRepository<Customer> _customerRepository;
	private readonly IRepository<ProductOrService> _productOrServiceRepository;

	public InvoiceService(IRepository<Invoice> invoiceRepository, IRepository<Customer> customerRepository, IRepository<ProductOrService> productOrServiceRepository)
	{
		_invoiceRepository = invoiceRepository;
		_customerRepository = customerRepository;
		_productOrServiceRepository = productOrServiceRepository;
	}


	public async Task<bool> AnyAsync(int userId, int invoiceNumber, CancellationToken cancellationToken)
		=> await _invoiceRepository.TableNoTracking.IgnoreQueryFilters().AnyAsync(i => i.UserId == userId && i.InvoiceNumber == invoiceNumber, cancellationToken);
	public async Task<bool> AnyRefrencedToThisAsync(int userId, int invoiceNumber, CancellationToken cancellationToken)
		=> await _invoiceRepository.TableNoTracking.IgnoreQueryFilters().AnyAsync(i => i.UserId == userId && i.PreviousInvoiceNumber == invoiceNumber, cancellationToken);


	public async Task<Invoice?> GetAsync(int userId, int invoiceNumber, CancellationToken cancellationToken)
		=> await _invoiceRepository.TableNoTracking.SingleOrDefaultAsync(i => i.UserId == userId && i.InvoiceNumber == invoiceNumber, cancellationToken);
	public async Task<IEnumerable<InvoiceOverviewVM?>> GetAsync(int userId, CancellationToken cancellationToken)
	{
		var invoicePreviewVMList = await _invoiceRepository.TableNoTracking.Where(i => i.UserId == userId)
			.Select(i => new InvoiceOverviewVM
			{
				InvoiceNumber = i.InvoiceNumber,
				InvoiceType = i.InvoiceType,
				CustomerFullName = i.CustomerFullName,
				FinalPrice = i.FinalPrice,
				CurrencyType = i.CurrencyType,
				SubmitDate = i.SubmitDate,
				PreviousInvoiceNumber = i.PreviousInvoiceNumber
			}).ToListAsync(cancellationToken);

		return invoicePreviewVMList;
	}
	public async Task<bool> CreateAsync(InvoiceCreateVM vM, int userId, CancellationToken cancellationToken)
	{
		// manage InvoiceNumber (id)

		if (vM.InvoiceNumber <= 0 || await AnyAsync(userId, vM.InvoiceNumber, cancellationToken))
			vM.InvoiceNumber = GenerateUniqueId(userId);



		// manage customer name

		if (vM.CustomerFullName.HasValue())
		{
			vM.CustomerFullName = vM.CustomerFullName.CleanString();

			var customer = await _customerRepository.GetByIdAsync(cancellationToken, userId, vM.CustomerFullName);

			if (customer is null)
			{
				customer = new Customer { UserId = userId, CustomerFullName = vM.CustomerFullName! };
				await _customerRepository.AddAsync(customer, cancellationToken, saveNow: false);
			}
		}

		// create invoice

		ICollection<Row> rows = RowVMsToRows(vM, userId);

		var invoice = new Invoice()
		{
			UserId = userId,
			InvoiceNumber = vM.InvoiceNumber,
			Rows = rows,
			CurrencyType = vM.CurrencyType,
			Debit = vM.Debit,
			Prepayment = vM.Prepayment,
			Description = vM.Description,
			InvoiceType = vM.InvoiceType,
			CustomerFullName = vM.CustomerFullName,
			SubmitDate = vM.WantToSubmit ? DateTime.Now : null,
			PreviousInvoiceNumber = vM.PreviousInvoiceNumber,
			PreviousUserId = vM.PreviousInvoiceNumber == null ? null : userId
		};
		invoice.CalculatePrivateFields();

		await _invoiceRepository.AddAsync(invoice, cancellationToken);
		return true;
	}
	public async Task UpdateAsync(InvoiceCreateVM vM, int userId, CancellationToken cancellationToken)
	{
		var invoice = await _invoiceRepository.GetByIdAsync(cancellationToken, userId, vM.InvoiceNumber);

		ICollection<Row> rows = RowVMsToRows(vM, userId);

		invoice.InvoiceNumber = vM.InvoiceNumber;
		invoice.InvoiceType = vM.InvoiceType;
		invoice.Rows = rows;
		invoice.CurrencyType = vM.CurrencyType;
		invoice.Debit = vM.Debit;
		invoice.Prepayment = vM.Prepayment;
		invoice.Description = vM.Description;
		invoice.CustomerFullName = vM.CustomerFullName;
		invoice.SubmitDate = vM.WantToSubmit ? DateTime.Now : null;
		invoice.CalculatePrivateFields();

		await _invoiceRepository.UpdateAsync(invoice, cancellationToken);
	}
	public async Task DeleteAsync(int userId, int invoiceNumber, CancellationToken cancellationToken)
	{

		var Invoice = await _invoiceRepository.GetByIdAsync(cancellationToken, userId, invoiceNumber);
		if (Invoice is null)
			throw new KeyNotFoundException("شماره فاکتور موردنظر یافت نشد.");


		if (Invoice.SubmitDate.HasValue)
		{
			if (await _invoiceRepository.TableNoTracking.AnyAsync(i => i.UserId == userId && i.PreviousInvoiceNumber == invoiceNumber, cancellationToken))
				throw new InvalidOperationException("یک یا چند فاکتور درحال اشاره به این شماره‌فاکتور هستند. برای پاک شدن این فاکتور ابتدا فاکتورهای ارجاع شده را پاک نمایید.");

			Invoice.IsDeleted = true;
			await _invoiceRepository.UpdateAsync(Invoice, cancellationToken);
		}
		else
		{
			await _invoiceRepository.DeleteAsync(Invoice, cancellationToken);
		}

	}

	private ICollection<Row> RowVMsToRows(InvoiceCreateVM vM, int userId)
	{

		ICollection<Row> rows = new List<Row>();
		
		foreach (var rowVM in vM.RowVMs)
		{
			if (rowVM.Title is null)
				continue;

			if (!_productOrServiceRepository.TableNoTracking.Any(p => p.UserId == userId && p.Title == rowVM.Title))
				_productOrServiceRepository.Add(new ProductOrService() { UserId = userId, Title = rowVM.Title }, false);

			var row = new Row()
			{
				Title = rowVM.Title,
				PriceForEach = rowVM.PriceForEach ?? 0,
				Quantity = rowVM.Quantity ?? 1,
				Discount = rowVM.Discount ?? 0,
				TaxPercent = rowVM.TaxPercent ?? 0,
				InvoiceNumber = vM.InvoiceNumber,
				InvoiceUserId = userId
			};
			row.CalculatePrivateFields();
			rows.Add(row);
		}
		if (rows.Count == 0)
			throw new NoNullAllowedException‌("تکمیل حداقل یک ردیف الزامی می‌باشد.");

		return rows;
	}

	public int GenerateUniqueId(int userId)
	{
		var userInvoices = _invoiceRepository.TableNoTracking.IgnoreQueryFilters().Where(i => i.UserId.Equals(userId));
		if (!userInvoices.Any())
			return 1;

		return userInvoices.Max(i => i.InvoiceNumber) + 1;
	}

	public async Task<IEnumerable<string>> GetAllCustomerFullNamesAsync(int userId, CancellationToken cancellationToken)
	{
		var customers = await _customerRepository.TableNoTracking.Where(c => c.UserId.Equals(userId)).Select(c => c.CustomerFullName).ToListAsync(cancellationToken);
		return customers;
	}
	public async Task<IEnumerable<string>> GetAllProductOrServicesAsync(int userId, CancellationToken cancellationToken)
	{
		var products = await _productOrServiceRepository.TableNoTracking.Where(c => c.UserId.Equals(userId)).Select(c => c.Title).ToListAsync(cancellationToken);
		return products;
	}
}
