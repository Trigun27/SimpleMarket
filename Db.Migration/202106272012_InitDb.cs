using FluentMigrator;

namespace Db.Migration
{
    [Migration(202106272012, "Init Db")]
    public class InitDb : Base
    {
        public override void Up()
        {
            Create.Table(ProductTable).WithDescription("Товары")
                .InSchema(PublicSchema)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString(200)
                .WithColumn("Info").AsString(2000);
        }

        public override void Down()
        {
            Delete.Table(ProductTable).InSchema(PublicSchema);
        }
    }
}