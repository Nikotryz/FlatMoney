using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.RESERVATIONS_TABLE_NAME)]
    public class ReservationModel : BaseTable
    {
        [SQLite.Column("PK_reservation_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id { get; set; }

        [SQLite.Column("FK_flat_id")]
        public int? FlatId { get; set; }

        [SQLite.Column("flat_name")]
        public string? FlatName { get; set; }

        [SQLite.Column("reservation_type")]
        [SQLite.NotNull]
        public string? Type { get; set; }

        [SQLite.Column("checkin_date")]
        [SQLite.NotNull]
        public DateTime CheckInDate { get; set; }

        [SQLite.Column("checkout_date")]
        [SQLite.NotNull]
        public DateTime CheckOutDate { get; set; }

        [SQLite.Column("guest_amount")]
        [SQLite.NotNull]
        public int PeopleAmount { get; set; }

        [SQLite.Column("kid_amount")]
        [SQLite.NotNull]
        public int ChildAmount { get; set; }

        [SQLite.Column("dm_amount")]
        [SQLite.NotNull]
        public int DaysOrMonthsAmount { get; set; }

        [SQLite.Column("cost_per_amount")]
        [SQLite.NotNull]
        public float CostPerAmount { get; set; }

        [SQLite.Column("deposit_cost")]
        [SQLite.NotNull]
        public float? DepositCost { get; set; }

        [SQLite.Column("deposit_status")]
        [SQLite.NotNull]
        public string? DepositStatus { get; set; }

        [SQLite.Column("reservation_status")]
        [SQLite.NotNull]
        public string? ReservationStatus { get; set; }

        [SQLite.Column("reservation_comment")]
        [SQLite.NotNull]
        public string? Comment { get; set; }
    }
}
