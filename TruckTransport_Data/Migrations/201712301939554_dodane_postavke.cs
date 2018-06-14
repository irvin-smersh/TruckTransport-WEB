namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodane_postavke : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.postavke",
                c => new
                {
                    postavka_id = c.Int(nullable: false, identity: true),
                    kljuc = c.String(nullable: false, maxLength: 500, unicode: false),
                    vrijednost = c.String(nullable: false, maxLength: 500, unicode: false),
                })
                .PrimaryKey(t => t.postavka_id);
        }
        
        public override void Down()
        {
            DropTable("dbo.postavke");
        }
    }
}
