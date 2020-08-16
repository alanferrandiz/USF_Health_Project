/* this following code drops the existing tables in case they already exist
*/
if object_id('tb_individuals_samples') is not null
	drop table tb_individuals_samples
go
if object_id('tb_individuals') is not null
	drop table tb_individuals
go
if object_id('tb_individuals_category') is not null
	drop table tb_individuals_category
go
if object_id('tb_pools_results') is not null
	drop table tb_pools_results
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
if object_id('tb_units_samples_surfaces') is not null
	drop table tb_units_samples_surfaces
go
if object_id('tb_units_samples_results') is not null
	drop table tb_units_samples_results
go
if object_id('tb_units_samples') is not null
	drop table tb_units_samples
go
if object_id('tb_tests') is not null
	drop table tb_tests
go
if object_id('tb_units') is not null
	drop table tb_units
go
if object_id('tb_locations') is not null
	drop table tb_locations
go
if object_id('tb_samples_types') is not null
	drop table tb_samples_types
go
if object_id('tb_samples_status') is not null
	drop table tb_samples_status
go
if object_id('tb_results_status') is not null
	drop table tb_results_status
go
if object_id('tb_surfaces') is not null
	drop table tb_surfaces
go
if object_id('tb_users') is not null
	drop table tb_users
go


/*tb_individuals table stores data for the individuals to whom samples will be taken. Data stored includes first name, last name,
email, contact phone, birth date, gender, document and any details for the individual. Every individual will be classified using 
an individual category id (player, coach, assistant, etc).
*/
create table tb_individuals
(
		ind_id					int not null		identity,		/*identifier number for tb_individuals*/
		ind_date_created		date				default getdate(),
		ind_time_created		time				default getdate(),
		ind_first_name			varchar(800),		/*first name for the individual*/
		ind_last_name			varchar(800),		/*last name for the individual*/
		ind_email				varchar(800),		/*email address for the individual*/
		ind_phone				varchar(800),		/*contact phone for the individual*/
		ind_birthdate			date,				/*birth date for the individual*/
		ind_gender				char(1),			/*gender for the individual (either M of F)*/
		ind_document			varchar(800),		/*document for the individual*/
		indcat_id				int,				/*foreign key for tb_individuals_category (individuals category id to classify the individual)*/
		ref_id					int,
		std_id					int,
		ind_details				varchar(max)		/*important details for the individual*/
)
go

/* tb_individuals_samples table stores data for the samples taken to the individuals. Dates and times for the collection
and registration of the samples are stored as well as the type of the sample taken, user id who registered the sample, 
individual id to whom the sample was taken and pool id to which the sample belongs
*/
create table tb_individuals_samples
(
		is_id							int not null	identity,	/*identifier number for tb_individuals_samples*/
		is_barcode as 'A' + right(convert(varchar(800),(10000000 + is_id)),7),
		is_date_created					date			default getdate(),			
		is_time_created					time			default getdate(),	
		is_date_collected				date			default getdate(),			/*date of the collection for the sample*/
		is_time_collected				time			default getdate(),			/*time of the collection for the sample*/
		is_date_registered				date,			/*date of the registration for the sample*/
		is_time_registered				time,			/*time of the registration for the sample*/
		st_id							int,			/*foreign key for tb_samples_types (sample type id to classify the sample collected)*/
		ss_id							int,			/*foreign key for tb_samples_status (sample status id to determine the status of the sample collected)*/	
		usr_id_registered				int,			/*foreign key for tb_users (user id who registered the sample)*/
		ind_id							int,			/*foreign key for tb_individuals (individual id to whom the sample was taken)*/
		poo_id							int,			/*foreign key for tb_pools (pool id to which the sample will belong)*/
		is_date_registered_pool			date			default getdate(),			
		is_time_registered_pool			time			default getdate(),
		is_well_number					varchar(800),
		is_details						varchar(max)	/*important details for the sample of the individual*/
)
go

/* tb_pools table stores data for the pools of the samples that are combined to be tested. The user who registers the pool
is stored as well as the date and the time when the pool is registered. Finally, the test id for the applied test to the pool
is also stored.  
*/
create table tb_pools
(
		poo_id					int not null	identity,	/*identifier number for tb_pools*/
		poo_date_created					date			default getdate(),			
		poo_time_created					time			default getdate(),	
		usr_id_registered		int,			/*foreign key for tb_users (user id who registered the pool)*/
		tst_id					int,			/*foreign key for tb_tests (test id of the test applied to the sample)*/
		poo_details				varchar(max)	/*important details for the pool*/
)
go

/* tb_pools_results table stores data for the results, reference and value for the pools results. Also date and time when the
pool results were registered and the user who registered the pool results.
*/
create table tb_pools_results
(
		pr_id					int not null	identity,	/*identifier number for tb_pools_results*/
		pr_date_created			date			default getdate(),			
		pr_time_created			time			default getdate(),	
		poo_id					int,			/*foreign key for tb_pools (pool id to which these results belong) */
		pr_date_result			date,			/*date of the registration for the pool results*/
		pr_time_result			time,			/*time of the registration for the pool results*/
		usr_id_registered		int,			/*foreign key for tb_users (user id who registered the pool results)*/
		rs_id					int,			/*foreign key for tb_results_status (results status id to determine the status of the generated results)*/	
		pr_result				varchar(800),	/*results value for the pool results*/
		pr_ct_value				varchar(800),	/*results value for the pool results*/
		pr_reference			varchar(800),	/*reference text for the pool results*/
		pr_units				varchar(800),	/*units text for the pool results*/
		pr_details				varchar(max)	/*important details for the pool results*/
)
go

/* tb_samples_types table stores data for the different categories to classify the samples that will be taken, 
for example: blood, swap, spit, etc.
*/
create table tb_samples_types
(
		st_id					int not null	identity,	/*identifier number for tb_samples_types*/
		st_name					varchar(800),	/*name of the sample type (blood, swap, spit, etc.)*/
		st_details				varchar(max)	/*important details for the sample type*/
)
go


create table tb_references
(
		ref_id					int not null	identity,	
		ref_date_created		date			default getdate(),			
		ref_time_created		time			default getdate(),	
		ref_n					varchar(800),	
		ref_name				varchar(800),	
		ref_details				varchar(max)	
)
go


create table tb_studies
(
		std_id					int not null	identity,	
		std_date_created		date			default getdate(),			
		std_time_created		time			default getdate(),	
		std_n					varchar(800),	
		std_name				varchar(800),	
		std_details				varchar(max)	
)
go

/* tb_samples_status table stores data for the different status of the samples that will be taken, 
for example: barcode printed, barcode saved, etc.
*/
create table tb_samples_status
(
		ss_id					int not null	identity,	/*identifier number for tb_samples_status*/
		ss_name					varchar(800),	/*name of the sample status (barcode printed, barcode saved, etc.)*/
		ss_details				varchar(max)	/*important details for the sample status*/
)
go

/* tb_results_status table stores data for the different status of the results that will be generated, 
for example: results ready, results exported, etc.
*/
create table tb_results_status
(
		rs_id					int not null	identity,	/*identifier number for tb_results_status*/
		rs_name					varchar(800),	/*name of the results status (results ready, results exported, etc.)*/
		rs_details				varchar(max)	/*important details for the results status*/
)
go

/* tb_individuals_category table stores data for the different categories to classify an individual whom a sample will be taken, 
for example: player, coach, assistant, etc
*/
create table tb_individuals_category
(
		indcat_id				int not null	identity,	/*identifier number for tb_individuals_category*/
		indcat_name				int,			/*name for the category that classifies an individual (player, coach, assistant, etc)*/
		indcat_details			varchar(max)	/*important details for the category that classifies an individual*/
)
go

/* tb_units table stores data for the units (inside locations) where the samples are taken. One unit is always inside a location.
Examples can be residence hall floor, dining facility, frequently used rest room, etc
*/
create table tb_units
(
		unt_id					int not null		identity,		/*identifier number for tb_units*/
		unt_date_created		date			default getdate(),			
		unt_time_created		time			default getdate(),	
		unt_name				varchar(800),		/*name for the unit (residence hall floor, dining facility, frequently used rest room, etc)*/
		unt_details				varchar(max),		/*important details for the unit*/
		loc_id					int					/*foreign key for tb_locations (location id where the unit is located)*/
)
go

/* tb_locations table stores data for the locations where the samples are taken. One location can contain or be related to 
many units and one unit can only belong to one location. Examples of locations are: C.W. Bill Young Hall, FIT Wellness, Campus Recreation Center, etc
*/
create table tb_locations
(
		loc_id				int not null		identity,		/*identifier number for tb_locations*/
		loc_name			varchar(800),		/*name for the location (C.W. Bill Young Hall, FIT Wellness, Campus Recreation Center, etc)*/
		loc_details			varchar(max)		/*important details for the location*/
)
go

/* tb_surfaces table stores data for the generic name of the surfaces in which samples are taken. It is important to notice
that surface names are generic and not specific, for example, we store "table", "door handle", "faucet handles", 
"elevator buttons", "ping pong paddles", "pool cues", etc.
*/
create table tb_surfaces
(
		sfc_id				int not null		identity,		/*identifier number for tb_surfaces*/
		sfc_name			varchar(800),		/*name (generic not specific) for the surface (table, restroom, door handle, etc.)*/
		sfc_details			varchar(max)		/*important details for the surface*/
)
go

/* tb_units_samples table stores data for the combination of a sample taken in an specific units. 
With this table we have the detail of not only the date and time of both the collection and registration of the sample
but also who collected and registered the sample in the system and what type of sample was taken. Finally, the test id for 
the applied test to the sample is also stored.
*/
create table tb_units_samples
(
		us_id					int not null	identity,	/*identifier number for tb_locations_samples*/
		us_date_created			date			default getdate(),			
		us_time_created			time			default getdate(),	
		us_date_collected		date			default getdate(),	/*date of the collection for the sample*/
		us_time_collected		time			default getdate(),	/*time of the collection for the sample*/
		us_date_registered		date,			/*date of the registration for the sample*/
		us_time_registered		time,			/*time of the registration for the sample*/
		unt_id					int,			/*foreign key for tb_units (unit id where the sample is collected)*/
		st_id					int,			/*foreign key for tb_samples_types (sample type id which classify the collection)*/
		ss_id					int,			/*foreign key for tb_samples_status (sample status id to determine the status of the sample collected)*/	
		usr_id_collected		int,			/*foreign key for tb_users (user id who collected the sample)*/
		usr_id_registered		int,			/*foreign key for tb_users (user id who registered the sample)*/
		tst_id					int,			/*foreign key for tb_tests (test id of the test applied to the sample)*/
		us_details				varchar(max)	/*important details for the surface*/
)
go

/* tb_units_samples_results table stores data for the results, reference and value for the units samples results. 
Also date and time when the units samples results were registered and the user who registered the units samples results.
*/
create table tb_units_samples_results
(
		usres_id				int not null	identity,	/*identifier number for tb_units_samples_results*/
		us_id					int,			/*foreign key for tb_units_samples (units samples id to which these results belong) */
		usres_date_created		date			default getdate(),			
		usres_time_created		time			default getdate(),	
		usr_id_registered		int,			/*foreign key for tb_users (user id who registered the units samples results)*/
		rs_id					int,			/*foreign key for tb_results_status (results status id to determine the status of the generated results)*/	
		usres_result			varchar(800),	/*results value for the units samples results*/
		usres_reference			varchar(800),	/*reference text for the units samples results*/
		usres_units				varchar(800),	/*units text for the units samples results*/
		usres_details			varchar(max)	/*important details for the units samples results*/
)
go

/* tb_tests table stores data for the tests applied to the samples. In a next version of the schema, it might be possible to
also store reference values for the test.
*/
create table tb_tests
(
		tst_id					int not null	identity,	/*identifier number for tb_tests*/
		tst_date_created		date			default getdate(),			
		tst_time_created		time			default getdate(),	
		tst_name				varchar(800),	/*name of the test*/			
		tst_detail				varchar(max)	/*important details for the test*/
)
go

/* tb_units_samples_surfaces table stores data for the combination of a sample taken in an specific units for 
an specific surface. With this table we have the detail of the specific surfaces in which samples were taken. It is
important to notice that the results of the sample include all the surfaces, it means that results are not surface-specific.
*/
create table tb_units_samples_surfaces
(
		ussfc_id				int not null	identity,	/*identifier number for tb_units_samples_surfaces*/
		us_id					int,			/*foreign key for tb_units_samples (unit sample id for the combination of the sample taken in an specific unit)*/
		sfc_id					int,			/*foreign key for tb_surfaces (surface id for which the sample was taken)*/
		ussfc_detail			varchar(max)	/*important details for the combination of the sample taken in an specific unit for an specific surface*/
)
go

/* tb_users table stores data for users, who are individuals who has access to the system in order to register data 
and who need to successfully authenticate with their username and password everytime they need to access the system. 
*/
create table tb_users
(
		usr_id						int not null		identity,		/*identifier number for tb_users*/
		usr_date_created			date				default getdate(),			
		usr_time_created			time				default getdate(),	
		usr_first_name				varchar(800),		/*first name for the user*/
		usr_last_name				varchar(800),		/*last name for the user*/
		usr_username				varchar(800),		/*username for the user, it is the same as their email address*/
		usr_password				varbinary(800),		/*password for the user, it is encrypted one-way, without possibility for decryption*/
		usr_document				varchar(800),		/*document for the user*/
		usr_detail					varchar(max)		/*important details for the user*/
)
go

/* this following code creates the primary key contraints in every table
*/
alter table tb_individuals add constraint pk_tb_individuals primary key (ind_id)
alter table tb_individuals_category add constraint pk_tb_tb_individuals_category primary key (indcat_id)
alter table tb_individuals_samples add constraint pk_tb_individuals_samples primary key (is_id)
alter table tb_pools add constraint pk_tb_pools primary key (poo_id)
alter table tb_users add constraint pk_tb_users primary key (usr_id)
alter table tb_references add constraint pk_tb_references primary key (ref_id)
alter table tb_studies add constraint pk_tb_studies primary key (std_id)
alter table tb_samples_types add constraint pk_tb_samples_types primary key (st_id)
alter table tb_units add constraint pk_tb_units primary key (unt_id)
alter table tb_locations add constraint pk_tb_locations primary key (loc_id)
alter table tb_units_samples add constraint pk_tb_units_samples primary key (us_id)
alter table tb_units_samples_results add constraint pk_tb_units_samples_results primary key (usres_id)
alter table tb_units_samples_surfaces add constraint pk_tb_units_samples_surfaces primary key (ussfc_id)
alter table tb_surfaces add constraint pk_tb_surfaces primary key (sfc_id)
alter table tb_tests add constraint pk_tb_tests primary key (tst_id)
alter table tb_pools_results add constraint pk_tb_pools_results primary key (pr_id)
alter table tb_samples_status add constraint pk_tb_samples_status primary key (ss_id)
alter table tb_results_status add constraint pk_tb_results_status primary key (rs_id)




/* this following code creates the foreign key contraints to create the relationships among tables
*/
go
alter table tb_individuals 
add constraint fk_tb_individuals_tb_individuals_category foreign key (indcat_id)
references tb_individuals_category (indcat_id)

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
add constraint fk_tb_individuals_samples_tb_samples_types foreign key (st_id)
references tb_samples_types (st_id)

alter table tb_individuals_samples 
add constraint fk_tb_individuals_samples_tb_users foreign key (usr_id_registered)
references tb_users (usr_id)

alter table tb_individuals
add constraint fk_tb_individuals_tb_references foreign key (ref_id)
references tb_references (ref_id)

alter table tb_individuals
add constraint fk_tb_individuals_tb_studies foreign key (std_id)
references tb_studies (std_id)

alter table tb_pools
add constraint fk_tb_pools_tb_users foreign key (usr_id_registered)
references tb_users (usr_id)

alter table tb_units
add constraint fk_tb_units_tb_locations foreign key (loc_id)
references tb_locations (loc_id)

alter table tb_units_samples
add constraint fk_tb_units_samples_tb_units foreign key (unt_id)
references tb_units (unt_id)

alter table tb_units_samples
add constraint fk_tb_units_samples_tb_samples_types foreign key (st_id)
references tb_samples_types (st_id)

alter table tb_units_samples
add constraint fk_tb_units_samples_tb_tests foreign key (tst_id)
references tb_tests (tst_id)

alter table tb_units_samples 
add constraint fk_tb_units_samples_tb_users_registered foreign key (usr_id_registered)
references tb_users (usr_id)

alter table tb_units_samples 
add constraint fk_tb_units_samples_tb_users_collected foreign key (usr_id_collected)
references tb_users (usr_id)

alter table tb_units_samples_surfaces
add constraint fk_tb_units_samples_surfaces_tb_units_samples foreign key (us_id)
references tb_units_samples (us_id)

alter table tb_units_samples_surfaces
add constraint fk_tb_units_samples_surfaces_tb_surfaces foreign key (sfc_id)
references tb_surfaces (sfc_id)

alter table tb_pools
add constraint fk_tb_pools_tb_tests foreign key (tst_id)
references tb_tests(tst_id)

alter table tb_pools_results
add constraint fk_tb_pools_results_tb_pools foreign key (poo_id)
references tb_pools(poo_id)
on delete cascade
on update cascade

alter table tb_units_samples_results
add constraint fk_tb_units_samples_results_tb_units_samples foreign key (us_id)
references tb_units_samples(us_id)

alter table tb_individuals_samples
add constraint fk_tb_individuals_samples_tb_samples_status foreign key (ss_id)
references tb_samples_status(ss_id)

alter table tb_units_samples
add constraint fk_tb_units_samples_tb_samples_status foreign key (ss_id)
references tb_samples_status(ss_id)

alter table tb_pools_results
add constraint fk_tb_pools_results_tb_results_status foreign key (rs_id)
references tb_results_status(rs_id)

alter table tb_units_samples_results
add constraint fk_tb_units_samples_results_tb_results_status foreign key (rs_id)
references tb_results_status(rs_id)
go
