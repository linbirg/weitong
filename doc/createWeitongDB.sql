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
# code ��ŵ�����ĸ�����ڹ��ҵ�Ӣ������ĸ����
# bottle �����ֺ͵�λ��ɣ���750ml, 375ml
# score ��ʱ���ַ�����¼������99 95.4��
# quality ��������AOC���ؼ���
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
# price ���� ����С�����2��λ
# caseprice �����̱���
# retailprice ���ۼ�
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

# 0�����Ϊ�����Ա��������������;����
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(0,100,"������;��Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(1,100,"���ƻ�Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(2,80,"ͭ�ƻ�Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(3,40,"���ƻ�Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(4,20,"���ƻ�Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(5,0,"����VIP��Ա");

# member table
# discount�ۿ�Ϊ�ٷֱ�
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
# �û���Ϊ�������𣬷ֱ���в�ͬ��Ȩ�ޡ�0����ߣ���������Ȩ��
# ������Ա��һ��Ĵ���Ȩ��
# ������Ա��������������ԱȨ�޵�Ȩ��
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

# ������ر�
# orders table
# oderstate����״̬�����������Ѹ�������,��ȡ����
# ��ȡ���Ķ������û����˵�������δ�������ĳ��ԭ��ȡ���ˡ�
# ����ȡ���Ķ����������û���Ϊ���������߹˿��ٴ����ѵ�ʱ����Խ�ȡ���ĵ��������µ����ɡ�
# amountΪ�˵��ܽ��
# receivedΪʵ�ս��
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

# �����оƵ���ϸ
# discountΪ�ۿۣ�ȡ0-100֮���������
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





