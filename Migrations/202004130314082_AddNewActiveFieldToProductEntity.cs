namespace ASPNET_MVC_EF_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewActiveFieldToProductEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsActive");
        }
    }
}
