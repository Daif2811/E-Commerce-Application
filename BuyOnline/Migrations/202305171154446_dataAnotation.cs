namespace BuyOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataAnotation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductName", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "ProductDescription", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "ProductImage", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "ProductPrice", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Products", "ProductImage", c => c.String());
            AlterColumn("dbo.Products", "ProductDescription", c => c.String());
            AlterColumn("dbo.Products", "ProductName", c => c.String());
        }
    }
}
