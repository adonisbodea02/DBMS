begin transaction
update ShoeModels set season = 'transaction2' where id = 1
waitfor delay '00:00:10'
update Women set amount = 2 where id = 8
commit transaction