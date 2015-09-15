CREATE TABLE Classes ( 
	ID int identity(1,1)  NOT NULL,
	EntityName nvarchar(500) NOT NULL,
	FilePath nvarchar(500) NOT NULL,
	Line int NOT NULL,
	ColumnNo int NOT NULL,
	FileID int
)
;

CREATE TABLE GlobalMethods ( 
	ID int identity(1,1)  NOT NULL,
	FileID int,
	MethodID int NOT NULL
)
;

CREATE TABLE GlobalVariables ( 
	ID int identity(1,1)  NOT NULL,
	VariableID int NOT NULL,
	FileID int
)
;

CREATE TABLE MemberMethods ( 
	ID int identity(1,1)  NOT NULL,
	ClassID int,
	MethodID int NOT NULL,
	IsPureVirtual bit NOT NULL,
	IsVirtual bit NOT NULL,
	IsStaticCxxMethod bit NOT NULL
)
;

CREATE TABLE MemberVariables ( 
	ID int identity(1,1)  NOT NULL,
	AccessScope int NOT NULL,
	ClassID int NOT NULL,
	VariableID int
)
;

CREATE TABLE MethodCalls ( 
	ID int identity(1,1)  NOT NULL,
	EntitiyName nvarchar(500),
	ReturnType nvarchar(500),
	FilePath nvarchar(500),
	Line int,
	ColumnNo int,
	Parameters nvarchar(500),
	CallerID int,
	IsCxxMethod bit NOT NULL,
	ParentName nvarchar(500),
	IsDefinedInParent bit,
	ParentFilePath nvarchar(500),
	UnderlyingReturnKind nvarchar(500)
)
;

CREATE TABLE Methods ( 
	ID int identity(1,1)  NOT NULL,
	EntityName nvarchar(500) NOT NULL,
	ReturnType nvarchar(255),
	Parameters nvarchar(500),
	AccessScope int,
	StorageClass int,
	IsDefined bit NOT NULL,
	IsConstMethod bit NOT NULL,
	FilePath nvarchar(500),
	ColumnNo int NOT NULL,
	Line int NOT NULL,
	UnderlyingParamKind nvarchar(500),
	UnderlyingReturnKind nvarchar(500)
)
;

CREATE TABLE Namespaces ( 
	ID int identity(1,1)  NOT NULL,
	EntityName nvarchar(500) NOT NULL,
	FilePath nvarchar(500) NOT NULL,
	Line int NOT NULL,
	ColumnNo int NOT NULL,
	FileID int
)
;

CREATE TABLE ProjectFiles ( 
	ID int identity(1,1)  NOT NULL,
	FilePath nvarchar(500),
	LastModifiedTime datetime
)
;

CREATE TABLE Variables ( 
	ID int identity(1,1)  NOT NULL,
	VariableName nvarchar(500),
	VariableType nvarchar(500),
	FilePath nvarchar(500) NOT NULL,
	ColumnNo int NOT NULL,
	Line int NOT NULL,
	StorageClass int
)
;


CREATE INDEX IXFK_Classes_ProjectFiles
	ON Classes (FileID ASC)
;

ALTER TABLE Classes
	ADD CONSTRAINT UQ_Classes_ID UNIQUE (ID)
;

CREATE INDEX IXFK_GlobalMethods_Methods
	ON GlobalMethods (MethodID ASC)
;

ALTER TABLE GlobalMethods
	ADD CONSTRAINT UQ_Methods_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Methods_ProjectFiles
	ON GlobalMethods (FileID ASC)
;

CREATE INDEX IXFK_GlobalVariables_Variables
	ON GlobalVariables (VariableID ASC)
;

ALTER TABLE GlobalVariables
	ADD CONSTRAINT UQ_Variables_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Variables_ProjectFiles
	ON GlobalVariables (FileID ASC)
;

ALTER TABLE MemberMethods
	ADD CONSTRAINT UQ_MemberMethods_ID UNIQUE (ID)
;

CREATE INDEX IXFK_MemberMethods_Methods
	ON MemberMethods (MethodID ASC)
;

CREATE INDEX IXFK_MemberMethods_Classes
	ON MemberMethods (ClassID ASC)
;

ALTER TABLE MemberVariables
	ADD CONSTRAINT UQ_MemberVariables_ID UNIQUE (ID)
;

CREATE INDEX IXFK_MemberVariables_Classes
	ON MemberVariables (ClassID ASC)
;

CREATE INDEX IXFK_MemberVariables_Variables
	ON MemberVariables (VariableID ASC)
;

CREATE INDEX IXFK_MethodCalls_Methods
	ON MethodCalls (CallerID ASC)
;

ALTER TABLE MethodCalls
	ADD CONSTRAINT UQ_MethodCalls_ID UNIQUE (ID)
;

ALTER TABLE Methods
	ADD CONSTRAINT UQ_Methods_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Namespaces_ProjectFiles
	ON Namespaces (FileID ASC)
;

ALTER TABLE Namespaces
	ADD CONSTRAINT UQ_Namespaces_ID UNIQUE (ID)
;

ALTER TABLE ProjectFiles
	ADD CONSTRAINT UQ_ProjectFiles_ID UNIQUE (ID)
;

ALTER TABLE Variables
	ADD CONSTRAINT UQ_Variables_ID UNIQUE (ID)
;

ALTER TABLE Classes ADD CONSTRAINT PK_Classes 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE GlobalMethods ADD CONSTRAINT PK_Methods 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE GlobalVariables ADD CONSTRAINT PK_Variables 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE MemberMethods ADD CONSTRAINT PK_MemberMethods 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE MemberVariables ADD CONSTRAINT PK_MemberVariables 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE MethodCalls ADD CONSTRAINT PK_MethodCalls 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE Methods ADD CONSTRAINT PK_Methods 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE Namespaces ADD CONSTRAINT PK_Namespaces 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE ProjectFiles ADD CONSTRAINT PK_ProjectFiles 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE Variables ADD CONSTRAINT PK_Variables 
	PRIMARY KEY NONCLUSTERED (ID)
;



ALTER TABLE Classes ADD CONSTRAINT FK_Classes_ProjectFiles 
	FOREIGN KEY (FileID) REFERENCES ProjectFiles (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE GlobalMethods ADD CONSTRAINT FK_GlobalMethods_Methods 
	FOREIGN KEY (MethodID) REFERENCES Methods (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE GlobalMethods ADD CONSTRAINT FK_Methods_ProjectFiles 
	FOREIGN KEY (FileID) REFERENCES ProjectFiles (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE GlobalVariables ADD CONSTRAINT FK_GlobalVariables_Variables 
	FOREIGN KEY (VariableID) REFERENCES Variables (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE GlobalVariables ADD CONSTRAINT FK_Variables_ProjectFiles 
	FOREIGN KEY (FileID) REFERENCES ProjectFiles (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE MemberMethods ADD CONSTRAINT FK_MemberMethods_Methods 
	FOREIGN KEY (MethodID) REFERENCES Methods (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE MemberMethods ADD CONSTRAINT FK_MemberMethods_Classes 
	FOREIGN KEY (ClassID) REFERENCES Classes (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE MemberVariables ADD CONSTRAINT FK_MemberVariables_Classes 
	FOREIGN KEY (ClassID) REFERENCES Classes (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE MemberVariables ADD CONSTRAINT FK_MemberVariables_Variables 
	FOREIGN KEY (VariableID) REFERENCES Variables (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE MethodCalls ADD CONSTRAINT FK_MethodCalls_Methods 
	FOREIGN KEY (CallerID) REFERENCES Methods (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE Namespaces ADD CONSTRAINT FK_Namespaces_ProjectFiles 
	FOREIGN KEY (FileID) REFERENCES ProjectFiles (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;
