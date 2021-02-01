namespace Id.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIDClaim : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        ClaimId = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        Value = c.String(),
                        Id = c.Int(),
                    })
                .PrimaryKey(t => t.ClaimId)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        AcceptedEula = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserClaims", "Id", "dbo.Users");
            DropIndex("dbo.UserClaims", new[] { "Id" });
            DropTable("dbo.Users");
            DropTable("dbo.UserClaims");
        }
    }
}
