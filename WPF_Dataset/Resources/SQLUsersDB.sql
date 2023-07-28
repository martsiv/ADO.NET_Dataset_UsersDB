create database UsersDB
go
use UsersDB
go
create table Roles
(
	ID int primary key identity,
	[Role name] nvarchar(50) not null,
);
go

create table Users
(
	[Login] nvarchar(50) primary key,
	[Password] varchar(20) not null,
	[Address] nvarchar(100) not null,
	[Phone] nvarchar(12) not null CHECK (Phone LIKE '380[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'), 
	[RoleID] int not null,
	CHECK ([Password] LIKE '%[0-9]%'
       AND [Password] LIKE '%[a-z]%'
       AND [Password] LIKE '%[A-Z]%'
       AND LEN([Password]) >= 8),
	FOREIGN KEY ([RoleID]) REFERENCES Roles(ID),
);
go

use UsersDB
go
insert into [Roles]
values	('Admin'),
		('Moderator'),
		('User');
go
insert into [Users]
([Login], [Password], [Address], [Phone], [RoleID])
values
		('Leonard', 'J32234r23I', 'Rivne, Soborna 13', '380958417483', 1),
		('Delamene', 'k384E385q43', 'Rivne, Drahomanova 25', '380972658190', 1),
		('Fred', 'He3938tW2l', 'Rivne, Wasylkiwska 17', '380937582943', 2),
		('John', 'L34895F34476q', 'Rivne, Karnaukhowa 15', '380673859104', 2),
		('Bernard', '457H43487q42', 'Rivne, Bila 5', '380735618503', 3),
		('Harry', 'L3434i355398Q', 'Rivne, Dubenska 172', '380983950271', 3);
go

create or alter proc sp_delete_admins
as
delete from Users where RoleID = (select ID from Roles where [Role name] = 'Admin')
go

create or alter proc sp_delete_moderators
as
delete from Users where RoleID = (select ID from Roles where [Role name] = 'Moderator')
go

create or alter proc sp_delete_users
as
delete from Users where RoleID = (select ID from Roles where [Role name] = 'User')
go