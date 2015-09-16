namespace AzureHunter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ViewModelFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        FileType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ViewModelFiles");
        }
    }
}
