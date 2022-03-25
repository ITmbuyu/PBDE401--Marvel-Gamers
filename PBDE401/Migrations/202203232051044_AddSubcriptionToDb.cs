namespace PBDE401.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubcriptionToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        MedicineId = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        ScheduledEndDate = c.DateTime(),
                        ActualEndDate = c.DateTime(),
                        SubscriptionPrice = c.Double(nullable: false),
                        SubscriptionDuration = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Subscriptions");
        }
    }
}
