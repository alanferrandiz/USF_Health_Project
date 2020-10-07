--STORED PROCEDURES


--select * from tb_individuals
--select * from tb_individuals_samples
--select * from tb_references
--select * from tb_audit

--exec usp_individuals_select @ind_id = 1
if OBJECT_ID('usp_individuals_select') is not null
	drop procedure usp_individuals_select
go
create procedure [dbo].[usp_individuals_select]
@ind_id			int = null
as
begin
--declare @table		table	(		ind_id								int,
--									ind_date_created					date,
--									ind_time_created					time,
--									ind_date_created_text				varchar(800),
--									ind_first_name						varchar(800),
--									ind_last_name						varchar(800),
--									first_name_last_name				varchar(800),
--									last_name_first_name_id				varchar(800),
--									ind_email							varchar(800),
--									ind_phone							varchar(800),
--									ind_birthdate						date,
--									ind_gender							varchar(800),
--									ind_document						varchar(800),
--									ref_id								int,
--									ref_name							varchar(800),
--									std_id								int,
--									std_name							varchar(800),
--									ind_details							varchar(max),
--									is_count							int
--							)		

--insert @table (ind_id, ind_date_created, ind_time_created, ind_date_created_text, ind_first_name, ind_last_name, first_name_last_name, last_name_first_name_id, ind_email, ind_phone, ind_birthdate, ind_gender, ind_document, ref_id, ref_name, std_id, std_name, ind_details, is_count)
if @ind_id is not null
	select	ind_id,
			ind_date_created,
			ind_time_created,
			ind_date_created_text = convert(varchar(800), convert(date,ind_date_created)), 
			ind_first_name,
			ind_last_name,
			(ind_first_name + ' ' + ind_last_name) 'first_name_last_name',
			(ind_last_name + ', ' + ind_first_name + ' (' + convert(varchar(800),ind_id) + ')') 'last_name_first_name_id',
			ind_email,
			ind_phone,
			ind_birthdate,
			ind_gender,
			ind_document,
			ref_id,
			(select ref_name from tb_references where ref_id = i.ref_id) ref_name,
			std_id,
			(select std_name from tb_studies where std_id = i.std_id) std_name,
			ind_details,
			(select count(*) from tb_individuals_samples where ind_id = i.ind_id) is_count
	from [dbo].[tb_individuals] as i
	where ind_id = @ind_id
	order by ind_id desc
--	select * from @table 
else
	select	ind_id,
			ind_date_created,
			ind_time_created,
			ind_date_created_text = convert(varchar(800), convert(date,ind_date_created)), 
			ind_first_name,
			ind_last_name,
			(ind_first_name + ' ' + ind_last_name) 'first_name_last_name',
			(ind_last_name + ', ' + ind_first_name + ' (' + convert(varchar(800),ind_id) + ')') 'last_name_first_name_id',
			ind_email,
			ind_phone,
			ind_birthdate,
			ind_gender,
			ind_document,
			ref_id,
			(select ref_name from tb_references where ref_id = i.ref_id) ref_name,
			std_id,
			(select std_name from tb_studies where std_id = i.std_id) std_name,
			ind_details,
			(select count(*) from tb_individuals_samples where ind_id = i.ind_id) is_count
	from [dbo].[tb_individuals] as i
	order by ind_id desc
end
go

--[usp_individuals_samples_select_barcode_print] @type = 1, @ind_id = 9
--[usp_individuals_samples_select_barcode_print] @type = 2, @is_id = 1
--[usp_individuals_samples_select_barcode_print] @type = 3, @is_barcode = 'A0000001'
--select * from [tb_individuals_samples] where ind_id = 1
--select * from [tb_individuals] where std_id = 342
--select * from tb_audit
--select * from tb_references where std_id = 342
--select * from tb_individuals_samples where std_id = 342
--usp_individuals_samples_select @type = 0
--usp_individuals_samples_select @type = 5, @date_start = '2020-08-01', @date_end = '2020-08-31' 

if OBJECT_ID('usp_individuals_insert') is not null
	drop procedure usp_individuals_insert
go
create procedure usp_individuals_insert
@usr_id_created		int = null,
@ind_first_name		varchar(800) = null,
@ind_last_name		varchar(800) = null,
@ind_email			varchar(800) = null,
@ind_phone			varchar(800) = null,
@ind_gender			varchar(800) = null,
@ind_document		varchar(800) = null,
@ref_id				int = null,
@std_id				int = null,
@ind_details		varchar(max) = null,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 


	insert into tb_individuals (usr_id_created,ind_first_name,ind_last_name,ind_email,ind_phone,ind_gender,ind_document,ref_id,std_id,ind_details)
	values (@usr_id_created, @ind_first_name, @ind_last_name, @ind_email, @ind_phone, @ind_gender, @ind_document, @ref_id, @std_id, @ind_details)
end
go

--select * from tb_individuals
--exec usp_individuals_delete @ind_id = 249
if OBJECT_ID('usp_individuals_delete') is not null
	drop procedure usp_individuals_delete
go
create procedure usp_individuals_delete
@ind_id				int,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	delete from tb_individuals where ind_id = @ind_id
end
go
--select * from tb_individuals where ind_id = 251
--select * from tb_audit
/*
	exec usp_individuals_update	@ind_id = 251, 
								@ind_first_name = 'Dean X',  
								@ind_last_name = 'Garza X', 
								@ind_email = 'dgarza@usf.edu X',
								@ind_gender = 'X',
								@ref_id = 127,
								@std_id = 127,
								@ind_details = 'Football X'
*/
if OBJECT_ID('usp_individuals_update') is not null
	drop procedure usp_individuals_update
go
create procedure usp_individuals_update
@ind_id				int = null,
@ind_first_name		varchar(800) = null,
@ind_last_name		varchar(800) = null,
@ind_email			varchar(800) = null,
@ind_phone			varchar(800) = null,
@ind_gender			varchar(800) = null,
@ind_document		varchar(800) = null,
@ref_id				int = null,
@std_id				int = null,
@ind_details		varchar(max) = null,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	update tb_individuals 
	set		ind_first_name = @ind_first_name,
			ind_last_name = @ind_last_name,
			ind_email = @ind_email,
			ind_phone = @ind_phone,
			ind_gender = @ind_gender,
			ind_document = @ind_document,
			ref_id = @ref_id,
			std_id = @std_id,
			ind_details = @ind_details
	where ind_id = @ind_id
end
go

--usp_individuals_insert @usr_id_created = 1, @ind_first_name = 'XXX', @ind_last_name = 'YYY', @usr_id_audit = 2
--select * from tb_audit
--select * from tb_references
--select * from tb_studies

--select * from tb_individuals_samples
if OBJECT_ID('usp_individuals_samples_insert') is not null
	drop procedure usp_individuals_samples_insert
go
create procedure usp_individuals_samples_insert
@type				int = 1,
@usr_id_created		int = null,
@is_date_collected	date = null,
@is_time_collected	time = null,
@usr_id_collected	int = null,
@ind_id				int = null,
@is_details			varchar(max) = null,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	if @type = 1
	begin
		insert into tb_individuals_samples (usr_id_created,is_date_collected,is_time_collected,usr_id_collected,ind_id,is_details)
		values (@usr_id_created, isnull(@is_date_collected,dbo.udf_getdatelocal(default)), isnull(@is_time_collected,dbo.udf_getdatelocal(default)), @usr_id_collected, @ind_id, @is_details)

		select IDENT_CURRENT('tb_individuals_samples') as is_id
	end
	else if @type = 2
	begin
		
		set xact_abort on

		begin try
			begin transaction

				declare @std_id_list varchar(max) = ''
				declare @std_id int
				declare @is_id int
				declare	@counter int = 0
			
				declare std_id_list cursor for select std_id from tb_individuals_barcode_generation ibg where ssn_id = @ssn_id
				open std_id_list
					fetch next from std_id_list into @std_id
					while @@fetch_status = 0 
					begin
						--set @std_id_list = @std_id_list + convert(varchar(800),@std_id) + ','
						--insert into  @std_id
						set @ind_id =  (select ind_id from tb_individuals where std_id = @std_id)

						insert into tb_individuals_samples (usr_id_created,is_date_collected,is_time_collected,usr_id_collected,ind_id,is_details)
						values (@usr_id_created, isnull(@is_date_collected,dbo.udf_getdatelocal(default)), isnull(@is_time_collected,dbo.udf_getdatelocal(default)), @usr_id_collected, @ind_id, @is_details)
					
						select @is_id = IDENT_CURRENT('tb_individuals_samples')

						update tb_individuals_barcode_generation
						set		is_id = @is_id, 
								is_id_date_created = dbo.udf_getdatelocal(default), 
								is_id_time_created = dbo.udf_getdatelocal(default)
						where std_id = @std_id and ssn_id = @ssn_id

						set	@counter += 1
						fetch next from std_id_list into @std_id
					end
				close std_id_list
				deallocate std_id_list

				insert into tb_individuals_barcode_printing (is_id, ssn_id)
				select is_id, ssn_id from tb_individuals_barcode_generation where ssn_id = @ssn_id
				
				--delete tb_individuals_barcode_generation where ssn_id = @ssn_id

				commit transaction
				select @counter 'counter'
		end try
		begin catch
			rollback transaction
			select 0 'counter'
		end catch
	end
end
go



--select * from tb_individuals_samples
--exec usp_individuals_samples_select @type = 1
--exec usp_individuals_samples_select @type = 2, @ind_id = 1
--exec usp_individuals_samples_select @type = 3, @is_id = 1
--exec usp_individuals_samples_select @type = 4, @is_barcode = 'A0000001'
--exec usp_individuals_samples_select @type = 5, @poo_id = 1
--exec usp_individuals_samples_select @type = 6, @date_start = '2020-08-01', @date_end  = '2020-09-01', @poo_id = 3, @poo_result = 'A'
--exec usp_individuals_samples_select @type = 7, @is_id_list = '1,3,4,5'


if OBJECT_ID('usp_individuals_samples_select') is not null
	drop procedure usp_individuals_samples_select
go
create procedure [dbo].usp_individuals_samples_select
@type				int = 1,
@ind_id				int = null,
@is_id				int = null,
@is_barcode			varchar(800) = null,
@pr_result			varchar(800) = null,
@poo_id				int = null,
@date_start			date = null,
@date_end			date = null,
@is_id_list			varchar(max) = null
as
if @type = 1
begin
--	select *,
	select	is_id	as is_id,
			is_barcode	as is_barcode,
			ind_id	as ind_id,
			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_first_name,
			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_last_name,
			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as 'first_name_last_name',
			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) as ind_gender,
			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) as ind_document,
			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) as ind_details,
			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) as ref_id,
			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) as ref_name,
			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) as std_id,
			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) as std_name,
			is_date_created as is_date_created,
			is_time_created as is_time_created,
			convert(varchar(800),convert(date,is_date_created)) as is_date_created_text,
			usr_id_created as usr_id_created,
			is_date_collected as is_date_collected,
			is_time_collected as is_time_collected,
			convert(varchar(800),convert(date,is_date_collected)) as is_date_collected_text,
			usr_id_collected as usr_id_collected,
			is_date_registered as is_date_registered,
			is_time_registered as is_time_registered,
			convert(varchar(800),convert(date,is_date_registered)) as is_date_registered_text,
			usr_id_registered as usr_id_registered,
			is_well_number as is_well_number,
			is_details as is_details,
			poo_id as poo_id,
			(select poo_details from tb_pools where poo_id = [is].poo_id) as poo_details,
			is_date_registered_pool as is_date_registered_pool,
			is_time_registered_pool as is_time_registered_pool,
			convert(varchar(800),convert(date,is_date_registered_pool)) as is_date_registered_pool_text,
			usr_id_registered_pool as usr_id_registered_pool,
			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_result,
			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_ct_value,
			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) as samples_count,
			ROW_NUMBER() over (order by is_date_created asc) as position
	from [dbo].[tb_individuals_samples] [is] with (index = ix_tb_individuals_samples_is_date_created)
	where is_date_created >= '2000-01-01'
	order by is_date_created desc, is_time_created desc
end
if @type = 2 
begin
	--select *, 
	--ROW_NUMBER() over (order by is_date_created asc) position
	--from @tabla where ind_id = @ind_id 
	select	is_id	as is_id,
			is_barcode	as is_barcode,
			ind_id	as ind_id,
			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_first_name,
			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_last_name,
			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as 'first_name_last_name',
			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) as ind_gender,
			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) as ind_document,
			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) as ind_details,
			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) as ref_id,
			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) as ref_name,
			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) as std_id,
			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) as std_name,
			is_date_created as is_date_created,
			is_time_created as is_time_created,
			convert(varchar(800),convert(date,is_date_created)) as is_date_created_text,
			usr_id_created as usr_id_created,
			is_date_collected as is_date_collected,
			is_time_collected as is_time_collected,
			convert(varchar(800),convert(date,is_date_collected)) as is_date_collected_text,
			usr_id_collected as usr_id_collected,
			is_date_registered as is_date_registered,
			is_time_registered as is_time_registered,
			convert(varchar(800),convert(date,is_date_registered)) as is_date_registered_text,
			usr_id_registered as usr_id_registered,
			is_well_number as is_well_number,
			is_details as is_details,
			poo_id as poo_id,
			(select poo_details from tb_pools where poo_id = [is].poo_id) as poo_details,
			is_date_registered_pool as is_date_registered_pool,
			is_time_registered_pool as is_time_registered_pool,
			convert(varchar(800),convert(date,is_date_registered_pool)) as is_date_registered_pool_text,
			usr_id_registered_pool as usr_id_registered_pool,
			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_result,
			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_ct_value,
			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) as samples_count,
			ROW_NUMBER() over (order by is_date_created asc) as position
	from [dbo].[tb_individuals_samples] [is] with (index = ix_tb_individuals_samples_ind_id)
	where ind_id = @ind_id 
	order by is_date_created desc, is_time_created desc
end
else if @type = 3
begin
	--select *, 
	--ROW_NUMBER() over (order by is_date_created asc) position
	--from @tabla where is_id = @is_id 
	select	is_id	as is_id,
			is_barcode	as is_barcode,
			ind_id	as ind_id,
			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_first_name,
			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_last_name,
			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as 'first_name_last_name',
			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) as ind_gender,
			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) as ind_document,
			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) as ind_details,
			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) as ref_id,
			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) as ref_name,
			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) as std_id,
			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) as std_name,
			is_date_created as is_date_created,
			is_time_created as is_time_created,
			convert(varchar(800),convert(date,is_date_created)) as is_date_created_text,
			usr_id_created as usr_id_created,
			is_date_collected as is_date_collected,
			is_time_collected as is_time_collected,
			convert(varchar(800),convert(date,is_date_collected)) as is_date_collected_text,
			usr_id_collected as usr_id_collected,
			is_date_registered as is_date_registered,
			is_time_registered as is_time_registered,
			convert(varchar(800),convert(date,is_date_registered)) as is_date_registered_text,
			usr_id_registered as usr_id_registered,
			is_well_number as is_well_number,
			is_details as is_details,
			poo_id as poo_id,
			(select poo_details from tb_pools where poo_id = [is].poo_id) as poo_details,
			is_date_registered_pool as is_date_registered_pool,
			is_time_registered_pool as is_time_registered_pool,
			convert(varchar(800),convert(date,is_date_registered_pool)) as is_date_registered_pool_text,
			usr_id_registered_pool as usr_id_registered_pool,
			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_result,
			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_ct_value,
			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) as samples_count,
			ROW_NUMBER() over (order by is_date_created asc) as position
	from [dbo].[tb_individuals_samples] [is]
	where is_id = @is_id 
	order by is_date_created desc, is_time_created desc
end
else if @type = 4
begin
	--select *, 
	--ROW_NUMBER() over (order by is_date_created asc) position
	--from @tabla where is_barcode = @is_barcode 
	select	is_id	as is_id,
			is_barcode	as is_barcode,
			ind_id	as ind_id,
			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_first_name,
			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_last_name,
			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as 'first_name_last_name',
			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) as ind_gender,
			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) as ind_document,
			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) as ind_details,
			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) as ref_id,
			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) as ref_name,
			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) as std_id,
			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) as std_name,
			is_date_created as is_date_created,
			is_time_created as is_time_created,
			convert(varchar(800),convert(date,is_date_created)) as is_date_created_text,
			usr_id_created as usr_id_created,
			is_date_collected as is_date_collected,
			is_time_collected as is_time_collected,
			convert(varchar(800),convert(date,is_date_collected)) as is_date_collected_text,
			usr_id_collected as usr_id_collected,
			is_date_registered as is_date_registered,
			is_time_registered as is_time_registered,
			convert(varchar(800),convert(date,is_date_registered)) as is_date_registered_text,
			usr_id_registered as usr_id_registered,
			is_well_number as is_well_number,
			is_details as is_details,
			poo_id as poo_id,
			(select poo_details from tb_pools where poo_id = [is].poo_id) as poo_details,
			is_date_registered_pool as is_date_registered_pool,
			is_time_registered_pool as is_time_registered_pool,
			convert(varchar(800),convert(date,is_date_registered_pool)) as is_date_registered_pool_text,
			usr_id_registered_pool as usr_id_registered_pool,
			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_result,
			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_ct_value,
			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) as samples_count,
			ROW_NUMBER() over (order by is_date_created asc) as position
	from [dbo].[tb_individuals_samples] [is] with (index = ix_tb_individuals_samples_is_barcode)
	where is_barcode = @is_barcode 
	order by is_date_created desc, is_time_created desc
end
else if @type = 5
begin
--	select *, 
--	ROW_NUMBER() over (order by is_date_created asc) position
--	from @tabla where poo_id = @poo_id 
	select	is_id	as is_id,
			is_barcode	as is_barcode,
			ind_id	as ind_id,
			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_first_name,
			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_last_name,
			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as 'first_name_last_name',
			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) as ind_gender,
			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) as ind_document,
			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) as ind_details,
			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) as ref_id,
			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) as ref_name,
			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) as std_id,
			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) as std_name,
			is_date_created as is_date_created,
			is_time_created as is_time_created,
			convert(varchar(800),convert(date,is_date_created)) as is_date_created_text,
			usr_id_created as usr_id_created,
			is_date_collected as is_date_collected,
			is_time_collected as is_time_collected,
			convert(varchar(800),convert(date,is_date_collected)) as is_date_collected_text,
			usr_id_collected as usr_id_collected,
			is_date_registered as is_date_registered,
			is_time_registered as is_time_registered,
			convert(varchar(800),convert(date,is_date_registered)) as is_date_registered_text,
			usr_id_registered as usr_id_registered,
			is_well_number as is_well_number,
			is_details as is_details,
			poo_id as poo_id,
			(select poo_details from tb_pools where poo_id = [is].poo_id) as poo_details,
			is_date_registered_pool as is_date_registered_pool,
			is_time_registered_pool as is_time_registered_pool,
			convert(varchar(800),convert(date,is_date_registered_pool)) as is_date_registered_pool_text,
			usr_id_registered_pool as usr_id_registered_pool,
			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_result,
			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_ct_value,
			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) as samples_count,
			ROW_NUMBER() over (order by is_date_created asc) as position
	from [dbo].[tb_individuals_samples] [is] with (index = ix_tb_individuals_samples_poo_id)	
	where poo_id = @poo_id 
	order by is_date_registered_pool desc, is_time_registered_pool desc
end
else if @type = 6
begin

	declare @tabla table	(
		is_id							int,
		is_barcode						varchar(800),
		ind_id							int,
		ind_first_name					varchar(800),
		ind_last_name					varchar(800),
		first_name_last_name			varchar(800),
		ind_gender						varchar(800),
		ind_document					varchar(800),
		ind_details						varchar(max),
		ref_id							int,
		ref_name						varchar(800),
		std_id							int,
		std_name						varchar(800),
		is_date_created					date,
		is_time_created					time,
		is_date_created_text			varchar(800),
		usr_id_created					int,
		is_date_collected				date,
		is_time_collected				time,
		is_date_collected_text			varchar(800),
		usr_id_collected				int,
		is_date_registered				date,
		is_time_registered				time,
		is_date_registered_text			varchar(800),
		usr_id_registered				int,
		is_well_number					varchar(800),
		is_details						varchar(max),
		poo_id							int,
		poo_details						varchar(max),
		is_date_registered_pool			date,
		is_time_registered_pool			time,
		is_date_registered_pool_text	varchar(800),
		usr_id_registered_pool			int,
		pr_result						varchar(800),
		pr_ct_value						varchar(800),
		samples_count					int,
		position						int
	)

	insert into @tabla
	select	is_id	as is_id,
			is_barcode	as is_barcode,
			ind_id	as ind_id,
			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_first_name,
			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_last_name,
			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as 'first_name_last_name',
			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) as ind_gender,
			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) as ind_document,
			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) as ind_details,
			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) as ref_id,
			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) as ref_name,
			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) as std_id,
			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) as std_name,
			is_date_created as is_date_created,
			is_time_created as is_time_created,
			convert(varchar(800),convert(date,is_date_created)) as is_date_created_text,
			usr_id_created as usr_id_created,
			is_date_collected as is_date_collected,
			is_time_collected as is_time_collected,
			convert(varchar(800),convert(date,is_date_collected)) as is_date_collected_text,
			usr_id_collected as usr_id_collected,
			is_date_registered as is_date_registered,
			is_time_registered as is_time_registered,
			convert(varchar(800),convert(date,is_date_registered)) as is_date_registered_text,
			usr_id_registered as usr_id_registered,
			is_well_number as is_well_number,
			is_details as is_details,
			poo_id as poo_id,
			(select poo_details from tb_pools where poo_id = [is].poo_id) as poo_details,
			is_date_registered_pool as is_date_registered_pool,
			is_time_registered_pool as is_time_registered_pool,
			convert(varchar(800),convert(date,is_date_registered_pool)) as is_date_registered_pool_text,
			usr_id_registered_pool as usr_id_registered_pool,
			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_result,
			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_ct_value,
			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) as samples_count,
			ROW_NUMBER() over (order by is_date_created asc) as position
	from [dbo].[tb_individuals_samples] [is] with (index = ix_tb_individuals_samples_is_date_created)	
	where is_date_created >= @date_start and is_date_created <= @date_end
	order by is_id asc

	if @poo_id > 0
	begin
		delete from @tabla where poo_id != @poo_id 
	end

	if @pr_result = 'P'
	begin
		delete from @tabla where not pr_result = 'P' or pr_result is null or pr_result = '' 
	end
	else if @pr_result = 'N'
	begin
		delete from @tabla where not pr_result = 'N' or pr_result is null or pr_result = ''
	end
	else if @pr_result = 'U'
	begin
		delete from @tabla where not (pr_result is null or pr_result = '' or poo_id is null)
	end

	select * from @tabla

end
else if @type = 7
begin


	declare @list table(is_id varchar(800))

	while len(@is_id_list) > 0
		begin
			insert into @list(is_id)
			select left(@is_id_list, charindex(',', @is_id_list+',') -1) as is_id
    
			set @is_id_list = stuff(@is_id_list, 1, charindex(',', @is_id_list + ','), '')
		end


	--select *, 
	--ROW_NUMBER() over (order by is_date_created asc) position
	--from @tabla where is_id = @is_id 
	select	is_id	as is_id,
			is_barcode	as is_barcode,
			ind_id	as ind_id,
			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_first_name,
			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_last_name,
			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as 'first_name_last_name',
			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) as ind_gender,
			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) as ind_document,
			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) as ind_details,
			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) as ref_id,
			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) as ref_name,
			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) as std_id,
			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) as std_name,
			is_date_created as is_date_created,
			is_time_created as is_time_created,
			convert(varchar(800),convert(date,is_date_created)) as is_date_created_text,
			usr_id_created as usr_id_created,
			is_date_collected as is_date_collected,
			is_time_collected as is_time_collected,
			convert(varchar(800),convert(date,is_date_collected)) as is_date_collected_text,
			usr_id_collected as usr_id_collected,
			is_date_registered as is_date_registered,
			is_time_registered as is_time_registered,
			convert(varchar(800),convert(date,is_date_registered)) as is_date_registered_text,
			usr_id_registered as usr_id_registered,
			is_well_number as is_well_number,
			is_details as is_details,
			poo_id as poo_id,
			(select poo_details from tb_pools where poo_id = [is].poo_id) as poo_details,
			is_date_registered_pool as is_date_registered_pool,
			is_time_registered_pool as is_time_registered_pool,
			convert(varchar(800),convert(date,is_date_registered_pool)) as is_date_registered_pool_text,
			usr_id_registered_pool as usr_id_registered_pool,
			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_result,
			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_ct_value,
			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) as samples_count,
			ROW_NUMBER() over (order by is_date_created asc) as position
	from [dbo].[tb_individuals_samples] [is]
	where is_id in (select is_id from @list) 
	order by is_date_created desc, is_time_created desc
end
--if @type = 8
--begin
----	select *,
--	select	top 500
--			is_id	as is_id,
--			is_barcode	as is_barcode,
--			ind_id	as ind_id,
--			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_first_name,
--			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as ind_last_name,
--			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) as 'first_name_last_name',
--			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) as ind_gender,
--			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) as ind_document,
--			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) as ind_details,
--			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) as ref_id,
--			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) as ref_name,
--			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) as std_id,
--			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) as std_name,
--			is_date_created as is_date_created,
--			is_time_created as is_time_created,
--			convert(varchar(800),convert(date,is_date_created)) as is_date_created_text,
--			usr_id_created as usr_id_created,
--			is_date_collected as is_date_collected,
--			is_time_collected as is_time_collected,
--			convert(varchar(800),convert(date,is_date_collected)) as is_date_collected_text,
--			usr_id_collected as usr_id_collected,
--			is_date_registered as is_date_registered,
--			is_time_registered as is_time_registered,
--			convert(varchar(800),convert(date,is_date_registered)) as is_date_registered_text,
--			usr_id_registered as usr_id_registered,
--			is_well_number as is_well_number,
--			is_details as is_details,
--			poo_id as poo_id,
--			(select poo_details from tb_pools where poo_id = [is].poo_id) as poo_details,
--			is_date_registered_pool as is_date_registered_pool,
--			is_time_registered_pool as is_time_registered_pool,
--			convert(varchar(800),convert(date,is_date_registered_pool)) as is_date_registered_pool_text,
--			usr_id_registered_pool as usr_id_registered_pool,
--			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_result,
--			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) as pr_ct_value,
--			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) as samples_count,
--			ROW_NUMBER() over (order by is_date_created asc) as position
--	from [dbo].[tb_individuals_samples] [is] with (index = ix_tb_individuals_samples_is_date_created)
--	where is_date_created >= '2000-01-01'
--	order by is_date_created desc, is_time_created desc
--end


go




if OBJECT_ID('usp_individuals_samples_delete') is not null
	drop procedure usp_individuals_samples_delete
go
create procedure usp_individuals_samples_delete
@is_id				int,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id  

	delete from tb_individuals_samples where is_id = @is_id
end
go

--alter procedure usp_individuals_samples_select_all
--as
--select	is_id,
--		is_barcode,
--		is_date_created,
--		is_time_created,
--		convert(varchar(800),convert(date,is_date_created)) is_date_created_text,
--		(	select ind_first_name from tb_individuals i
--			where i.ind_id = [is].ind_id
--		) ind_first_name,
--		(	select ind_last_name from tb_individuals i
--			where i.ind_id = [is].ind_id
--		) ind_last_name,
--		(	select ind_gender from tb_individuals i
--			where i.ind_id = [is].ind_id
--		) ind_gender,
--		(	select ind_document from tb_individuals i
--			where i.ind_id = [is].ind_id
--		) ind_document,
--		(	select std_id from tb_individuals i
--			where i.ind_id = [is].ind_id
--		) std_id,
--		(	select (select ref_name from tb_references where std_id = i.std_id)
--			from tb_individuals i
--			where i.ind_id = [is].ind_id
--		) ref_name,
--		(	select ind_details from tb_individuals i
--			where i.ind_id = [is].ind_id
--		) ind_details,
--		ind_id,
--		is_date_collected,
--		is_time_collected,
--		convert(varchar(800),convert(date,is_date_collected)) is_date_collected_text,
--		is_date_registered,
--		is_time_registered,
--		convert(varchar(800),convert(date,is_date_registered)) is_date_registered_text,
--		is_details,
--		poo_id,
--		is_well_number
--from [dbo].[tb_individuals_samples] [is]
--order by is_barcode desc
--go


if OBJECT_ID('usp_references_select') is not null
	drop procedure usp_references_select
go
--usp_references_select @type = 2, @ref_id = 5
--select * from tb_individuals_samples
create procedure usp_references_select
@type	int = 1,
@ref_id int = 0
as
if @type = 1
begin
	select	ref_id,
			ref_n,
			ref_name, 
			ref_details
	from [dbo].tb_references 
	where ref_id not in (select distinct ref_id from tb_individuals where ref_id is not null)
	order by ref_id asc
end
else if @type = 2
begin
	select	ref_id,
			ref_n,
			ref_name, 
			ref_details
	from [dbo].tb_references 
	where	ref_id not in (select distinct ref_id from tb_individuals where ref_id is not null) or
			ref_id = @ref_id
	order by ref_id asc
end
else if @type = 3
begin
	select	ref_id,
			ref_n,
			ref_name, 
			ref_details
	from [dbo].tb_references 
	where	ref_id = @ref_id
	order by ref_id asc
end
go


--usp_studies_select @type = 2, @std_id = 1
--select * from tb_individuals_samples
if OBJECT_ID('usp_studies_select') is not null
	drop procedure usp_studies_select
go
create procedure usp_studies_select
@type	int = 1,
@std_id int = 0
as
if @type = 1
begin
	select	std_id,
			std_n,
			std_name, 
			std_details
	from [dbo].tb_studies
	where std_id not in (select distinct std_id from tb_individuals where std_id is not null)
	order by std_id asc
end
else if @type = 2
begin
	select	std_id,
			std_n,
			std_name, 
			std_details
	from [dbo].tb_studies 
	where	std_id not in (select distinct std_id from tb_individuals where std_id is not null) or
			std_id = @std_id
	order by std_id asc
end
else if @type = 3
begin
	select	std_id,
			std_n,
			std_name, 
			std_details
	from [dbo].tb_studies 
	where	std_id = @std_id
	order by std_id asc
end
go

--if OBJECT_ID('usp_studies_select') is not null
--	drop procedure usp_studies_select
--go
--create procedure usp_studies_select
--@std_id		int
--as
--select	std_id,
--		ref_n,
--		ref_name, 
--		std_details
--from [dbo].tb_references 
--where std_id = @std_id
--order by ref_name asc
--go


if OBJECT_ID('usp_pools_select') is not null
	drop procedure usp_pools_select
go
create procedure [dbo].[usp_pools_select]
@type	int = 1,
@poo_id int = null
as
declare @tabla table	(
	poo_id						int,
	poo_date_created			date,
	poo_date_created_text		varchar(800),
	poo_time_created			time,
	pr_result					varchar(800),
	pr_ct_value					varchar(800),
	poo_details					varchar(max),
	poo_count					int
)
insert into @tabla
	select	poo_id,
			poo_date_created,
			convert(varchar(800),convert(date,poo_date_created)) poo_date_created_text,
			poo_time_created,
			(select top 1 pr_result from [dbo].[tb_pools_results] where poo_id = p.poo_id order by pr_date_result desc, pr_time_result desc) pr_result,
			(select top 1 pr_ct_value from [dbo].[tb_pools_results] where poo_id = p.poo_id order by pr_date_result desc, pr_time_result desc) pr_ct_value,
			poo_details,
			(select count(*) from tb_individuals_samples where poo_id = p.poo_id) poo_count
	from [dbo].[tb_pools] as p 
	order by poo_date_created desc, poo_time_created desc

if @type = 1 
begin
	select * from @tabla order by poo_date_created desc, poo_time_created desc
end
else if @type = 2 
begin
	select * from @tabla where poo_id = @poo_id order by poo_date_created desc, poo_time_created desc
end
else if @type = 3
begin
	select distinct poo_id from @tabla order by poo_id asc
end
go




--select * from tb_pools
if OBJECT_ID('usp_pools_insert') is not null
	drop procedure usp_pools_insert
go
create procedure dbo.usp_pools_insert
@poo_details			varchar(max) = null,
@usr_id_audit			int = null,
@ssn_id					int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	insert into tb_pools (usr_id_created, poo_details)
	values (@usr_id_audit, @poo_details)

	select IDENT_CURRENT('tb_pools') as poo_id
end

go







--select * from tb_individuals_samples
if OBJECT_ID('usp_individuals_samples_update') is not null
	drop procedure usp_individuals_samples_update
go
create procedure usp_individuals_samples_update
@type						int = null,
@is_id						int = null,
@is_date_collected			date = null,
@is_time_collected			time = null,
@usr_id_collected			int = null,
@is_date_registered			date = null,
@is_time_registered			time = null,
@usr_id_registered			int = null,
@ind_id						int = null,
@poo_id						int = null,
@is_date_registered_pool	date = null,
@is_time_registered_pool	time = null,
@usr_id_registered_pool		int = null,
@is_well_number				varchar(800) = null,
@is_details					varchar(max) = null,
@is_barcode					varchar(800) = null,
@operation					int = null,
@usr_id_audit				int = null,
@ssn_id						int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	if	@type = 1 
	begin
			update	tb_individuals_samples 
			set		is_date_collected = @is_date_collected,
					is_time_collected  = @is_time_collected,
					usr_id_collected = @usr_id_collected,
					is_date_registered = @is_date_registered,
					is_time_registered  = case when @is_time_registered = '' then dbo.udf_getdatelocal(default) else @is_time_registered end ,
					usr_id_registered = case when @usr_id_registered = '' then @usr_id_audit else @usr_id_registered end,
					ind_id = @ind_id,
					is_well_number	= @is_well_number,
					is_details	= @is_details
			where is_id = @is_id
	end
	if	@type = 2 --pools
	begin
		if @operation = 1
		begin
			update	tb_individuals_samples
			set		poo_id = @poo_id,
					is_date_registered_pool = dbo.udf_getdatelocal(default),
					is_time_registered_pool = dbo.udf_getdatelocal(default),
					usr_id_registered_pool = @usr_id_registered
			where is_barcode = @is_barcode
		end
		else	if @operation = 2
		begin
			update	tb_individuals_samples
			set		poo_id = null,
					is_date_registered_pool = null,
					is_time_registered_pool = null,
					usr_id_registered_pool = null
			where is_barcode = @is_barcode
		end
		else if @operation = 3
		begin
			update	tb_individuals_samples
			set		poo_id = null,
					is_date_registered_pool = null,
					is_time_registered_pool = null,
					usr_id_registered_pool = null
			where is_id in (select is_id from tb_individuals_samples where poo_id = @poo_id) 

			delete tb_pools where poo_id = @poo_id
		end
	end
	if	@type = 3 --register barcde
	begin
		update tb_individuals_samples
		set		is_date_registered = dbo.udf_getdatelocal(default),
				is_time_registered = dbo.udf_getdatelocal(default),
				usr_id_registered = @usr_id_registered
		where is_id = @is_id
	end
	if	@type = 4
	begin
		update tb_individuals_samples
		set		is_well_number = (case when (@is_well_number = '') then null else @is_well_number end)
		where is_id = @is_id
	end
end
go

--select * from tb_individuals_samples
--select * from tb_audit

--if OBJECT_ID('usp_individuals_samples_update_pool_id') is not null
--	drop procedure usp_individuals_samples_update_pool_id
--go
--create procedure usp_individuals_samples_update_pool_id
--@is_barcode			varchar(800),
--@poo_id				int,
--@operation			int 
--as
--begin
--	if @operation = 1
--	begin
--		update	tb_individuals_samples
--		set		poo_id = @poo_id,
--				is_date_registered_pool = dbo.udf_getdatelocal(default),
--				is_time_registered_pool = dbo.udf_getdatelocal(default)
--		where is_barcode = @is_barcode
--	end
--	else	if @operation = 2
--	begin
--		update	tb_individuals_samples
--		set		poo_id = null,
--				is_date_registered_pool = null,
--				is_time_registered_pool = null
--		where is_barcode = @is_barcode
--	end
--	else if @operation = 3
--	begin
--		delete tb_pools where poo_id = @poo_id
--	end
--end
--go

--if OBJECT_ID('usp_individuals_samples_register_barcode') is not null
--	drop procedure usp_individuals_samples_register_barcode
--go
--create procedure usp_individuals_samples_register_barcode
--@is_id		int
--as
--update tb_individuals_samples
--set		is_date_registered = dbo.udf_getdatelocal(default),
--		is_time_registered = dbo.udf_getdatelocal(default)
--where is_id = @is_id
--go

----select * from tb_individuals_samples
----select * from tb_pools_results
--if OBJECT_ID('usp_individuals_samples_update_well_number') is not null
--	drop procedure usp_individuals_samples_update_well_number
--go
--create procedure usp_individuals_samples_update_well_number
--@is_id			int,
--@is_well_number	varchar(800)= null
--as
--go


--if OBJECT_ID('usp_pools_results_update_result') is not null
--	drop procedure usp_pools_results_update_result
--go
--create procedure usp_pools_results_update_result
--@poo_id				int,
--@pr_result			varchar(800) 
--as
--begin
--	if exists (select * from [dbo].[tb_pools_results] where poo_id= @poo_id)
--	begin
--		update	[dbo].[tb_pools_results]
--		set		pr_result = (case when (@pr_result = '') then null else @pr_result end),
--				[pr_date_result] = dbo.udf_getdatelocal(default),
--				[pr_time_result] = dbo.udf_getdatelocal(default)
--		where	poo_id = @poo_id
--	end
--	else 
--	begin
--		insert into	[dbo].[tb_pools_results] (poo_id, pr_date_result, pr_time_result, pr_result)
--		values		(@poo_id,dbo.udf_getdatelocal(default), dbo.udf_getdatelocal(default),(case when (@pr_result = '') then null else @pr_result end))
--	end
--end
--go


--if OBJECT_ID('usp_pools_results_update_ct_value') is not null
--	drop procedure usp_pools_results_update_ct_value
--go
--create procedure usp_pools_results_update_ct_value
--@poo_id				int,
--@pr_ct_value		varchar(800) 
--as
--begin
--	if exists (select * from [dbo].[tb_pools_results] where poo_id= @poo_id)
--	begin
--		update	[dbo].[tb_pools_results]
--		set		pr_ct_value = (case when (@pr_ct_value = '') then null else @pr_ct_value end)
--		where	poo_id = @poo_id
--	end
--	else 
--	begin
--		insert into	[dbo].[tb_pools_results] (poo_id, pr_date_result, pr_time_result, pr_ct_value)
--		values		(@poo_id,dbo.udf_getdatelocal(default), dbo.udf_getdatelocal(default),(case when (@pr_ct_value = '') then null else @pr_ct_value end))
--	end
--end
--go



if OBJECT_ID('usp_pools_results_update') is not null
	drop procedure usp_pools_results_update
go
create procedure usp_pools_results_update
@type				int,
@poo_id				int,
@pr_value			varchar(800), 
@usr_id				int,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id  

		if @type = 1
		begin

			if exists (select * from tb_pools_results where poo_id= @poo_id)
			begin
				update	tb_pools_results
				set		pr_ct_value = (case when (@pr_value = '') then null else @pr_value end),
						pr_date_ct_value = dbo.udf_getdatelocal(default),
						pr_time_ct_value = dbo.udf_getdatelocal(default),
						usr_id_ct_value = @usr_id
				where	poo_id = @poo_id
			end
			else 
			begin
				insert into	tb_pools_results (poo_id, pr_date_result, pr_time_result, usr_id_created, pr_ct_value, usr_id_ct_value)
				values		(@poo_id,dbo.udf_getdatelocal(default), dbo.udf_getdatelocal(default), @usr_id,(case when (@pr_value = '') then null else @pr_value end), @usr_id)
			end

		end
		else if @type = 2
		begin

			if exists (select * from tb_pools_results where poo_id= @poo_id)
			begin
				update	tb_pools_results
				set		pr_result = (case when (@pr_value = '') then null else @pr_value end),
						pr_date_result = dbo.udf_getdatelocal(default),
						pr_time_result = dbo.udf_getdatelocal(default),
						usr_id_result = @usr_id
				where	poo_id = @poo_id
			end
			else 
			begin
				insert into	[dbo].[tb_pools_results] (poo_id, pr_date_result, pr_time_result, usr_id_created, pr_result, usr_id_result)
				values		(@poo_id,dbo.udf_getdatelocal(default), dbo.udf_getdatelocal(default), @usr_id,(case when (@pr_value = '') then null else @pr_value end), @usr_id)
			end
		end
end
go

--exec usp_sessions_insert @username = 'richter1@usf.edu'
--select * from tb_sessions
if OBJECT_ID('usp_sessions_insert') is not null
	drop procedure usp_sessions_insert
go
create procedure [dbo].usp_sessions_insert
@type		int = 1,
@username	nvarchar(800) 
as
begin
	if @type = 1
	begin
		declare @ssn_id int
		declare @usr_id_audit int
		declare @usr_username_audit nvarchar(800)
		select @usr_id_audit = usr_id, @usr_username_audit = usr_username 
		from tb_users with (index = ix_tb_users_usr_username) where @username like '%'+ usr_username +'%'

		insert into tb_sessions (usr_id) values (@usr_id_audit)
		set @ssn_id = IDENT_CURRENT('tb_sessions')

		select @ssn_id 'ssn_id', @usr_id_audit 'usr_id_audit', @usr_username_audit 'usr_username_audit'
	end
end
--select * from tb_sessions
--select * from tb_users

go












--AUDITS
drop trigger if exists tr_tb_individuals
go
create trigger tr_tb_individuals
on tb_individuals
after insert, update, delete
as
set nocount on
begin
		declare @operation_id nvarchar(800)
		declare @tablename nvarchar(800)
		declare @columnname nvarchar(800)
		declare @columnname_first nvarchar(800)
		declare @tablecolumnsinserted int = 1
		declare @tablerowsinserted int = 1
		declare @tablecolumnsdeleted int = 1
		declare @tablerowsdeleted int = 1
		declare @contadorfilas int = 1
		declare @contadorcolumnas int = 1
		declare @id_value nvarchar(max)
		declare @query_id_value nvarchar(max)
		declare @column_value_after nvarchar(max)
		declare @query_column_value_after nvarchar(max)
		declare @column_value_before nvarchar(max)
		declare @query_column_value_before nvarchar(max)
		declare @usr_id_audit int
		declare @ssn_id int

		select @operation_id = isnull(max(aud_operation_id),0) + 1 from tb_audit 

		select @usr_id_audit = cast(SESSION_CONTEXT(N'usr_id_audit') as int)
		select @ssn_id = cast(SESSION_CONTEXT(N'ssn_id') as int)		

	if exists (select * from inserted) and not exists (select * from deleted)
	begin

		select * into #inserted_i from inserted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if (@column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id, aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'INSERT',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, null, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_i 

	end
	else if exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #inserted_u from inserted
		select * into #deleted_u from deleted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output


					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if @column_value_before != @column_value_after or (@column_value_before is not null and @column_value_after is null) or (@column_value_before is null and @column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit,ssn_id)
						select HOST_NAME(),@operation_id,'UPDATE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_u
		drop table #deleted_u

	end
	else if not exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #deleted_d from deleted
		
		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsdeleted= COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsdeleted = COUNT(*) 
		from deleted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsdeleted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsdeleted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output

					if (@column_value_before is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'DELETE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, null, @usr_id_audit, @ssn_id
					end
					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #deleted_d 

	end
end
go






drop trigger if exists tr_tb_individuals_samples
go
create trigger tr_tb_individuals_samples
on tb_individuals_samples
after insert, update, delete
as
set nocount on
begin
		declare @operation_id nvarchar(800)
		declare @tablename nvarchar(800)
		declare @columnname nvarchar(800)
		declare @columnname_first nvarchar(800)
		declare @tablecolumnsinserted int = 1
		declare @tablerowsinserted int = 1
		declare @tablecolumnsdeleted int = 1
		declare @tablerowsdeleted int = 1
		declare @contadorfilas int = 1
		declare @contadorcolumnas int = 1
		declare @id_value nvarchar(max)
		declare @query_id_value nvarchar(max)
		declare @column_value_after nvarchar(max)
		declare @query_column_value_after nvarchar(max)
		declare @column_value_before nvarchar(max)
		declare @query_column_value_before nvarchar(max)
		declare @usr_id_audit int
		declare @ssn_id int

		select @operation_id = isnull(max(aud_operation_id),0) + 1 from tb_audit 

		select @usr_id_audit = cast(SESSION_CONTEXT(N'usr_id_audit') as int)
		select @ssn_id = cast(SESSION_CONTEXT(N'ssn_id') as int)		

	if exists (select * from inserted) and not exists (select * from deleted)
	begin

		select * into #inserted_i from inserted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if (@column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id, aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'INSERT',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, null, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_i 

	end
	else if exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #inserted_u from inserted
		select * into #deleted_u from deleted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output


					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if @column_value_before != @column_value_after or (@column_value_before is not null and @column_value_after is null) or (@column_value_before is null and @column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit,ssn_id)
						select HOST_NAME(),@operation_id,'UPDATE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_u
		drop table #deleted_u

	end
	else if not exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #deleted_d from deleted
		
		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsdeleted= COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsdeleted = COUNT(*) 
		from deleted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsdeleted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsdeleted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output

					if (@column_value_before is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'DELETE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, null, @usr_id_audit, @ssn_id
					end
					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #deleted_d 

	end
end
go










drop trigger if exists tr_tb_pools
go
create trigger tr_tb_pools
on tb_pools
after insert, update, delete
as
set nocount on
begin
		declare @operation_id nvarchar(800)
		declare @tablename nvarchar(800)
		declare @columnname nvarchar(800)
		declare @columnname_first nvarchar(800)
		declare @tablecolumnsinserted int = 1
		declare @tablerowsinserted int = 1
		declare @tablecolumnsdeleted int = 1
		declare @tablerowsdeleted int = 1
		declare @contadorfilas int = 1
		declare @contadorcolumnas int = 1
		declare @id_value nvarchar(max)
		declare @query_id_value nvarchar(max)
		declare @column_value_after nvarchar(max)
		declare @query_column_value_after nvarchar(max)
		declare @column_value_before nvarchar(max)
		declare @query_column_value_before nvarchar(max)
		declare @usr_id_audit int
		declare @ssn_id int

		select @operation_id = isnull(max(aud_operation_id),0) + 1 from tb_audit 

		select @usr_id_audit = cast(SESSION_CONTEXT(N'usr_id_audit') as int)
		select @ssn_id = cast(SESSION_CONTEXT(N'ssn_id') as int)		

	if exists (select * from inserted) and not exists (select * from deleted)
	begin

		select * into #inserted_i from inserted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if (@column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id, aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'INSERT',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, null, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_i 

	end
	else if exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #inserted_u from inserted
		select * into #deleted_u from deleted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output


					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if @column_value_before != @column_value_after or (@column_value_before is not null and @column_value_after is null) or (@column_value_before is null and @column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit,ssn_id)
						select HOST_NAME(),@operation_id,'UPDATE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_u
		drop table #deleted_u

	end
	else if not exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #deleted_d from deleted
		
		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsdeleted= COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsdeleted = COUNT(*) 
		from deleted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsdeleted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsdeleted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output

					if (@column_value_before is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'DELETE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, null, @usr_id_audit, @ssn_id
					end
					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #deleted_d 

	end
end
go




drop trigger if exists tr_tb_pools_results
go
create trigger tr_tb_pools_results
on tb_pools_results
after insert, update, delete
as
set nocount on
begin
		declare @operation_id nvarchar(800)
		declare @tablename nvarchar(800)
		declare @columnname nvarchar(800)
		declare @columnname_first nvarchar(800)
		declare @tablecolumnsinserted int = 1
		declare @tablerowsinserted int = 1
		declare @tablecolumnsdeleted int = 1
		declare @tablerowsdeleted int = 1
		declare @contadorfilas int = 1
		declare @contadorcolumnas int = 1
		declare @id_value nvarchar(max)
		declare @query_id_value nvarchar(max)
		declare @column_value_after nvarchar(max)
		declare @query_column_value_after nvarchar(max)
		declare @column_value_before nvarchar(max)
		declare @query_column_value_before nvarchar(max)
		declare @usr_id_audit int
		declare @ssn_id int

		select @operation_id = isnull(max(aud_operation_id),0) + 1 from tb_audit 

		select @usr_id_audit = cast(SESSION_CONTEXT(N'usr_id_audit') as int)
		select @ssn_id = cast(SESSION_CONTEXT(N'ssn_id') as int)		

	if exists (select * from inserted) and not exists (select * from deleted)
	begin

		select * into #inserted_i from inserted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if (@column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id, aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'INSERT',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, null, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_i 

	end
	else if exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #inserted_u from inserted
		select * into #deleted_u from deleted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output


					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if @column_value_before != @column_value_after or (@column_value_before is not null and @column_value_after is null) or (@column_value_before is null and @column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit,ssn_id)
						select HOST_NAME(),@operation_id,'UPDATE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_u
		drop table #deleted_u

	end
	else if not exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #deleted_d from deleted
		
		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsdeleted= COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsdeleted = COUNT(*) 
		from deleted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsdeleted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsdeleted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output

					if (@column_value_before is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'DELETE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, null, @usr_id_audit, @ssn_id
					end
					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #deleted_d 

	end
end
go


--INDEXES

--INDIVIDUALS
if exists(select * from sys.indexes 
where name='ix_tb_individuals_std_id' and object_id = object_id('tb_individuals'))
begin
	drop index tb_individuals.ix_tb_individuals_std_id
end
create nonclustered index ix_tb_individuals_std_id on tb_individuals (std_id)
go


if exists(select * from sys.indexes 
where name='ix_tb_individuals_ref_id' and object_id = object_id('tb_individuals'))
begin
	drop index tb_individuals.ix_tb_individuals_ref_id
end
create nonclustered index ix_tb_individuals_ref_id on tb_individuals (ref_id)
go



--INDIVIDUALS SAMPLES

if exists(select * from sys.indexes 
where name='ix_tb_individuals_samples_ind_id' and object_id = object_id('tb_individuals_samples'))
begin
	drop index tb_individuals_samples.ix_tb_individuals_samples_ind_id
end
create nonclustered index ix_tb_individuals_samples_ind_id on tb_individuals_samples (ind_id, is_date_created, is_time_created)
include (	is_barcode, usr_id_created, is_date_collected, 
			is_time_collected, usr_id_collected, is_date_registered, is_time_registered, usr_id_registered,
			is_well_number, is_details, poo_id, is_date_registered_pool, is_time_registered_pool, usr_id_registered_pool)
go



if exists(select * from sys.indexes 
where name='ix_tb_individuals_samples_is_barcode' and object_id = object_id('tb_individuals_samples'))
begin
	drop index tb_individuals_samples.ix_tb_individuals_samples_is_barcode
end
create nonclustered index ix_tb_individuals_samples_is_barcode on tb_individuals_samples (is_barcode, is_date_created, is_time_created)
include (	ind_id , usr_id_created, is_date_collected, 
			is_time_collected, usr_id_collected, is_date_registered, is_time_registered, usr_id_registered,
			is_well_number, is_details, poo_id, is_date_registered_pool, is_time_registered_pool, usr_id_registered_pool)
go



if exists(select * from sys.indexes 
where name='ix_tb_individuals_samples_poo_id' and object_id = object_id('tb_individuals_samples'))
begin
	drop index tb_individuals_samples.ix_tb_individuals_samples_poo_id
end
create nonclustered index ix_tb_individuals_samples_poo_id on tb_individuals_samples (poo_id, is_date_registered_pool, is_time_registered_pool)
include (	is_barcode, is_date_created, is_time_created, usr_id_created, is_date_collected, 
			is_time_collected, usr_id_collected, is_date_registered, is_time_registered, usr_id_registered,
			is_well_number, is_details, ind_id, usr_id_registered_pool)
go



if exists(select * from sys.indexes 
where name='ix_tb_individuals_samples_is_date_created' and object_id = object_id('tb_individuals_samples'))
begin
	drop index tb_individuals_samples.ix_tb_individuals_samples_is_date_created
end
create nonclustered index ix_tb_individuals_samples_is_date_created on tb_individuals_samples (is_date_created, is_time_created, is_id)
include (	is_barcode, ind_id, usr_id_created, is_date_collected, 
			is_time_collected, usr_id_collected, is_date_registered, is_time_registered, usr_id_registered,
			is_well_number, is_details, poo_id, is_date_registered_pool, is_time_registered_pool, usr_id_registered_pool)
go



--STUDIES

if exists(select * from sys.indexes 
where name='ix_tb_studies_std_id' and object_id = object_id('tb_studies'))
begin
	drop index tb_studies.ix_tb_studies_std_id
end
create nonclustered index ix_tb_studies_std_id on tb_studies (std_id)
include (std_n, std_details, std_name)
go



--REFERENCES

if exists(select * from sys.indexes 
where name='ix_tb_studies_ref_id' and object_id = object_id('tb_references'))
begin
	drop index tb_references.ix_tb_studies_ref_id
end
create nonclustered index ix_tb_studies_ref_id on tb_references (ref_id)
include (ref_n, ref_details, ref_name)
go



--USERS

if exists(select * from sys.indexes 
where name='ix_tb_users_usr_username' and object_id = object_id('tb_users'))
begin
	drop index tb_users.ix_tb_users_usr_username
end
create nonclustered index ix_tb_users_usr_username on tb_users (usr_username)

go















if OBJECT_ID('usp_places_select') is not null
	drop procedure [usp_places_select]
go
create procedure [dbo].[usp_places_select]
@pla_id			int = null
as
begin
if @pla_id is not null
	select	pla_id,
			pla_date_created,
			pla_time_created,
			pla_date_created_text = convert(varchar(800), convert(date,pla_date_created)), 
			usr_id_created,
			pla_name,
			(pla_name + ' (' + convert(varchar(800),pla_id) + ')') 'name_id',
			pla_location_reference,
			pla_campus,
			pla_details,
			(select count(*) from tb_places_samples where pla_id = p.pla_id) ps_count
	from [dbo].[tb_places] as p
	where pla_id = @pla_id
	order by pla_id desc
else
	select	pla_id,
			pla_date_created,
			pla_time_created,
			pla_date_created_text = convert(varchar(800), convert(date,pla_date_created)), 
			usr_id_created,
			pla_name,
			(pla_name + ' (' + convert(varchar(800),pla_id) + ')') 'name_id',
			pla_location_reference,
			pla_campus,
			pla_details,
			(select count(*) from tb_places_samples where pla_id = p.pla_id) ps_count
	from [dbo].[tb_places] as p
	order by pla_id desc
end
go




if OBJECT_ID('usp_places_insert') is not null
	drop procedure usp_places_insert
go
create procedure usp_places_insert
@usr_id_created				int = null,
@pla_name					varchar(800) = null,
@pla_location_reference		varchar(800) = null,
@pla_campus					varchar(800) = null,
@pla_details				varchar(max) = null,
@usr_id_audit				int = null,
@ssn_id						int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 


	insert into tb_places (usr_id_created,pla_name,pla_location_reference,pla_campus,pla_details)
	values (@usr_id_created,@pla_name,@pla_location_reference,@pla_campus,@pla_details)
end
go




if OBJECT_ID('usp_places_delete') is not null
	drop procedure usp_places_delete
go
create procedure usp_places_delete
@pla_id				int,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	delete from tb_places where pla_id = @pla_id
end
go








if OBJECT_ID('usp_places_samples_select') is not null
	drop procedure usp_places_samples_select
go
create procedure [dbo].usp_places_samples_select
@type				int = 1,
@pla_id				int = null,
@ps_id				int = null,
@ps_barcode			varchar(800) = null,
@psres_result		varchar(800) = null,
@date_start			date = null,
@date_end			date = null,
@ps_id_list			varchar(max) = null
as
if @type = 1
begin
	select	ps_id	as ps_id,
			ps_barcode	as ps_barcode,
			pla_id	as pla_id,
			(select pla_name from tb_places p where p.pla_id = [ps].pla_id) as pla_name,
			(select pla_location_reference from tb_places p where p.pla_id = [ps].pla_id) as pla_location_reference,
			(select pla_campus from tb_places p where p.pla_id = [ps].pla_id) as pla_campus,
			(select pla_details from tb_places p where p.pla_id = [ps].pla_id) as pla_details,
			ps_date_created as ps_date_created,
			ps_time_created as ps_time_created,
			convert(varchar(800),convert(date,ps_date_created)) as ps_date_created_text,
			usr_id_created as usr_id_created,
			ps_date_collected as ps_date_collected,
			ps_time_collected as ps_time_collected,
			convert(varchar(800),convert(date,ps_date_collected)) as ps_date_collected_text,
			usr_id_collected as usr_id_collected,
			ps_date_registered as ps_date_registered,
			ps_time_registered as ps_time_registered,
			convert(varchar(800),convert(date,ps_date_registered)) as ps_date_registered_text,
			usr_id_registered as usr_id_registered,
			ps_well_number as ps_well_number,
			ps_details as ps_details,
			(select top 1 psres_result from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_result,
			(select top 1 psres_ct_value from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_ct_value,
			(select count(*) samples_count from [tb_places_samples] [ps2] where [ps2].ps_id = [ps].ps_id) as samples_count,
			ROW_NUMBER() over (order by ps_date_created asc) as position
	from [dbo].[tb_places_samples] [ps] with (index = ix_tb_places_samples_ps_date_created)
	where ps_date_created >= '2000-01-01'
	order by ps_date_created desc, ps_time_created desc
end
if @type = 2 
begin
	select	ps_id	as ps_id,
			ps_barcode	as ps_barcode,
			pla_id	as pla_id,
			(select pla_name from tb_places p where p.pla_id = [ps].pla_id) as pla_name,
			(select pla_location_reference from tb_places p where p.pla_id = [ps].pla_id) as pla_location_reference,
			(select pla_campus from tb_places p where p.pla_id = [ps].pla_id) as pla_campus,
			(select pla_details from tb_places p where p.pla_id = [ps].pla_id) as pla_details,
			ps_date_created as ps_date_created,
			ps_time_created as ps_time_created,
			convert(varchar(800),convert(date,ps_date_created)) as ps_date_created_text,
			usr_id_created as usr_id_created,
			ps_date_collected as ps_date_collected,
			ps_time_collected as ps_time_collected,
			convert(varchar(800),convert(date,ps_date_collected)) as ps_date_collected_text,
			usr_id_collected as usr_id_collected,
			ps_date_registered as ps_date_registered,
			ps_time_registered as ps_time_registered,
			convert(varchar(800),convert(date,ps_date_registered)) as ps_date_registered_text,
			usr_id_registered as usr_id_registered,
			ps_well_number as ps_well_number,
			ps_details as ps_details,
			(select top 1 psres_result from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_result,
			(select top 1 psres_ct_value from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_ct_value,
			(select count(*) samples_count from [tb_places_samples] [ps2] where [ps2].ps_id = [ps].ps_id) as samples_count,
			ROW_NUMBER() over (order by ps_date_created asc) as position
	from [dbo].[tb_places_samples] [ps] with (index = ix_tb_places_samples_ps_date_created)
	where pla_id = @pla_id
	order by ps_date_created desc, ps_time_created desc
end
else if @type = 3
begin
	select	ps_id	as ps_id,
			ps_barcode	as ps_barcode,
			pla_id	as pla_id,
			(select pla_name from tb_places p where p.pla_id = [ps].pla_id) as pla_name,
			(select pla_location_reference from tb_places p where p.pla_id = [ps].pla_id) as pla_location_reference,
			(select pla_campus from tb_places p where p.pla_id = [ps].pla_id) as pla_campus,
			(select pla_details from tb_places p where p.pla_id = [ps].pla_id) as pla_details,
			ps_date_created as ps_date_created,
			ps_time_created as ps_time_created,
			convert(varchar(800),convert(date,ps_date_created)) as ps_date_created_text,
			usr_id_created as usr_id_created,
			ps_date_collected as ps_date_collected,
			ps_time_collected as ps_time_collected,
			convert(varchar(800),convert(date,ps_date_collected)) as ps_date_collected_text,
			usr_id_collected as usr_id_collected,
			ps_date_registered as ps_date_registered,
			ps_time_registered as ps_time_registered,
			convert(varchar(800),convert(date,ps_date_registered)) as ps_date_registered_text,
			usr_id_registered as usr_id_registered,
			ps_well_number as ps_well_number,
			ps_details as ps_details,
			(select top 1 psres_result from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_result,
			(select top 1 psres_ct_value from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_ct_value,
			(select count(*) samples_count from [tb_places_samples] [ps2] where [ps2].ps_id = [ps].ps_id) as samples_count,
			ROW_NUMBER() over (order by ps_date_created asc) as position
	from [dbo].[tb_places_samples] [ps] 
	where ps_id = @ps_id
	order by ps_date_created desc, ps_time_created desc
end
else if @type = 4
begin
	select	ps_id	as ps_id,
			ps_barcode	as ps_barcode,
			pla_id	as pla_id,
			(select pla_name from tb_places p where p.pla_id = [ps].pla_id) as pla_name,
			(select pla_location_reference from tb_places p where p.pla_id = [ps].pla_id) as pla_location_reference,
			(select pla_campus from tb_places p where p.pla_id = [ps].pla_id) as pla_campus,
			(select pla_details from tb_places p where p.pla_id = [ps].pla_id) as pla_details,
			ps_date_created as ps_date_created,
			ps_time_created as ps_time_created,
			convert(varchar(800),convert(date,ps_date_created)) as ps_date_created_text,
			usr_id_created as usr_id_created,
			ps_date_collected as ps_date_collected,
			ps_time_collected as ps_time_collected,
			convert(varchar(800),convert(date,ps_date_collected)) as ps_date_collected_text,
			usr_id_collected as usr_id_collected,
			ps_date_registered as ps_date_registered,
			ps_time_registered as ps_time_registered,
			convert(varchar(800),convert(date,ps_date_registered)) as ps_date_registered_text,
			usr_id_registered as usr_id_registered,
			ps_well_number as ps_well_number,
			ps_details as ps_details,
			(select top 1 psres_result from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_result,
			(select top 1 psres_ct_value from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_ct_value,
			(select count(*) samples_count from [tb_places_samples] [ps2] where [ps2].ps_id = [ps].ps_id) as samples_count,
			ROW_NUMBER() over (order by ps_date_created asc) as position
	from [dbo].[tb_places_samples] [ps]  with (index = ix_tb_places_samples_ps_barcode)
	where ps_barcode = @ps_barcode
	order by ps_date_created desc, ps_time_created desc
end
else if @type = 6
begin
	declare @table	table	(
			ps_id									int,
			ps_barcode								varchar(800),
			pla_id									int,
			pla_name								varchar(800),
			pla_location_reference					varchar(800),
			pla_campus								varchar(800),
			pla_details								varchar(800),
			ps_date_created							date,
			ps_time_created							time,
			ps_date_created_text					varchar(800),
			usr_id_created							int,
			ps_date_collected						date,
			ps_time_collected						time,
			ps_date_collected_text					varchar(800),
			usr_id_collected						int,
			ps_date_registered						date,
			ps_time_registered						time,
			ps_date_registered_text					varchar(800),
			usr_id_registered						int,
			ps_well_number							varchar(800),
			ps_details								varchar(max),
			psres_result							varchar(800),
			psres_ct_value							varchar(800),
			samples_count							int,
			position								int
	)

	insert into @table
	select	ps_id	as ps_id,
			ps_barcode	as ps_barcode,
			pla_id	as pla_id,
			(select pla_name from tb_places p where p.pla_id = [ps].pla_id) as pla_name,
			(select pla_location_reference from tb_places p where p.pla_id = [ps].pla_id) as pla_location_reference,
			(select pla_campus from tb_places p where p.pla_id = [ps].pla_id) as pla_campus,
			(select pla_details from tb_places p where p.pla_id = [ps].pla_id) as pla_details,
			ps_date_created as ps_date_created,
			ps_time_created as ps_time_created,
			convert(varchar(800),convert(date,ps_date_created)) as ps_date_created_text,
			usr_id_created as usr_id_created,
			ps_date_collected as ps_date_collected,
			ps_time_collected as ps_time_collected,
			convert(varchar(800),convert(date,ps_date_collected)) as ps_date_collected_text,
			usr_id_collected as usr_id_collected,
			ps_date_registered as ps_date_registered,
			ps_time_registered as ps_time_registered,
			convert(varchar(800),convert(date,ps_date_registered)) as ps_date_registered_text,
			usr_id_registered as usr_id_registered,
			ps_well_number as ps_well_number,
			ps_details as ps_details,
			(select top 1 psres_result from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_result,
			(select top 1 psres_ct_value from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_ct_value,
			(select count(*) samples_count from [tb_places_samples] [ps2] where [ps2].ps_id = [ps].ps_id) as samples_count,
			ROW_NUMBER() over (order by ps_date_created asc) as position
	from [dbo].[tb_places_samples] [ps]  with (index = ix_tb_places_samples_ps_barcode)
	where ps_date_created >= @date_start and ps_date_created <= @date_end
	order by ps_date_created desc, ps_time_created desc

	if @psres_result = 'P'
	begin
		delete from @table where not psres_result = 'P' or psres_result is null or psres_result = '' 
	end
	else if @psres_result = 'N'
	begin
		delete from @table where not psres_result = 'N' or psres_result is null or psres_result = ''
	end
	else if @psres_result = 'U'
	begin
		delete from @table where not (psres_result is null or psres_result = '')
	end

	select * from @table

end
else if @type = 7
begin

	declare @list table(ps_id varchar(800))

	while len(@ps_id_list) > 0
		begin
			insert into @list(ps_id)
			select left(@ps_id_list, charindex(',', @ps_id_list+',') -1) as ps_id
    
			set @ps_id_list = stuff(@ps_id_list, 1, charindex(',', @ps_id_list + ','), '')
		end


	select	ps_id	as ps_id,
			ps_barcode	as ps_barcode,
			pla_id	as pla_id,
			(select pla_name from tb_places p where p.pla_id = [ps].pla_id) as pla_name,
			(select pla_location_reference from tb_places p where p.pla_id = [ps].pla_id) as pla_location_reference,
			(select pla_campus from tb_places p where p.pla_id = [ps].pla_id) as pla_campus,
			(select pla_details from tb_places p where p.pla_id = [ps].pla_id) as pla_details,
			usr_id_created as usr_id_created,
			ps_date_collected as ps_date_collected,
			ps_time_collected as ps_time_collected,
			convert(varchar(800),convert(date,ps_date_collected)) as ps_date_collected_text,
			usr_id_collected as usr_id_collected,
			ps_date_registered as ps_date_registered,
			ps_time_registered as ps_time_registered,
			convert(varchar(800),convert(date,ps_date_registered)) as ps_date_registered_text,
			usr_id_registered as usr_id_registered,
			ps_well_number as ps_well_number,
			ps_details as ps_details,
			(select top 1 psres_result from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_result,
			(select top 1 psres_ct_value from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_ct_value,
			(select count(*) samples_count from [tb_places_samples] [ps2] where [ps2].ps_id = [ps].ps_id) as samples_count,
			ROW_NUMBER() over (order by ps_date_created asc) as position
	from [dbo].[tb_places_samples] [ps] 
	where ps_id in (select ps_id from @list) 
	order by ps_date_created desc, ps_time_created desc
end
if @type = 8
begin
	select	top 500
			ps_id	as ps_id,
			ps_barcode	as ps_barcode,
			pla_id	as pla_id,
			(select pla_name from tb_places p where p.pla_id = [ps].pla_id) as pla_name,
			(select pla_location_reference from tb_places p where p.pla_id = [ps].pla_id) as pla_location_reference,
			(select pla_campus from tb_places p where p.pla_id = [ps].pla_id) as pla_campus,
			(select pla_details from tb_places p where p.pla_id = [ps].pla_id) as pla_details,
			usr_id_created as usr_id_created,
			ps_date_collected as ps_date_collected,
			ps_time_collected as ps_time_collected,
			convert(varchar(800),convert(date,ps_date_collected)) as ps_date_collected_text,
			usr_id_collected as usr_id_collected,
			ps_date_registered as ps_date_registered,
			ps_time_registered as ps_time_registered,
			convert(varchar(800),convert(date,ps_date_registered)) as ps_date_registered_text,
			usr_id_registered as usr_id_registered,
			ps_well_number as ps_well_number,
			ps_details as ps_details,
			(select top 1 psres_result from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_result,
			(select top 1 psres_ct_value from tb_places_samples_results psres where psres.ps_id = [ps].ps_id order by psres_date_result desc, psres_time_result desc) as psres_ct_value,
			(select count(*) samples_count from [tb_places_samples] [ps2] where [ps2].ps_id = [ps].ps_id) as samples_count,
			ROW_NUMBER() over (order by ps_date_created asc) as position
	from [dbo].[tb_places_samples] [ps] with (index = ix_tb_places_samples_ps_date_created)
	where ps_date_created >= '2000-01-01'
	order by ps_date_created desc, ps_time_created desc
end

go



if OBJECT_ID('usp_places_update') is not null
	drop procedure usp_places_update
go
create procedure usp_places_update
@pla_id						int = null,
@pla_name					varchar(800) = null,
@pla_location_reference		varchar(800) = null,
@pla_campus					varchar(800) = null,
@pla_details				varchar(max) = null,
@usr_id_audit				int = null,
@ssn_id						int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	update tb_places 
	set		pla_name = @pla_name,
			pla_location_reference = @pla_location_reference,
			pla_campus = @pla_campus,
			pla_details = @pla_details
	where pla_id = @pla_id
end
go


if OBJECT_ID('usp_places_samples_insert') is not null
	drop procedure usp_places_samples_insert
go
create procedure usp_places_samples_insert
@usr_id_created		int = null,
@ps_date_collected	date = null,
@ps_time_collected	time = null,
@usr_id_collected	int = null,
@pla_id				int = null,
@ps_details			varchar(max) = null,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	insert into tb_places_samples (usr_id_created,ps_date_collected,ps_time_collected,usr_id_collected,pla_id,ps_details)
	values (@usr_id_created, isnull(@ps_date_collected,dbo.udf_getdatelocal(default)), isnull(@ps_time_collected,dbo.udf_getdatelocal(default)), @usr_id_collected, @pla_id, @ps_details)

	select IDENT_CURRENT('tb_places_samples') as ps_id
end
go


if OBJECT_ID('usp_places_samples_update') is not null
	drop procedure usp_places_samples_update
go
create procedure usp_places_samples_update
@type						int = null,
@ps_id						int = null,
@ps_date_collected			date = null,
@ps_time_collected			time = null,
@usr_id_collected			int = null,
@ps_date_registered			date = null,
@ps_time_registered			time = null,
@usr_id_registered			int = null,
@pla_id						int = null,
@ps_well_number				varchar(800) = null,
@ps_details					varchar(max) = null,
@ps_barcode					varchar(800) = null,
@operation					int = null,
@usr_id_audit				int = null,
@ssn_id						int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id 

	if	@type = 1 
	begin
			update	tb_places_samples 
			set		ps_date_collected = @ps_date_collected,
					ps_time_collected  = @ps_time_collected,
					usr_id_collected = @usr_id_collected,
					ps_date_registered = @ps_date_registered,
					ps_time_registered  = case when @ps_time_registered = '' then dbo.udf_getdatelocal(default) else @ps_time_registered end ,
					usr_id_registered = case when @usr_id_registered = '' then @usr_id_audit else @usr_id_registered end,
					pla_id = @pla_id,
					ps_well_number	= @ps_well_number,
					ps_details	= @ps_details
			where ps_id = @ps_id
	end
	if	@type = 3 --register barcode
	begin
		update tb_places_samples
		set		ps_date_registered = dbo.udf_getdatelocal(default),
				ps_time_registered = dbo.udf_getdatelocal(default),
				usr_id_registered = @usr_id_registered
		where ps_id = @ps_id
	end
	if	@type = 4
	begin
		update tb_places_samples
		set		ps_well_number = (case when (@ps_well_number = '') then null else @ps_well_number end)
		where ps_id = @ps_id
	end
end
go


if OBJECT_ID('usp_places_samples_delete') is not null
	drop procedure usp_places_samples_delete
go
create procedure usp_places_samples_delete
@ps_id				int,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id  

	delete from tb_places_samples where ps_id = @ps_id
end
go




if OBJECT_ID('usp_places_samples_results_update') is not null
	drop procedure usp_places_samples_results_update
go
create procedure usp_places_samples_results_update
@type				int = null,
@ps_id				int = null,
@psres_value		varchar(800) = null, 
@usr_id				int = null,
@usr_id_audit		int = null,
@ssn_id				int = null
as
begin
	exec sp_set_session_context @key = N'usr_id_audit', @value = @usr_id_audit 
	exec sp_set_session_context @key = N'ssn_id', @value = @ssn_id  

		if @type = 1
		begin
	
			if exists (select * from tb_places_samples_results where ps_id= @ps_id)
			begin
				update	tb_places_samples_results
				set		psres_ct_value = (case when (@psres_value = '') then null else @psres_value end),
						psres_date_ct_value = dbo.udf_getdatelocal(default),
						psres_time_ct_value = dbo.udf_getdatelocal(default),
						usr_id_ct_value = @usr_id
				where	ps_id = @ps_id
			end
			else 
			begin
				insert into	tb_places_samples_results (ps_id, psres_date_result, psres_time_result, usr_id_created, psres_ct_value, usr_id_ct_value)
				values		(@ps_id,dbo.udf_getdatelocal(default), dbo.udf_getdatelocal(default), @usr_id,(case when (@psres_value = '') then null else @psres_value end), @usr_id)
			end

		end
		else if @type = 2
		begin

			if exists (select * from tb_places_samples_results where ps_id= @ps_id)
			begin
				update	tb_places_samples_results
				set		psres_result = (case when (@psres_value = '') then null else @psres_value end),
						psres_date_result = dbo.udf_getdatelocal(default),
						psres_time_result = dbo.udf_getdatelocal(default),
						usr_id_result = @usr_id
				where	ps_id = @ps_id
			end
			else 
			begin
				insert into	tb_places_samples_results (ps_id, psres_date_result, psres_time_result, usr_id_created, psres_result, usr_id_result)
				values		(@ps_id,dbo.udf_getdatelocal(default), dbo.udf_getdatelocal(default), @usr_id,(case when (@psres_value = '') then null else @psres_value end), @usr_id)
			end
		end
end
go



--AUDITS
--exec usp_audit_individuals_select @type =  1
--exec usp_audit_individuals_select @type =  2, @date_start = '2020-08-30', @date_end = '2020-09-15', @poo_id = 3
if OBJECT_ID('usp_audit_individuals_select') is not null
	drop procedure usp_audit_individuals_select
go
create procedure usp_audit_individuals_select
@type			int = 1,
@date_start		date = null,
@date_end		date = null,
@poo_id			int = null,
@pr_result		varchar(800) = null
as
begin
		declare @tabla table	(
			aud_id								int,	
			aud_operation_id					int,
			aud_operation						varchar(800),
			aud_date							date,
			aud_time							time, 
			aud_table							varchar(800),
			aud_poo_id							int,
			aud_identifier_id					varchar(800), 
			aud_identifier_field				varchar(800),
			aud_field							varchar(800),
			aud_before							varchar(800),
			aud_after							varchar(800),
			usr_id_audit						int,
			usr_username						varchar(800),
			ssn_id								int
		)


	if @type = 1
	begin
		insert		@tabla
		select		aud_id,	
					aud_operation_id,
					aud_operation,
					aud_date,
					aud_time, 
					aud_table,
					(select poo_id from tb_pools_results pr where pr_id = aud.aud_identifier_id and aud_identifier_field = 'pr_id') aud_poo_id,
					aud_identifier_id, 
					aud_identifier_field,
					aud_field,
					aud_before,
					aud_after,
					usr_id_audit,
					(select usr_username from tb_users where usr_id = aud.usr_id_audit) usr_username,
					ssn_id
		from tb_audit aud 
		where aud_table = 'tb_pools_results' and aud_field in ('pr_ct_value','pr_result')
		order by aud_id asc

		select * from @tabla
	end
	if @type = 2	
	begin
		insert		@tabla
		select		aud_id,	
					aud_operation_id,
					aud_operation,
					aud_date,
					aud_time, 
					aud_table,
					(select poo_id from tb_pools_results pr where pr_id = aud.aud_identifier_id and aud_identifier_field = 'pr_id') aud_poo_id,
					aud_identifier_id, 
					aud_identifier_field,
					aud_field,
					aud_before,
					aud_after,
					usr_id_audit,
					(select usr_username from tb_users where usr_id = aud.usr_id_audit) usr_username,
					ssn_id
		from tb_audit aud 
		where aud_date >= @date_start and aud_date <= @date_end 
		and aud_table = 'tb_pools_results' and aud_field in ('pr_ct_value','pr_result')
		order by aud_id asc

		if @poo_id > 0
		begin
			delete from @tabla where not (aud_poo_id = @poo_id) 
		end

		if @pr_result = 'P'
		begin
			delete from @tabla where not (aud_field = 'pr_result' and (aud_after = 'P' and aud_after is not null))  
		end
		else if @pr_result = 'N'
		begin
			delete from @tabla where not (aud_field = 'pr_result' and (aud_after = 'N' and aud_after is not null))  
		end
		else if @pr_result = 'U'
		begin
			delete from @tabla where not (aud_field = 'pr_result' and (aud_after is null or aud_after = ''))  
		end
		select * from @tabla
	end
end

go


drop trigger if exists tr_tb_places
go
create trigger tr_tb_places
on tb_places
after insert, update, delete
as
set nocount on
begin
		declare @operation_id nvarchar(800)
		declare @tablename nvarchar(800)
		declare @columnname nvarchar(800)
		declare @columnname_first nvarchar(800)
		declare @tablecolumnsinserted int = 1
		declare @tablerowsinserted int = 1
		declare @tablecolumnsdeleted int = 1
		declare @tablerowsdeleted int = 1
		declare @contadorfilas int = 1
		declare @contadorcolumnas int = 1
		declare @id_value nvarchar(max)
		declare @query_id_value nvarchar(max)
		declare @column_value_after nvarchar(max)
		declare @query_column_value_after nvarchar(max)
		declare @column_value_before nvarchar(max)
		declare @query_column_value_before nvarchar(max)
		declare @usr_id_audit int
		declare @ssn_id int

		select @operation_id = isnull(max(aud_operation_id),0) + 1 from tb_audit 

		select @usr_id_audit = cast(SESSION_CONTEXT(N'usr_id_audit') as int)
		select @ssn_id = cast(SESSION_CONTEXT(N'ssn_id') as int)		

	if exists (select * from inserted) and not exists (select * from deleted)
	begin

		select * into #inserted_i from inserted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if (@column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id, aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'INSERT',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, null, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_i 

	end
	else if exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #inserted_u from inserted
		select * into #deleted_u from deleted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output


					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if @column_value_before != @column_value_after or (@column_value_before is not null and @column_value_after is null) or (@column_value_before is null and @column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit,ssn_id)
						select HOST_NAME(),@operation_id,'UPDATE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_u
		drop table #deleted_u

	end
	else if not exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #deleted_d from deleted
		
		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsdeleted= COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsdeleted = COUNT(*) 
		from deleted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsdeleted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsdeleted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output

					if (@column_value_before is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'DELETE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, null, @usr_id_audit, @ssn_id
					end
					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #deleted_d 

	end
end
go




drop trigger if exists tr_tb_places_samples
go
create trigger tr_tb_places_samples
on tb_places_samples
after insert, update, delete
as
set nocount on
begin
		declare @operation_id nvarchar(800)
		declare @tablename nvarchar(800)
		declare @columnname nvarchar(800)
		declare @columnname_first nvarchar(800)
		declare @tablecolumnsinserted int = 1
		declare @tablerowsinserted int = 1
		declare @tablecolumnsdeleted int = 1
		declare @tablerowsdeleted int = 1
		declare @contadorfilas int = 1
		declare @contadorcolumnas int = 1
		declare @id_value nvarchar(max)
		declare @query_id_value nvarchar(max)
		declare @column_value_after nvarchar(max)
		declare @query_column_value_after nvarchar(max)
		declare @column_value_before nvarchar(max)
		declare @query_column_value_before nvarchar(max)
		declare @usr_id_audit int
		declare @ssn_id int

		select @operation_id = isnull(max(aud_operation_id),0) + 1 from tb_audit 

		select @usr_id_audit = cast(SESSION_CONTEXT(N'usr_id_audit') as int)
		select @ssn_id = cast(SESSION_CONTEXT(N'ssn_id') as int)		

	if exists (select * from inserted) and not exists (select * from deleted)
	begin

		select * into #inserted_i from inserted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if (@column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id, aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'INSERT',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, null, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_i 

	end
	else if exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #inserted_u from inserted
		select * into #deleted_u from deleted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output


					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if @column_value_before != @column_value_after or (@column_value_before is not null and @column_value_after is null) or (@column_value_before is null and @column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit,ssn_id)
						select HOST_NAME(),@operation_id,'UPDATE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_u
		drop table #deleted_u

	end
	else if not exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #deleted_d from deleted
		
		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsdeleted= COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsdeleted = COUNT(*) 
		from deleted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsdeleted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsdeleted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output

					if (@column_value_before is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'DELETE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, null, @usr_id_audit, @ssn_id
					end
					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #deleted_d 

	end
end
go




drop trigger if exists tr_tb_places_samples_results
go
create trigger tr_tb_places_samples_results
on tb_places_samples_results
after insert, update, delete
as
set nocount on
begin
		declare @operation_id nvarchar(800)
		declare @tablename nvarchar(800)
		declare @columnname nvarchar(800)
		declare @columnname_first nvarchar(800)
		declare @tablecolumnsinserted int = 1
		declare @tablerowsinserted int = 1
		declare @tablecolumnsdeleted int = 1
		declare @tablerowsdeleted int = 1
		declare @contadorfilas int = 1
		declare @contadorcolumnas int = 1
		declare @id_value nvarchar(max)
		declare @query_id_value nvarchar(max)
		declare @column_value_after nvarchar(max)
		declare @query_column_value_after nvarchar(max)
		declare @column_value_before nvarchar(max)
		declare @query_column_value_before nvarchar(max)
		declare @usr_id_audit int
		declare @ssn_id int

		select @operation_id = isnull(max(aud_operation_id),0) + 1 from tb_audit 

		select @usr_id_audit = cast(SESSION_CONTEXT(N'usr_id_audit') as int)
		select @ssn_id = cast(SESSION_CONTEXT(N'ssn_id') as int)		

	if exists (select * from inserted) and not exists (select * from deleted)
	begin

		select * into #inserted_i from inserted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_i) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if (@column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id, aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'INSERT',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, null, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_i 

	end
	else if exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #inserted_u from inserted
		select * into #deleted_u from deleted

		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsinserted = COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsinserted = COUNT(*) 
		from inserted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsinserted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u ) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsinserted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output


					set @query_column_value_after = 'select @column_value_after_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #inserted_u) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_after, N'@column_value_after_out nvarchar(800) output', @column_value_after_out = @column_value_after output

					if @column_value_before != @column_value_after or (@column_value_before is not null and @column_value_after is null) or (@column_value_before is null and @column_value_after is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit,ssn_id)
						select HOST_NAME(),@operation_id,'UPDATE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, @column_value_after, @usr_id_audit, @ssn_id
					end

					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #inserted_u
		drop table #deleted_u

	end
	else if not exists (select * from inserted) and exists (select * from deleted)
	begin

		select * into #deleted_d from deleted
		
		select @tablename = OBJECT_NAME(parent_object_id) 
		FROM sys.objects 
		WHERE sys.objects.name = OBJECT_NAME(@@PROCID)

		select @tablecolumnsdeleted= COUNT(*) 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename

		select @tablerowsdeleted = COUNT(*) 
		from deleted

		select @columnname_first = COLUMN_NAME 
		from INFORMATION_SCHEMA.COLUMNS 
		where TABLE_NAME = @tablename and ORDINAL_POSITION = 1

		set @contadorfilas = 1
		while @contadorfilas <= @tablerowsdeleted
		begin
				set @query_id_value = 'select @id_value_out = ' + @columnname_first  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
				exec sp_executesql @query_id_value, N'@id_value_out nvarchar(800) output', @id_value_out = @id_value output

				set @contadorcolumnas = 1
				while @contadorcolumnas <= @tablecolumnsdeleted
				begin
					select @columnname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tablename and ORDINAL_POSITION = @contadorcolumnas 
			
					set @query_column_value_before = 'select @column_value_before_out = ' + @columnname  + ' from (select *, (row_number() over (order by ' + @columnname_first  + ')) pos from #deleted_d) a where pos = ' + convert(varchar(800),@contadorfilas) 
					exec sp_executesql @query_column_value_before, N'@column_value_before_out nvarchar(800) output', @column_value_before_out = @column_value_before output

					if (@column_value_before is not null)
					begin
						insert into tb_audit (aud_station,aud_operation_id,aud_operation,aud_date,aud_time, aud_user, aud_table, aud_identifier_id, aud_identifier_field, aud_field, aud_before, aud_after, usr_id_audit, ssn_id)
						select HOST_NAME(),@operation_id,'DELETE',dbo.udf_getdatelocal(default),dbo.udf_getdatelocal(default),SYSTEM_USER,@tablename, @id_value, @columnname_first, @columnname, @column_value_before, null, @usr_id_audit, @ssn_id
					end
					set @contadorcolumnas = @contadorcolumnas + 1
				end
			set @contadorfilas = @contadorfilas + 1
		end
		drop table #deleted_d 

	end
end
go




--INDEXES

--INDIVIDUALS


--INDIVIDUALS SAMPLES

if exists(select * from sys.indexes 
where name='ix_tb_places_samples_pla_id' and object_id = object_id('tb_places_samples'))
begin
	drop index tb_places_samples.ix_tb_places_samples_pla_id
end
create nonclustered index ix_tb_places_samples_pla_id on tb_places_samples (pla_id, ps_date_created, ps_time_created)
include (	ps_barcode, usr_id_created, ps_date_collected, 
			ps_time_collected, usr_id_collected, ps_date_registered, ps_time_registered, usr_id_registered,
			ps_well_number, ps_details)
go



if exists(select * from sys.indexes 
where name='ix_tb_places_samples_ps_barcode' and object_id = object_id('tb_places_samples'))
begin
	drop index tb_places_samples.ix_tb_places_samples_ps_barcode
end
create nonclustered index ix_tb_places_samples_ps_barcode on tb_places_samples (ps_barcode, ps_date_created, ps_time_created)
include (	pla_id , usr_id_created, ps_date_collected, 
			ps_time_collected, usr_id_collected, ps_date_registered, ps_time_registered, usr_id_registered,
			ps_well_number, ps_details)
go


if exists(select * from sys.indexes 
where name='ix_tb_places_samples_ps_date_created' and object_id = object_id('tb_places_samples'))
begin
	drop index tb_places_samples.ix_tb_places_samples_ps_date_created
end
create nonclustered index ix_tb_places_samples_ps_date_created on tb_places_samples (ps_date_created, ps_time_created, ps_id)
include (	ps_barcode, pla_id, usr_id_created, ps_date_collected, 
			ps_time_collected, usr_id_collected, ps_date_registered, ps_time_registered, usr_id_registered,
			ps_well_number, ps_details)
go


--BARCODE PRINTING

if object_id('tb_individuals_barcode_printing') is not null
	drop table tb_individuals_barcode_printing
go
create table tb_individuals_barcode_printing (
	ibp_id				int					identity,
	ibp_date_created	date				default dbo.udf_getdatelocal(default),
	ibp_time_created	time				default dbo.udf_getdatelocal(default),
	is_id				int,
	ssn_id				int
)
go

if OBJECT_ID('usp_individuals_barcode_insert') is not null
	drop procedure usp_individuals_barcode_insert
go
create procedure [dbo].usp_individuals_barcode_insert
@type				int = null,
@std_id				int = null,
@ssn_id				int = null
as
begin
			delete from tb_individuals_barcode_generation
			where datediff(day, ibg_date_created ,dbo.udf_getdatelocal(default)) >= 1

			if @type = 1
			begin
				if not exists (select * from tb_individuals_barcode_generation where std_id = @std_id and ssn_id = @ssn_id) and exists (select * from tb_individuals where std_id = @std_id)
				begin
					insert into tb_individuals_barcode_generation (std_id, ssn_id)
					values (@std_id,@ssn_id)
				end
			end
			if @type = 2
			begin
				delete from tb_individuals_barcode_generation
				where std_id = @std_id and ssn_id = @ssn_id
			end
	end
go



if OBJECT_ID('usp_individuals_barcode_select') is not null
	drop procedure usp_individuals_barcode_select
go
create procedure [dbo].usp_individuals_barcode_select
@type				int = 1,
@ssn_id				int = null
as
begin
if @type = 1
	begin
			select	ibg.std_id, 
					ssn_id,
					ROW_NUMBER() over (order by ibg.std_id asc) as position
			from	tb_individuals_barcode_generation ibg 
			where ssn_id = @ssn_id
	end
else if @type = 2
	begin
		declare @is_id_list varchar(max) = ''
		declare @is_id int

			declare is_id_list cursor for select is_id from tb_individuals_barcode_printing ibp where ssn_id = @ssn_id
			open is_id_list
				fetch next from is_id_list into @is_id
				while @@fetch_status = 0 
				begin
					set @is_id_list = @is_id_list + convert(varchar(800),@is_id) + ','
					fetch next from is_id_list into @is_id
				end
			close is_id_list
			deallocate is_id_list

			set @is_id_list = left(@is_id_list, case when len(@is_id_list) > 0 then len(@is_id_list)-1 else 0 end) 


		select @is_id_list is_id_list
	end
end


--BARCODE GENERATION
go
if object_id('tb_individuals_barcode_generation') is not null
	drop table tb_individuals_barcode_generation
go
create table tb_individuals_barcode_generation(
	ibg_id int identity(1,1) NOT NULL,
	ibg_date_created date NULL default dbo.udf_getdatelocal(default),
	ibg_time_created time(7) NULL default dbo.udf_getdatelocal(default),
	std_id int NULL,
	ssn_id int NULL,
	is_id  int null,
	is_id_date_created date NULL,
	is_id_time_created time(7) NULL
)
go
