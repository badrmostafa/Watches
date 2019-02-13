namespace Watches.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chooses",
                c => new
                    {
                        ChooseID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Image = c.String(),
                        Head = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ChooseID);
            
            CreateTable(
                "dbo.TypeWatches",
                c => new
                    {
                        TypeWatchID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Image = c.String(),
                        ChooseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TypeWatchID)
                .ForeignKey("dbo.Chooses", t => t.ChooseID, cascadeDelete: true)
                .Index(t => t.ChooseID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        FeatureID = c.Int(nullable: false),
                        TypeWatchID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Features", t => t.FeatureID, cascadeDelete: true)
                .ForeignKey("dbo.TypeWatches", t => t.TypeWatchID, cascadeDelete: true)
                .Index(t => t.FeatureID)
                .Index(t => t.TypeWatchID);
            
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        FeatureID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Head = c.String(),
                        Icon = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.FeatureID);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeID = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        Head = c.String(),
                        Description1 = c.String(),
                        Description2 = c.String(),
                    })
                .PrimaryKey(t => t.GradeID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Head1 = c.String(),
                        Head2 = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.ReviewID);
            
            CreateTable(
                "dbo.Updates",
                c => new
                    {
                        UpdateID = c.Int(nullable: false, identity: true),
                        Head = c.String(),
                        Description = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UpdateID);
            
            CreateTable(
                "dbo.Watches",
                c => new
                    {
                        WatchID = c.Int(nullable: false, identity: true),
                        Head = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        Button = c.String(),
                    })
                .PrimaryKey(t => t.WatchID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "TypeWatchID", "dbo.TypeWatches");
            DropForeignKey("dbo.Products", "FeatureID", "dbo.Features");
            DropForeignKey("dbo.TypeWatches", "ChooseID", "dbo.Chooses");
            DropIndex("dbo.Products", new[] { "TypeWatchID" });
            DropIndex("dbo.Products", new[] { "FeatureID" });
            DropIndex("dbo.TypeWatches", new[] { "ChooseID" });
            DropTable("dbo.Watches");
            DropTable("dbo.Updates");
            DropTable("dbo.Reviews");
            DropTable("dbo.Grades");
            DropTable("dbo.Features");
            DropTable("dbo.Products");
            DropTable("dbo.TypeWatches");
            DropTable("dbo.Chooses");
        }
    }
}
