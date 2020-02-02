namespace CompanyRatesAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removednavigations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        Website = c.String(),
                        LogoUrl = c.String(),
                        Verified = c.Boolean(nullable: false),
                        TotalRating = c.Int(nullable: false),
                        User_FK = c.Int(nullable: false),
                        UserFK_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.CompanyID)
                .ForeignKey("dbo.Users", t => t.UserFK_UserID)
                .Index(t => t.UserFK_UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        isAdmin = c.Boolean(nullable: false),
                        isCompany = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.LoginSessions",
                c => new
                    {
                        LoginSessionID = c.Int(nullable: false, identity: true),
                        SessionKey = c.String(),
                        User_FK = c.Int(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        UserFK_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.LoginSessionID)
                .ForeignKey("dbo.Users", t => t.UserFK_UserID)
                .Index(t => t.UserFK_UserID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Category = c.String(),
                        User_FK = c.Int(nullable: false),
                        Company_FK = c.Int(nullable: false),
                        DateTimeAdded = c.DateTime(nullable: false),
                        TotalRating = c.Int(nullable: false),
                        CompanyFK_CompanyID = c.Int(),
                        UserFK_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.Companies", t => t.CompanyFK_CompanyID)
                .ForeignKey("dbo.Users", t => t.UserFK_UserID)
                .Index(t => t.CompanyFK_CompanyID)
                .Index(t => t.UserFK_UserID);
            
            CreateTable(
                "dbo.VoteCompanies",
                c => new
                    {
                        VoteCompanyID = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        User_FK = c.Int(nullable: false),
                        Company_FK = c.Int(nullable: false),
                        CompanyFK_CompanyID = c.Int(),
                        UserFK_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.VoteCompanyID)
                .ForeignKey("dbo.Companies", t => t.CompanyFK_CompanyID)
                .ForeignKey("dbo.Users", t => t.UserFK_UserID)
                .Index(t => t.CompanyFK_CompanyID)
                .Index(t => t.UserFK_UserID);
            
            CreateTable(
                "dbo.VoteReviews",
                c => new
                    {
                        VoteReviewID = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        User_FK = c.Int(nullable: false),
                        Review_FK = c.Int(nullable: false),
                        ReviewFK_ReviewID = c.Int(),
                        UserFK_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.VoteReviewID)
                .ForeignKey("dbo.Reviews", t => t.ReviewFK_ReviewID)
                .ForeignKey("dbo.Users", t => t.UserFK_UserID)
                .Index(t => t.ReviewFK_ReviewID)
                .Index(t => t.UserFK_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VoteReviews", "UserFK_UserID", "dbo.Users");
            DropForeignKey("dbo.VoteReviews", "ReviewFK_ReviewID", "dbo.Reviews");
            DropForeignKey("dbo.VoteCompanies", "UserFK_UserID", "dbo.Users");
            DropForeignKey("dbo.VoteCompanies", "CompanyFK_CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Reviews", "UserFK_UserID", "dbo.Users");
            DropForeignKey("dbo.Reviews", "CompanyFK_CompanyID", "dbo.Companies");
            DropForeignKey("dbo.LoginSessions", "UserFK_UserID", "dbo.Users");
            DropForeignKey("dbo.Companies", "UserFK_UserID", "dbo.Users");
            DropIndex("dbo.VoteReviews", new[] { "UserFK_UserID" });
            DropIndex("dbo.VoteReviews", new[] { "ReviewFK_ReviewID" });
            DropIndex("dbo.VoteCompanies", new[] { "UserFK_UserID" });
            DropIndex("dbo.VoteCompanies", new[] { "CompanyFK_CompanyID" });
            DropIndex("dbo.Reviews", new[] { "UserFK_UserID" });
            DropIndex("dbo.Reviews", new[] { "CompanyFK_CompanyID" });
            DropIndex("dbo.LoginSessions", new[] { "UserFK_UserID" });
            DropIndex("dbo.Companies", new[] { "UserFK_UserID" });
            DropTable("dbo.VoteReviews");
            DropTable("dbo.VoteCompanies");
            DropTable("dbo.Reviews");
            DropTable("dbo.LoginSessions");
            DropTable("dbo.Users");
            DropTable("dbo.Companies");
        }
    }
}
