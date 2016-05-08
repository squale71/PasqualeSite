namespace PasqualeSite.Data.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "ImageId", "dbo.PostImages");
            DropIndex("dbo.Posts", new[] { "ImageId" });
            AlterColumn("dbo.Posts", "ImageId", c => c.Int());
            CreateIndex("dbo.Posts", "ImageId");
            AddForeignKey("dbo.Posts", "ImageId", "dbo.PostImages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ImageId", "dbo.PostImages");
            DropIndex("dbo.Posts", new[] { "ImageId" });
            AlterColumn("dbo.Posts", "ImageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "ImageId");
            AddForeignKey("dbo.Posts", "ImageId", "dbo.PostImages", "Id", cascadeDelete: true);
        }
    }
}
