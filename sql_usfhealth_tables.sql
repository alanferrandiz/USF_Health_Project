/* this following code drops the existing tables in case they already exist
*/
set nocount on
go
if object_id('tb_pools_results') is not null
	drop table tb_pools_results
go
if object_id('tb_individuals_samples') is not null
	drop table tb_individuals_samples
go
if object_id('tb_individuals') is not null
	drop table tb_individuals
go
if object_id('tb_pools') is not null
	drop table tb_pools
go
if object_id('tb_references') is not null
	drop table tb_references
go
if object_id('tb_studies') is not null
	drop table tb_studies
go
if object_id('tb_places_samples_results') is not null
	drop table tb_places_samples_results
go
if object_id('tb_places_samples') is not null
	drop table tb_places_samples
go
if object_id('tb_places') is not null
	drop table tb_places
go
if object_id('tb_audit') is not null
	drop table tb_audit
go
if object_id('tb_sessions') is not null
	drop table tb_sessions
go
if object_id('tb_users') is not null
	drop table tb_users
go


--USER DEFINED FUNCTIONS

if object_id('udf_getdatelocal') is not null
	drop function dbo.udf_getdatelocal
go
create function dbo.udf_getdatelocal(@datetime datetime = null)
returns datetime
as
begin
	declare @datetimeout datetime

	if @datetime is null
	begin
		select @datetimeout = SYSDATETIMEOFFSET() AT TIME ZONE 'UTC' AT TIME ZONE 'US Eastern Standard Time'
	end
	else
	begin
		select @datetimeout = @datetime AT TIME ZONE 'UTC' AT TIME ZONE 'US Eastern Standard Time'
	end
	return @datetimeout
end
--select dbo.udf_getdatelocal(default)
--select dbo.udf_getdatelocal('2020-08-19T08:00:00')
go


create table tb_individuals
(
		ind_id					int not null		identity,		/*identifier number for tb_individuals*/
		ind_date_created		date				default dbo.udf_getdatelocal(default),
		ind_time_created		time				default dbo.udf_getdatelocal(default),
		usr_id_created			int,
		ind_first_name			varchar(800),		/*first name for the individual*/
		ind_last_name			varchar(800),		/*last name for the individual*/
		ind_email				varchar(800),		/*email address for the individual*/
		ind_phone				varchar(800),		/*contact phone for the individual*/
		ind_birthdate			date,				/*birth date for the individual*/
		ind_gender				char(1),			/*gender for the individual (either M of F)*/
		ind_document			varchar(800),		/*document for the individual*/
		ref_id					int,
		std_id					int,
		ind_details				varchar(max)		/*important details for the individual*/
)
go

create table tb_individuals_samples
(
		is_id							int not null	identity,	
		is_barcode as 'A' + right(convert(varchar(800),(10000000 + is_id)),7),
		is_date_created					date			default dbo.udf_getdatelocal(default),			
		is_time_created					time			default dbo.udf_getdatelocal(default),	
		usr_id_created					int,
		is_date_collected				date			default dbo.udf_getdatelocal(default),			
		is_time_collected				time			default dbo.udf_getdatelocal(default),			
		usr_id_collected				int,
		is_date_registered				date,			
		is_time_registered				time,			
		usr_id_registered				int,			
		ind_id							int,			
		poo_id							int,			
		is_date_registered_pool			date,			
		is_time_registered_pool			time,
		usr_id_registered_pool			int,			
		is_well_number					varchar(800),
		is_details						varchar(max)	
)
go

create table tb_pools
(
		poo_id					int not null	identity,	/*identifier number for tb_pools*/
		poo_date_created		date			default dbo.udf_getdatelocal(default),			
		poo_time_created		time			default dbo.udf_getdatelocal(default),	
		usr_id_created			int,			/*foreign key for tb_users (user id who registered the pool)*/
		poo_details				varchar(max)	/*important details for the pool*/
)
go

create table tb_pools_results
(
		pr_id					int not null	identity,	
		pr_date_created			date			default dbo.udf_getdatelocal(default),			
		pr_time_created			time			default dbo.udf_getdatelocal(default),	
		usr_id_created			int,
		poo_id					int,			/*foreign key for tb_pools (pool id to which these results belong) */
		pr_result				varchar(800),	/*results value for the pool results*/
		pr_date_result			date,			/*date of the registration for the pool results*/
		pr_time_result			time,			/*time of the registration for the pool results*/
		usr_id_result			int,			/*foreign key for tb_users (user id who registered the pool results)*/		
		pr_ct_value				varchar(800),	/*results value for the pool results*/
		pr_date_ct_value		date,			/*date of the registration for the pool results*/
		pr_time_ct_value		time,			/*time of the registration for the pool results*/
		usr_id_ct_value			int			/*foreign key for tb_users (user id who registered the pool results)*/)
go

create table tb_references
(
		ref_id					int not null	identity,	
		ref_date_created		date			default dbo.udf_getdatelocal(default),			
		ref_time_created		time			default dbo.udf_getdatelocal(default),	
		ref_n					varchar(800),	
		ref_name				varchar(800),	
		ref_details				varchar(max)	
)
go


create table tb_studies
(
		std_id					int not null	identity,	
		std_date_created		date			default dbo.udf_getdatelocal(default),			
		std_time_created		time			default dbo.udf_getdatelocal(default),	
		std_n					varchar(800),	
		std_name				varchar(800),	
		std_details				varchar(max)	
)
go


create table tb_places
(
		pla_id					int not null		identity,		
		pla_date_created		date				default dbo.udf_getdatelocal(default),			
		pla_time_created		time				default dbo.udf_getdatelocal(default),
		usr_id_created			int,
		pla_name				varchar(800),	
		pla_location_reference	varchar(800),	
		pla_campus				varchar(800),		
		pla_details				varchar(max),		
)
go


create table tb_places_samples
(
		ps_id					int not null	identity,	
		ps_barcode as 'B' + right(convert(varchar(800),(10000000 + ps_id)),7),
		ps_date_created			date			default dbo.udf_getdatelocal(default),			
		ps_time_created			time			default dbo.udf_getdatelocal(default),	
		usr_id_created			int,
		ps_date_collected		date			default dbo.udf_getdatelocal(default),	
		ps_time_collected		time			default dbo.udf_getdatelocal(default),	
		usr_id_collected		int,			
		ps_date_registered		date,			
		ps_time_registered		time,			
		usr_id_registered		int,			
		pla_id					int,			
		ps_well_number			varchar(800),
		ps_surface_detail		varchar(max),	
		ps_details				varchar(max)	
)
go

create table tb_places_samples_results
(
		psres_id				int not null	identity,	/*identifier number for tb_places_samples_results*/
		ps_id					int,			/*foreign key for tb_places_samples (units samples id to which these results belong) */
		psres_date_created		date			default dbo.udf_getdatelocal(default),			
		psres_time_created		time			default dbo.udf_getdatelocal(default),	
		usr_id_created			int,			/*foreign key for tb_users (user id who registered the units samples results)*/
		psres_result			varchar(800),	/*results value for the units samples results*/
		psres_date_result		date			default dbo.udf_getdatelocal(default),			
		psres_time_result		time			default dbo.udf_getdatelocal(default),		
		usr_id_result			int,			/*foreign key for tb_users (user id who registered the units samples results)*/
		psres_ct_value			varchar(800),	/*results value for the units samples results*/
		psres_date_ct_value		date			default dbo.udf_getdatelocal(default),			
		psres_time_ct_value		time			default dbo.udf_getdatelocal(default),		
		usr_id_ct_value			int,		
		psres_details			varchar(max)	/*important details for the units samples results*/
)
go

create table tb_users
(
		usr_id						int not null		identity,		/*identifier number for tb_users*/
		usr_date_created			date				default dbo.udf_getdatelocal(default),			
		usr_time_created			time				default dbo.udf_getdatelocal(default),	
		usr_first_name				varchar(800),		/*first name for the user*/
		usr_last_name				varchar(800),		/*last name for the user*/
		usr_username				varchar(800),		/*username for the user, it is the same as their email address*/
		usr_password				varbinary(800),		/*password for the user, it is encrypted one-way, without possibility for decryption*/
		usr_detail					varchar(max)		/*important details for the user*/
)
go

create table tb_sessions
(
		ssn_id						int not null		identity,		/*identifier number for tb_users*/
		ssn_date_created			date				default dbo.udf_getdatelocal(default),			
		ssn_time_created			time				default dbo.udf_getdatelocal(default),	
		usr_id						int
)
go

create table tb_audit
(
	aud_id					int		identity,
	aud_station				varchar(800),
	aud_operation_id		int,
	aud_operation			varchar(800),
	aud_date				date,
	aud_time				time,
	aud_user				varchar(800),
	aud_table				varchar(800),
	aud_identifier_id		varchar(800),
	aud_identifier_field	varchar(800),
	aud_field				varchar(800),
	aud_before				varchar(max),
	aud_after				varchar(max),
	usr_id_audit			int,
	ssn_id					int
)
go


/* this following code creates the primary key contraints in every table
*/
alter table tb_individuals add constraint pk_tb_individuals primary key (ind_id)
alter table tb_individuals_samples add constraint pk_tb_individuals_samples primary key (is_id)
alter table tb_pools add constraint pk_tb_pools primary key (poo_id)
alter table tb_pools_results add constraint pk_tb_pools_results primary key (pr_id)
alter table tb_references add constraint pk_tb_references primary key (ref_id)
alter table tb_studies add constraint pk_tb_studies primary key (std_id)
alter table tb_places add constraint pk_tb_places primary key (pla_id)
alter table tb_places_samples add constraint pk_tb_places_samples primary key (ps_id)
alter table tb_places_samples_results add constraint pk_tb_places_samples_results primary key (psres_id)
alter table tb_users add constraint pk_tb_users primary key (usr_id)
alter table tb_sessions add constraint pk_tb_sessions primary key (ssn_id)
alter table tb_audit add constraint pk_tb_audit primary key (aud_id)




/* this following code creates the foreign key contraints to create the relationships among tables
*/
go

--INDIVIDUALS

alter table tb_individuals
add constraint fk_tb_individuals_tb_references foreign key (ref_id)
references tb_references (ref_id)

alter table tb_individuals
add constraint fk_tb_individuals_tb_studies foreign key (std_id)
references tb_studies (std_id)

alter table tb_individuals
add constraint fk_tb_individuals_tb_users foreign key (usr_id_created)
references tb_users (usr_id)



--INDIVIDUALS SAMPLES

alter table tb_individuals_samples 
add constraint fk_tb_individuals_samples_tb_individuals foreign key (ind_id)
references tb_individuals (ind_id)
on delete cascade
on update cascade

alter table tb_individuals_samples 
add constraint fk_tb_individuals_samples_tb_pools foreign key (poo_id)
references tb_pools (poo_id)
on delete set null
on update set null

alter table tb_individuals_samples 
add constraint fk_tb_individuals_samples_tb_users_created foreign key (usr_id_created)
references tb_users (usr_id)

alter table tb_individuals_samples 
add constraint fk_tb_individuals_samples_tb_users_collected foreign key (usr_id_collected)
references tb_users (usr_id)

alter table tb_individuals_samples 
add constraint fk_tb_individuals_samples_tb_users_registered foreign key (usr_id_registered)
references tb_users (usr_id)

alter table tb_individuals_samples 
add constraint fk_tb_individuals_samples_tb_users_registered_pool foreign key (usr_id_registered_pool)
references tb_users (usr_id)



--POOLS

alter table tb_pools
add constraint fk_tb_pools_tb_users foreign key (usr_id_created)
references tb_users (usr_id)



--POOLS RESULTS

alter table tb_pools_results
add constraint fk_tb_pools_results_tb_pools foreign key (poo_id)
references tb_pools(poo_id)
on delete cascade
on update cascade

alter table tb_pools_results 
add constraint fk_tb_pools_results_tb_users_created foreign key (usr_id_created)
references tb_users (usr_id)

alter table tb_pools_results 
add constraint fk_tb_pools_results_tb_users_result foreign key (usr_id_result)
references tb_users (usr_id)

alter table tb_pools_results 
add constraint fk_tb_pools_results_tb_users_ct_value foreign key (usr_id_ct_value)
references tb_users (usr_id)


--PLACES

alter table tb_places 
add constraint fk_tb_places_tb_users_created foreign key (usr_id_created)
references tb_users (usr_id)


--PLACES SAMPLES

alter table tb_places_samples 
add constraint fk_tb_places_samples_tb_places foreign key (pla_id)
references tb_places (pla_id)
on delete cascade
on update cascade

alter table tb_places_samples 
add constraint fk_tb_places_samples_tb_users_created foreign key (usr_id_created)
references tb_users (usr_id)

alter table tb_places_samples 
add constraint fk_tb_places_samples_tb_users_collected foreign key (usr_id_collected)
references tb_users (usr_id)

alter table tb_places_samples 
add constraint fk_tb_places_samples_tb_users_registered foreign key (usr_id_registered)
references tb_users (usr_id)




--PLACES SAMPLES RESULTS

alter table tb_places_samples_results
add constraint fk_tb_places_samples_results_tb_places_samples foreign key (ps_id)
references tb_places_samples(ps_id)
on delete cascade
on update cascade

alter table tb_places_samples_results 
add constraint fk_tb_places_samples_results_tb_users_created foreign key (usr_id_created)
references tb_users (usr_id)

alter table tb_places_samples_results 
add constraint fk_tb_places_samples_results_tb_users_result foreign key (usr_id_result)
references tb_users (usr_id)

alter table tb_places_samples_results 
add constraint fk_tb_places_samples_results_tb_users_ct_value foreign key (usr_id_ct_value)
references tb_users (usr_id)
go


--AUDIT

alter table tb_audit 
add constraint fk_tb_audit_tb_users foreign key (usr_id_audit)
references tb_users (usr_id)



--SESSIONS

alter table tb_sessions 
add constraint fk_tb_sessions_tb_users foreign key (usr_id)
references tb_users (usr_id)
