using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deepin.Infrastructure.Migrations.Chats
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "chats");

            migrationBuilder.CreateTable(
                name: "chat_join_requests",
                schema: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    reviewed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    reviewed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_join_requests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chats",
                schema: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    avatar_file_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat_members",
                schema: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    is_muted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_banned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    joined_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_members", x => x.id);
                    table.ForeignKey(
                        name: "FK_chat_members_chats_chat_id",
                        column: x => x.chat_id,
                        principalSchema: "chats",
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_messages",
                schema: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: true),
                    sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_chat_messages_chats_chat_id",
                        column: x => x.chat_id,
                        principalSchema: "chats",
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_read_statuses",
                schema: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    last_read_message_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_read_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_read_statuses", x => x.id);
                    table.ForeignKey(
                        name: "FK_chat_read_statuses_chats_chat_id",
                        column: x => x.chat_id,
                        principalSchema: "chats",
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_settings",
                schema: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_pinned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_muted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    notification_level = table.Column<string>(type: "text", nullable: false, defaultValue: "All"),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_chat_settings_chats_chat_id",
                        column: x => x.chat_id,
                        principalSchema: "chats",
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_join_requests_chat_id_user_id",
                schema: "chats",
                table: "chat_join_requests",
                columns: new[] { "chat_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_members_chat_id",
                schema: "chats",
                table: "chat_members",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_chat_id_message_id",
                schema: "chats",
                table: "chat_messages",
                columns: new[] { "chat_id", "message_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_read_statuses_chat_id",
                schema: "chats",
                table: "chat_read_statuses",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_settings_chat_id",
                schema: "chats",
                table: "chat_settings",
                column: "chat_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_join_requests",
                schema: "chats");

            migrationBuilder.DropTable(
                name: "chat_members",
                schema: "chats");

            migrationBuilder.DropTable(
                name: "chat_messages",
                schema: "chats");

            migrationBuilder.DropTable(
                name: "chat_read_statuses",
                schema: "chats");

            migrationBuilder.DropTable(
                name: "chat_settings",
                schema: "chats");

            migrationBuilder.DropTable(
                name: "chats",
                schema: "chats");
        }
    }
}
