namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class izmjena_stajalista_zadaci_umjesto_s_nalozi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.stajalista_zadaci",
                c => new {
                    stajaliste_zadatak_id = c.Int(nullable: false, identity: true),
                    stajaliste_id = c.Int(nullable: false),
                    zadatak_id = c.Int(nullable: false),
                    checkin = c.Long(),
                    checkout = c.Long(),
                })
                .PrimaryKey(t => t.stajaliste_zadatak_id)
                .ForeignKey("dbo.zadaci", t => t.zadatak_id)
                .ForeignKey("dbo.stajalista", t => t.stajaliste_id)
                .Index(t => t.zadatak_id)
                .Index(t => t.stajaliste_id);
        }

        public override void Down()
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
                .PrimaryKey(t => t.stajaliste_nalog_id);

            DropForeignKey("dbo.stajalista_zadaci", "zadatak_id", "dbo.zadaci");
            DropForeignKey("dbo.stajalista_zadaci", "stajaliste_id", "dbo.stajalista");
            DropIndex("dbo.stajalista_zadaci", new[] { "zadatak_id" });
            DropIndex("dbo.stajalista_zadaci", new[] { "stajaliste_id" });
            DropTable("dbo.stajalista_zadaci");

            CreateIndex("dbo.stajalista_nalozi", "nalog_id");
            CreateIndex("dbo.stajalista_nalozi", "stajaliste_id");
            AddForeignKey("dbo.stajalista_nalozi", "nalog_id", "dbo.nalozi", "nalog_id");
            AddForeignKey("dbo.stajalista_nalozi", "stajaliste_id", "dbo.stajalista", "stajaliste_id");
        }
    }
}
