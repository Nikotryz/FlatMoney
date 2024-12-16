using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.PAYMENTS_TABLE_NAME)]
    public class PaymentModel : BaseTable
    {
        [SQLite.Column("PK_payment_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id { get; set; }

        [SQLite.Column("FK_reservation_id")]
        public int ReservationId { get; set; }

        [SQLite.Column("payment_name")]
        [SQLite.NotNull]
        public string? Name { get; set; }

        [SQLite.Column("payment_type")]
        [SQLite.NotNull]
        public string? Type { get; set; }

        [SQLite.Column("payment_date")]
        [SQLite.NotNull]
        public DateTime Date {  get; set; }

        [SQLite.Column("payment_cost")]
        [SQLite.NotNull]
        public float? Cost { get; set; }
    }
}
