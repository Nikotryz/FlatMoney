using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.CLIENTS_TABLE_NAME)]
    public class ClientModel : BaseTable
    {
        [SQLite.Column("PK_client_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id { get; set; }

        [SQLite.Column("client_name")]
        [SQLite.NotNull]
        public string? Name { get; set; }

        [SQLite.Column("phone")]
        [SQLite.Unique]
        public string? Phone {  get; set; }

        [SQLite.Column("email")]
        [SQLite.Unique]
        public string? Email { get; set; }

        [SQLite.Column("passport")]
        [SQLite.Unique]
        public string? Passport { get; set; }

        [SQLite.Column("registration")]
        public string? Registration { get; set; }
    }
}
