# Warehouse Management System

A data science-focused passion project designed to simulate warehouse and supply chain operations. Built with Visual Studio using WinForms.

---

## ℹ️ About the Project

The **Warehouse Management System** is an educational simulation that features:

- Multiple warehouse, item, supplier and customer tracking
- Inventory panel with general and warehouse-specific views
- CRUD operations through a GUI
- SQL database connectivity
- Simulated supply chain features (e.g., transfers, purchase orders, shipment approval)

---

## 🛠️ Built With

- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) — primary programming language
- [WinForms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/) — UI development
- [Microsoft Visual Studio](https://visualstudio.microsoft.com/) — development environment
- [SQL Server / MySQL](https://www.mysql.com/) — backend database
- [Guna UI2](https://www.nuget.org/packages/Guna.UI2.WinForms/) — modern UI elements

---

## 📦 Getting Started

### Prerequisites

To run and build the project locally, you'll need:

- Visual Studio (2022 or newer)
- .NET Desktop Development workload installed
- Access to a SQL database (e.g., SQL Server or MySQL)
- Git (optional)

---

### Installation & Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/CennethWilson/warehouse-management-system.git
   cd warehouse-management-system

2. **Open the project in Visual Studio:**

   Open `Warehouse Management System.sln`.

3. **Configure database connection:**

   Update the database connection string in `settings.json`.
   ```bash
   Server= ;Port= ;Database= ;User ID= ;Password= ;

4. **Create database tables:**

   Run the provided `init_db.sql` to create the necessary tables.

5. **Build the project:**

   `Ctrl + Shift + B`  (or use the Build menu)

6. **Run the application**

## 📃 License

This project is licensed under the MIT License. See the `LICENSE.txt` file for more information.
