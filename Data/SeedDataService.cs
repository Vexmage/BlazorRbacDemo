using BlazorRbacDemo;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

public class SeedDataService : IHostedService
{
    private readonly IServiceProvider _sp;
    public SeedDataService(IServiceProvider sp) => _sp = sp;

    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = _sp.CreateScope();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        foreach (var role in new[] { "Admin", "Manager", "User" })
            if (!await roleMgr.RoleExistsAsync(role))
                await roleMgr.CreateAsync(new IdentityRole(role));

        async Task Ensure(string email, string role, params (string type, string val)[] claims)
        {
            var u = await userMgr.FindByEmailAsync(email);
            if (u == null)
            {
                u = new AppUser { UserName = email, Email = email, EmailConfirmed = true };
                await userMgr.CreateAsync(u, "P@ssw0rd!");
                await userMgr.AddToRoleAsync(u, role);
                foreach (var (t, v) in claims)
                    await userMgr.AddClaimAsync(u, new Claim(t, v));
            }
        }

        await Ensure("admin@demo.local", "Admin", ("perm", "approve"), ("perm", "export"));
        await Ensure("manager@demo.local", "Manager", ("perm", "approve"));
        await Ensure("user@demo.local", "User");
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
