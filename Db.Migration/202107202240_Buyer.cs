using FluentMigrator;

namespace Db.Migration
{
    [Migration(202107202240, "Add Buyer")]
    public class Buyer : Base
    {
        public override void Up()
        {
            Create.Table(BuyerTable)
                .InSchema(PublicSchema)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("FirstName").AsString(200)
                .WithColumn("LastName").AsString(200).Nullable()
                .WithColumn("Email").AsString(400)
                .WithColumn("Phone").AsString(200);
        }

        public override void Down()
        {
            Delete.Table(BuyerTable)
                .InSchema(PublicSchema);
        }
    }
}