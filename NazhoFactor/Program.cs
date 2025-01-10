var builder = WebApplication.CreateBuilder(args);
var siteSettings = builder.Configuration.GetSection(nameof(SiteSettings));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddCors();
builder.Services.Configure<SiteSettings>(siteSettings);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

var option = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")
			?? throw new InvalidOperationException("Connection string not found."));

using (var context = new ApplicationDbContext(option.Options))
{
	context.Database.Migrate();
}

builder.Services.AddCustomIdentity(siteSettings.Get<IdentitySettings>() ?? new());

builder.Services.AddControllersWithViews(options =>
{
	options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.SameSite = SameSiteMode.Lax;
});
builder.Services.AddResponseCompression();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.SetIsOriginAllowed(origin => true)
	.AllowCredentials());

app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var userManager = services.GetRequiredService<UserManager<User>>();
	var roleManager = services.GetRequiredService<RoleManager<Role>>();

	await RolesInitializer.Initialize(userManager, roleManager);
}

await app.RunAsync();