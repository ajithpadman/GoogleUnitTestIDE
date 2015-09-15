CREATE TABLE Arguments ( 
	ID int identity(1,1)  NOT NULL,
	type int NOT NULL,
	MethodID int NOT NULL
)
;

CREATE TABLE ArithmeticType ( 
	ID int identity(1,1)  NOT NULL,
	MaxValue nvarchar(500),
	MinValue nvarchar(500),
	Type int NOT NULL
)
;

CREATE TABLE Classes ( 
	ID int identity(1,1)  NOT NULL,
	EntityName nvarchar(500) NOT NULL,
	FilePath nvarchar(500) NOT NULL,
	Line int NOT NULL,
	ColumnNo int NOT NULL,
	FileID int,
	Type int
)
;

CREATE TABLE DataType ( 
	ID int identity(1,1)  NOT NULL,
	TypeKind int NOT NULL,
	EntityName nvarchar(500),
	IsConstQualified bit NOT NULL
)
;

CREATE TABLE EnumType ( 
	ID int identity(1,1)  NOT NULL,
	Type int NOT NULL
)
;

CREATE TABLE EnumValues ( 
	ID int identity(1,1)  NOT NULL,
	EnumID int NOT NULL,
	EnumValue nvarchar(500)
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
	UnderlyingReturnKind nvarchar(500),
	ReturnTypeObject int NOT NULL
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
	UnderlyingReturnKind nvarchar(500),
	ReturnTypeObject int NOT NULL
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

CREATE TABLE PointerType ( 
	ID int identity(1,1)  NOT NULL,
	PointerTo int NOT NULL,
	TypeID int NOT NULL
)
;

CREATE TABLE ProjectFiles ( 
	ID int identity(1,1)  NOT NULL,
	FilePath nvarchar(500),
	LastModifiedTime datetime
)
;

CREATE TABLE RecordType ( 
	ID int identity(1,1)  NOT NULL,
	TypeKind int NOT NULL,
	Type int NOT NULL
)
;

CREATE TABLE ReferenceType ( 
	ID int identity(1,1)  NOT NULL,
	ReferenceTo int NOT NULL,
	TypeID int NOT NULL
)
;

CREATE TABLE Structure ( 
	ID int identity(1,1)  NOT NULL,
	Type int
)
;

CREATE TABLE StructureFields ( 
	ID int identity(1,1)  NOT NULL,
	StructureID int NOT NULL,
	VariableID int NOT NULL
)
;

CREATE TABLE Typedef ( 
	ID int identity(1,1)  NOT NULL,
	type int NOT NULL,
	UnderlyingType int NOT NULL
)
;

CREATE TABLE Variables ( 
	ID int identity(1,1)  NOT NULL,
	VariableName nvarchar(500),
	VariableType nvarchar(500),
	FilePath nvarchar(500) NOT NULL,
	ColumnNo int NOT NULL,
	Line int NOT NULL,
	StorageClass int,
	TypeKind int
)
;


ALTER TABLE Arguments
	ADD CONSTRAINT UQ_Parameters_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Parameters_Type
	ON Arguments (type ASC)
;

CREATE INDEX IXFK_Parameters_Methods
	ON Arguments (MethodID ASC)
;

CREATE INDEX IXFK_ArithmeticType_Type
	ON ArithmeticType (Type ASC)
;

ALTER TABLE ArithmeticType
	ADD CONSTRAINT UQ_ArithmeticType_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Classes_ProjectFiles
	ON Classes (FileID ASC)
;

ALTER TABLE Classes
	ADD CONSTRAINT UQ_Classes_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Classes_RecordType
	ON Classes (Type ASC)
;

ALTER TABLE DataType
	ADD CONSTRAINT UQ_Type_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Enum_Type
	ON EnumType (Type ASC)
;

ALTER TABLE EnumType
	ADD CONSTRAINT UQ_Enum_ID UNIQUE (ID)
;

ALTER TABLE EnumValues
	ADD CONSTRAINT UQ_EnumValues_ID UNIQUE (ID)
;

CREATE INDEX IXFK_EnumValues_Enum
	ON EnumValues (EnumID ASC)
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

CREATE INDEX IXFK_MethodCalls_DataType
	ON MethodCalls (ReturnTypeObject ASC)
;

ALTER TABLE Methods
	ADD CONSTRAINT UQ_Methods_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Methods_DataType
	ON Methods (ReturnTypeObject ASC)
;

CREATE INDEX IXFK_Namespaces_ProjectFiles
	ON Namespaces (FileID ASC)
;

ALTER TABLE Namespaces
	ADD CONSTRAINT UQ_Namespaces_ID UNIQUE (ID)
;

ALTER TABLE PointerType
	ADD CONSTRAINT UQ_PointerType_ID UNIQUE (ID)
;

CREATE INDEX IXFK_PointerType_Type
	ON PointerType (TypeID ASC)
;

CREATE INDEX IXFK_PointerType_Type_02
	ON PointerType (PointerTo ASC)
;

ALTER TABLE ProjectFiles
	ADD CONSTRAINT UQ_ProjectFiles_ID UNIQUE (ID)
;

ALTER TABLE RecordType
	ADD CONSTRAINT UQ_RecordType_ID UNIQUE (ID)
;

CREATE INDEX IXFK_RecordType_Type
	ON RecordType (Type ASC)
;

ALTER TABLE ReferenceType
	ADD CONSTRAINT UQ_ReferenceType_ID UNIQUE (ID)
;

CREATE INDEX IXFK_ReferenceType_Type
	ON ReferenceType (TypeID ASC)
;

CREATE INDEX IXFK_ReferenceType_Type_02
	ON ReferenceType (ReferenceTo ASC)
;

ALTER TABLE Structure
	ADD CONSTRAINT UQ_Structure_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Structure_RecordType
	ON Structure (Type ASC)
;

ALTER TABLE StructureFields
	ADD CONSTRAINT UQ_StructureFields_ID UNIQUE (ID)
;

CREATE INDEX IXFK_StructureFields_Structure
	ON StructureFields (StructureID ASC)
;

CREATE INDEX IXFK_StructureFields_Variables
	ON StructureFields (VariableID ASC)
;

ALTER TABLE Typedef
	ADD CONSTRAINT UQ_Typedef_ID UNIQUE (ID)
;

CREATE INDEX IXFK_Typedef_Type
	ON Typedef (type ASC)
;

CREATE INDEX IXFK_Typedef_Type_02
	ON Typedef (UnderlyingType ASC)
;

ALTER TABLE Variables
	ADD CONSTRAINT UQ_Variables_ID UNIQUE (ID)
;

ALTER TABLE Arguments ADD CONSTRAINT PK_Parameters 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE ArithmeticType ADD CONSTRAINT PK_ArithmeticType 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE Classes ADD CONSTRAINT PK_Classes 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE DataType ADD CONSTRAINT PK_Type 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE EnumType ADD CONSTRAINT PK_Enum 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE EnumValues ADD CONSTRAINT PK_EnumValues 
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

ALTER TABLE PointerType ADD CONSTRAINT PK_PointerType 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE ProjectFiles ADD CONSTRAINT PK_ProjectFiles 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE RecordType ADD CONSTRAINT PK_RecordType 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE ReferenceType ADD CONSTRAINT PK_ReferenceType 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE Structure ADD CONSTRAINT PK_Structure 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE StructureFields ADD CONSTRAINT PK_StructureFields 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE Typedef ADD CONSTRAINT PK_Typedef 
	PRIMARY KEY NONCLUSTERED (ID)
;

ALTER TABLE Variables ADD CONSTRAINT PK_Variables 
	PRIMARY KEY NONCLUSTERED (ID)
;



ALTER TABLE Arguments ADD CONSTRAINT FK_Parameters_Type 
	FOREIGN KEY (type) REFERENCES DataType (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE Arguments ADD CONSTRAINT FK_Parameters_Methods 
	FOREIGN KEY (MethodID) REFERENCES Methods (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE ArithmeticType ADD CONSTRAINT FK_ArithmeticType_Type 
	FOREIGN KEY (Type) REFERENCES DataType (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE Classes ADD CONSTRAINT FK_Classes_ProjectFiles 
	FOREIGN KEY (FileID) REFERENCES ProjectFiles (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE Classes ADD CONSTRAINT FK_Classes_RecordType 
	FOREIGN KEY (Type) REFERENCES RecordType (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE EnumType ADD CONSTRAINT FK_Enum_Type 
	FOREIGN KEY (Type) REFERENCES DataType (ID)
	ON DELETE CASCADE ON UPDATE CASCADE
;

ALTER TABLE EnumValues ADD CONSTRAINT FK_EnumValues_Enum 
	FOREIGN KEY (EnumID) REFERENCES EnumType (ID)
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

ALTER TABLE MethodCalls ADD CONSTRAINT FK_MethodCalls_DataType 
	FOREIGN KEY (ReturnTypeObject) REFERENCES DataType (ID)
	ON DELETE NO ACTION ON UPDATE NO ACTION
;

ALTER TABLE Methods ADD CONSTRAINT FK_Methods_DataType 
	FOREIGN KEY (ReturnTypeObject) REFERENCES DataType (ID)
	ON DELETE NO ACTION ON UPDATE NO ACTION
;

ALTER TABLE Namespaces ADD CONSTRAINT FK_Namespaces_ProjectFiles 
	FOREIGN KEY (FileID) REFERENCES ProjectFiles (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE PointerType ADD CONSTRAINT FK_PointerType_Type 
	FOREIGN KEY (TypeID) REFERENCES DataType (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE PointerType ADD CONSTRAINT FK_PointerType_Type_02 
	FOREIGN KEY (PointerTo) REFERENCES DataType (ID)
	ON DELETE NO ACTION ON UPDATE NO ACTION
;

ALTER TABLE RecordType ADD CONSTRAINT FK_RecordType_Type 
	FOREIGN KEY (Type) REFERENCES DataType (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE ReferenceType ADD CONSTRAINT FK_ReferenceType_Type 
	FOREIGN KEY (TypeID) REFERENCES DataType (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE ReferenceType ADD CONSTRAINT FK_ReferenceType_Type_02 
	FOREIGN KEY (ReferenceTo) REFERENCES DataType (ID)
	ON DELETE NO ACTION ON UPDATE NO ACTION
;

ALTER TABLE Structure ADD CONSTRAINT FK_Structure_RecordType 
	FOREIGN KEY (Type) REFERENCES RecordType (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE StructureFields ADD CONSTRAINT FK_StructureFields_Structure 
	FOREIGN KEY (StructureID) REFERENCES Structure (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE StructureFields ADD CONSTRAINT FK_StructureFields_Variables 
	FOREIGN KEY (VariableID) REFERENCES Variables (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE Typedef ADD CONSTRAINT FK_Typedef_Type 
	FOREIGN KEY (type) REFERENCES DataType (ID)
	ON DELETE CASCADE ON UPDATE NO ACTION
;

ALTER TABLE Typedef ADD CONSTRAINT FK_Typedef_Type_02 
	FOREIGN KEY (UnderlyingType) REFERENCES DataType (ID)
	ON DELETE NO ACTION ON UPDATE NO ACTION
;
