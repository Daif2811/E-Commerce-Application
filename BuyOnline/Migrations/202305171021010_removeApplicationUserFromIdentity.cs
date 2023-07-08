namespace BuyOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeApplicationUserFromIdentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        CategoryDescription = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProductDescription = c.String(),
                        ProductImage = c.String(),
                        ProductQuantity = c.Int(nullable: false),
                        Rating = c.Byte(nullable: false),
                        ProductPrice = c.Single(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.BuyProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        BuyDate = c.DateTime(nullable: false),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Pay = c.Boolean(nullable: false),
                        Message = c.String(),
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
            DropForeignKey("dbo.Products", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.BuyProducts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BuyProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.BuyProducts", new[] { "UserId" });
            DropIndex("dbo.BuyProducts", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Products", new[] { "UserId" });
            DropTable("dbo.BuyProducts");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
