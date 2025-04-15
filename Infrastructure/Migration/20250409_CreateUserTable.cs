using FluentMigrator;

[Migration(20240101000000)] // Use a unique timestamp-based migration ID
public class CreateUsersTable : Migration
{
    public override void Up()
    {
        if (!Schema.Table("users").Exists())
        {
            Create.Table("users")
            .WithColumn("user_id").AsInt32().PrimaryKey().Identity()
            .WithColumn("email").AsString(255).NotNullable().Unique()
            .WithColumn("password_hash").AsString(255).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
            .WithColumn("updated_at").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
            .WithColumn("first_name").AsString(50).Nullable()
            .WithColumn("last_name").AsString(50).Nullable();
        }
    }

    public override void Down()
    {
        // Drop the table only if it exists
        if (Schema.Table("users").Exists())
        {
            Delete.Table("users");
        }
    }
}