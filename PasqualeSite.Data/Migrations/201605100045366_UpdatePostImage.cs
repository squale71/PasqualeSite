namespace PasqualeSite.Data.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePostImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostImages", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostImages", "Name");
        }
    }
}
