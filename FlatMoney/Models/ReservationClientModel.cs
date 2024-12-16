using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.RESERVATION_CLIENTS_TABLE_NAME)]
    public class ReservationClientModel : BaseTable
    {
        [SQLite.Column("PK_reservation_client_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id { get; set; }

        [SQLite.Column("FK_reservation_id")]
        public int? ReservationId { get; set; }

        [SQLite.Column("FK_client_id")]
        public int? ClientId { get; set; }

        [SQLite.Column("client_name")]
        [SQLite.NotNull]
        public string? Name { get; set; }
    }
}
