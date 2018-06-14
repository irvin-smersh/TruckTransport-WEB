namespace TruckTransport_Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodano_polje_lokalna_id_u_geotacke : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.geotacke", "lokalna_id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.geotacke", "lokalna_id");
        }
    }
}
