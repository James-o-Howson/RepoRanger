using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepoRanger.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dependencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DependencySources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DependencySources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RetryCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Processed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProcessStartTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ProcessFinishedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    LastErrorDetails = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VersionControlSystems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionControlSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DependencyVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DependencyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DependencyVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DependencyVersions_Dependencies_DependencyId",
                        column: x => x.DependencyId,
                        principalTable: "Dependencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RemoteUrl = table.Column<string>(type: "TEXT", nullable: false),
                    VersionControlSystemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefaultBranch = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                    table.UniqueConstraint("AK_Repositories_VersionControlSystemId_Name_RemoteUrl", x => new { x.VersionControlSystemId, x.Name, x.RemoteUrl });
                    table.ForeignKey(
                        name: "FK_Repositories_VersionControlSystems_VersionControlSystemId",
                        column: x => x.VersionControlSystemId,
                        principalTable: "VersionControlSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DependencySourceDependencyVersion",
                columns: table => new
                {
                    SourcesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VersionsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DependencySourceDependencyVersion", x => new { x.SourcesId, x.VersionsId });
                    table.ForeignKey(
                        name: "FK_DependencySourceDependencyVersion_DependencySources_SourcesId",
                        column: x => x.SourcesId,
                        principalTable: "DependencySources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DependencySourceDependencyVersion_DependencyVersions_VersionsId",
                        column: x => x.VersionsId,
                        principalTable: "DependencyVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vulnerabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OsvId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DependencyVersionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Published = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Withdrawn = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Summary = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vulnerabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vulnerabilities_DependencyVersions_DependencyVersionId",
                        column: x => x.DependencyVersionId,
                        principalTable: "DependencyVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type_Value = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.UniqueConstraint("AK_Projects_RepositoryId_Name_Path", x => new { x.RepositoryId, x.Name, x.Path });
                    table.ForeignKey(
                        name: "FK_Projects_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDependencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DependencyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VersionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    VersionControlSystemId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDependencies_Dependencies_DependencyId",
                        column: x => x.DependencyId,
                        principalTable: "Dependencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectDependencies_DependencySources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "DependencySources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectDependencies_DependencyVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "DependencyVersions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectDependencies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDependencies_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectDependencies_VersionControlSystems_VersionControlSystemId",
                        column: x => x.VersionControlSystemId,
                        principalTable: "VersionControlSystems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMetadata_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dependencies_Name",
                table: "Dependencies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DependencySourceDependencyVersion_VersionsId",
                table: "DependencySourceDependencyVersion",
                column: "VersionsId");

            migrationBuilder.CreateIndex(
                name: "IX_DependencySources_Name",
                table: "DependencySources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DependencyVersions_DependencyId",
                table: "DependencyVersions",
                column: "DependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDependencies_DependencyId",
                table: "ProjectDependencies",
                column: "DependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDependencies_ProjectId",
                table: "ProjectDependencies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDependencies_RepositoryId",
                table: "ProjectDependencies",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDependencies_SourceId",
                table: "ProjectDependencies",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDependencies_VersionControlSystemId",
                table: "ProjectDependencies",
                column: "VersionControlSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDependencies_VersionId",
                table: "ProjectDependencies",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMetadata_ProjectId",
                table: "ProjectMetadata",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionControlSystems_Name",
                table: "VersionControlSystems",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vulnerabilities_DependencyVersionId",
                table: "Vulnerabilities",
                column: "DependencyVersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DependencySourceDependencyVersion");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ProjectDependencies");

            migrationBuilder.DropTable(
                name: "ProjectMetadata");

            migrationBuilder.DropTable(
                name: "Vulnerabilities");

            migrationBuilder.DropTable(
                name: "DependencySources");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "DependencyVersions");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropTable(
                name: "Dependencies");

            migrationBuilder.DropTable(
                name: "VersionControlSystems");
        }
    }
}
