# Blazor RBAC Demo

A quick **role-based access control (RBAC) demo** built with **Blazor Server (.NET 9)** and **ASP.NET Core Identity**.  
This project shows how to implement **roles, claims-based policies, and guarded UI navigation** — a common requirement in ERP and enterprise applications.

---

## What This Proves
- ✅ Integration of **ASP.NET Core Identity** with EF Core & SQLite  
- ✅ **Seeded roles and users** (Admin, Manager, User) created at startup  
- ✅ **Claims-based policies** (e.g., “CanApprove”, “CanExport”)  
- ✅ **Role-specific dashboards** with restricted access  
- ✅ **Blazor `<AuthorizeView>` and `[Authorize]` attributes** for dynamic nav and page security  
- ✅ Ready to extend with data grids, exports, and ERP-style features  

---

## Seeded Users

| Role    | Email                 | Password   | Access                          |
|---------|-----------------------|------------|---------------------------------|
| Admin   | `admin@demo.local`   | `P@ssw0rd!` | Admin + Manager + User pages<br/>Can Approve + Export |
| Manager | `manager@demo.local` | `P@ssw0rd!` | Manager + User pages<br/>Can Approve |
| User    | `user@demo.local`    | `P@ssw0rd!` | User dashboard only |

---

## 🛠Running Locally

1. Clone the repo:
   ```bash
   git clone https://github.com/<your-username>/BlazorRbacDemo.git
   cd BlazorRbacDemo
