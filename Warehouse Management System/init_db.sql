CREATE TABLE Items (
	item_id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	name varchar(255),
	date_of_creation date,
	cost int,
	price int
);

CREATE TABLE Warehouses (
	warehouse_id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	name varchar(255),
	location varchar(255),
	capacity int,
	date_of_creation date,
);

CREATE TABLE Inventory (
	warehouse_id int,
	item_id int,
	quantity int,
	FOREIGN KEY (warehouse_id) references Warehouses(warehouse_id),
	FOREIGN KEY (item_id) references Items(item_id),
);

CREATE TABLE Suppliers (
	supplier_id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	name varchar(255),
	date_of_creation date,
);

CREATE TABLE Customers (
	customer_id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	name varchar(255),
	date_of_creation date,
);

CREATE TABLE Shipments(
	shipment_id int not null primary KEY auto_increment,
    shipment_type int,
    origin_warehouse_id int,
    supplier_id int,
    target_warehouse_id int,
    customer_id int,
    item_id int,
    quantity int,
    FOREIGN KEY (origin_warehouse_id) references Warehouses(warehouse_id),
    FOREIGN KEY (supplier_id) references Suppliers(supplier_id),
    FOREIGN KEY (target_warehouse_id) references Warehouses(warehouse_id),
    FOREIGN KEY (customer_id) references Customers(customer_id),
    FOREIGN KEY (item_id) references Items(item_id)
);

CREATE TABLE StatusText (
	statusInt int,
	statusString varchar(255),
);

INSERT INTO StatusText (statusInt, statusString) VALUES (0, "Pending");
INSERT INTO StatusText (statusInt, statusString) VALUES (1, "Completed"); 