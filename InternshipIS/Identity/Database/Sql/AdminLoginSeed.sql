BEGIN TRY

		INSERT INTO [InternshipDB].[auth].[AspNetUsers]
        (
            [Id],
            [UserName],
            [NormalizedUserName],
            [Email],
            [NormalizedEmail],
            [EmailConfirmed],
            [PasswordHash],
            [SecurityStamp],
            [ConcurrencyStamp],
            [PhoneNumber],
            [PhoneNumberConfirmed],
            [TwoFactorEnabled],
            [LockoutEnd],
            [LockoutEnabled],
            [AccessFailedCount],
            [LastName],
            [Name],
            [CreationAuthor],
            [CreationDate],
            [UpdateAuthor],
            [UpdatedDate],
            [Code]
        )
        VALUES
        (
            '367381E3-6085-4E4F-8D09-F60F71B36819',
            'admin@osu.cz',
            'ADMIN@OSU.CZ',
            'admin@osu.cz',
            'ADMIN@OSU.CZ',
            0,
            'AQAAAAIAAYagAAAAEH+lvndhg9GnkdSMPVcLwYov42BNNYqFxBOVbbAB3PfCH1ZkjlfYXpjo18bnocA12Q==',
            'YQHM57P35IR6WRGRGPMITS7EYDIWGEOL',
            '45780b0e-8048-4c84-a80b-1d9ed9072dc4',
            NULL,
            0,
            0,
            NULL,
            1,
            0,
            'Administrátor',
            'Administrátor',
            'System',
            '2025-08-14 15:42:51.5969168',
            'System',
            '2025-08-14 15:42:51.5969168',
            NULL
        );

END TRY
BEGIN CATCH
	PRINT ERROR_MESSAGE();
END CATCH