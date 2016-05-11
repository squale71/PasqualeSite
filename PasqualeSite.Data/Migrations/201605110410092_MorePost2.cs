namespace PasqualeSite.Data.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MorePost2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "UrlTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "UrlTitle");
        }
    }
}
