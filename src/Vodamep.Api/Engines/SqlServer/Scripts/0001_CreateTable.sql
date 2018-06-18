

CREATE TABLE dbo.[Institution]
( 
	[Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] varchar (100) NOT NULL 
)


CREATE TABLE dbo.[User]
( 
	[Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] varchar (100) NOT NULL
)


CREATE TABLE dbo.[Message]
   ([Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [UserId] int NOT NULL,
	[InstitutionId] int NOT NULL,
	[Month] smallint NOT NULL,  
    [Year] smallint NOT NULL,   
	[Hash_SHA256] varchar(100) NOT NULL,        
	[Date] datetime2 NOT NULL CONSTRAINT DF_Messages_Date_GETDATE DEFAULT GETDATE(),
    [Data] varbinary(max) NOT NULL

   ,INDEX IX_Message_HashId NONCLUSTERED ([Hash_SHA256])   
   ,INDEX IX_Message_Year NONCLUSTERED ([Year])
   ,INDEX IX_Message_Month NONCLUSTERED ([Month])  
   ,CONSTRAINT FK_Institution_Messages FOREIGN KEY ([InstitutionId]) REFERENCES dbo.[Institution] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION   
   ,CONSTRAINT FK_User_Message FOREIGN KEY ([UserId]) REFERENCES dbo.[User] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION   
   )
