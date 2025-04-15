using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Migrations
{
    [Migration(20250412)]
    public class _20250415_RemoveCategoryTable : Migration
    {
        public override void Up()
        {
            // Step 1: Drop the foreign key constraint 'FK_Products_Categories' using raw SQL
            Execute.Sql("ALTER TABLE Products DROP CONSTRAINT FK_Products_Categories");

            // Step 2: Remove the 'CategoryId' column from the 'Products' table
            if (Schema.Table("Products").Column("CategoryId").Exists())
            {
                Delete.Column("CategoryId").FromTable("Products");
            }
                // Check if the 'Categories' table exists, then delete it
                if (Schema.Table("Categories").Exists())
            {
                Delete.Table("Categories");
            }
        }

        public override void Down()
        {
           //MM
        }
    }
}

