# 🛒 MyShop — E-Commerce Shopping Cart

A full-featured e-commerce web application built with **ASP.NET Core 8 MVC**, featuring product browsing, shopping cart management, Stripe payment integration, and a complete admin dashboard.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Areas & Pages](#areas--pages)
- [Roles & Permissions](#roles--permissions)
- [Payment Flow](#payment-flow)
- [Order Lifecycle](#order-lifecycle)
- [Database](#database)
- [Configuration](#configuration)

---

## Overview

MyShop is a multi-layer ASP.NET Core 8 MVC e-commerce application. Customers can browse paginated products, view product details with related items, add to cart, and checkout via **Stripe**. Admins manage products, categories, orders, and users through a dedicated dashboard area.

---

## Architecture

The project follows a **layered N-tier architecture** split into 4 projects:

```
┌────────────────────────────────────────┐
│         MyshopwebApplication           │  ← MVC Web (Areas, Views, Controllers)
├────────────────────────────────────────┤
│          MyShop.DataAccess             │  ← EF Core, Repositories, Migrations
├────────────────────────────────────────┤
│           MyShop.Entities              │  ← Domain Models, Interfaces, ViewModels
├────────────────────────────────────────┤
│          MyShop.Etuilities             │  ← Constants, Email Sender, Stripe Config
└────────────────────────────────────────┘
```

**Patterns used:** Repository Pattern, Unit of Work, Dependency Injection.

---

## Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core 8 MVC | Web framework with Razor Views |
| Entity Framework Core 8 | ORM & database access |
| SQL Server | Relational database |
| ASP.NET Core Identity | Authentication & user management |
| Stripe.net | Payment processing |
| X.PagedList | Server-side pagination |
| Razor Runtime Compilation | Hot-reload for Razor views |

---

## Features

- ✅ **Product Catalog** — Paginated product listing (8 per page) with category filtering
- ✅ **Product Details** — Full product page with related products from the same category
- ✅ **Shopping Cart** — Add, increment, decrement, remove items; session-based cart count badge
- ✅ **Order Summary** — Auto-populated from user profile with editable shipping info
- ✅ **Stripe Checkout** — Secure hosted payment via Stripe Sessions
- ✅ **Order Confirmation** — Payment verification and cart clearance post-payment
- ✅ **Stripe Refunds** — Automatic refund on order cancellation if already paid
- ✅ **Admin Dashboard** — Manage products, categories, orders, and users
- ✅ **Image Upload** — Product images stored in `wwwroot/Images/Products`; old images deleted on update
- ✅ **Role-Based Access** — Admin, Editor, and Customer roles with area-level authorization
- ✅ **Email Confirmation** — Account registration requires email confirmation
- ✅ **Auto DB Seeding** — Roles and seed data initialized on first run
- ✅ **Account Lockout** — 4-hour lockout policy on failed login attempts

---

## Project Structure

```
ShoppingCart/
│
├── MyshopwebApplication/                   ← Main MVC Web Project
│   ├── Areas/
│   │   ├── Admin/
│   │   │   ├── Controllers/
│   │   │   │   ├── DashboardController.cs  # Admin home
│   │   │   │   ├── CatigoryController.cs   # Category CRUD
│   │   │   │   ├── ProductController.cs    # Product CRUD + image upload
│   │   │   │   ├── OrderController.cs      # Order management + refunds
│   │   │   │   └── UsersController.cs      # User management
│   │   │   └── Views/
│   │   │       ├── Dashboard/
│   │   │       ├── Catigory/
│   │   │       ├── Product/
│   │   │       └── Order/
│   │   │
│   │   └── Customer/
│   │       ├── Controllers/
│   │       │   ├── CustomerController.cs   # Product listing & details
│   │       │   └── CartController.cs       # Cart, checkout, Stripe, confirmation
│   │       └── Views/
│   └── Program.cs
│
├── MyShop.DataAccess/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Dbintializer/
│   │   └── Dbintializer.cs                 # Seeds roles & default data
│   ├── Implemention/
│   │   ├── GenericRepository.cs
│   │   ├── ProductRepository.cs
│   │   ├── CatigoryRepository.cs
│   │   ├── OrderHeaderRepository.cs        # UpdateOrderStatus helper
│   │   ├── OrderDetailRepository.cs
│   │   ├── ShopingCartRepository.cs
│   │   ├── ApplicationUserRepository.cs
│   │   └── UnitOfWork.cs
│   └── Migrations/
│
├── MyShop.Entities/
│   ├── Models/
│   │   ├── Product.cs                      # Name, Price, BeforePrice, Image, Category
│   │   ├── Catigory.cs
│   │   ├── ShopingCart.cs                  # Count 1–100, FK to Product & User
│   │   ├── OrderHeader.cs                  # Status, Payment, Stripe Session/Intent IDs
│   │   ├── OrderDetail.cs                  # Line items per order
│   │   └── ApplicationUser.cs             # Extended Identity with Name, City, Address
│   ├── Repositories/                       # Interfaces: IGenericRepository, IUnitOfWork...
│   └── ViewModel/
│       ├── ProductVm.cs
│       ├── ProductShopingCart.cs
│       ├── ShoppingCartView.cs
│       ├── ShoppingCartVM.cs
│       └── OrderVM.cs
│
└── MyShop.Etuilities/
    ├── DS.cs                               # Roles constants & Session key
    ├── StripeDetails.cs                    # Publishable/Secret key config binding
    └── EmailSender.cs                      # IEmailSender implementation (SMTP ready)
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Stripe account](https://stripe.com) (test keys)

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/your-username/MyShop.git
cd MyShop
```

**2. Configure the connection string**

Edit `MyshopwebApplication/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=MyShopDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

**3. Configure Stripe keys**
```json
"Stripe": {
  "PublishableKey": "pk_test_...",
  "SecretKey": "sk_test_..."
}
```

**4. Apply migrations**
```bash
dotnet ef database update --project MyShop.DataAccess --startup-project MyshopwebApplication
```

> The `DbInitializer` also runs automatically on startup to seed roles and default data.

**5. Run the application**
```bash
dotnet run --project MyshopwebApplication
```

**6. Open the app**
```
https://localhost:44311
```

---

## Areas & Pages

### 🛍️ Customer Area (`/Customer/`)

| Route | Description |
|---|---|
| `/Customer/Customer/Index` | Home — paginated product listing |
| `/Customer/Customer/Details/{id}` | Product detail with related products |
| `/Customer/Cart/Index` | Shopping cart with item controls |
| `/Customer/Cart/Summary` | Order summary & shipping details |
| `/Customer/Cart/OrderConfirmation?id=` | Post-payment confirmation page |

### ⚙️ Admin Area (`/Admin/`)

| Route | Description |
|---|---|
| `/Admin/Dashboard` | Admin overview dashboard |
| `/Admin/Catigory` | Category management (CRUD) |
| `/Admin/Product` | Product management with image upload |
| `/Admin/Order` | Order list and detail management |
| `/Admin/Users` | User management |

---

## Roles & Permissions

| Role | Access |
|---|---|
| **Admin** | Full access to Admin area — products, categories, orders, users |
| **Editor** | (Defined, extendable for limited admin access) |
| **Customer** | Browse products, manage own cart, place orders |

Roles are defined in `DS.cs` and seeded automatically by `DbInitializer`.

---

## Payment Flow

```
Cart → Summary → POST Summary
         │
         ▼
  Stripe Checkout Session Created
         │
         ▼
  Customer completes payment on Stripe
         │
         ├── Success → /Cart/OrderConfirmation
         │               ├── Verify PaymentStatus == "paid"
         │               ├── Update order: Status="Approve", PaymentStatus="Approve"
         │               └── Clear cart + reset session count
         │
         └── Cancel → /Cart/Index
```

### Refund Flow (Order Cancellation)
```
Admin cancels order
    │
    ├── If PaymentStatus == "Approve"
    │       └── Stripe RefundService.Create() → PaymentStatus = "Refund"
    │
    └── If not paid → Status = "Cancelled", PaymentStatus = "Cancelled"
```

---

## Order Lifecycle

```
Pending  ──►  Processing  ──►  Shipped  ──►  Approve (Delivered)
                                                     │
                                              Cancelled (+ Refund if paid)
```

Order status is managed from the Admin Order Detail page and stored alongside `TrackingNumber`, `Carrier`, and `ShippingDate`.

---

## Database

### Key Entities

**Product** — `Id`, `Name`, `Description`, `Img`, `Price`, `BeforePrice`, `CatigoryId`

**ShoppingCart** — `ProductId`, `ApplicationUserId`, `Count` (1–100)

**OrderHeader** — Customer snapshot (Name, Address, City, Phone), `TotalPrice`, `OrderStatus`, `PaymentStatus`, `TrackingNumber`, `Carrier`, `SessionId`, `PaymentIntentId`

**OrderDetail** — `OrderId`, `ProductId`, `Count`, `Price` (price at time of order)

**ApplicationUser** *(extends IdentityUser)* — `Name`, `City`, `Address`, `PictureProfile`

---

## Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<SQL Server connection string>"
  },
  "Stripe": {
    "PublishableKey": "<pk_test_...>",
    "SecretKey": "<sk_test_...>",
    "WebhookSecret": "<whsec_...>"
  }
}
```

**Session** — Used to track shopping cart item count across pages via `DS.SessionKey`.

**Email** — `EmailSender` is registered as `IEmailSender`. The SMTP implementation is prepared but commented out — configure your SMTP credentials and uncomment to enable transactional email.

> ⚠️ **Security Note:** Never commit real Stripe secret keys to source control. Use environment variables or .NET User Secrets in development:
> ```bash
> dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."
> ```

---

## License

This project is licensed under the MIT License.
