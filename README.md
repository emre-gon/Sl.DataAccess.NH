[![NuGet](https://img.shields.io/nuget/v/Sl.DataAccess.NH.svg)](https://www.nuget.org/packages/Sl.DataAccess.NH)

# Sl.DataAccess.NH

NHibernate AutoMapping configurations with custom attributes and json column support.

It uses NHibernate's AutoMapping conventions for reading custom attributes over classes and properties and does database mapping, without the need of xml and and fluent mapping.



## Installation

```
dotnet add package Sl.DataAccess.NH
```

## Usage


```cs

//The assembly that contains table class definitions
Assembly myAssembly = Assembly.GetAssembly(typeof(MyTable));

IPersistenceConfigurer dbConfig = MsSqlConfiguration.MsSql2012.ConnectionString("foo");

IAuditService myAuditService = new MyAuditService();


SlSession.ConfigureSessionFactory(myAssembly, dbConfig,
        SessionContextType.ThreadStatic, 
        myAuditService, 
        DBSchemaUpdateMode.Update_Tables);

```





## Standart DataAnnotations Attributes


### [Key]
Generates primary key. It can generate a composite primary key if it was put over two properties.

It can also be used over foreign key fields.

### [Required]

Makes any field, including foreign key fields, NotNull.

### [MaxLength(n)]

Can be used over string fields to indicate Maximum Length.

### [DataType(DataType.Time)]

Creates time typed columns in supported databases. (Time without date)


### [DataType(DataType.Date)]

Creates date typed columns in supported databases (Date without time)


## Custom Attributes

### [Index]

Crates an index over specified column or columns.

### [Unique]

Creates a unique index over specified column or columns.

### [AnsiString]

Sets the string columns type to varchar instead of nvarchar supported databases.

### [ColumnName]

Sets the column name in database.

### [DBIgnore]

Ignores the property during mapping.

### [TableName]

Sets the table Name in database when put on a class.

### [JsonColumn]

Can be put on a property of complex type. It generates an nvarchar(max) column in database and parses the value into json.

It generates a jsonb typed column in Postgres.



## Session Context Types

### Web

### Web_Old

### Hybrid

### ThreadStatic

### Thread Local

## Audit

Audit can be done using dependency injection by extending IAuditService.


## Authors <a name = "authors"></a>

- [@emre-gon](https://github.com/emre-gon)