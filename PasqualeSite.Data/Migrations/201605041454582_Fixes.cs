namespace PasqualeSite.Data.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropColumn("dbo.Posts", "UserId");
            RenameColumn(table: "dbo.Posts", name: "User_Id", newName: "UserId");
            AddColumn("dbo.Posts", "PostContent", c => c.String());
            AlterColumn("dbo.Posts", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "UserId");
            DropColumn("dbo.Posts", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "Content", c => c.String());
            DropIndex("dbo.Posts", new[] { "UserId" });
            AlterColumn("dbo.Posts", "UserId", c => c.Guid(nullable: false));
            DropColumn("dbo.Posts", "PostContent");
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Posts", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Posts", "User_Id");
        }
    }
}
