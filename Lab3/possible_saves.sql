create or alter  procedure usp_insert_possible @name varchar(50), @amount varchar(50), @shoe_model varchar(50), @price varchar(50)
AS
	SET XACT_ABORT OFF;
    BEGIN TRANSACTION
		if len(@name) = 0
			begin
				print('Address is empty!')
				RAISERROR('Address is null', 16, 1)
			end
		if ISNUMERIC(@amount) = 0
			begin
				print('Amount is not a number!')
				RAISERROR('Amount is not a number!', 16, 1)
			end
		if ISNUMERIC(@shoe_model) = 0
			begin
				print('Shoe Model is not a number!')
				RAISERROR('Shoe Model is not a number!', 16, 1)
			end
		if ISNUMERIC(@price) = 0
			begin
				print('Price is not a number!')
				RAISERROR('Price is not a number!', 16, 1)
			end
		declare @woman_id int	
		declare	@shoe_id int	
        INSERT INTO Shoes (shoe_model, price) VALUES (@shoe_model, @price);
		set @shoe_id = (select IDENT_CURRENT('Shoes'))
		INSERT INTO Women(wname, amount) VALUES (@name, @amount);
		set @woman_id = (select IDENT_CURRENT('Women'))
        INSERT INTO LinkWomenShoes(woman, shoe) VALUES (@woman_id, @shoe_id);
    COMMIT TRAN -- Transaction Success!
GO

select * from Women
select * from Shoes
select * from LinkWomenShoes

execute usp_insert_possible Camelia, 1999, 10, 29
execute usp_insert_possible Diana, 'not_int', 1, 30