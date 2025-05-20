using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20250410)]
    public class CreateProductsTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            // Check if the 'Products' table exists before creating it
            if (!Schema.Table("Products").Exists())
            {
                Create.Table("Products")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("Name").AsString(255).NotNullable()
                    .WithColumn("Price").AsDecimal(19, 5).NotNullable()
                    .WithColumn("CreatedAt").AsDateTime().Nullable()
                    .WithColumn("Description").AsString(int.MaxValue).Nullable()
                    .WithColumn("Stock").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsAvailable").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("Category").AsString(255).Nullable()
                    .WithColumn("Sku").AsString(255).Nullable()
                    .WithColumn("Supplier").AsString(255).NotNullable()
                    .WithColumn("Discount").AsDecimal(19, 5).Nullable()
                    .WithColumn("UserID").AsInt32().Nullable();
            }
        }

        public override void Down()
        {
            // Drop the 'Products' table if it exists
            if (Schema.Table("Products").Exists())
            {
                Delete.Table("Products");
            }
        }
    }
}
