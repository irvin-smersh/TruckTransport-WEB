namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodana_stajalista_i_stajalista_nalozi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.stajalista_nalozi",
                c => new
                    {
                        stajaliste_nalog_id = c.Int(nullable: false, identity: true),
                        stajaliste_id = c.Int(nullable: false),
                        nalog_id = c.Int(nullable: false),
                        checkin = c.Long(),
                        checkout = c.Long(),
                    })
                .PrimaryKey(t => t.stajaliste_nalog_id)
                .ForeignKey("dbo.stajalista", t => t.stajaliste_id)
                .ForeignKey("dbo.nalozi", t => t.nalog_id)
                .Index(t => t.stajaliste_id)
                .Index(t => t.nalog_id);
            
            CreateTable(
                "dbo.stajalista",
                c => new
                    {
                        stajaliste_id = c.Int(nullable: false, identity: true),
                        naziv = c.String(nullable: false, maxLength: 500, unicode: false),
                        opis = c.String(nullable: false, maxLength: 500, unicode: false),
                        duzina = c.String(nullable: false, maxLength: 500, unicode: false),
                        sirina = c.String(nullable: false, maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.stajaliste_id);           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.stajalista_nalozi", "nalog_id", "dbo.nalozi");
            DropForeignKey("dbo.stajalista_nalozi", "stajaliste_id", "dbo.stajalista");
            DropIndex("dbo.stajalista_nalozi", new[] { "nalog_id" });
            DropIndex("dbo.stajalista_nalozi", new[] { "stajaliste_id" });
            DropTable("dbo.stajalista");
            DropTable("dbo.stajalista_nalozi");
        }
    }
}
