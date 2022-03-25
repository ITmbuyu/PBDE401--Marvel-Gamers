namespace PBDE401.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedMembershipTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [dbo].[MembershipTypes](Name) VALUES('Doctor')");
            Sql("INSERT INTO [dbo].[MembershipTypes](Name) VALUES('Patient')");
        }
        
        public override void Down()
        {
        }
    }
}
