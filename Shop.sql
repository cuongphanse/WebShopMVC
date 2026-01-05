Create database Shop;
Go
use Shop;
Go
create table Category(
	CategoryId tinyint not null primary key identity(1,1),
	CategoryName nvarchar(64) not null,
	Description nvarchar(256) not null
);
go
insert into Category (CategoryName,Description) values
(N'Laptop', N'Thong tin mo ta cua laptop'),
(N'Tai nghe', N'Thong tin mo ta cua tai nghe'),
(N'Ban phim', N'Thong tin mo ta cua ban phim');
Go

create table Product(
	ProductId int not null primary key identity(1,1),
	CategoryId tinyint not null foreign key references Category(CategoryId),
	ProductName nvarchar(128) not null,
	Description nvarchar(512) not null,
	content nvarchar(max) not null,
	ImageUrl nvarchar(32) not null,
	Price decimal(10,2) not null,
	Quantity SMALLINT not null,
	SaleOff DECIMAL(10,2) 
);
go

-- ALTER TABLE Product 
-- ALTER COLUMN ImageUrl NVARCHAR(32) NOT NULL;

insert into Product (CategoryId, ProductName, Description, content, ImageUrl, Price, Quantity, SaleOff) values
(1, N'Laptop Dell XPS 13', N'Mo ta laptop Dell XPS 13', N'Chi tiet ve laptop Dell XPS 13', N'dellxps13.jpg', 1200.00, 50, 100.00),
(2, N'Tai nghe Sony WH-1000XM4', N'Mo ta tai nghe Sony WH-1000XM4', N'Chi tiet ve tai nghe Sony WH-1000XM4', N'sonywh1000xm4.jpg', 350.00, 100, 50.00),
(2, N'Ban phim Logitech MX Keys', N'Mo ta ban phim Logitech MX Keys', N'Chi tiet ve ban phim Logitech MX Keys', N'logitechmxkeys.jpg', 100.00, 75, 20.00);
go

create proc AddProduct(
	@CategoryId tinyint,
	@Name nvarchar(128),
	@Description nvarchar(512),
	@content nvarchar(max),
	@ImageUrl nvarchar(32),
	@Price decimal(10,2),
	@Quantity SMALLINT,
	@SaleOff DECIMAL(10,2) = NULL
)AS
INSERT INTO Product (CategoryId, ProductName, Description, Content, Price, Quantity, SaleOff, ImageUrl) 
            VALUES (@CategoryId, @Name, @Description, @Content, @Price, @Quantity, @SaleOff, @ImageUrl);
			go

create proc UpdateProduct(
	@ProductId INT,
	@CategoryId tinyint,
	@Name nvarchar(128),
	@Description nvarchar(512),
	@content nvarchar(max),
	@ImageUrl nvarchar(32),
	@Price decimal(10,2),
	@Quantity SMALLINT,
	@SaleOff DECIMAL(10,2) = NULL
)AS
    UPDATE Product
SET 
    CategoryId = @CategoryId, 
    ProductName = @Name, 
    Description = @Description, 
    Content = @content, 
    Price = @Price, 
    Quantity = @Quantity, 
    SaleOff = @SaleOff, 
    ImageUrl = @ImageUrl
WHERE ProductId = @ProductId;
GO

Create table Member(
	MemberId nvarchar(32) not null primary key,
	GivenName nvarchar(32) not null,
	Surname nvarchar(32),
	Name nvarchar(64) not null,
	Email nvarchar(64) not null unique,
	Password binary(64) not null,
	LoginCount smallint not null default 0,
	Token char(32),
	LoginDate datetime not null default getdate(),
	RegisterDate datetime not null default getdate()
	);
go

alter table Member ADD IsActived Bit not null default 0;
go

--drop proc ActiveAccount
go
create proc ActiveAccount(
	@Token Char(32)
)as
	update Member Set IsActived = 1, Token = null Where Token=@Token;
go

create proc AddMember(
	@Id nvarchar(32) ,
	@GivenName nvarchar(32),
	@Surname nvarchar(32) = null,
	@Name nvarchar(64),
	@Email nvarchar(64),
	@Password binary(64) ,
	@Token char(32) = null
)as
begin
	if not exists(select * from Member where Email = @Email or MemberId = @Id)
	insert into Member (MemberId, GivenName, Surname, Name, Email, Password, Token) values
	(@Id, @GivenName, @Surname, @Name, @Email, @Password, @Token);
end
go
--delete from Member
--go