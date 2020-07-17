SET DEADLOCK_PRIORITY HIGH;
begin transaction
update Women set amount = 1 where id = 8
waitfor delay '00:00:10'
update ShoeModels set season = 'transaction1' where id = 1
commit transaction