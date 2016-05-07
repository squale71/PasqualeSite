namespace PasqualeSite.Data.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Text = c.String(maxLength: 500),
                        DatePosted = c.DateTime(nullable: false),
                        PostId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLikes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        CommentId = c.String(maxLength: 128),
                        DidLike = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.CommentId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CommentId);
            
            AddColumn("dbo.Posts", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.UserLikes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserLikes", "CommentId", "dbo.Comments");
            DropIndex("dbo.UserLikes", new[] { "CommentId" });
            DropIndex("dbo.UserLikes", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropColumn("dbo.Posts", "Priority");
            DropTable("dbo.UserLikes");
            DropTable("dbo.Comments");
        }
    }
}
