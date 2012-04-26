# linbirg@2012.04.10 for weitong red wine management

DROP DATABASE IF EXISTS weitong;
CREATE DATABASE weitong character set GB2312;

USE weitong;

# customers table
DROP TABLE IF EXISTS customers;
CREATE TABLE customers(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	name TEXT NOT NULL,
	phonenumber CHAR(30),
	registedate DATETIME,
	sex int,
	job TEXT,
	birthday Date,
	address TEXT,
	email CHAR(100),
	PRIMARY KEY(id)
)TYPE=INNODB;

# wines table
# code 编号的首字母按所在国家的英文首字母区分
# bottle 由数字和单位组成，如750ml, 375ml
# score 暂时由字符串记录，例如99 95.4等
# quality 级别，例如AOC，特级等
DROP TABLE IF EXISTS wines;
CREATE TABLE wines(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	code CHAR(100) UNIQUE NOT NULL,
	chateau TEXT,
	country TEXT,
	appellation TEXT,
	quality TEXT,
	vintage YEAR,
	description TEXT,
	bottle TEXT,
	score TEXT,
	INDEX (code),
	PRIMARY KEY(id)
)TYPE=INNODB;

# supplier table
DROP TABLE IF EXISTS supplier;
CREATE TABLE supplier(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	name CHAR(100) UNIQUE NOT NULL,
	PRIMARY KEY(id)
)TYPE=INNODB;

# storage table
# price 进价 保留小数点后2两位
# caseprice 经销商报价
# retailprice 零售价
DROP TABLE IF EXISTS storage;
CREATE TABLE storage(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	code CHAR(100) UNIQUE NOT NULL,
	supplierid INT,
	price DECIMAL(10,2) DEFAULT 0,
	retailprice DECIMAL(10,2),
	units INT NOT NULL DEFAULT 0,
	FOREIGN KEY(code) REFERENCES wines(code) 
	ON DELETE CASCADE,
	FOREIGN KEY(supplierid) REFERENCES supplier(id)
)TYPE=INNODB;

DROP TABLE IF EXISTS memberlevel;
CREATE TABLE memberlevel(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	memlevel INT UNIQUE NOT NULL,
	discount INT DEFAULT 100,
	levelname TEXT
)TYPE=INNODB;

# 0级别的为特殊会员（促销等特殊用途）。
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(0,100,"特殊用途会员");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(1,100,"铁牌会员");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(2,80,"铜牌会员");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(3,40,"银牌会员");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(4,20,"金牌会员");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(5,0,"超级VIP会员");

# member table
# discount折扣为百分比
DROP TABLE IF EXISTS member;
CREATE TABLE member(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	memberid CHAR(100) UNIQUE NOT NULL,
	customerid INT NOT NULL,
	memlevel INT NOT NULL DEFAULT 1,
	registerdate DATETIME,
	discount INT DEFAULT 100,
	FOREIGN KEY(customerid) REFERENCES customers(id)
	ON DELETE CASCADE,
	FOREIGN KEY(memlevel) REFERENCES memberlevel(memlevel),
	PRIMARY KEY(id)
)TYPE=INNODB;

DROP PROCEDURE IF EXISTS sp_insertmember;
delimiter //
CREATE PROCEDURE sp_insertmember(IN customerid INT, OUT mem_id CHAR(100))
BEGIN
	SELECT MAX(memberid) INTO mem_id FROM member;
	IF mem_id IS NULL THEN SET mem_id = '20120000'; END IF;
	SET mem_id = mem_id + 1;
	INSERT INTO member(memberid,customerid,registerdate) VALUES(mem_id,customerid,NOW());
END//
delimiter ;

# users table
# 用户分为几个级别，分别具有不同的权限。0级最高，具有所以权限
# 销售人员有一点的打折权限
# 管理人员具有设置销售人员权限等权限
DROP TABLE IF EXISTS users;
CREATE TABLE users(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	name CHAR(100) UNIQUE NOT NULL,
	email TEXT,
	passwd TEXT NOT NULL,
	usertype int NOT NULL,
	userlevel int NOT NULL,
	PRIMARY KEY(id)
)TYPE=INNODB;

# 销售相关表
# orders table
# oderstate订单状态，例如待付款，已付款，已完成,已取消等
# 已取消的订单是用户下了单，但是未付款或者某种原因取消了。
# 保留取消的订单便于做用户行为分析，或者顾客再次消费的时候可以将取消的单子重新下单即可。
# amount为账单总金额
# received为实收金额
DROP TABLE IF EXISTS orders;
CREATE TABLE orders(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	customerid INT NOT NULL,
	effectdate DATETIME NOT NULL,
	orderstate INT,
	amount DECIMAL DEFAULT 0,
	received DECIMAL DEFAULT 0,
	FOREIGN KEY(customerid) REFERENCES customers(id),
	PRIMARY KEY(id)
);

# 订单中酒的明细
# discount为折扣，取0-100之间的整数。
DROP TABLE IF EXISTS order_wines;
CREATE TABLE order_wines(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	orderid INT NOT NULL,
	code CHAR(100) NOT NULL,
	units INT NOT NULL DEFAULT 1,
	knockdownprice DECIMAL DEFAULT 0,
	PRIMARY KEY(id),
	FOREIGN KEY(orderid) REFERENCES orders(id)
	ON DELETE CASCADE,
	FOREIGN KEY(code) REFERENCES wines(code)
);





