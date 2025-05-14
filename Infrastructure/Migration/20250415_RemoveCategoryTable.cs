using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20250412)]
    public class _20250415_RemoveCategoryTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            // Drop the foreign key constraint 'FK_Products_Categories'
            Execute.Sql("IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Products_Categories') ALTER TABLE Products DROP CONSTRAINT FK_Products_Categories");

            // Remove the 'CategoryId' column if it exists
            if (Schema.Table("Products").Column("CategoryId").Exists())
            {
                Delete.Column("CategoryId").FromTable("Products");
            }

            // Delete the 'Categories' table if it exists
            if (Schema.Table("Categories").Exists())
            {
                Delete.Table("Categories");
            }
        }

        public override void Down()
        {
            // Rollback logic (if required)
        }
    }
}
