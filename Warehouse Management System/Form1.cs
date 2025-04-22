using Google.Protobuf.WellKnownTypes;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace Warehouse_Management_System
{
    public partial class Form1 : Form
    {
        Panel selectedButton;
        Panel selectedPanel;
        List<Panel> panelList;

        Panel selectedDropdown = null;
        bool isBusyDropdwn = false;
        Panel dropdownExpand;
        Panel dropdownCollapse;

        Dictionary<string, Guna2Panel> sampleDictionary = new Dictionary<string, Guna2Panel>();
        Dictionary<Panel, List<List<object>>> entryTextBox = new Dictionary<Panel, List<List<object>>>();
        Dictionary<string, FlowLayoutPanel> flowLayoutDictionary = new Dictionary<string, FlowLayoutPanel>();
        Dictionary<string, Guna2TextBox> textBoxDictionary = new Dictionary<string, Guna2TextBox>();
        Dictionary<string, Dictionary<string, string>> dataTypeDictionary = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> insertQueryTemplate = new Dictionary<string, string>();

        Dictionary<int, string> itemIdToName = new Dictionary<int, string>();
        bool haveInitItemIdToName = false;

        public Form1()
        {
            InitializeComponent();
            selectedButton = button_1;
            selectedPanel = mainPanel_1;
            panelList = new List<Panel>{
                mainPanel_1,
                mainPanel_2,
                mainPanel_3,
                mainPanel_4,
                mainPanel_5,
                mainPanel_6,
                mainPanel_7,
                mainPanel_8,
            };

            // sampleDictionary
            sampleDictionary = new Dictionary<string, Guna2Panel>
            {
                ["2"] = sampleCell_2,
                ["3"] = sampleCell_3,
                ["4"] = sampleCell_4,
                ["5"] = sampleCell_5,
                ["6"] = sampleCell_6,
                ["7"] = sampleCell_7,
                ["8"] = sampleCell_8,
            };

            // flowLayoutDictionary
            flowLayoutDictionary = new Dictionary<string, FlowLayoutPanel>
            {
                ["2"] = mainPanel_2_flowLayoutPanel,
                ["3"] = mainPanel_3_flowLayoutPanel,
                ["4"] = mainPanel_4_flowLayoutPanel,
                ["5"] = mainPanel_5_flowLayoutPanel,
                ["6"] = mainPanel_6_flowLayoutPanel,
                ["7"] = mainPanel_7_flowLayoutPanel,
                ["8"] = mainPanel_8_flowLayoutPanel,
            };

            // TextboxDictionary
            textBoxDictionary = new Dictionary<string, Guna2TextBox>
            {
                ["2"] = mainPanel_2_searchByIdBox,
                ["3"] = mainPanel_3_searchByIdBox,
                ["4"] = mainPanel_4_searchByIdBox,
                ["5"] = mainPanel_5_searchByIdBox,
                ["6"] = mainPanel_6_searchByIdBox,
                ["7"] = mainPanel_7_searchByIdBox,
                ["8"] = mainPanel_8_searchByIdBox,
            };

            // entryTextBox
            entryTextBox = new Dictionary<Panel, List<List<object>>>
            {
                [mainPanel_2_addButton] = new List<List<object>> // Items
                {
                    new List<object>() {"name", ReturnStringValidator(
                        mainPanel_2_q1_textbox, 
                        "Item Name")},

                    new List<object>() {"cost", ReturnNumValidator(
                        mainPanel_2_q2_textbox, 
                        "Item Cost")},

                    new List<object>() {"price", ReturnNumValidator(
                        mainPanel_2_q3_textbox, 
                        "Item Price")},

                    new List<object>() {"date_of_creation", new Func<object>(() => DateTime.Now)},
                },

                [mainPanel_3_addButton] = new List<List<object>> // Warehouses
                {
                    new List<object>() {"name", ReturnStringValidator(
                        mainPanel_3_q1_textbox, 
                        "Warehouse Name")},

                    new List<object>() {"location", ReturnStringValidator(
                        mainPanel_3_q2_textbox, 
                        "Warehouse Location")},

                    new List<object>() {"capacity", ReturnNumValidator(
                        mainPanel_3_q3_textbox, 
                        "Warehouse Capacity")},

                    new List<object>() {"date_of_creation", new Func<object>(() => DateTime.Now)},
                },

                [mainPanel_4_addButton] = new List<List<object>> // Warehouse Transfers
                {
                    new List<object>() {"origin_warehouse_id", ReturnIdValidator(
                        mainPanel_4_q1_textbox, 
                        "Warehouses", 
                        "warehouse_id", 
                        "Origin Warehouse Id")},

                    new List<object>() {"target_warehouse_id", ReturnIdValidator(
                        mainPanel_4_q2_textbox, 
                        "Warehouses", 
                        "warehouse_id", 
                        "Target Warehouse Id")},

                    new List<object>() {"item_id", ReturnIdValidator(
                        mainPanel_4_q3_textbox, 
                        "Items", 
                        "item_id", 
                        "Item Id")},

                    new List<object>() {"quantity", ReturnNumValidator(
                        mainPanel_4_q4_textbox, 
                        "Quantity")},

                    new List<object>() {"status", new Func<object>(() => 0)},
                    new List<object>() {"shipment_type", new Func<object>(() => 0)},
                },

                [mainPanel_5_addButton] = new List<List<object>> // Suppliers
                {
                     new List<object>() {"name", ReturnStringValidator(
                        mainPanel_5_q1_textbox,
                        "Supplier Name")},
                     new List<object>() {"date_of_creation", new Func<object>(() => DateTime.Now)},
                },

                [mainPanel_6_addButton] = new List<List<object>> // Purchses
                {
                    new List<object>() {"supplier_id", ReturnIdValidator(
                        mainPanel_6_q1_textbox, 
                        "Suppliers", 
                        "supplier_id", 
                        "Supplier Id")},

                    new List<object>() {"target_warehouse_id", ReturnIdValidator(
                        mainPanel_6_q2_textbox,
                        "Warehouses",
                        "warehouse_id",
                        "Warehouse Id")},

                    new List<object>() {"item_id", ReturnIdValidator(
                        mainPanel_6_q3_textbox,
                        "Items",
                        "item_id",
                        "Item Id")},

                    new List<object>() {"quantity", ReturnNumValidator(
                        mainPanel_6_q4_textbox, 
                        "Quantity")},

                    new List<object>() {"status", new Func<object>(() => 0)},
                    new List<object>() {"shipment_type", new Func<object>(() => 1)},
                },

                [mainPanel_7_addButton] = new List<List<object>> // Customers
                {
                     new List<object>() {"name", ReturnStringValidator(
                        mainPanel_7_q1_textbox,
                        "Customer Name")},
                     new List<object>() {"date_of_creation", new Func<object>(() => DateTime.Now)},
                },

                [mainPanel_8_addButton] = new List<List<object>> // Sales
                {
                    new List<object>() {"origin_warehouse_id", ReturnIdValidator(
                        mainPanel_8_q1_textbox,
                        "Warehouses",
                        "warehouse_id",
                        "Origin Warehouse Id")},

                    new List<object>() {"customer_id", ReturnIdValidator(
                        mainPanel_8_q2_textbox, 
                        "Customers", 
                        "customer_id", 
                        "Customer Id")},

                    new List<object>() {"item_id", ReturnIdValidator(
                        mainPanel_8_q3_textbox,
                        "Items",
                        "item_id",
                        "Item Id")},

                    new List<object>() {"quantity", ReturnNumValidator(
                        mainPanel_8_q4_textbox, 
                        "Quantity")},

                    new List<object>() {"status", new Func<object>(() => 0)},
                    new List<object>() {"shipment_type", new Func<object>(() => 2)},
                },
            };

            // dataTypeDictionary
            dataTypeDictionary = new Dictionary<String, Dictionary<string, string>>
            {
                ["2"] = new Dictionary<string, string> // Items
                {
                    ["1"] = "item_id",
                    ["2"] = "name",
                    ["3"] = "date_of_creation",
                    ["4"] = "formatted_cost",
                    ["5"] = "formatted_price",
                    ["6"] = "total_quantity",
                },

                ["3"] = new Dictionary<string, string> // Warehouse
                {
                    ["1"] = "warehouse_id",
                    ["2"] = "name",
                    ["3"] = "date_of_creation",
                    ["4"] = "location",
                    ["5"] = "storage",
                },

                ["4"] = new Dictionary<string, string> // Warehouse Transfers
                {
                    ["1"] = "shipment_id",
                    ["2"] = "origin_warehouse_name",
                    ["3"] = "target_warehouse_name",
                    ["4"] = "item_name",
                    ["5"] = "quantity",
                    ["6"] = "status_text",
                },

                ["5"] = new Dictionary<string, string> // Suppliers
                {
                    ["1"] = "supplier_id",
                    ["2"] = "name",
                    ["3"] = "date_of_creation",
                    ["4"] = "total_purchases",
                    ["5"] = "completed_purchases",
                },

                ["6"] = new Dictionary<string, string> // Purchase Orders
                {
                    ["1"] = "shipment_id",
                    ["2"] = "supplier_name",
                    ["3"] = "warehouse_name",
                    ["4"] = "item_name",
                    ["5"] = "quantity",
                    ["6"] = "status_text",
                },

                ["7"] = new Dictionary<string, string> // Customers
                {
                    ["1"] = "customer_id",
                    ["2"] = "name",
                    ["3"] = "date_of_creation",
                    ["4"] = "total_sales",
                    ["5"] = "completed_sales",
                },

                ["8"] = new Dictionary<string, string> // Orders
                {
                    ["1"] = "shipment_id",
                    ["2"] = "customer_name",
                    ["3"] = "warehouse_name",
                    ["4"] = "item_name",
                    ["5"] = "quantity",
                    ["6"] = "status_text",
                }
            };

            // insertQueryTemplate
            insertQueryTemplate = new Dictionary<string, string>
            {
                ["2"] = "INSERT INTO Items (name, date_of_creation, cost, price) " +
                    "VALUES (@name, @date_of_creation, @cost, @price)",

                ["3"] = "INSERT INTO Warehouses (name, date_of_creation, location, capacity) " +
                    "VALUES (@name, @date_of_creation, @location, @capacity)",

                ["4"] = "INSERT INTO Shipments (origin_warehouse_id, target_warehouse_id, item_id, quantity, status, shipment_type) " +
                   "VALUES (@origin_warehouse_id, @target_warehouse_id, @item_id, @quantity, @status, @shipment_type)",

                ["5"] = "INSERT INTO Suppliers (name, date_of_creation) " +
                    "VALUES (@name, @date_of_creation)",

                ["6"] = "INSERT INTO Shipments (supplier_id, target_warehouse_id, item_id, quantity, status, shipment_type) " +
                   "VALUES (@supplier_id, @target_warehouse_id, @item_id, @quantity, @status, @shipment_type)",

                ["7"] = "INSERT INTO Customers (name, date_of_creation) " +
                    "VALUES (@name, @date_of_creation)",

                ["8"] = "INSERT INTO Shipments (origin_warehouse_id, customer_id, item_id, quantity, status, shipment_type) " +
                   "VALUES (@origin_warehouse_id, @customer_id, @item_id, @quantity, @status, @shipment_type)",
            };

            refresh("2");
            refresh("3");
            refresh("4");
            refresh("5");
            refresh("6");
            refresh("7");
            refresh("8");
        }

        private DataRow checkId(string dataId, string key, int num)
        {
            Database db = new Database();
            DataTable dt = db.ExecuteQuery($"SELECT * FROM {dataId} WHERE {key} = {num} LIMIT 1");

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            return null;
        }

        private Func<object> ReturnIdValidator(
            Guna2TextBox textBox, 
            string dataId, 
            string key, 
            string label
            )
        {
            return new Func<object>(() =>
            {
                if (int.TryParse(textBox.Text, out int num))
                {
                    if (checkId(dataId, key, num) != null)
                    {
                        return num;
                    }
                    else
                    {
                        return "Error: " + label + " not found";
                    }
                }
                return "Error: Invalid number for " + label;
            });
        }

        private Func<object> ReturnNumValidator(Guna2TextBox textBox, string label)
        {
            return new Func<object>(() =>
            {
                if (int.TryParse(textBox.Text, out int num))
                    return num;
                return "Error: Invalid number for " + label;
            });
        }

        private Func<object> ReturnStringValidator(Guna2TextBox textBox, string label)
        {
            return new Func<object>(() =>
            {
                if (textBox.Text != "")
                    return textBox.Text;
                return "Error: " + label + " cannot be empty" ;
            });
        }

        private Guna2Panel ClonePanel(Guna2Panel originalPanel)
        {
            Guna2Panel clonedPanel = new Guna2Panel()
            {
                Size = originalPanel.Size,
                BackColor = originalPanel.BackColor,
                Margin = originalPanel.Margin,
                Name = originalPanel.Name
            };

            clonedPanel.ShadowDecoration.Enabled = originalPanel.ShadowDecoration.Enabled;
            clonedPanel.ShadowDecoration.Color = originalPanel.ShadowDecoration.Color;
            clonedPanel.ShadowDecoration.Depth = originalPanel.ShadowDecoration.Depth;
            clonedPanel.ShadowDecoration.Shadow = originalPanel.ShadowDecoration.Shadow;
            clonedPanel.ShadowDecoration.BorderRadius = originalPanel.ShadowDecoration.BorderRadius;
            clonedPanel.ShadowDecoration.Parent = clonedPanel;

            foreach (Control ctrl in originalPanel.Controls)
            {
                Control newCtrl = CloneControl(ctrl);
                clonedPanel.Controls.Add(newCtrl);
            }

            return clonedPanel;
        }

        private Control CloneControl(Control original)
        {
            Control clone = (Control)Activator.CreateInstance(original.GetType());

            // Common prop
            clone.Size = original.Size;
            clone.Location = original.Location;
            clone.BackColor = original.BackColor;
            clone.ForeColor = original.ForeColor;
            clone.Font = original.Font;
            clone.Text = original.Text;
            clone.Enabled = original.Enabled;
            clone.Dock = original.Dock;
            clone.Margin = original.Margin;
            clone.Padding = original.Padding;
            clone.Name = original.Name;

            if (clone.GetType() == typeof(Guna2PictureBox))
            {
                Guna2PictureBox pictureBox = clone as Guna2PictureBox;
                pictureBox.Image = (original as Guna2PictureBox).Image;
                pictureBox.SizeMode = (original as Guna2PictureBox).SizeMode;
            }

            // Recursive
            foreach (Control child in original.Controls)
            {
                clone.Controls.Add(CloneControl(child));
            }

            return clone;
        }

        private void onSideButtonClick(object sender, EventArgs e)
        {
            if (selectedButton == sender as Panel) return;
            Panel panel = selectedButton;
            if (!string.IsNullOrEmpty(panel.Name) && panel.Name[0] == 'd')
            {
                selectedButton.BackColor = Color.FromArgb(25, 42, 76);
            }
            else
            {
                selectedButton.BackColor = Color.FromArgb(20, 33, 61);
            }
            foreach (Control control in selectedButton.Controls)
            {
                if (control is Guna2HtmlLabel label)
                {
                    control.ForeColor = Color.FromArgb(255, 255, 255);
                    break;
                }
            }
            selectedButton = sender as Panel;
            selectedButton.BackColor = Color.FromArgb(252, 163, 17);
            foreach (Control control in selectedButton.Controls)
            {
                if (control is Guna2HtmlLabel label)
                {
                    control.ForeColor = Color.FromArgb(0, 0, 0);
                    break;
                }
            }

            string buttonName = selectedButton.Name;
            string[] split = buttonName.Split('_');
            int buttonNum = Int32.Parse(split[1]);

            selectedPanel.Visible = false;
            selectedPanel = panelList[buttonNum - 1];
            selectedPanel.Visible = true;
        }

        private void onDropDownClick(object sender, EventArgs e)
        {
            if (selectedDropdown != sender as Panel)
            {
                if (selectedDropdown != null)
                {
                    selectedDropdown.Parent.Size = new Size(165, 40);
                    var forwardImage = selectedDropdown.Parent.Controls
                        .OfType<Guna2PictureBox>()
                        .FirstOrDefault(c => c.Name.Contains("forwardImage"));
                    forwardImage.Visible = true;
                }
                selectedDropdown = sender as Panel;
                Panel button = sender as Panel;
                Panel parent = button.Parent as Panel;

                parent.Size = new Size(165, 120);
                var forwardImage2 = parent.Controls
                        .OfType<Guna2PictureBox>()
                        .FirstOrDefault(c => c.Name.Contains("forwardImage"));
                forwardImage2.Visible = false;
            }
            else
            {
                selectedDropdown = null;
                Panel button = sender as Panel;
                Panel parent = button.Parent as Panel;

                parent.Size = new Size(165, 40);
                var forwardImage = parent.Controls
                        .OfType<Guna2PictureBox>()
                        .FirstOrDefault(c => c.Name.Contains("forwardImage"));
                forwardImage.Visible = true;
            }
        }

        private string approveShipment(int id)
        {
            Database db = new Database();
            DataTable dt = db.ExecuteQuery($"SELECT * FROM Shipments WHERE shipment_id = {id} LIMIT 1");
            DataRow data = dt.Rows[0];

            if ((int)data["shipment_type"] == 0) // Warehouse Transfers
            {
                int originWarehouseId = (int)data["origin_warehouse_id"]; // Origin Warehouse Check
                DataRow originData = checkId("Warehouses", "warehouse_id", originWarehouseId);
                if (originData == null)
                {
                    return "Error: Origin Warehouse not found";
                }

                int targetWarehouseId = (int)data["target_warehouse_id"]; // Target Warehouse Check
                DataRow targetData = checkId("Warehouses", "warehouse_id", targetWarehouseId);
                if (targetData == null)
                {
                    return "Error: Target Warehouse not found";
                }

                int itemId = (int)data["item_id"]; // Item Check
                DataRow itemData = checkId("Items", "item_id", itemId);
                if (itemData == null)
                {
                    return "Error: Item not found";
                }

                DataTable originInventoryTable = db.ExecuteQuery("" + // Inventory Avability Check
                    "SELECT * " +
                    "FROM Inventory " +
                    $"WHERE warehouse_id = {originWarehouseId} AND item_id = {itemId} " +
                    "LIMIT 1");
                if (originInventoryTable.Rows.Count == 0)
                {
                    return "Error: Insufficient Item";
                }

                DataRow originInventoryData = originInventoryTable.Rows[0]; // Inventory Quantity Check
                int currentQuantity = (int)originInventoryData["quantity"];
                int shipmentQuantity = (int)data["quantity"];
                if (currentQuantity < shipmentQuantity)
                {
                    return "Error: Insufficient Item";
                }

                db.ExecuteNonQuery("" + // Update Shipment Status
                   "UPDATE Shipments " +
                   "SET status = 1 " +
                   $"WHERE shipment_id = {id}");

                db.ExecuteNonQuery("" + // Update Origin Warehouse Inventory
                    "UPDATE Inventory " +
                    $"SET quantity = quantity - {shipmentQuantity} " +
                    $"WHERE warehouse_id = {originWarehouseId} AND item_id = {itemId}");

                DataTable targetInventoryTable = db.ExecuteQuery("" + // Update Target Warehouse Inventory
                    "SELECT * " +
                    "FROM Inventory " +
                    $"WHERE warehouse_id = {targetWarehouseId} AND item_id = {itemId}");
                if (targetInventoryTable.Rows.Count > 0)
                {
                    db.ExecuteNonQuery("" +
                        "UPDATE Inventory " +
                        $"SET quantity = quantity + {shipmentQuantity} " +
                        $"WHERE warehouse_id = {targetWarehouseId} AND item_id = {itemId}");
                }
                else
                {
                    db.ExecuteNonQuery("" +
                        "INSERT INTO Inventory (warehouse_id, item_id, quantity) " +
                        $"VALUES ({targetWarehouseId}, {itemId}, {shipmentQuantity})");
                }

                return "Approval Successful";
            }
            else if ((int)data["shipment_type"] == 1) // Purchases
            {
                int supplierId = (int)data["supplier_id"]; // Supplier Check
                DataRow supplierData = checkId("Suppliers", "supplier_id", supplierId);
                if (supplierData == null)
                {
                    return "Error: Supplier not found";
                }

                int targetWarehouseId = (int)data["target_warehouse_id"]; // Target Warehouse Check
                DataRow targetData = checkId("Warehouses", "warehouse_id", targetWarehouseId);
                if (targetData == null)
                {
                    return "Error: Target Warehouse not found";
                }

                int itemId = (int)data["item_id"]; // Item Check
                DataRow itemData = checkId("Items", "item_id", itemId);
                if (itemData == null)
                {
                    return "Error: Item not found";
                }

                int shipmentQuantity = (int)data["quantity"];

                db.ExecuteNonQuery("" + // Update Shipment Status
                   "UPDATE Shipments " +
                   "SET status = 1 " +
                   $"WHERE shipment_id = {id}");

                DataTable targetInventoryTable = db.ExecuteQuery("" + // Update Target Warehouse Inventory
                    "SELECT * " +
                    "FROM Inventory " +
                    $"WHERE warehouse_id = {targetWarehouseId} AND item_id = {itemId}");
                if (targetInventoryTable.Rows.Count > 0)
                {
                    db.ExecuteNonQuery("" +
                        "UPDATE Inventory " +
                        $"SET quantity = quantity + {shipmentQuantity} " +
                        $"WHERE warehouse_id = {targetWarehouseId} AND item_id = {itemId}");
                }
                else
                {
                    db.ExecuteNonQuery("" +
                        "INSERT INTO Inventory (warehouse_id, item_id, quantity) " +
                        $"VALUES ({targetWarehouseId}, {itemId}, {shipmentQuantity})");
                }

                return "Approval Successful";
            }
            else // shipment type = 2 - Sales
            {
                int originWarehouseId = (int)data["origin_warehouse_id"]; // Origin Warehouse Check
                DataRow originData = checkId("Warehouses", "warehouse_id", originWarehouseId);
                if (originData == null)
                {
                    return "Error: Origin Warehouse not found";
                }

                int customerId = (int)data["customer_id"]; // Customer Check
                DataRow customerData = checkId("Customers", "customer_id", customerId);
                if (customerData == null)
                {
                    return "Error: Customer not found";
                }

                int itemId = (int)data["item_id"]; // Item Check
                DataRow itemData = checkId("Items", "item_id", itemId);
                if (itemData == null)
                {
                    return "Error: Item not found";
                }

                DataTable originInventoryTable = db.ExecuteQuery("" + // Inventory Avability Check
                    "SELECT * " +
                    "FROM Inventory " +
                    $"WHERE warehouse_id = {originWarehouseId} AND item_id = {itemId} " +
                    "LIMIT 1");
                if (originInventoryTable.Rows.Count == 0)
                {
                    return "Error: Insufficient Item";
                }

                DataRow originInventoryData = originInventoryTable.Rows[0]; // Inventory Quantity Check
                int currentQuantity = (int)originInventoryData["quantity"];
                int shipmentQuantity = (int)data["quantity"];
                if (currentQuantity < shipmentQuantity)
                {
                    return "Error: Insufficient Item";
                }

                db.ExecuteNonQuery("" + // Update Shipment Status
                   "UPDATE Shipments " +
                   "SET status = 1 " +
                   $"WHERE shipment_id = {id}");

                db.ExecuteNonQuery("" + // Update Origin Warehouse Inventory
                    "UPDATE Inventory " +
                    $"SET quantity = quantity - {shipmentQuantity} " +
                    $"WHERE warehouse_id = {originWarehouseId} AND item_id = {itemId}");

                return "Approval Successful";
            }
        }

        public class AppSettings
        {
            public string ConnectionString { get; set; }
        }

        public class Database
        {
            private readonly string connectionString;

            public Database()
            {
                string json = File.ReadAllText("settings.json");
                AppSettings settings = JsonSerializer.Deserialize<AppSettings>(json);
                connectionString = settings.ConnectionString;
            }

            public MySqlConnection GetConnection()
            {
                return new MySqlConnection(connectionString);
            }

            public DataTable ExecuteQuery(string query)
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }

            public void ExecuteNonQuery(string query)
            {
                int affectedRows = 0;
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                }
                // return affectedRows;
            }
        }

        private DataTable getData(string id)
        {
            Database db = new Database();
            if (id == "2")
            {
                return db.ExecuteQuery("" +
                    "SELECT " +
                    "it.item_id, " +
                    "it.name, " +
                    "it.date_of_creation, " +
                    "CONCAT('Rp. ', REPLACE(FORMAT(it.cost, 0), ',', '.')) AS formatted_cost, " +
                    "CONCAT('Rp. ', REPLACE(FORMAT(it.price, 0), ',', '.')) AS formatted_price, " +
                    "COALESCE(SUM(i.quantity), 0) AS total_quantity " +

                    "FROM Items it " +
                    "LEFT JOIN Inventory i ON it.item_id = i.item_id " +
                    "GROUP BY it.item_id");
            }
            else if (id == "3")
            {
                return db.ExecuteQuery("" +
                    "SELECT " +
                    "w.warehouse_id, " +
                    "w.name, " +
                    "w.date_of_creation, " +
                    "w.location, " +
                    "CONCAT(" +
                    "   COALESCE(REPLACE(FORMAT(SUM(i.quantity), 0), ',', '.'), 0), " +
                    "   ' / ', " +
                    "   COALESCE(REPLACE(FORMAT(w.capacity, 0), ',', '.'), 0), " +
                    "   ' (', " +
                    "   FORMAT(COALESCE(SUM(i.quantity), 0) / w.capacity * 100, 0), " +
                    "   '%)'" +
                    ") AS storage " +

                    "FROM Warehouses w " +
                    "LEFT JOIN Inventory i ON w.warehouse_id = i.warehouse_id " +
                    "GROUP BY w.warehouse_id");
            }
            else if (id == "3_1")
            {
                return db.ExecuteQuery("" +
                    "SELECT * " +
                    "FROM Inventory");
            }
            else if (id == "4")
            {
                return db.ExecuteQuery("" +
                    "SELECT " +
                    "shipment_id, " +
                    "ow.name AS origin_warehouse_name, " +
                    "tw.Name AS target_warehouse_name, " +
                    "i.name AS item_name, " +
                    "quantity, " +
                    "st.statusString AS status_text " +

                    "FROM Shipments s " +
                    "LEFT JOIN Warehouses ow ON s.origin_warehouse_id = ow.warehouse_id " +
                    "LEFT JOIN Warehouses tw ON s.target_warehouse_id = tw.warehouse_id " +
                    "LEFT JOIN Items i ON s.item_id = i.item_id " +
                    "LEFT JOIN StatusText st ON s.status = st.statusInt " +
                    "WHERE shipment_type = 0");
            }
            else if (id == "5")
            {
                return db.ExecuteQuery("" +
                    "SELECT " +
                    "sp.supplier_id, " +
                    "sp.name, " +
                    "sp.date_of_creation, " +
                    "COUNT(s.shipment_id) AS total_purchases, " +
                    "COALESCE(SUM(s.status), 0) AS completed_purchases " +
                    "FROM Suppliers sp " +
                    "LEFT JOIN Shipments s ON sp.supplier_id = s.supplier_id " +
                    "GROUP BY sp.supplier_id");
            }
            else if (id == "6")
            {
                return db.ExecuteQuery("" +
                    "SELECT " +
                    "shipment_id, " +
                    "sp.name AS supplier_name, " +
                    "tw.name AS warehouse_name, " +
                    "i.name AS item_name, " +
                    "quantity, " +
                    "st.statusString AS status_text " +

                    "FROM Shipments s " +
                    "LEFT JOIN Suppliers sp ON s.supplier_id = sp.supplier_id " +
                    "LEFT JOIN Warehouses tw ON s.target_warehouse_id = tw.warehouse_id " +
                    "LEFT JOIN Items i ON s.item_id = i.item_id " +
                    "LEFT JOIN StatusText st ON s.status = st.statusInt " +
                    "WHERE shipment_type = 1");
            }
            else if (id == "7")
            {
                return db.ExecuteQuery("" +
                    "SELECT " +
                    "c.customer_id, " +
                    "c.name, " +
                    "c.date_of_creation, " +
                    "COUNT(s.shipment_id) AS total_sales, " +
                    "COALESCE(SUM(s.status), 0) AS completed_sales " +
                    "FROM Customers c " +
                    "LEFT JOIN Shipments s ON c.customer_id = s.customer_id " +
                    "GROUP BY c.customer_id");
            }
            else // if (id == "8")
            {
                return db.ExecuteQuery("" +
                    "SELECT " +
                    "shipment_id, " +
                    "ow.name AS warehouse_name, " +
                    "c.name AS customer_name, " +
                    "i.name AS item_name, " +
                    "quantity, " +
                    "st.statusString AS status_text " +

                    "FROM Shipments s " +
                    "LEFT JOIN Warehouses ow ON s.origin_warehouse_id = ow.warehouse_id " +
                    "LEFT JOIN Customers c ON s.customer_id = c.customer_id " +
                    "LEFT JOIN Items i ON s.item_id = i.item_id " +
                    "LEFT JOIN StatusText st ON s.status = st.statusInt " +
                    "WHERE shipment_type = 2");
            }
        }

        private void addEntry(object sender, EventArgs e)
        {
            string rawId = (sender as Control)?.Name;
            string[] split1 = rawId.Split('_');
            string id = split1[1];

            List<List<object>> boxes = entryTextBox[(sender as Panel)];
            string tempQuery = insertQueryTemplate[id];
            Database db = new Database();
            MySqlConnection conn = db.GetConnection();
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(tempQuery, conn))
            {
                bool isError = false;
                foreach (List<object> table in boxes)
                {
                    if (table[1] is Func<object> func)
                    {
                        object value = func();
                        if (value is string str && str.Contains("Error"))
                        {
                            MessageBox.Show(str);
                            isError = true;
                            break;
                        }

                        cmd.Parameters.AddWithValue("@" + table[0], value);
                    }
                }

                if (!isError)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully added");

                    if (id == "2")
                    {
                        haveInitItemIdToName = false;
                    }
                }
            }
        }

        private void refresh(string id1)
        {
            DataTable dt = getData(id1);
            flowLayoutDictionary[id1].Controls.Clear();

            Guna2TextBox searchTextBox = textBoxDictionary[id1];

            Dictionary<string, List<Dictionary<string, object>>> warehouseItems = new Dictionary<string, List<Dictionary<string, object>>>();

            if (id1 == "3") // Warehouses, list all items
            {
                if (!haveInitItemIdToName)
                {
                    DataTable itemTable = getData("2");
                    foreach (DataRow dr in itemTable.Rows)
                    {
                        itemIdToName[Convert.ToInt32(dr["item_id"])] = dr["name"].ToString();
                    }
                    haveInitItemIdToName = true;
                }

                DataTable inventoryTable = getData("3_1");
                foreach (DataRow dr in inventoryTable.Rows)
                {
                    if ((int)dr["quantity"] > 0)
                    {
                        string key = dr["warehouse_id"].ToString();
                        if (!warehouseItems.ContainsKey(key))
                        {
                            warehouseItems[key] = new List<Dictionary<string, object>>();
                        }

                        Dictionary<string, object> value = new Dictionary<string, object>
                        {
                            ["itemId"] = dr["item_id"],
                            ["itemName"] = itemIdToName[Convert.ToInt32(dr["item_id"])],
                            ["itemQuantity"] = dr["quantity"],
                        };
                        warehouseItems[key].Add(value);
                    }
                };
            }

            foreach (DataRow dr in dt.Rows)
            {
                Guna2Panel clone = ClonePanel(sampleDictionary[id1]);
                flowLayoutDictionary[id1].Controls.Add(clone);
                clone.Visible = true;
                
                FlowLayoutPanel flow = clone.Controls
                    .OfType<FlowLayoutPanel>()
                    .FirstOrDefault();

                string mainTargetId = null;
                string mainTargetName = null;

                bool canApprove = false;

                if (flow != null)
                {
                    foreach (Control panel in flow.Controls.OfType<Panel>())
                    {
                        foreach (Control panelControl in panel.Controls)
                        {
                            if (panelControl is Guna2HtmlLabel label && label.Name.Contains("value"))
                            {
                                string buttonName = label.Name;
                                string[] split2 = buttonName.Split('_');
                                string id2 = split2[2];

                                string dataType = dataTypeDictionary[id1][id2];
                                object data = dr[dataType];
                                if (data is DateTime date)
                                {
                                    label.Text = date.ToString("dd/MM/yyyy");
                                }
                                /*else if (dataType == "cost" || dataType == "price")
                                {
                                    label.Text = $"Rp. {dr[dataType]:N0}";
                                }*/
                                else
                                {
                                    label.Text = dr[dataType].ToString();
                                }

                                if (mainTargetId == null)
                                {
                                    mainTargetId = label.Text;
                                }
                                else if (mainTargetName == null && (id1 == "2" || id1 == "3" || id1 == "5" || id1 == "7")) // Libraries
                                {
                                    mainTargetName = label.Text.ToLower();
                                }

                                if (dataType == "status_text")
                                {
                                    canApprove = (label.Text == "Pending");
                                }
                            }
                        }
                    }

                    if (id1 == "3") // Warehouses, list all items
                    {
                        var dropdown = flow.Controls
                            .OfType<Guna2Panel>()
                            .FirstOrDefault(c => c.Name == "toggleItems");
                        List<Guna2Panel> itemPanels = new List<Guna2Panel>();

                        string key = dr["warehouse_id"].ToString();

                        if (warehouseItems.ContainsKey(key))
                        {
                            int index = 0;
                            foreach (Dictionary<string, object> info in warehouseItems[key])
                            {
                                Guna2Panel itemPanelClone = ClonePanel(sampleItemCell);

                                string subTargetId = info["itemId"].ToString();
                                string subTargetName = info["itemName"].ToString().ToLower();
                                bool dropdownEnabled = false;

                                foreach (Control control in itemPanelClone.Controls)
                                {
                                    Guna2HtmlLabel textLabel = (control as Guna2HtmlLabel);
                                    textLabel.Text = info[control.Name].ToString();
                                }
                                flowLayoutDictionary[id1].Controls.Add(itemPanelClone);
                                itemPanels.Add(itemPanelClone);
                                itemPanelClone.Visible = false;

                                bool isFirst = (index == 0);
                                index++;

                                if (index == warehouseItems[key].Count)
                                {
                                    itemPanelClone.Margin = new Padding(0, 0, 0, 20); // normal = 5
                                }

                                searchTextBox.TextChanged += (s2, e2) =>
                                {
                                    string[] split = searchTextBox.Text.ToLower().Split(';');

                                    string firstText = split[0];
                                    bool firstBool = (firstText == "" || mainTargetId.Contains(firstText) || mainTargetName.Contains(firstText));
                                    if (split.Count() > 1)
                                    {
                                        string secondText = split[1];
                                        bool secondBool = (secondText == "" || subTargetId.Contains(secondText) || subTargetName.Contains(secondText));

                                        itemPanelClone.Visible = (firstBool && secondBool && dropdownEnabled);
                                    }
                                    else
                                    {
                                        itemPanelClone.Visible = (dropdownEnabled && firstBool);
                                    }
                                };

                                if (dropdown != null)
                                {
                                    var leftArrow = dropdown.Controls
                                        .OfType<Guna2PictureBox>()
                                        .FirstOrDefault(c => c.Name == "toggleItems_leftArrow");

                                    dropdown.MouseDown += (s2, e2) =>
                                    {
                                        dropdownEnabled = !dropdownEnabled;
                                        if (isFirst)
                                        {
                                            leftArrow.Visible = !dropdownEnabled;
                                        }

                                        string[] split = searchTextBox.Text.ToLower().Split(';');

                                        string firstText = split[0];
                                        bool firstBool = (firstText == "" || mainTargetId.Contains(firstText) || mainTargetName.Contains(firstText));
                                        if (split.Count() > 1)
                                        {
                                            string secondText = split[1];
                                            bool secondBool = (secondText == "" || subTargetId.Contains(secondText) || subTargetName.Contains(secondText));

                                            itemPanelClone.Visible = (firstBool && secondBool && dropdownEnabled);
                                        }
                                        else
                                        {
                                            itemPanelClone.Visible = (dropdownEnabled && firstBool);
                                        }
                                    };
                                }
                            }
                        }
                    }
                    else if (id1 == "4" || id1 == "6" || id1 == "8")
                    {
                        var approve = flow.Controls
                            .OfType<Guna2PictureBox>()
                            .FirstOrDefault(c => c.Name.Contains("approve"));

                        if (canApprove)
                        {
                            int shipmentId = Int32.Parse(mainTargetId);
                            if (approve != null)
                            {
                                approve.MouseDown += (s2, e2) =>
                                {
                                    string output = approveShipment(shipmentId);
                                    MessageBox.Show(output);
                                };
                            }
                        }
                        else
                        {
                            approve.Visible = false;
                            var statusText = flow.Controls
                                .OfType<Guna2Panel>()
                                .FirstOrDefault(c => c.Name.Contains("status"));
                            statusText.Size = new Size(120, 60);
                        }
                    }
                }

                if (mainTargetId != null && mainTargetName != null && searchTextBox != null)
                {
                    searchTextBox.TextChanged += (s2, e2) =>
                    {
                        string[] split = searchTextBox.Text.ToLower().Split(';');
                        string text = split[0];
                        clone.Visible = (text == "" || mainTargetId.Contains(text) || mainTargetName.Contains(text));
                    };
                }
            };
        }

        private void onRefresh(object sender, EventArgs e)
        {
            MessageBox.Show("Refresh");
            string rawId = (sender as Control)?.Name;
            string[] split1 = rawId.Split('_');
            string id1 = split1[1];
            refresh(id1);
        }
    }
} 