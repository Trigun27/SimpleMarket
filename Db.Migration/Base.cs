using System;

namespace Db.Migration
{
    public abstract class Base : FluentMigrator.Migration
    {
        protected string PublicSchema = "public";
        
        protected string ProductTable = "product";

        protected string BuyerTable = "buyer";

    }
}