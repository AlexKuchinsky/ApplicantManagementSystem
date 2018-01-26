USE [master]
GO
/****** Object:  Database [UniversitySpecialtiesDatabase]    Script Date: 26.01.2018 22:24:50 ******/
CREATE DATABASE [UniversitySpecialtiesDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'UniversitySpecialtiesDatabase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\UniversitySpecialtiesDatabase.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'UniversitySpecialtiesDatabase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\UniversitySpecialtiesDatabase_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [UniversitySpecialtiesDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [UniversitySpecialtiesDatabase]
GO
/****** Object:  Table [dbo].[Applications]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applications](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[PaymentGroup] [int] NOT NULL,
 CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Colors]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Colors](
	[ColorID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[RGB] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_Colors] PRIMARY KEY CLUSTERED 
(
	[ColorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GroupFriendships]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupFriendships](
	[GroupFriendshipID] [int] IDENTITY(1,1) NOT NULL,
	[RequestingGroupID] [int] NOT NULL,
	[AccessibleGroupID] [int] NOT NULL,
 CONSTRAINT [PK_GroupFriendships] PRIMARY KEY CLUSTERED 
(
	[GroupFriendshipID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Groups]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[GroupID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IntermediatePaymentOptions]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntermediatePaymentOptions](
	[IntermediatePaymentOptionID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentOptionID] [int] NOT NULL,
	[PaymentTypeID] [int] NOT NULL,
 CONSTRAINT [PK_IntermediatePaymentOptions] PRIMARY KEY CLUSTERED 
(
	[IntermediatePaymentOptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IntermediateStudyFormOptions]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntermediateStudyFormOptions](
	[IntermediateStudyFormOptionID] [int] IDENTITY(1,1) NOT NULL,
	[StudyFormOptionID] [int] NOT NULL,
	[StudyFormTypeID] [int] NOT NULL,
 CONSTRAINT [PK_IntermediateStudyFormOptions] PRIMARY KEY CLUSTERED 
(
	[IntermediateStudyFormOptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MinScores]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MinScores](
	[MinScoreID] [int] IDENTITY(1,1) NOT NULL,
	[Score] [int] NOT NULL,
 CONSTRAINT [PK_MinScores] PRIMARY KEY CLUSTERED 
(
	[MinScoreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OptionalSubjectsGroups]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OptionalSubjectsGroups](
	[OptionalSubjectsGroupID] [int] IDENTITY(1,1) NOT NULL,
	[SpecialitySubjectID] [int] NOT NULL,
	[SpecialityTestOptionID] [int] NOT NULL,
 CONSTRAINT [PK_OptionalSubjectsGroups] PRIMARY KEY CLUSTERED 
(
	[OptionalSubjectsGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PaymentOptions]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentOptions](
	[PaymentOptionID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_PaymentOptions] PRIMARY KEY CLUSTERED 
(
	[PaymentOptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PaymentTypes]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentTypes](
	[PaymentTypeID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentGroup] [int] NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PaymentTypes] PRIMARY KEY CLUSTERED 
(
	[PaymentTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProfileSubjectsGroups]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileSubjectsGroups](
	[ProfileSubjectsGroupID] [int] IDENTITY(1,1) NOT NULL,
	[SpecialityTestOptionID] [int] NOT NULL,
	[SpecialitySubjectID] [int] NOT NULL,
	[Rang] [int] NOT NULL,
 CONSTRAINT [PK_ProfileSubjectsGroups] PRIMARY KEY CLUSTERED 
(
	[ProfileSubjectsGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Specialities]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specialities](
	[SpecialityID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Qualification] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Specialities] PRIMARY KEY CLUSTERED 
(
	[SpecialityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SpecialityApplications]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpecialityApplications](
	[SpecialityApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[SpecialityID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[StudyFormTypeID] [int] NOT NULL,
	[ProfileSubjectsGroupID] [int] NOT NULL,
	[OptionalSubjectID] [int] NULL,
 CONSTRAINT [PK_SpecialityApplications] PRIMARY KEY CLUSTERED 
(
	[SpecialityApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SpecialityCapacityTable]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpecialityCapacityTable](
	[SpecialityCapacityID] [int] IDENTITY(1,1) NOT NULL,
	[TotalNumberOfPlaces] [int] NOT NULL,
	[TotalNumberOfPaidPlaces] [int] NOT NULL,
	[TotalNumberOfFreePayingPlaces] [int] NOT NULL,
	[CurrnetNumberOfPlaces] [int] NOT NULL,
	[CurrentNumberOfPaidPlaces] [int] NOT NULL,
	[CureentNumberOfFreePayingPlaces] [int] NOT NULL,
 CONSTRAINT [PK_SpecialityCapacityTable] PRIMARY KEY CLUSTERED 
(
	[SpecialityCapacityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SpecialitySubjects]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpecialitySubjects](
	[SpecialitySubjectID] [int] IDENTITY(1,1) NOT NULL,
	[SubjectID] [int] NOT NULL,
	[MinScoreID] [int] NOT NULL,
 CONSTRAINT [PK_SpecialitySubjects] PRIMARY KEY CLUSTERED 
(
	[SpecialitySubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SpecialityTestOptions]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpecialityTestOptions](
	[SpecialityTestOptionID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_SpecialityTestOptions] PRIMARY KEY CLUSTERED 
(
	[SpecialityTestOptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudyFormOptions]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudyFormOptions](
	[StudyFormOptionID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_StudyFormOptions] PRIMARY KEY CLUSTERED 
(
	[StudyFormOptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudyFormTypes]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudyFormTypes](
	[StudyFormTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_StudyFormTypes] PRIMARY KEY CLUSTERED 
(
	[StudyFormTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[SubjectID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
(
	[SubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UniversitySpecialities]    Script Date: 26.01.2018 22:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UniversitySpecialities](
	[UniversitySpecialityID] [int] IDENTITY(1,1) NOT NULL,
	[SpecialityID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[OpenDate] [date] NOT NULL,
	[ExpiringDate] [date] NOT NULL,
	[ColorID] [int] NOT NULL,
	[PaymentOptionID] [int] NOT NULL,
	[StudyFormOptionID] [int] NOT NULL,
	[SpecialityTestOptionID] [int] NOT NULL,
	[SpecialityCapacityID] [int] NOT NULL,
 CONSTRAINT [PK_UniversitySpecialities] PRIMARY KEY CLUSTERED 
(
	[UniversitySpecialityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[GroupFriendships]  WITH CHECK ADD  CONSTRAINT [FK_GroupFriednships_Groups] FOREIGN KEY([RequestingGroupID])
REFERENCES [dbo].[Groups] ([GroupID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupFriendships] CHECK CONSTRAINT [FK_GroupFriednships_Groups]
GO
ALTER TABLE [dbo].[GroupFriendships]  WITH CHECK ADD  CONSTRAINT [FK_GroupFriednships_Groups_accesible_group] FOREIGN KEY([AccessibleGroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[GroupFriendships] CHECK CONSTRAINT [FK_GroupFriednships_Groups_accesible_group]
GO
ALTER TABLE [dbo].[IntermediatePaymentOptions]  WITH CHECK ADD  CONSTRAINT [FK_IntermediatePaymentOptions_PaymentOptions] FOREIGN KEY([PaymentOptionID])
REFERENCES [dbo].[PaymentOptions] ([PaymentOptionID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IntermediatePaymentOptions] CHECK CONSTRAINT [FK_IntermediatePaymentOptions_PaymentOptions]
GO
ALTER TABLE [dbo].[IntermediatePaymentOptions]  WITH CHECK ADD  CONSTRAINT [FK_IntermediatePaymentOptions_PaymentTypes] FOREIGN KEY([PaymentTypeID])
REFERENCES [dbo].[PaymentTypes] ([PaymentTypeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IntermediatePaymentOptions] CHECK CONSTRAINT [FK_IntermediatePaymentOptions_PaymentTypes]
GO
ALTER TABLE [dbo].[IntermediateStudyFormOptions]  WITH CHECK ADD  CONSTRAINT [FK_IntermediateStudyFormOptions_StudyFormOptions] FOREIGN KEY([StudyFormOptionID])
REFERENCES [dbo].[StudyFormOptions] ([StudyFormOptionID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IntermediateStudyFormOptions] CHECK CONSTRAINT [FK_IntermediateStudyFormOptions_StudyFormOptions]
GO
ALTER TABLE [dbo].[IntermediateStudyFormOptions]  WITH CHECK ADD  CONSTRAINT [FK_IntermediateStudyFormOptions_StudyFormTypes] FOREIGN KEY([StudyFormTypeID])
REFERENCES [dbo].[StudyFormTypes] ([StudyFormTypeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IntermediateStudyFormOptions] CHECK CONSTRAINT [FK_IntermediateStudyFormOptions_StudyFormTypes]
GO
ALTER TABLE [dbo].[OptionalSubjectsGroups]  WITH CHECK ADD  CONSTRAINT [FK_OptionalSubjectsGroups_SpecialitySubjects] FOREIGN KEY([SpecialitySubjectID])
REFERENCES [dbo].[SpecialitySubjects] ([SpecialitySubjectID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[OptionalSubjectsGroups] CHECK CONSTRAINT [FK_OptionalSubjectsGroups_SpecialitySubjects]
GO
ALTER TABLE [dbo].[OptionalSubjectsGroups]  WITH CHECK ADD  CONSTRAINT [FK_OptionalSubjectsGroups_SpecialityTestOptions] FOREIGN KEY([SpecialityTestOptionID])
REFERENCES [dbo].[SpecialityTestOptions] ([SpecialityTestOptionID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[OptionalSubjectsGroups] CHECK CONSTRAINT [FK_OptionalSubjectsGroups_SpecialityTestOptions]
GO
ALTER TABLE [dbo].[ProfileSubjectsGroups]  WITH CHECK ADD  CONSTRAINT [FK_ProfileSubjectsGroups_SpecialitySubjects] FOREIGN KEY([SpecialitySubjectID])
REFERENCES [dbo].[SpecialitySubjects] ([SpecialitySubjectID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ProfileSubjectsGroups] CHECK CONSTRAINT [FK_ProfileSubjectsGroups_SpecialitySubjects]
GO
ALTER TABLE [dbo].[ProfileSubjectsGroups]  WITH CHECK ADD  CONSTRAINT [FK_ProfileSubjectsGroups_SpecialityTestOptions] FOREIGN KEY([SpecialityTestOptionID])
REFERENCES [dbo].[SpecialityTestOptions] ([SpecialityTestOptionID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ProfileSubjectsGroups] CHECK CONSTRAINT [FK_ProfileSubjectsGroups_SpecialityTestOptions]
GO
ALTER TABLE [dbo].[SpecialityApplications]  WITH CHECK ADD  CONSTRAINT [FK_SpecialityApplications_Applications] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[SpecialityApplications] CHECK CONSTRAINT [FK_SpecialityApplications_Applications]
GO
ALTER TABLE [dbo].[SpecialityApplications]  WITH CHECK ADD  CONSTRAINT [FK_SpecialityApplications_ProfileSubjectsGroups] FOREIGN KEY([ProfileSubjectsGroupID])
REFERENCES [dbo].[ProfileSubjectsGroups] ([ProfileSubjectsGroupID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[SpecialityApplications] CHECK CONSTRAINT [FK_SpecialityApplications_ProfileSubjectsGroups]
GO
ALTER TABLE [dbo].[SpecialityApplications]  WITH CHECK ADD  CONSTRAINT [FK_SpecialityApplications_Specialities] FOREIGN KEY([SpecialityID])
REFERENCES [dbo].[Specialities] ([SpecialityID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[SpecialityApplications] CHECK CONSTRAINT [FK_SpecialityApplications_Specialities]
GO
ALTER TABLE [dbo].[SpecialityApplications]  WITH CHECK ADD  CONSTRAINT [FK_SpecialityApplications_SpecialitySubjects] FOREIGN KEY([OptionalSubjectID])
REFERENCES [dbo].[SpecialitySubjects] ([SpecialitySubjectID])
GO
ALTER TABLE [dbo].[SpecialityApplications] CHECK CONSTRAINT [FK_SpecialityApplications_SpecialitySubjects]
GO
ALTER TABLE [dbo].[SpecialityApplications]  WITH CHECK ADD  CONSTRAINT [FK_SpecialityApplications_StudyFormTypes] FOREIGN KEY([StudyFormTypeID])
REFERENCES [dbo].[StudyFormTypes] ([StudyFormTypeID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[SpecialityApplications] CHECK CONSTRAINT [FK_SpecialityApplications_StudyFormTypes]
GO
ALTER TABLE [dbo].[SpecialitySubjects]  WITH CHECK ADD  CONSTRAINT [FK_SpecialitySubjects_MinScores] FOREIGN KEY([MinScoreID])
REFERENCES [dbo].[MinScores] ([MinScoreID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SpecialitySubjects] CHECK CONSTRAINT [FK_SpecialitySubjects_MinScores]
GO
ALTER TABLE [dbo].[SpecialitySubjects]  WITH CHECK ADD  CONSTRAINT [FK_SpecialitySubjects_Subjects] FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subjects] ([SubjectID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SpecialitySubjects] CHECK CONSTRAINT [FK_SpecialitySubjects_Subjects]
GO
ALTER TABLE [dbo].[UniversitySpecialities]  WITH CHECK ADD  CONSTRAINT [FK_UniversitySpecialities_Colors] FOREIGN KEY([ColorID])
REFERENCES [dbo].[Colors] ([ColorID])
GO
ALTER TABLE [dbo].[UniversitySpecialities] CHECK CONSTRAINT [FK_UniversitySpecialities_Colors]
GO
ALTER TABLE [dbo].[UniversitySpecialities]  WITH CHECK ADD  CONSTRAINT [FK_UniversitySpecialities_Groups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[UniversitySpecialities] CHECK CONSTRAINT [FK_UniversitySpecialities_Groups]
GO
ALTER TABLE [dbo].[UniversitySpecialities]  WITH CHECK ADD  CONSTRAINT [FK_UniversitySpecialities_PaymentOptions] FOREIGN KEY([PaymentOptionID])
REFERENCES [dbo].[PaymentOptions] ([PaymentOptionID])
GO
ALTER TABLE [dbo].[UniversitySpecialities] CHECK CONSTRAINT [FK_UniversitySpecialities_PaymentOptions]
GO
ALTER TABLE [dbo].[UniversitySpecialities]  WITH CHECK ADD  CONSTRAINT [FK_UniversitySpecialities_Specialities] FOREIGN KEY([SpecialityID])
REFERENCES [dbo].[Specialities] ([SpecialityID])
GO
ALTER TABLE [dbo].[UniversitySpecialities] CHECK CONSTRAINT [FK_UniversitySpecialities_Specialities]
GO
ALTER TABLE [dbo].[UniversitySpecialities]  WITH CHECK ADD  CONSTRAINT [FK_UniversitySpecialities_SpecialityCapacityTable] FOREIGN KEY([SpecialityCapacityID])
REFERENCES [dbo].[SpecialityCapacityTable] ([SpecialityCapacityID])
GO
ALTER TABLE [dbo].[UniversitySpecialities] CHECK CONSTRAINT [FK_UniversitySpecialities_SpecialityCapacityTable]
GO
ALTER TABLE [dbo].[UniversitySpecialities]  WITH CHECK ADD  CONSTRAINT [FK_UniversitySpecialities_SpecialityTestOptions] FOREIGN KEY([SpecialityTestOptionID])
REFERENCES [dbo].[SpecialityTestOptions] ([SpecialityTestOptionID])
GO
ALTER TABLE [dbo].[UniversitySpecialities] CHECK CONSTRAINT [FK_UniversitySpecialities_SpecialityTestOptions]
GO
ALTER TABLE [dbo].[UniversitySpecialities]  WITH CHECK ADD  CONSTRAINT [FK_UniversitySpecialities_StudyFormOptions] FOREIGN KEY([StudyFormOptionID])
REFERENCES [dbo].[StudyFormOptions] ([StudyFormOptionID])
GO
ALTER TABLE [dbo].[UniversitySpecialities] CHECK CONSTRAINT [FK_UniversitySpecialities_StudyFormOptions]
GO
USE [master]
GO
ALTER DATABASE [UniversitySpecialtiesDatabase] SET  READ_WRITE 
GO
