# Blazor RBAC Demo

![.NET 9](https://img.shields.io/badge/.NET-9.0-blue)  
![Blazor Server](https://img.shields.io/badge/Blazor-Server-purple)  
![License](https://img.shields.io/badge/License-MIT-green)  

Role-based access control (RBAC) demo built with **Blazor Server (.NET 9)** and **ASP.NET Core Identity**.  
This project demonstrates common enterprise patterns through two focused case studies:

- **Case Study 1: RBAC Dashboards + Policies**  
  Seeded roles (Admin / Manager / User), claims-based policies (`CanApprove`, `CanExport`), guarded dashboards, and Admin-only CSV export.

- **Case Study 2: Orders CRUD + Approvals**  
  Full CRUD page for an `Order` entity with role-gated actions:  
  - Users can create/edit their own orders.  
  - Managers can approve/reject submitted orders.  
  - Admins can edit/delete any order and export to CSV.

---

## ✨ Highlights

- **ASP.NET Core Identity** with EF Core (SQLite)
- **Seeded roles & users**: Admin, Manager, User
- **Case Study 1: Dashboards & Policies**  
  - Claims-based policies: `CanApprove`, `CanExport`  
  - Role-based dashboards: Admin / Manager / User  
  - Admin-only CSV export at `/exports/system-report.csv`
- **Case Study 2: Orders CRUD + Approvals**  
  - Create, edit, delete, approve, reject workflow  
  - Role-gated actions (User, Manager, Admin)  
  - Concurrency-safe EF Core updates
- Clean **Nav** using `<AuthorizeView>` and protected routes via `[Authorize]`

---

## Seeded Users

(Demo accounts are seeded locally for testing — not connected to any external service.)

| Role    | Email                 | Password   | Access                          |
|---------|-----------------------|------------|---------------------------------|
| Admin   | `admin@demo.local`   | `P@ssw0rd!` | Admin + Manager + User pages<br/>Can Approve + Export |
| Manager | `manager@demo.local` | `P@ssw0rd!` | Manager + User pages<br/>Can Approve |
| User    | `user@demo.local`    | `P@ssw0rd!` | User dashboard only |

---

🔒 Policies

CanApprove → requires claim perm=approve (Manager/Admin)

CanExport → Admin role or claim perm=export

Configured in Program.cs:

opts.AddPolicy("CanApprove", p => p.RequireClaim("perm", "approve"));
opts.AddPolicy("CanExport",  p => p.RequireAssertion(ctx =>
    ctx.User.IsInRole("Admin") || ctx.User.HasClaim("perm","export")));

---

🧭 Pages

Home (/) – role matrix, quick links, login status

My Dashboard (/me) – profile card, role badges, claims viewer

Manager (/manager) – pending requests table, approve action (policy-gated)

Admin (/admin) – Admin-only; CSV export link (policy-gated)

---

📄 CSV Export

Minimal API endpoint protected by CanExport:

GET /exports/system-report.csv

---

## 🛠Running Locally

1. Clone the repo:
   ```bash
   git clone https://github.com/<your-username>/BlazorRbacDemo.git
   cd BlazorRbacDemo
2. Apply migrations & create the SQLite database:
   dotnet ef database update
3. Run the app:
   dotnet run
4. Navigate to https://localhost:5001

## 📸 Screenshots

### Case Study 1: RBAC Dashboards + Policies

#### 🔐 Login
![Login](docs/Screenshot-Login.png)

#### 🏠 Home / Role Matrix
![Home](docs/ScreenshotHome.png)

#### 👤 User Dashboard
![User Dashboard](docs/ScreenshotUserDashboard.png)

#### 👔 Manager Dashboard
![Manager Dashboard](docs/ScreenshotManagerDashboard.png)

#### 🛡️ Admin Dashboard
![Admin Dashboard](docs/ScreenshotAdminDashboard.png)

---
### Case Study 2: Orders CRUD + Approvals

#### 📦 Orders CRUD
Users can create new orders, Managers can approve/reject submitted orders, and Admins have full control (edit/delete).  

#### ✏️ Add / Edit Order
![Orders Create](docs/Screenshot-Orders-Edit.png)

#### 📋 Orders List
![Orders List](docs/Screenshot-Orders-List.png)


🔮 Next Steps

**Case Study 1: RBAC Dashboards + Policies**
- [ ] Add external authentication providers (Google, Microsoft, etc.)
- [ ] Role management UI for Admins (create/edit roles, assign users)
- [ ] Expand claims-based policies (e.g., `CanDelete`, `CanViewReports`)

**Case Study 2: Orders CRUD + Approvals**
- [ ] Add file attachments (e.g., PDFs, invoices) to orders
- [ ] Implement audit logs for approvals/rejections
- [ ] Add pagination, filtering, and search to the Orders grid
- [ ] Export orders to CSV/Excel (Admin only)
- [ ] Replace SQLite with SQL Server/PostgreSQL for enterprise workflows

**General / Infrastructure**
- [ ] Deploy to Azure App Service for live demo access
- [ ] Improve UI polish with Bootstrap cards, modals, and responsive layout 


📚 Tech Stack

Blazor Server (.NET 9)

ASP.NET Core Identity

Entity Framework Core 9 (SQLite provider)

Bootstrap 5 for UI

---

📦 Project structure (key files)
Data/
  AppDbContext.cs        # AppUser + DbContext
  SeedDataService.cs     # seeds roles/users/claims
Pages/
  Index.razor            # Role matrix & quick links
  Me.razor               # Profile dashboard
  Manager.razor          # Approvals (policy-gated)
  Admin.razor            # Export (policy-gated)
  _ViewImports.cshtml
  Shared/_LoginPartial.cshtml
Program.cs               # Identity, policies, minimal API for CSV

---

📝 Notes

This is a demo project—credentials and claims are seeded in code for clarity.

Swap SQLite for SQL Server/Postgres by changing the EF provider & connection string.
