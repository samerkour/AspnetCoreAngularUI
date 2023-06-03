
In order to scaffold databese entities follow steps

1- open cmd in Administrator mode
2- cd to Domain project where you want to create the entities
3- modefy the command below (data base connction and entities folder) then run it

	dotnet ef dbcontext scaffold "Data Source=CLIENT01\MSSQLSERVER2019;Initial Catalog=IdentityServer4Admin;user id=sa;password=Admin@123" Microsoft.EntityFrameworkCore.SqlServer -o Entities/IdentityServer4AdminEntities

	

	dotnet ef dbcontext scaffold "Data Source=DESKTOP-53A5H27\LOCALSERVER;Initial Catalog=PortEmployeesDb;user id=sa;password=Admin@123" Microsoft.EntityFrameworkCore.SqlServer -o Entities/EmployeeEntities

	