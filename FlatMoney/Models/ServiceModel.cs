using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.SERVICES_TABLE_NAME)]
    public class ServiceModel : BaseTable
    {
        [SQLite.Column("PK_service_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id { get; set; }

        [SQLite.Column("service_name")]
        [SQLite.NotNull]
        public string Name { get; set; }

        [SQLite.Column("service_cost")]
        public float Cost { get; set; }
    }
}
