namespace PasqualeSite.Data.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RSS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RSSFeeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FeedUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RSSFeeds");
        }
    }
}
