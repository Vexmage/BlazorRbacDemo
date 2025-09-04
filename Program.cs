using BlazorRbacDemo; // AppUser, AppDbContext
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorRbacDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=rbac.db"));

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 6;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("CanApprove", p => p.RequireClaim("perm", "approve"));
    opts.AddPolicy("CanExport", p => p.RequireAssertion(ctx =>
        ctx.User.IsInRole("Admin") || ctx.User.HasClaim("perm", "export")));
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHostedService<SeedDataService>();
builder.Services.AddHttpContextAccessor(); // optional but useful
builder.Services.AddScoped<IAuditLogger, AuditLogger>();

var app = builder.Build();

// OPTIONAL: auto-migrate DB on startup
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     db.Database.Migrate();
// }

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// CSV export endpoint (policy-protected)
app.MapGet("/orders/export.csv", [Authorize(Roles = "Admin")] async (AppDbContext db) =>
{
    var rows = await db.Orders
        .OrderBy(o => o.Id)
        .Select(o => new
        {
            o.Id,
            o.Title,
            o.Amount,
            o.Status,
            o.CreatedByUserId,
            o.CreatedAtUtc,
            o.ApprovedAtUtc
        })
        .ToListAsync();

    var sb = new System.Text.StringBuilder();
    sb.AppendLine("Id,Title,Amount,Status,CreatedBy,CreatedAtUtc,ApprovedAtUtc");
    foreach (var r in rows)
    {
        var safeTitle = (r.Title ?? string.Empty).Replace("\"", "\"\"");
        sb.AppendLine($"{r.Id},\"{safeTitle}\",{r.Amount},{r.Status},{r.CreatedByUserId},{r.CreatedAtUtc:o},{r.ApprovedAtUtc:o}");
    }

    var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
    return Results.File(bytes, "text/csv", "Orders.csv");
});

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
