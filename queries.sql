use Library;

-- create table users(uid int identity(1,1) primary key, username varchar(10) unique, password varchar(10));
-- insert into users values('AvinashSharma','Avi');
-- insert into users values('ManikantaSai','Mani123');
-- insert into users values('Vamshi','Vam@');

-- create table books(bid int identity(100,1) primary key, bookName varchar(100) unique, BooksCount int);
-- insert into books values('LetUsC',5);
-- insert into books values('JavaForBeginners',10);
-- insert into books values('DotNetForPros',2);

-- create table orders(orderid int identity(63487,7) primary key, bid int REFERENCES books(bid) , uid int REFERENCES users(uid));
select * from users ;
select * from books;
select * from orders;
