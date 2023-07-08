namespace BuyOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MveRatingFromProductToBuyProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuyProducts", "Rating", c => c.Byte(nullable: false));
            DropColumn("dbo.Products", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Rating", c => c.Byte(nullable: false));
            DropColumn("dbo.BuyProducts", "Rating");
        }
    }
}
