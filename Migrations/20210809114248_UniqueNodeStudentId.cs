using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectAllocationSystem.Migrations
{
    public partial class UniqueNodeStudentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LecturerStudentNodes_StudentId",
                table: "LecturerStudentNodes",
                column: "StudentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LecturerStudentNodes_StudentId",
                table: "LecturerStudentNodes");
        }
    }
}
