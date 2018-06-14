namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class izmjena_dispecer_login_lastlogin_unixtime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.dispecerlogin", "lastlogin", c => c.Long(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.dispecerlogin", "lastlogin", c => c.String(nullable: false, maxLength: 500, unicode: false));
        }
    }
}
