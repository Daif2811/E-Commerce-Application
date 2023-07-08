namespace BuyOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allowImageNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductImage", c => c.String(nullable: false));
        }
    }
}
