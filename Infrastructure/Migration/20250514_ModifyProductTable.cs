using FluentMigrator;

namespace Infrastructure.Migration
{
    [Migration(20250514_2032)]
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
                    // Add the 'Discount' column with a default value
                    Alter.Table("Products")
                        .AddColumn("Discount").AsDecimal(19, 5).NotNullable().WithDefaultValue(0.0);
                }
                if (!Schema.Table("Products").Column("ImageUrl").Exists())
                {
                    Alter.Table("Products")
                        .AddColumn("ImageUrl").AsString(255).Nullable();
                }
                if (!Schema.Table("Products").Column("UserID").Exists())
                {
                    // Add the 'UserID' column with a default value
                    Alter.Table("Products")
                        .AddColumn("UserID").AsInt32().NotNullable().WithDefaultValue(0);
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
            // Drop the columns and index if they exist
            if (Schema.Table("Products").Exists())
            {
                if (Schema.Table("Products").Index("IX_Products_UserID").Exists())
                {
                    Delete.Index("IX_Products_UserID").OnTable("Products");
                }
                if (Schema.Table("Products").Column("Discount").Exists())
                {
                    Delete.Column("Discount").FromTable("Products");
                }
                if (Schema.Table("Products").Column("UserID").Exists())
                {
                    Delete.Column("UserID").FromTable("Products");
                }
                if (Schema.Table("Products").Column("SkuId").Exists())
                {
                    Delete.Column("SkuId").FromTable("Products");
                }
                if (Schema.Table("Products").Column("Supplier").Exists())
                {
                    Delete.Column("Supplier").FromTable("Products");
                }
                if (Schema.Table("Products").Column("ImageUrl").Exists())
                {
                    Delete.Column("ImageUrl").FromTable("Products");
                }
            }
        }
    }
}
