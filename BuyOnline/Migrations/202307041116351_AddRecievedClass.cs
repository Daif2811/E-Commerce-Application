namespace BuyOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecievedClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecievedProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Recieved = c.Boolean(nullable: false),
                        RecievedDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        BuyProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BuyProducts", t => t.BuyProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BuyProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecievedProducts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RecievedProducts", "BuyProductId", "dbo.BuyProducts");
            DropIndex("dbo.RecievedProducts", new[] { "BuyProductId" });
            DropIndex("dbo.RecievedProducts", new[] { "UserId" });
            DropTable("dbo.RecievedProducts");
        }
    }
}
