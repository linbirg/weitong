use weitong
# his_storage table
DROP TABLE IF EXISTS his_storage;
CREATE TABLE his_storage(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	code CHAR(100) NOT NULL,
	supplierid INT,
	price DECIMAL(10,2) DEFAULT 0,
	retailprice DECIMAL(10,2),
	units INT NOT NULL DEFAULT 0,
	effectdate DATETIME NOT NULL,
	FOREIGN KEY(code) REFERENCES wines(code) 
	ON DELETE CASCADE,
	FOREIGN KEY(supplierid) REFERENCES supplier(id)
)TYPE=INNODB;