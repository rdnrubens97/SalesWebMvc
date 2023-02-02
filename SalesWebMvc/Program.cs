using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using SalesWebMvc;
using Microsoft.Extensions.Localization;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionStringMysql = builder.Configuration.GetConnectionString("SalesWebMvcContext");
builder.Services.AddDbContext<SalesWebMvcContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMvcContext"), MySqlServerVersion.LatestSupportedServerVersion, builder => builder.MigrationsAssembly("SalesWebMvc")));

builder.Services.AddScoped<SeedingService>();
//builder.Services.AddScoped<SellerService>();
//builder.Services.AddScoped<DepartmentService>();
//builder.Services.AddScoped<SalesRecordService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Abaixo exemplo de como rodar a injeção de dependência, no caso o SeedService adicionado acima: "builder.Services.AddScoped<SeedingService>();",
//no Startup do programa. Apesar de ter uma lógica para não repopular o banco, vou deixar aqui comentado pra
//não ficar rodando sempre que executar o programa.

app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingService>().Seed();

app.UseDeveloperExceptionPage();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

