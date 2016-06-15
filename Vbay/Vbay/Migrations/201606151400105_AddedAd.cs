namespace Vbay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Headline = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Approved = c.Boolean(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ads");
        }
    }
}
