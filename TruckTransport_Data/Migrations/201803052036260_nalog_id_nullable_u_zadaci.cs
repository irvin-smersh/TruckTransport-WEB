namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nalog_id_nullable_u_zadaci : DbMigration
    {
        public override void Up()
        {
            AlterColumn("zadaci", "nalog_id", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("zadaci", "nalog_id", c => c.Int(nullable: false));
        }
    }
}
