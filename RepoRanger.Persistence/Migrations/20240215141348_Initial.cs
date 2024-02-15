using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepoRanger.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RemoteUrl = table.Column<string>(type: "TEXT", nullable: false),
                    SourceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefaultBranchId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repositories_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefaultRepositoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Repositories_DefaultRepositoryId",
                        column: x => x.DefaultRepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branches_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dependencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SourceId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dependencies_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dependencies_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BranchProject",
                columns: table => new
                {
                    BranchesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchProject", x => new { x.BranchesId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_BranchProject_Branches_BranchesId",
                        column: x => x.BranchesId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DependencyProject",
                columns: table => new
                {
                    DependenciesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DependencyProject", x => new { x.DependenciesId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_DependencyProject_Dependencies_DependenciesId",
                        column: x => x.DependenciesId,
                        principalTable: "Dependencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DependencyProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchProject_ProjectsId",
                table: "BranchProject",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_DefaultRepositoryId",
                table: "Branches",
                column: "DefaultRepositoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_RepositoryId",
                table: "Branches",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dependencies_RepositoryId",
                table: "Dependencies",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dependencies_SourceId",
                table: "Dependencies",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DependencyProject_ProjectsId",
                table: "DependencyProject",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_SourceId",
                table: "Repositories",
                column: "SourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchProject");

            migrationBuilder.DropTable(
                name: "DependencyProject");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Dependencies");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
