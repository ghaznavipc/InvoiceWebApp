namespace NazhoFactor.Controllers;

[Authorize]
public class InvoiceController(IInvoiceService invoiceService) : Controller
{
	private readonly IInvoiceService _invoiceService = invoiceService;


    // GET: Invoice/List
    [Route("[controller]")]
	public async Task<ActionResult> List(CancellationToken cancellationToken)
	{
		var userId = User.Identity!.GetUserId().ToInt();
		var overviewVMs = await _invoiceService.GetAsync(userId, cancellationToken);

		return View(overviewVMs);
	}

    // GET: Invoice/5
    [HttpGet("{controller}/{id:int:required}")]
	public async Task<ActionResult> Index(int id, CancellationToken cancellationToken)
	{
		var userId = User.Identity!.GetUserId().ToInt();

		var invoice = await _invoiceService.GetAsync(userId: userId, invoiceNumber: id, cancellationToken);
		if (invoice is null) return NotFound();

		var invoiceVM = invoice.ToInvoiceDetailsVM();
		return View(invoiceVM);
	}

	// GET: Invoice/Create
	[HttpGet]
	public async Task<ActionResult> Create(int id, CancellationToken cancellationToken)
	{
		var userId = User.Identity!.GetUserId().ToInt();

		var vM = new InvoiceCreateVM() { RowVMs = null! };

		if (id > 0)
		{
			var invoice = await _invoiceService.GetAsync(userId: userId, invoiceNumber: id, cancellationToken);
			if (invoice is null)
				return NotFound();

			vM = invoice.ToInvoiceCreateVM();
			vM.PreviousInvoiceNumber = id;
		}

		vM.InvoiceNumber = _invoiceService.GenerateUniqueId(userId);
		vM.CustomerFullNames = await _invoiceService.GetAllCustomerFullNamesAsync(userId, cancellationToken);
		vM.ProductOrServiceTitles = await _invoiceService.GetAllProductOrServicesAsync(userId, cancellationToken);

		return View(vM);
	}

	// POST: Invoice/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> CreateAsync(InvoiceCreateVM vM, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View();

		var userId = User.Identity!.GetUserId().ToInt();

		await _invoiceService.CreateAsync(vM, userId, cancellationToken);
		return RedirectToAction(nameof(List));
	}


	// GET: Invoice/Update/5
	[HttpGet]
	public async Task<ActionResult> Update(int id, CancellationToken cancellationToken)
	{

		var userId = User.Identity!.GetUserId().ToInt();
		var invoice = await _invoiceService.GetAsync(userId: userId, invoiceNumber: id, cancellationToken);
		if (invoice is null)
			return NotFound();

		if (!invoice.SubmitDate.HasValue)
		{
			// Update is allowed for not submitted ones.

			var vM = invoice.ToInvoiceCreateVM();

			vM.CustomerFullNames = await _invoiceService.GetAllCustomerFullNamesAsync(userId, cancellationToken);
			vM.ProductOrServiceTitles = await _invoiceService.GetAllProductOrServicesAsync(userId, cancellationToken);

			return View(vM);
		}

		if (!await _invoiceService.AnyRefrencedToThisAsync(userId, invoiceNumber: id, cancellationToken))
			return RedirectToAction(nameof(Create), new { id }); // create a new one that reference on it.

		// not allowed to reference multiple
		TempData["Type"] = "danger";
		TempData["Message"] = "نمی‌توان چند فاکتور را به یکی عطف داد. در حال حاضر فاکتور دیگری به این فاکتور عطف دارد.";

		return RedirectToAction(nameof(List));

	}

	// POST: Invoice/Update/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> UpdateAsync(InvoiceCreateVM vM, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View();

		var userId = User.Identity!.GetUserId().ToInt();

		// update it.
		await _invoiceService.UpdateAsync(vM, userId, cancellationToken);

		return RedirectToAction(nameof(Index), vM.InvoiceNumber);
	}



	// GET: Invoice/Delete/5
	[HttpGet]
	public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
	{
		var userId = User.Identity!.GetUserId().ToInt();
		var invoice = await _invoiceService.GetAsync(userId: userId, invoiceNumber: id, cancellationToken);

		if (invoice is null)
			return NotFound();

		var invoiceDeleteVM = new InvoiceDeleteVM()
		{
			InvoiceNumber = invoice.InvoiceNumber,
			InvoiceType = invoice.InvoiceType,
			FinalPrice = invoice.FinalPrice,
			CurrencyType = invoice.CurrencyType,
		};

		return View(invoiceDeleteVM);
	}

	// POST: Invoice/Delete/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteAsync(InvoiceDeleteVM vM, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View(vM);

		var userId = User.Identity!.GetUserId().ToInt();

		try
		{
			await _invoiceService.DeleteAsync(userId, vM.InvoiceNumber, cancellationToken);
			return RedirectToAction(nameof(List));
		}
        catch (KeyNotFoundException e)
		{
			ModelState.AddModelError("", e.Message);
		}
        catch (InvalidOperationException e)
		{
			ModelState.AddModelError("", e.Message);
		}
		catch (Exception)
		{
			ModelState.AddModelError("", "خطای غیرمنتظره‌ای در هنگام حذف کردن رخ داد.");
        }

		return View(vM);
	}
}
