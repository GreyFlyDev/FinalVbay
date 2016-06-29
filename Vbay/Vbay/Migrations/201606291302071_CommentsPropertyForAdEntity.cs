namespace Vbay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsPropertyForAdEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "AdminComments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ads", "AdminComments");
        }
    }
}
