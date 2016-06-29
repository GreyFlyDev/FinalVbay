namespace Vbay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminFilterAddedToAd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "AdminFilter", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ads", "AdminFilter");
        }
    }
}
