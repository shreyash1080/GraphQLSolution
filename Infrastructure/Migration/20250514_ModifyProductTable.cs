using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Migration
{
    [Migration(20250514)]
    public class ModifyProductTable : FluentMigrator.Migration
    {

        public override void Up()
        {
            if (Schema.Table("Products").Exists())
            {
                if (!Schema.Table("Products").Column("SkuId").Exists())
                {
                    Alter.Table("Products")
                        .AddColumn("SkuId").AsString(255).Nullable();
                }
                if (!Schema.Table("Products").Column("Supplier").Exists())
                {
                    Alter.Table("Products")
                        .AddColumn("Supplier").AsString(255).Nullable();
                }
                if (!Schema.Table("Products").Column("Discount").Exists())
                {
                    Alter.Table("Products")
                        .AddColumn("Discount").AsString(255).Nullable();
                }
                if (!Schema.Table("Products").Column("ImageUrl").Exists())
                {
                    Alter.Table("Products")
                        .AddColumn("ImageUrl").AsString(255).Nullable();
                }
                if (!Schema.Table("Products").Column("UserID").Exists())
                {
                    Alter.Table("Products")
                        .AddColumn("UserID").AsString(255).Nullable();
                }

                // Add an index on the UserID column for better query performance
                if (Schema.Table("Products").Exists() && !Schema.Table("Products").Index("IX_Products_UserID").Exists())
                {
                    Create.Index("IX_Products_UserID")
                        .OnTable("Products")
                        .OnColumn("UserID");
                }
            }
        }

        public override void Down()
        {
            // Drop the 'Products' table if it exists  
            if (Schema.Table("Products").Exists())
            {
                Delete.Table("Products");
            }
            // Remove the index if it exists
            if (Schema.Table("Products").Exists() && Schema.Table("Products").Index("IX_Products_UserID").Exists())
            {
                Delete.Index("IX_Products_UserID").OnTable("Products");
            }
        }
    }
}
