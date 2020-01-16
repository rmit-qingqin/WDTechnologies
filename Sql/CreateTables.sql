create table Person
(
	PersonID int not null,
	FirstName nvarchar(max) not null,
	LastName nvarchar(max) not null,
    constraint PK_Person primary key (PersonID)
);
