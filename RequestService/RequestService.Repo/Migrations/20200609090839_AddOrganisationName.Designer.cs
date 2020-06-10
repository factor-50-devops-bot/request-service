﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RequestService.Repo;

namespace RequestService.Repo.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200609090839_AddOrganisationName")]
    partial class AddOrganisationName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HelpMyStreet.PostcodeCoordinates.EF.Entities.PostcodeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(0)")
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Postcode")
                        .IsUnique()
                        .HasName("UX_Postcode_Postcode");

                    b.HasIndex("Postcode", "IsActive")
                        .HasName("IX_Postcode_Postcode_IsActive")
                        .HasAnnotation("SqlServer:Include", new[] { "Latitude", "Longitude" });

                    b.HasIndex("Latitude", "Longitude", "IsActive")
                        .HasName("IX_Postcode_Latitude_Longitude_IsActive")
                        .HasAnnotation("SqlServer:Include", new[] { "Postcode" });

                    b.ToTable("Postcode","Address");
                });

            modelBuilder.Entity("HelpMyStreet.PostcodeCoordinates.EF.Entities.PostcodeEntityOldEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(0)")
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Postcode")
                        .IsUnique()
                        .HasName("UX_Postcode_Postcode");

                    b.HasIndex("Postcode", "IsActive")
                        .HasName("IX_Postcode_Postcode_IsActive")
                        .HasAnnotation("SqlServer:Include", new[] { "Latitude", "Longitude" });

                    b.HasIndex("Latitude", "Longitude", "IsActive")
                        .HasName("IX_Postcode_Latitude_Longitude_IsActive")
                        .HasAnnotation("SqlServer:Include", new[] { "Postcode" });

                    b.ToTable("Postcode_Old","Staging");
                });

            modelBuilder.Entity("HelpMyStreet.PostcodeCoordinates.EF.Entities.PostcodeEntitySwitchEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(0)")
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Postcode")
                        .IsUnique()
                        .HasName("UX_Postcode_Postcode");

                    b.HasIndex("Postcode", "IsActive")
                        .HasName("IX_Postcode_Postcode_IsActive")
                        .HasAnnotation("SqlServer:Include", new[] { "Latitude", "Longitude" });

                    b.HasIndex("Latitude", "Longitude", "IsActive")
                        .HasName("IX_Postcode_Latitude_Longitude_IsActive")
                        .HasAnnotation("SqlServer:Include", new[] { "Postcode" });

                    b.ToTable("Postcode_Switch","Staging");
                });

            modelBuilder.Entity("HelpMyStreet.PostcodeCoordinates.EF.Entities.PostcodeStagingEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Postcode_Staging","Staging");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.ActivityQuestions", b =>
                {
                    b.Property<int>("ActivityId")
                        .HasColumnName("ActivityID");

                    b.Property<int>("QuestionId")
                        .HasColumnName("QuestionID");

                    b.Property<int>("Order")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.HasKey("ActivityId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("ActivityQuestions","QuestionSet");

                    b.HasData(
                        new
                        {
                            ActivityId = 1,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 1,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 2,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 2,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 3,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 3,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 4,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 4,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 5,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 5,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 6,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 6,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 7,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 7,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 8,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 8,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 9,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 9,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 10,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 10,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 11,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 11,
                            QuestionId = 6,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 12,
                            QuestionId = 2,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 12,
                            QuestionId = 3,
                            Order = 2
                        },
                        new
                        {
                            ActivityId = 12,
                            QuestionId = 4,
                            Order = 3
                        },
                        new
                        {
                            ActivityId = 12,
                            QuestionId = 5,
                            Order = 4
                        },
                        new
                        {
                            ActivityId = 13,
                            QuestionId = 1,
                            Order = 1
                        },
                        new
                        {
                            ActivityId = 13,
                            QuestionId = 6,
                            Order = 2
                        });
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Details")
                        .IsUnicode(false);

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsHealthCritical");

                    b.Property<byte?>("JobStatusId")
                        .HasColumnName("JobStatusID");

                    b.Property<int>("RequestId");

                    b.Property<byte>("SupportActivityId")
                        .HasColumnName("SupportActivityID");

                    b.Property<int?>("VolunteerUserId")
                        .HasColumnName("VolunteerUserID");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("Job","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressLine1")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("AddressLine2")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("AddressLine3")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Locality")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(15)
                        .IsUnicode(false);

                    b.Property<string>("OtherPhone")
                        .HasMaxLength(15)
                        .IsUnicode(false);

                    b.Property<string>("Postcode")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Person","RequestPersonal");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.PersonalDetails", b =>
                {
                    b.Property<int>("RequestId")
                        .HasColumnName("RequestID");

                    b.Property<string>("FurtherDetails")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.Property<bool>("OnBehalfOfAnother");

                    b.Property<string>("RequestorEmailAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("RequestorFirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("RequestorLastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("RequestorPhoneNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.HasKey("RequestId");

                    b.ToTable("PersonalDetails","RequestPersonal");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalData")
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<byte>("QuestionType");

                    b.Property<bool>("Required");

                    b.HasKey("Id");

                    b.ToTable("Question","QuestionSet");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AdditionalData = "[]",
                            Name = "Please tell us more about the help or support you're requesting",
                            QuestionType = (byte)3,
                            Required = false
                        },
                        new
                        {
                            Id = 2,
                            AdditionalData = "[]",
                            Name = "Please tell us about any specific requirements (e.g. colour, style etc.)",
                            QuestionType = (byte)3,
                            Required = false
                        },
                        new
                        {
                            Id = 3,
                            AdditionalData = "[]",
                            Name = "How many face coverings do you need?",
                            QuestionType = (byte)1,
                            Required = true
                        },
                        new
                        {
                            Id = 4,
                            AdditionalData = "[{\"Key\":\"keyworkers\",\"Value\":\"Key workers\"},{\"Key\":\"somonekeyworkers\",\"Value\":\"Someone helping key workers stay safe in their role (e.g. care home residents, visitors etc.)\"},{\"Key\":\"someone\",\"Value\":\"Someone else\"}]",
                            Name = "Who will be using the face coverings?",
                            QuestionType = (byte)4,
                            Required = false
                        },
                        new
                        {
                            Id = 5,
                            AdditionalData = "[{\"Key\":\"Yes\",\"Value\":\"Yes\"},{\"Key\":\"No\",\"Value\":\"No\"},{\"Key\":\"Contribution\",\"Value\":\"I can make a contribution\"}]",
                            Name = "Are you able to pay the cost of materials for your face covering (usually £2 - £3 each)?",
                            QuestionType = (byte)4,
                            Required = false
                        },
                        new
                        {
                            Id = 6,
                            AdditionalData = "[{\"Key\":\"true\",\"Value\":\"Yes\"},{\"Key\":\"false\",\"Value\":\"No\"}]",
                            Name = "Is this request critical to someone's health or wellbeing?",
                            QuestionType = (byte)4,
                            Required = true
                        });
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("AcceptedTerms");

                    b.Property<bool>("CommunicationSent");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnName("CreatedByUserID");

                    b.Property<DateTime>("DateRequested")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<bool?>("ForRequestor");

                    b.Property<byte?>("FulfillableStatus");

                    b.Property<bool>("IsFulfillable");

                    b.Property<string>("OrganisationName")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("OtherDetails")
                        .IsUnicode(false);

                    b.Property<int?>("PersonIdRecipient")
                        .HasColumnName("PersonID_Recipient");

                    b.Property<int?>("PersonIdRequester")
                        .HasColumnName("PersonID_Requester");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<bool?>("ReadPrivacyNotice");

                    b.Property<byte?>("RequestorType");

                    b.Property<string>("SpecialCommunicationNeeds")
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("PersonIdRecipient");

                    b.HasIndex("PersonIdRequester");

                    b.ToTable("Request","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.RequestJobStatus", b =>
                {
                    b.Property<int>("JobId")
                        .HasColumnName("JobID");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<byte>("JobStatusId")
                        .HasColumnName("JobStatusID");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnName("CreatedByUserID");

                    b.Property<int?>("VolunteerUserId")
                        .HasColumnName("VolunteerUserID");

                    b.HasKey("JobId", "DateCreated", "JobStatusId");

                    b.ToTable("RequestJobStatus","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.RequestQuestions", b =>
                {
                    b.Property<int>("RequestId")
                        .HasColumnName("RequestID");

                    b.Property<int>("QuestionId")
                        .HasColumnName("QuestionID");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .IsUnicode(false);

                    b.HasKey("RequestId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("RequestQuestions","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.SupportActivities", b =>
                {
                    b.Property<int>("RequestId")
                        .HasColumnName("RequestID");

                    b.Property<int>("ActivityId")
                        .HasColumnName("ActivityID");

                    b.HasKey("RequestId", "ActivityId");

                    b.ToTable("SupportActivities","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.ActivityQuestions", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Question", "Question")
                        .WithMany("ActivityQuestions")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Job", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Request", "NewRequest")
                        .WithMany("Job")
                        .HasForeignKey("RequestId")
                        .HasConstraintName("FK_NewRequest_NewRequestID");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.PersonalDetails", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Request", "Request")
                        .WithOne("PersonalDetails")
                        .HasForeignKey("RequestService.Repo.EntityFramework.Entities.PersonalDetails", "RequestId")
                        .HasConstraintName("FK_PersonalDetails_RequestID");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Request", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Person", "PersonIdRecipientNavigation")
                        .WithMany("RequestPersonIdRecipientNavigation")
                        .HasForeignKey("PersonIdRecipient")
                        .HasConstraintName("FK_RequestPersonal_Person_PersonID_Recipient");

                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Person", "PersonIdRequesterNavigation")
                        .WithMany("RequestPersonIdRequesterNavigation")
                        .HasForeignKey("PersonIdRequester")
                        .HasConstraintName("FK_RequestPersonal_Person_PersonID_Requester");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.RequestJobStatus", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Job", "Job")
                        .WithMany("RequestJobStatus")
                        .HasForeignKey("JobId")
                        .HasConstraintName("FK_Job_JobID");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.RequestQuestions", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Question", "Question")
                        .WithMany("RequestQuestions")
                        .HasForeignKey("QuestionId");

                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Request", "Request")
                        .WithMany("RequestQuestions")
                        .HasForeignKey("RequestId");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.SupportActivities", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Request", "Request")
                        .WithMany("SupportActivities")
                        .HasForeignKey("RequestId")
                        .HasConstraintName("FK_SupportActivities_RequestID");
                });
#pragma warning restore 612, 618
        }
    }
}
