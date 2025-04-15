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
            // Check if the 'CategoryId' column exists in the 'users' table, then delete it
            if (Schema.Table("users").Column("CategoryId").Exists())
            {
                Delete.Column("CategoryId").FromTable("users");
            }

            // Check if the 'Category' table exists, then delete it
            if (Schema.Table("Category").Exists())
            {
                Delete.Table("Category");
            }
        }

        public override void Down()
        {
            // Recreate the 'Category' table if it was deleted
            if (!Schema.Table("Category").Exists())
            {
                Create.Table("Category")
                    .WithColumn("CategoryId").AsInt32().PrimaryKey().Identity()
                    .WithColumn("Name").AsString(255).NotNullable()
                    .WithColumn("Description").AsString(1000).Nullable()
                    .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
            }

            // Re-add the 'CategoryId' column to the 'users' table
            if (!Schema.Table("users").Column("CategoryId").Exists())
            {
                Alter.Table("users")
                    .AddColumn("CategoryId").AsInt32().Nullable();

                // Optionally, if there was a foreign key relationship, recreate it
                Create.ForeignKey("FK_users_Category_CategoryId")
                    .FromTable("users").ForeignColumn("CategoryId")
                    .ToTable("Category").PrimaryColumn("CategoryId")
                    .OnDeleteOrUpdate(System.Data.Rule.None);
            }
        }
    }
}

