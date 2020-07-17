create or alter procedure updateWomenTable @name varchar(50)
as begin
               update Women set wname = @name where id = 8;
end

create or alter procedure updateShoeModelTable @season varchar(50)
as begin
               update ShoeModels set season = @season where id = 2
end

select * from women
select * from ShoeModels