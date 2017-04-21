using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ESPL.KP.Entities;

namespace ESPL.KP.Migrations
{
    [DbContext(typeof(LibraryContext))]
    partial class LibraryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ESPL.KP.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DateOfBirth");

                    b.Property<DateTimeOffset?>("DateOfDeath");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("ESPL.KP.Entities.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AuthorId");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("ESPL.KP.Entities.CfgUserArea", b =>
                {
                    b.Property<Guid>("UserAreaID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AreaID");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDelete");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.Property<string>("UserID");

                    b.HasKey("UserAreaID");

                    b.HasIndex("AreaID");

                    b.HasIndex("UserID");

                    b.ToTable("CfgUserArea");
                });

            modelBuilder.Entity("ESPL.KP.Entities.CfgUserDepartment", b =>
                {
                    b.Property<Guid>("UserDepartmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("DepartmentID");

                    b.Property<bool>("IsDelete");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.Property<string>("UserID");

                    b.HasKey("UserDepartmentID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("UserID");

                    b.ToTable("CfgUserDepartment");
                });

            modelBuilder.Entity("ESPL.KP.Entities.CfgUserDesignation", b =>
                {
                    b.Property<Guid>("UserDesignationID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("DesignationID");

                    b.Property<bool>("IsDelete");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.Property<string>("UserID");

                    b.HasKey("UserDesignationID");

                    b.HasIndex("DesignationID");

                    b.HasIndex("UserID");

                    b.ToTable("CfgUserDesignation");
                });

            modelBuilder.Entity("ESPL.KP.Entities.CfgUserShift", b =>
                {
                    b.Property<Guid>("UserShiftID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDelete");

                    b.Property<Guid>("ShiftID");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.Property<string>("UserID");

                    b.HasKey("UserShiftID");

                    b.HasIndex("ShiftID");

                    b.HasIndex("UserID");

                    b.ToTable("CfgUserShift");
                });

            modelBuilder.Entity("ESPL.KP.Entities.ESPLUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address1")
                        .HasMaxLength(500);

                    b.Property<string>("Address2")
                        .HasMaxLength(500);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("FailedPasswordAttemptCount");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastLogin");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Mobile");

                    b.Property<Guid?>("MstAreaAreaID");

                    b.Property<Guid?>("MstDesignationDesignationID");

                    b.Property<Guid?>("MstShiftShiftID");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("MstAreaAreaID");

                    b.HasIndex("MstDesignationDesignationID");

                    b.HasIndex("MstShiftShiftID");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstArea", b =>
                {
                    b.Property<Guid>("AreaID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AreaCode")
                        .HasMaxLength(20);

                    b.Property<string>("AreaName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("PinCode")
                        .HasMaxLength(20);

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("AreaID");

                    b.ToTable("MstArea");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstDepartment", b =>
                {
                    b.Property<Guid>("DepartmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("DepartmentDespcription")
                        .HasMaxLength(500);

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsDelete");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("DepartmentID");

                    b.ToTable("MstDepartment");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstDesignation", b =>
                {
                    b.Property<Guid>("DesignationID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("DesignationCode")
                        .HasMaxLength(20);

                    b.Property<string>("DesignationName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsDelete");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("DesignationID");

                    b.ToTable("MstDesignation");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstOccurrenceBook", b =>
                {
                    b.Property<Guid>("OBID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AreaID");

                    b.Property<string>("CaseFileNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("DepartmentID");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("NatureOfOccurrence")
                        .IsRequired();

                    b.Property<string>("OBNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("OBTime");

                    b.Property<Guid>("OBTypeID");

                    b.Property<string>("Remark")
                        .HasMaxLength(50);

                    b.Property<Guid>("StatusID");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("OBID");

                    b.HasIndex("AreaID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("OBTypeID");

                    b.HasIndex("StatusID");

                    b.ToTable("MstOccurrenceBook");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstOccurrenceType", b =>
                {
                    b.Property<Guid>("OBTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("OBTypeName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("OBTypeID");

                    b.ToTable("MstOccurrenceType");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstPermission", b =>
                {
                    b.Property<Guid>("PermissionID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("FormName");

                    b.Property<bool>("IsDelete");

                    b.Property<int>("LogicalSequence");

                    b.Property<string>("PermissionName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid>("PermissionParentID");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("PermissionID");

                    b.ToTable("MstPermission");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstShift", b =>
                {
                    b.Property<Guid>("ShiftID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<TimeSpan>("EndTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("ShiftName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<TimeSpan>("StartTime");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("ShiftID");

                    b.ToTable("MstShift");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstStatus", b =>
                {
                    b.Property<Guid>("StatusID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("StatusID");

                    b.ToTable("MstStatus");
                });

            modelBuilder.Entity("ESPL.KP.Entities.OccurrenceAssignment", b =>
                {
                    b.Property<Guid>("OBAssignmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AssignedTO");

                    b.Property<string>("Comments")
                        .HasMaxLength(500);

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDelete");

                    b.Property<Guid>("OBID");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("OBAssignmentID");

                    b.HasIndex("AssignedTO");

                    b.HasIndex("OBID");

                    b.ToTable("OccurrenceAssignment");
                });

            modelBuilder.Entity("ESPL.KP.Entities.OccurrenceReviewHistory", b =>
                {
                    b.Property<Guid>("OBReviewHistoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDelete");

                    b.Property<Guid>("OBID");

                    b.Property<string>("ReveiwComments")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("OBReviewHistoryID");

                    b.HasIndex("OBID");

                    b.ToTable("OccurrenceReviewHistory");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ESPL.KP.Entities.Book", b =>
                {
                    b.HasOne("ESPL.KP.Entities.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESPL.KP.Entities.CfgUserArea", b =>
                {
                    b.HasOne("ESPL.KP.Entities.MstArea", "MstArea")
                        .WithMany()
                        .HasForeignKey("AreaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESPL.KP.Entities.ESPLUser", "ESPLUser")
                        .WithMany("CfgUserArea")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("ESPL.KP.Entities.CfgUserDepartment", b =>
                {
                    b.HasOne("ESPL.KP.Entities.MstDepartment", "MstDepartment")
                        .WithMany("CfgUserDepartment")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESPL.KP.Entities.ESPLUser", "ESPLUser")
                        .WithMany("CfgUserDepartments")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("ESPL.KP.Entities.CfgUserDesignation", b =>
                {
                    b.HasOne("ESPL.KP.Entities.MstDesignation", "MstDesignation")
                        .WithMany()
                        .HasForeignKey("DesignationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESPL.KP.Entities.ESPLUser", "ESPLUser")
                        .WithMany("CfgUserDesignation")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("ESPL.KP.Entities.CfgUserShift", b =>
                {
                    b.HasOne("ESPL.KP.Entities.MstShift", "MstShift")
                        .WithMany()
                        .HasForeignKey("ShiftID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESPL.KP.Entities.ESPLUser", "ESPLUser")
                        .WithMany("CfgUserShift")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("ESPL.KP.Entities.ESPLUser", b =>
                {
                    b.HasOne("ESPL.KP.Entities.MstArea")
                        .WithMany("ESPLUser")
                        .HasForeignKey("MstAreaAreaID");

                    b.HasOne("ESPL.KP.Entities.MstDesignation")
                        .WithMany("ESPLUser")
                        .HasForeignKey("MstDesignationDesignationID");

                    b.HasOne("ESPL.KP.Entities.MstShift")
                        .WithMany("ESPLUser")
                        .HasForeignKey("MstShiftShiftID");
                });

            modelBuilder.Entity("ESPL.KP.Entities.MstOccurrenceBook", b =>
                {
                    b.HasOne("ESPL.KP.Entities.MstArea", "MstArea")
                        .WithMany("MstOccurrenceBooks")
                        .HasForeignKey("AreaID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESPL.KP.Entities.MstDepartment", "MstDepartment")
                        .WithMany("MstOccurrenceBooks")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESPL.KP.Entities.MstOccurrenceType", "MstOccurrenceType")
                        .WithMany("MstOccurrenceBooks")
                        .HasForeignKey("OBTypeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESPL.KP.Entities.MstStatus", "MstStatus")
                        .WithMany("MstOccurrenceBooks")
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESPL.KP.Entities.OccurrenceAssignment", b =>
                {
                    b.HasOne("ESPL.KP.Entities.ESPLUser", "ESPLUser")
                        .WithMany("OccurrenceAssignments")
                        .HasForeignKey("AssignedTO");

                    b.HasOne("ESPL.KP.Entities.MstOccurrenceBook", "MstOccurrenceBook")
                        .WithMany("OccurrenceAssignments")
                        .HasForeignKey("OBID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESPL.KP.Entities.OccurrenceReviewHistory", b =>
                {
                    b.HasOne("ESPL.KP.Entities.MstOccurrenceBook", "MstOccurrenceBook")
                        .WithMany("OccurrenceReveiwHistories")
                        .HasForeignKey("OBID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ESPL.KP.Entities.ESPLUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ESPL.KP.Entities.ESPLUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESPL.KP.Entities.ESPLUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
