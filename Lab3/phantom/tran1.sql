set transaction isolation level repeatable read
begin transaction
select * from women where amount > 3000
waitfor delay '00:00:07'
select * from women where amount > 3000
commit tran