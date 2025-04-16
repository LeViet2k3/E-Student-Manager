using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseRegistrationKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseRegistrations",
                table: "CourseRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_CourseRegistrations_MaSV",
                table: "CourseRegistrations");

            migrationBuilder.DropColumn(
                name: "MaDK",
                table: "CourseRegistrations");

            migrationBuilder.AlterColumn<string>(
                name: "NamHoc",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MaSV",
                table: "CourseRegistrations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "MaLHP",
                table: "CourseRegistrations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseRegistrations",
                table: "CourseRegistrations",
                columns: new[] { "MaSV", "MaLHP" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseRegistrations",
                table: "CourseRegistrations");

            migrationBuilder.AlterColumn<int>(
                name: "NamHoc",
                table: "Courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaLHP",
                table: "CourseRegistrations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "MaSV",
                table: "CourseRegistrations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<string>(
                name: "MaDK",
                table: "CourseRegistrations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseRegistrations",
                table: "CourseRegistrations",
                column: "MaDK");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRegistrations_MaSV",
                table: "CourseRegistrations",
                column: "MaSV");
        }
    }
}
