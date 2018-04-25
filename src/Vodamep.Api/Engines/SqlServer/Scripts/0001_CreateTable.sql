

CREATE TABLE dbo.Institutions
( 
	[Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] varchar (100) NOT NULL 
)


CREATE TABLE dbo.Users
( 
	[Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] varchar (100) NOT NULL
)


CREATE TABLE dbo.Messages  
   ([Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [UserId] int NOT NULL,
	[InstitutionId] int NOT NULL,
	[Month] smallint NOT NULL,  
    [Year] smallint NOT NULL,   
	[Hash_SHA256] varchar(100) NOT NULL,        
	[Date] datetime2 NOT NULL CONSTRAINT DF_Messages_Date_GETDATE DEFAULT GETDATE(),
    [Data] varbinary(max) NOT NULL

   ,INDEX IX_Messages_HashId NONCLUSTERED ([Hash_SHA256])   
   ,INDEX IX_Messages_Year NONCLUSTERED ([Year])
   ,INDEX IX_Messages_Month NONCLUSTERED ([Month])  
   ,CONSTRAINT FK_Institutions_Messages FOREIGN KEY ([InstitutionId]) REFERENCES dbo.Institutions ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION   
   ,CONSTRAINT FK_Users_Messages FOREIGN KEY ([UserId]) REFERENCES dbo.Users ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION   
   )
