namespace AzureHunter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTableForViewFiles : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ViewModelFiles");
        }
        
        public override void Down()
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
    }
}
