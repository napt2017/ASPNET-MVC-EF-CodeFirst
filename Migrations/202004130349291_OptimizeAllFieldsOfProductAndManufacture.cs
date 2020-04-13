namespace ASPNET_MVC_EF_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptimizeAllFieldsOfProductAndManufacture : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Manufacturers", "Name", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Manufacturers", "Address", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Manufacturers", "Address", c => c.String());
            AlterColumn("dbo.Manufacturers", "Name", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
