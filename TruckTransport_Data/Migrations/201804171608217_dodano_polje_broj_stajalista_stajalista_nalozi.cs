namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodano_polje_broj_stajalista_stajalista_nalozi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.stajalista_nalozi", "broj stajalista", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.stajalista_nalozi", "broj stajalista");
        }
    }
}
