--select * from tb_individuals
--select * from tb_references
--[usp_individuals_select_with_stats] @ind_id = 1
if OBJECT_ID('usp_individuals_select_with_stats') is not null
	drop procedure usp_individuals_select_with_stats
go
create procedure [dbo].[usp_individuals_select_with_stats]
@ind_id			int = null
as
begin
declare @table		table	(		ind_id								int,
									ind_date_created					date,
									ind_time_created					time,
									ind_date_created_text				varchar(800),
									ind_first_name						varchar(800),
									ind_last_name						varchar(800),
									first_name_last_name				varchar(800),
									last_name_first_name_id				varchar(800),
									ind_email							varchar(800),
									ind_phone							varchar(800),
									ind_birthdate						date,
									ind_gender							varchar(800),
									ind_document						varchar(800),
									indcat_id							int,
									ref_id								int,
									ref_name							varchar(800),
									std_id								int,
									std_name							varchar(800),
									ind_details							varchar(max),
									is_count							int
							)		

insert @table (ind_id, ind_date_created, ind_time_created, ind_date_created_text, ind_first_name, ind_last_name, first_name_last_name, last_name_first_name_id, ind_email, ind_phone, ind_birthdate, ind_gender, ind_document, indcat_id, ref_id, ref_name, std_id, std_name, ind_details, is_count)
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
		indcat_id,
		ref_id,
		(select ref_name from tb_references where ref_id = i.ref_id) ref_name,
		std_id,
		(select std_name from tb_studies where std_id = i.std_id) std_name,
		ind_details,
		(select count(*) from tb_individuals_samples where ind_id = i.ind_id) is_count
from [dbo].[tb_individuals] as i
order by ind_id desc

if @ind_id is not null
	select * from @table where ind_id = @ind_id
else
	select * from @table 
end
go

--[usp_individuals_samples_select_barcode_print] @type = 1, @ind_id = 9
--[usp_individuals_samples_select_barcode_print] @type = 2, @is_id = 1
--[usp_individuals_samples_select_barcode_print] @type = 3, @is_barcode = 'A0000001'
--select * from [tb_individuals_samples] where ind_id = 1
--select * from [tb_individuals] where std_id = 342
--select * from tb_references where std_id = 342
--select * from tb_individuals_samples where std_id = 342
--usp_individuals_samples_select_with_stats @type = 0
--usp_individuals_samples_select_with_stats @type = 5, @date_start = '2020-08-01', @date_end = '2020-08-31' 

if OBJECT_ID('usp_individuals_samples_select_with_stats') is not null
	drop procedure usp_individuals_samples_select_with_stats
go
create procedure [dbo].usp_individuals_samples_select_with_stats
@type	int = 0,
@ind_id int = null,
@is_id int = null,
@is_barcode varchar(800) = null,
@poo_id int = null,
@date_start	date = null,
@date_end	date = null
as
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
	is_date_collected				date,
	is_time_collected				time,
	is_date_collected_text			varchar(800),
	is_date_registered				date,
	is_time_registered				time,
	is_date_registered_text			varchar(800),
	is_well_number					varchar(800),
	is_details						varchar(max),
	poo_id							int,
	poo_details						varchar(max),
	is_date_registered_pool			date,
	is_time_registered_pool			time,
	is_date_registered_pool_text	varchar(800),
	pr_result						varchar(800),
	pr_ct_value						varchar(800),
	samples_count					int
)
insert into @tabla
	select	is_id,
			is_barcode,
			ind_id,
			(select ind_first_name from tb_individuals i where i.ind_id = [is].ind_id) ind_first_name,
			(select ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) ind_last_name,
			(select ind_first_name + ' ' + ind_last_name from tb_individuals i where i.ind_id = [is].ind_id) 'first_name_last_name',
			(select ind_gender from tb_individuals i where i.ind_id = [is].ind_id) ind_gender,
			(select ind_document from tb_individuals i where i.ind_id = [is].ind_id) ind_document,
			(select ind_details from tb_individuals i where i.ind_id = [is].ind_id) ind_details,
			(select ref_id from tb_individuals i where i.ind_id = [is].ind_id) ref_id,
			(select (select ref_name from tb_references where ref_id = i.ref_id) from tb_individuals i where i.ind_id = [is].ind_id) ref_name,
			(select std_id from tb_individuals i where i.ind_id = [is].ind_id) std_id,
			(select (select std_name from tb_studies where std_id = i.std_id) from tb_individuals i where i.ind_id = [is].ind_id) std_name,
			is_date_created,
			is_time_created,
			convert(varchar(800),convert(date,is_date_created)),
			is_date_collected,
			is_time_collected,
			convert(varchar(800),convert(date,is_date_collected)),
			is_date_registered,
			is_time_registered,
			convert(varchar(800),convert(date,is_date_registered)),
			is_well_number,
			is_details,
			poo_id,
			(select poo_details from tb_pools where poo_id = [is].poo_id) poo_details,
			is_date_registered_pool,
			is_time_registered_pool,
			convert(varchar(800),convert(date,is_date_registered_pool)),
			(select top 1 pr_result from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) pr_result,
			(select top 1 pr_ct_value from tb_pools_results pr where pr.poo_id = [is].poo_id order by pr_date_result desc, pr_time_result desc) pr_ct_value,
			(select count(*) samples_count from [tb_individuals_samples] [is2] where [is2].ind_id = [is].ind_id) samples_count
	from [dbo].[tb_individuals_samples] [is]
	order by is_date_created desc, is_time_created desc

if @type = 0
begin
	select *, 
	ROW_NUMBER() over (order by is_date_created asc) position
	from @tabla 
	order by is_date_created desc, is_time_created desc
end
if @type = 1 
begin
	select *, 
	ROW_NUMBER() over (order by is_date_created asc) position
	from @tabla where ind_id = @ind_id 
	order by is_date_created desc, is_time_created desc
end
else if @type = 2
begin
	select *, 
	ROW_NUMBER() over (order by is_date_created asc) position
	from @tabla where is_id = @is_id 
	order by is_date_created desc, is_time_created desc
end
else if @type = 3
begin
	select *, 
	ROW_NUMBER() over (order by is_date_created asc) position
	from @tabla where is_barcode = @is_barcode 
	order by is_date_created desc, is_time_created desc
end
else if @type = 4
begin
	select *, 
	ROW_NUMBER() over (order by is_date_created asc) position
	from @tabla where poo_id = @poo_id 
	order by is_date_registered_pool desc, is_time_registered_pool desc
end
else if @type = 5
begin
	select *, 
	ROW_NUMBER() over (order by is_date_created asc) position
	from @tabla t where is_date_created >= @date_start and is_date_created <= @date_end
	order by is_id asc
end
go


--alter procedure usp_individuals_samples_select_all_with_stats
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
	order by ref_name asc
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
	order by ref_name asc
end
else if @type = 3
begin
	select	ref_id,
			ref_n,
			ref_name, 
			ref_details
	from [dbo].tb_references 
	where	ref_id = @ref_id
	order by ref_name asc
end
go


if OBJECT_ID('usp_studies_select') is not null
	drop procedure usp_studies_select
go
--usp_studies_select_all @type = 2, @std_id = 1
--select * from tb_individuals_samples
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
	order by std_name asc
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
	order by std_name asc
end
else if @type = 3
begin
	select	std_id,
			std_n,
			std_name, 
			std_details
	from [dbo].tb_studies 
	where	std_id = @std_id
	order by std_name asc
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

if OBJECT_ID('usp_individuals_samples_register_barcode') is not null
	drop procedure usp_individuals_samples_register_barcode
go
create procedure usp_individuals_samples_register_barcode
@is_id		int
as
update tb_individuals_samples
set		is_date_registered = getdate(),
		is_time_registered = getdate()
where is_id = @is_id
go

--select * from tb_individuals_samples
--select * from tb_pools_results
if OBJECT_ID('usp_individuals_samples_update_well_number') is not null
	drop procedure usp_individuals_samples_update_well_number
go
create procedure usp_individuals_samples_update_well_number
@is_id			int,
@is_well_number	varchar(800)= null
as
update tb_individuals_samples
set		is_well_number = (case when (@is_well_number = '') then null else @is_well_number end)
where is_id = @is_id
go


if OBJECT_ID('usp_pools_select_with_stats') is not null
	drop procedure usp_pools_select_with_stats
go
create procedure [dbo].[usp_pools_select_with_stats]
@type	int = 0,
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

if @type = 0 
begin
	select * from @tabla order by poo_date_created desc, poo_time_created desc
end
if @type = 1 
begin
	select * from @tabla where poo_id = @poo_id order by poo_date_created desc, poo_time_created desc
end
go



if OBJECT_ID('usp_individuals_samples_update_pool_id') is not null
	drop procedure usp_individuals_samples_update_pool_id
go
create procedure usp_individuals_samples_update_pool_id
@is_barcode			varchar(800),
@poo_id				int,
@operation			int 
as
begin
	if @operation = 1
	begin
		update	tb_individuals_samples
		set		poo_id = @poo_id
		where is_barcode = @is_barcode
	end
	else	if @operation = 2
	begin
		update	tb_individuals_samples
		set		poo_id = null
		where is_barcode = @is_barcode
	end
	else if @operation = 3
	begin
		delete tb_pools where poo_id = @poo_id
	end
end
go

if OBJECT_ID('usp_pools_results_update_result') is not null
	drop procedure usp_pools_results_update_result
go
create procedure usp_pools_results_update_result
@poo_id				int,
@pr_result			varchar(800) 
as
begin
	if exists (select * from [dbo].[tb_pools_results] where poo_id= @poo_id)
	begin
		update	[dbo].[tb_pools_results]
		set		pr_result = (case when (@pr_result = '') then null else @pr_result end),
				[pr_date_result] = getdate(),
				[pr_time_result] = getdate()
		where	poo_id = @poo_id
	end
	else 
	begin
		insert into	[dbo].[tb_pools_results] (poo_id, pr_date_result, pr_time_result, pr_result)
		values		(@poo_id,getdate(), getdate(),(case when (@pr_result = '') then null else @pr_result end))
	end
end
go


if OBJECT_ID('usp_pools_results_update_ct_value') is not null
	drop procedure usp_pools_results_update_ct_value
go
create procedure usp_pools_results_update_ct_value
@poo_id				int,
@pr_ct_value		varchar(800) 
as
begin
	if exists (select * from [dbo].[tb_pools_results] where poo_id= @poo_id)
	begin
		update	[dbo].[tb_pools_results]
		set		pr_ct_value = (case when (@pr_ct_value = '') then null else @pr_ct_value end),
				[pr_date_result] = getdate(),
				[pr_time_result] = getdate()
		where	poo_id = @poo_id
	end
	else 
	begin
		insert into	[dbo].[tb_pools_results] (poo_id, pr_date_result, pr_time_result, pr_ct_value)
		values		(@poo_id,getdate(), getdate(),(case when (@pr_ct_value = '') then null else @pr_ct_value end))
	end
end
go

