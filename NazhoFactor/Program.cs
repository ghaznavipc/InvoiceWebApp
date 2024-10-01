var builder = WebApplication.CreateBuilder(args);
var siteSettings = builder.Configuration.GetSection(nameof(SiteSettings));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddCors();
builder.Services.Configure<SiteSettings>(siteSettings);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

var option = new DbContextOptionsBuilder<ApplicationDbContext>();
option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string not found."));
using (var context = new ApplicationDbContext(option.Options))
{
	context.Database.Migrate();
}

builder.Services.AddCustomIdentity(siteSettings.Get<IdentitySettings>());

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

	await RolesInitializer.InitializeAsync(userManager, roleManager);
}

await app.RunAsync();

/*
public class Program
{
public static int itest { get; set; }
public static int MyProperty { get; set; }
public static HttpContextAccessor h2 { get; set; }


public static void Main(string[] args)
{

	IRepository<MasterSetting> mastersetting;

	//   itest = tt.db.Set<MasterSetting>().FromSqlRaw("select * from MasterSettings").ToList().FirstOrDefault().CustomerID;
	CreateHostBuilder(args).Build().Run();
	//ViewData["customerId"] = 3;
	//HttpContext.Session.SetString("SettingCustomerName", "3");

}
public static IHostBuilder CreateHostBuilder(string[] args) =>
	Host.CreateDefaultBuilder(args)           
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>()
		   .UseKestrel(opt => {
			   var sp = opt.ApplicationServices;

		   using (var scope = sp.CreateScope)
			   {
				   var dbContext = scope.ServiceProvider.grt .get<ApplicationDbContext>();
				   var e = dbContext.Certificates.FirstOrDefault();
				   // now you get the certificates
			   }
		   });
		});
   

}
*/
