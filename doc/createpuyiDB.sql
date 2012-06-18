# linbirg@2012.04.10 for weitong red wine management

DROP DATABASE IF EXISTS puyi;
CREATE DATABASE puyi character set GB2312;

USE puyi;

# customers table
DROP TABLE IF EXISTS customers;
CREATE TABLE customers(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	name TEXT NOT NULL,
	phonenumber CHAR(30) UNIQUE,
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

## ���ӻ�Ա��������ֶΣ���һ����Ա�ﵽ�������ʱ���Զ�����Ϊ��һ����Ա
DROP TABLE IF EXISTS memberlevel;
CREATE TABLE memberlevel(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	memlevel INT UNIQUE NOT NULL,
	discount INT DEFAULT 100,
	levelname TEXT,
	minconsuption INT DEFAULT 99999999 
)TYPE=INNODB;

# 0�����Ϊ�����Ա��������������;����
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(0,100,"������;��Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(1,100,"���ƻ�Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(2,80,"ͭ�ƻ�Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(3,40,"���ƻ�Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(4,20,"���ƻ�Ա");
INSERT INTO memberlevel(memlevel,discount,levelname) VALUES(5,0,"����VIP��Ա");

# customeridӦ����Ψһ�ģ��ͻ��ͻ�Ա��һ��һ�Ĺ�ϵ��
# memberidӦ����4λ����ݼ���8λ�Ļ�Ա���롣
# member table
# discount�ۿ�Ϊ�ٷֱ�
DROP TABLE IF EXISTS member;
CREATE TABLE member(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	memberid CHAR(100) UNIQUE NOT NULL,
	customerid INT UNIQUE NOT NULL,
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


DROP TABLE IF EXISTS roles;
CREATE TABLE roles(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	name CHAR(50) NOT NULL,
	discount int DEFAULT 100,
	PRIMARY KEY(id)
)TYPE=INNODB;

# ��ɫ��ϵͳ����Ա������������Ա�Ϳͻ��ȼ��֡�
INSERT INTO roles(name) VALUES("administrator");
INSERT INTO roles(name) VALUES("manager");
INSERT INTO roles(name) VALUES("saler");
INSERT INTO roles(name) VALUES("customer");

# �û���
DROP TABLE IF EXISTS users;
CREATE TABLE users(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	user_name CHAR(255) NOT NULL UNIQUE,
	passwd TEXT NOT NULL,
	salt TEXT,
	alias_name TEXT,
	email TEXT,
	reg_date DATETIME,
	role_id INT NOT NULL,
	PRIMARY KEY(id),
	FOREIGN KEY(role_id) REFERENCES roles(id)
)TYPE=INNODB;

INSERT INTO users(user_name,passwd,salt,alias_name,reg_date,role_id) VALUES('admin',SHA('adminxxbucunzai'),'xxbucunzai','������',NOW(),1);
INSERT INTO users(user_name,passwd,salt,alias_name,reg_date,role_id) VALUES('wentong',SHA('wentongxxbucunzai'),'xxbucunzai','����ͨ',NOW(),3);


# ������ر�
# orders table
# oderstate����״̬�����������Ѹ�������,��ȡ����
# ��ȡ���Ķ������û����˵�������δ�������ĳ��ԭ��ȡ���ˡ�
# ����ȡ���Ķ����������û���Ϊ���������߹˿��ٴ����ѵ�ʱ����Խ�ȡ���ĵ��������µ����ɡ�
# amountΪ�˵��ܽ��
# receivedΪʵ�ս��
# userid Ϊ���۶������û�ID��
DROP TABLE IF EXISTS orders;
CREATE TABLE orders(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	customerid INT NOT NULL,
	userid INT NOT NULL,
	effectdate DATETIME NOT NULL,
	orderstate INT,
	amount DECIMAL DEFAULT 0,
	received DECIMAL DEFAULT 0,
	FOREIGN KEY(customerid) REFERENCES customers(id),
	FOREIGN KEY(userid) REFERENCES users(id),
	PRIMARY KEY(id)
)TYPE=INNODB;

# �����оƵ���ϸ
# discountΪ�ۿۣ�ȡ0-100֮���������
DROP TABLE IF EXISTS order_wines;
CREATE TABLE order_wines(
	id INT UNIQUE NOT NULL AUTO_INCREMENT,
	orderid INT NOT NULL,
	code CHAR(100) NOT NULL,
	units INT NOT NULL DEFAULT 1,
	knockdownprice DECIMAL DEFAULT 0,
	discount INT DEFAULT 100,
	PRIMARY KEY(id),
	FOREIGN KEY(orderid) REFERENCES orders(id)
	ON DELETE CASCADE,
	FOREIGN KEY(code) REFERENCES wines(code)
)TYPE=INNODB;





