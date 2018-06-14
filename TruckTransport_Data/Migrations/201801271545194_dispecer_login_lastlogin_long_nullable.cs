namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dispecer_login_lastlogin_long_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.dispecerlogin", "lastlogin", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.dispecerlogin", "lastlogin", c => c.Long(nullable: false));
        }
    }
}
