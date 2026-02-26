using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chet.CCLR.WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassicBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Subtitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Author = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Dynasty = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CoverImage = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    TotalChapters = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalSentences = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<byte>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassicBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfigKey = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ConfigValue = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassicChapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    BookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TotalSentences = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassicChapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassicChapters_ClassicBooks_BookId",
                        column: x => x.BookId,
                        principalTable: "ClassicBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Operation = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TargetType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TargetId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ExtraData = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ClassicSentences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: false),
                    Pinyin = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    Note = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    Translation = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    AudioUrl = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: false),
                    AudioDuration = table.Column<int>(type: "INTEGER", nullable: true),
                    AudioFileSize = table.Column<int>(type: "INTEGER", nullable: false),
                    AudioFormat = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ChapterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    FavoriteCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassicSentences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassicSentences_ClassicChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ClassicChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserListenRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChapterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SentenceIds = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    ListenDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DurationSec = table.Column<int>(type: "INTEGER", nullable: false),
                    SentenceCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsValidDay = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListenRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserListenRecord_ClassicBooks_BookId",
                        column: x => x.BookId,
                        principalTable: "ClassicBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserListenRecord_ClassicChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ClassicChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserListenRecord_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoriteSentences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SentenceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteSentences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavoriteSentences_ClassicSentences_SentenceId",
                        column: x => x.SentenceId,
                        principalTable: "ClassicSentences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteSentences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserListenProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChapterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SentenceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProgressSec = table.Column<int>(type: "INTEGER", nullable: false),
                    PlaySpeed = table.Column<decimal>(type: "TEXT", precision: 2, scale: 1, nullable: false),
                    AutoScroll = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShowPinyin = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastPlayTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastPositionPercent = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListenProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserListenProgress_ClassicBooks_BookId",
                        column: x => x.BookId,
                        principalTable: "ClassicBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserListenProgress_ClassicChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ClassicChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserListenProgress_ClassicSentences_SentenceId",
                        column: x => x.SentenceId,
                        principalTable: "ClassicSentences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserListenProgress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassicChapters_BookId",
                table: "ClassicChapters",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassicSentences_ChapterId",
                table: "ClassicSentences",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLogs_CreatedAt",
                table: "OperationLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLogs_Operation",
                table: "OperationLogs",
                column: "Operation");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLogs_TargetType_TargetId",
                table: "OperationLogs",
                columns: new[] { "TargetType", "TargetId" });

            migrationBuilder.CreateIndex(
                name: "IX_OperationLogs_UserId",
                table: "OperationLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigs_ConfigKey",
                table: "SystemConfigs",
                column: "ConfigKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteSentences_SentenceId",
                table: "UserFavoriteSentences",
                column: "SentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteSentences_UserId_SentenceId",
                table: "UserFavoriteSentences",
                columns: new[] { "UserId", "SentenceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserListenProgress_BookId",
                table: "UserListenProgress",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListenProgress_ChapterId",
                table: "UserListenProgress",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListenProgress_SentenceId",
                table: "UserListenProgress",
                column: "SentenceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListenProgress_UserId_BookId",
                table: "UserListenProgress",
                columns: new[] { "UserId", "BookId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserListenRecord_BookId",
                table: "UserListenRecord",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListenRecord_ChapterId",
                table: "UserListenRecord",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListenRecord_UserId_ListenDate",
                table: "UserListenRecord",
                columns: new[] { "UserId", "ListenDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationLogs");

            migrationBuilder.DropTable(
                name: "SystemConfigs");

            migrationBuilder.DropTable(
                name: "UserFavoriteSentences");

            migrationBuilder.DropTable(
                name: "UserListenProgress");

            migrationBuilder.DropTable(
                name: "UserListenRecord");

            migrationBuilder.DropTable(
                name: "ClassicSentences");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClassicChapters");

            migrationBuilder.DropTable(
                name: "ClassicBooks");
        }
    }
}
