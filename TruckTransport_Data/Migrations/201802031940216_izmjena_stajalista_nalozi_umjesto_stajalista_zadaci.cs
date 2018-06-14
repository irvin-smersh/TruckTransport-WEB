namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class izmjena_stajalista_nalozi_umjesto_stajalista_zadaci : DbMigration
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
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.stajalista_zadaci",
                c => new
                    {
                        stajaliste_zadatak_id = c.Int(nullable: false, identity: true),
                        stajaliste_id = c.Int(nullable: false),
                        zadatak_id = c.Int(nullable: false),
                        checkin = c.Long(),
                        checkout = c.Long(),
                    })
                .PrimaryKey(t => t.stajaliste_zadatak_id);
            
            DropForeignKey("dbo.stajalista_nalozi", "nalog_id", "dbo.nalozi");
            DropForeignKey("dbo.stajalista_nalozi", "stajaliste_id", "dbo.stajalista");
            DropIndex("dbo.stajalista_nalozi", new[] { "nalog_id" });
            DropIndex("dbo.stajalista_nalozi", new[] { "stajaliste_id" });
            DropTable("dbo.stajalista_nalozi");
            CreateIndex("dbo.stajalista_zadaci", "zadatak_id");
            CreateIndex("dbo.stajalista_zadaci", "stajaliste_id");
            AddForeignKey("dbo.stajalista_zadaci", "zadatak_id", "dbo.zadaci", "zadatak_id");
            AddForeignKey("dbo.stajalista_zadaci", "stajaliste_id", "dbo.stajalista", "stajaliste_id");
        }
    }
}
