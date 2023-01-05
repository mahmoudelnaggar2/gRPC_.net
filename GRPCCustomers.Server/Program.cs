using GRPCCustomers.Server.Data;
using GRPCCustomers.Server.gRPCServiceImplmentation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterServices(builder);

var app = builder.Build();

SetupMiddleware(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

    builder.Services.AddRazorPages();

    builder.Services.AddGrpc(cfg => cfg.EnableDetailedErrors = true);

    builder.Services.AddGrpcReflection();
}

static void SetupMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
       
        app.UseHsts();
        app.UseHttpsRedirection();
    }   

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapRazorPages();

    app.MapGrpcService<CustomerService>();

    app.MapGrpcReflectionService();
}
