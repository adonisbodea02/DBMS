set transaction isolation level read committed
begin transaction
select wname from Women where id = 4
waitfor delay '00:00:07'
select wname from Women where id = 4
commit tran