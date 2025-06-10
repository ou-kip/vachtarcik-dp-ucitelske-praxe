BEGIN TRY

		INSERT INTO auth.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
		VALUES (newid(), 'Admin', 'ADMIN', null)

		INSERT INTO auth.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
		VALUES (newid(), 'Teacher', 'TEACHER', null)

		INSERT INTO auth.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
		VALUES (newid(), 'Student', 'STUDENT', null)

		INSERT INTO auth.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
		VALUES (newid(), 'CompanyRelative', 'COMPANYRELATIVE', null)

END TRY
BEGIN CATCH
	PRINT ERROR_MESSAGE();
END CATCH