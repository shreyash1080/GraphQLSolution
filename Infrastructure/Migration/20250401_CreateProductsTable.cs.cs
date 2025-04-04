using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20250401)] // Unique ID for migration
    public class CreateProductsTable : Migration
    {
        public override void Up()
        {
            this.CreateTableIfNotExists("Products", table =>
            {
                table.WithColumn("Id").AsInt32().PrimaryKey().Identity();
                table.WithColumn("Name").AsString(255).NotNullable();
                table.WithColumn("Price").AsDecimal().NotNullable();
            });
        }

        // Removes table on rollback.
        public override void Down()
        {
            Delete.Table("Products");
        }
    }
}
