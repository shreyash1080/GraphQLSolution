using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20250410)]
    public class CreateProductsTable : Migration
    {
        public override void Up()
        {
            // Drop the table only if it exists
            if (Schema.Table("users").Exists())
            {
                Create.Table("Products")
                        .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                        .WithColumn("Name").AsString(255).NotNullable()
                        .WithColumn("Price").AsDecimal().NotNullable()
                        .WithColumn("CreatedAt").AsDateTime().Nullable()
                        .WithColumn("Description").AsString(int.MaxValue).Nullable()
                        .WithColumn("Stock").AsInt32().WithDefaultValue(0).NotNullable()
                        .WithColumn("IsAvailable").AsBoolean().WithDefaultValue(true).NotNullable()
                        .WithColumn("Category").AsString(255).Nullable();
            }
        }

        public override void Down()
        {
            // Drop the Products table
            if (Schema.Table("Products").Exists())
            {
                Delete.Table("Products");
            }
        }
    }
}
