namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodani_logirani_dogadjaji : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.logiranidogadjaji",
                c => new
                    {
                        logirani_dogadjaj_id = c.Long(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 500, unicode: false),
                        ip_adresa = c.String(nullable: false, maxLength: 500, unicode: false),
                        url = c.String(nullable: false, maxLength: 500, unicode: false),
                        vrijeme_dogadjaja = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.logirani_dogadjaj_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.logiranidogadjaji");
        }
    }
}
