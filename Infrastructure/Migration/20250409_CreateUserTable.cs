using FluentMigrator;

[Migration(20240101000000)] // Use a unique timestamp-based migration ID
public class CreateUsersTable : Migration
{
    public override void Up()
    {
        Create.Table("users")
            // Essential Fields
            .WithColumn("user_id").AsInt32().PrimaryKey().Identity()
            .WithColumn("email").AsString(255).NotNullable().Unique()
            .WithColumn("password_hash").AsString(255).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
            .WithColumn("first_name").AsString(50).Nullable()
            .WithColumn("last_name").AsString(50).Nullable();
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}