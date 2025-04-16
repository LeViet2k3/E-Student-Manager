using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    MaHP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenHP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoTinChi = table.Column<int>(type: "int", nullable: false),
                    KiHoc = table.Column<int>(type: "int", nullable: false),
                    NamHoc = table.Column<int>(type: "int", nullable: false),
                    Khoa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nganh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.MaHP);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    MaSV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Khoa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nganh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.MaSV);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    MaGV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Khoa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.MaGV);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlans",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Khoa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nganh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HocKi = table.Column<int>(type: "int", nullable: false),
                    MaHP = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlans", x => x.MaKH);
                    table.ForeignKey(
                        name: "FK_StudyPlans_Courses_MaHP",
                        column: x => x.MaHP,
                        principalTable: "Courses",
                        principalColumn: "MaHP",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                columns: table => new
                {
                    MaCTDT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Khoa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nganh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaHP = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.MaCTDT);
                    table.ForeignKey(
                        name: "FK_TrainingPrograms_Courses_MaHP",
                        column: x => x.MaHP,
                        principalTable: "Courses",
                        principalColumn: "MaHP",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseClasses",
                columns: table => new
                {
                    MaLHP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaHP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaGV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhongHoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tiet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiSoToiDa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClasses", x => x.MaLHP);
                    table.ForeignKey(
                        name: "FK_CourseClasses_Courses_MaHP",
                        column: x => x.MaHP,
                        principalTable: "Courses",
                        principalColumn: "MaHP",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseClasses_Teachers_MaGV",
                        column: x => x.MaGV,
                        principalTable: "Teachers",
                        principalColumn: "MaGV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseRegistrations",
                columns: table => new
                {
                    MaDK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLHP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayDK = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRegistrations", x => x.MaDK);
                    table.ForeignKey(
                        name: "FK_CourseRegistrations_CourseClasses_MaLHP",
                        column: x => x.MaLHP,
                        principalTable: "CourseClasses",
                        principalColumn: "MaLHP",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseRegistrations_Students_MaSV",
                        column: x => x.MaSV,
                        principalTable: "Students",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    MaDiem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLHP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiemQT = table.Column<double>(type: "float", nullable: false),
                    DiemThi = table.Column<double>(type: "float", nullable: false),
                    DiemTong = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.MaDiem);
                    table.ForeignKey(
                        name: "FK_Grades_CourseClasses_MaLHP",
                        column: x => x.MaLHP,
                        principalTable: "CourseClasses",
                        principalColumn: "MaLHP",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Students_MaSV",
                        column: x => x.MaSV,
                        principalTable: "Students",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    MaTKB = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLHP = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.MaTKB);
                    table.ForeignKey(
                        name: "FK_Schedules_CourseClasses_MaLHP",
                        column: x => x.MaLHP,
                        principalTable: "CourseClasses",
                        principalColumn: "MaLHP",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schedules_Students_MaSV",
                        column: x => x.MaSV,
                        principalTable: "Students",
                        principalColumn: "MaSV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeachingSchedules",
                columns: table => new
                {
                    MaLD = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaGV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLHP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDay = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingSchedules", x => x.MaLD);
                    table.ForeignKey(
                        name: "FK_TeachingSchedules_CourseClasses_MaLHP",
                        column: x => x.MaLHP,
                        principalTable: "CourseClasses",
                        principalColumn: "MaLHP",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeachingSchedules_Teachers_MaGV",
                        column: x => x.MaGV,
                        principalTable: "Teachers",
                        principalColumn: "MaGV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseClasses_MaGV",
                table: "CourseClasses",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClasses_MaHP",
                table: "CourseClasses",
                column: "MaHP");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRegistrations_MaLHP",
                table: "CourseRegistrations",
                column: "MaLHP");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRegistrations_MaSV",
                table: "CourseRegistrations",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_MaLHP",
                table: "Grades",
                column: "MaLHP");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_MaSV",
                table: "Grades",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_MaLHP",
                table: "Schedules",
                column: "MaLHP");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_MaSV",
                table: "Schedules",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_MaHP",
                table: "StudyPlans",
                column: "MaHP");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingSchedules_MaGV",
                table: "TeachingSchedules",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingSchedules_MaLHP",
                table: "TeachingSchedules",
                column: "MaLHP");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_MaHP",
                table: "TrainingPrograms",
                column: "MaHP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseRegistrations");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "StudyPlans");

            migrationBuilder.DropTable(
                name: "TeachingSchedules");

            migrationBuilder.DropTable(
                name: "TrainingPrograms");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "CourseClasses");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
