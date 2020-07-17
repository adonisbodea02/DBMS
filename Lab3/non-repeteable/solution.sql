set transaction isolation level repeatable read
begin transaction
select wname from Women where id = 6
waitfor delay '00:00:07'
select wname from Women where id = 6
commit tran