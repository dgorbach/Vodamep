

CREATE TABLE dbo.Messages  
   ([Id] int IDENTITY(1,1),      
	[HashId] varchar(100) NOT NULL,
    [Source] int NOT NULL,  
    [Month] int NOT NULL,  
    [Year] int NOT NULL,   
	[Date] datetime2 NOT NULL CONSTRAINT DF_Messages_Date_GETDATE DEFAULT GETDATE(),
    [Data] varbinary(max) NOT NULL 

   ,INDEX IX_Messages_HashId NONCLUSTERED ([HashId])
   ,INDEX IX_Messages_Source NONCLUSTERED ([Source])
   ,INDEX IX_Messages_Year NONCLUSTERED ([Year])
   ,INDEX IX_Messages_Month NONCLUSTERED ([Month])
   )
