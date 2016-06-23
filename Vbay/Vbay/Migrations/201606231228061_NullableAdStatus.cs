namespace Vbay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableAdStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ads", "Approved", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ads", "Approved", c => c.Boolean(nullable: false));
        }
    }
}
