namespace BuyOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddToCarts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddToCartDate = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AddToCarts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AddToCarts", "ProductId", "dbo.Products");
            DropIndex("dbo.AddToCarts", new[] { "UserId" });
            DropIndex("dbo.AddToCarts", new[] { "ProductId" });
            DropTable("dbo.AddToCarts");
        }
    }
}
