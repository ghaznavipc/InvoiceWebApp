
namespace MyServices;

public interface IInvoiceService
{
	Task<bool> AnyAsync(int userId, int invoiceNumber, CancellationToken cancellationToken);
	Task<bool> AnyRefrencedToThisAsync(int userId, int invoiceNumber, CancellationToken cancellationToken);
	Task<Invoice?> GetAsync(int userId, int invoiceNumber, CancellationToken cancellationToken);
	Task<IEnumerable<InvoiceOverviewVM?>> GetAsync(int userId, CancellationToken cancellationToken);
	Task<bool> CreateAsync(InvoiceCreateVM vM, int userId, CancellationToken cancellationToken);
	Task UpdateAsync(InvoiceCreateVM vM, int userId, CancellationToken cancellationToken);
	Task DeleteAsync(int userId, int invoiceNumber, CancellationToken cancellationToken);
	int GenerateUniqueId(int userId);
	Task<IEnumerable<string>> GetAllCustomerFullNamesAsync(int userId, CancellationToken cancellationToken);
	Task<IEnumerable<string>> GetAllProductOrServicesAsync(int userId, CancellationToken cancellationToken);
}