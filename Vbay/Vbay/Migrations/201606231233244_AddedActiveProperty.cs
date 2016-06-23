namespace Vbay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedActiveProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ads", "Active");
        }
    }
}
