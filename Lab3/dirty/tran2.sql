begin transaction
update Women set wname = 'Lolita' where id = 4
waitfor delay '00:00:10'
rollback tran