using FlatMoney.LocalDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.RESERVATION_SERVICES_TABLE_NAME)]
    public class ReservationServiceModel : BaseTable
    {
        [SQLite.Column("PK_reservation_service_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id { get; set; }

        [SQLite.Column("FK_reservation_id")]
        [SQLite.NotNull]
        public int ReservationId { get; set; }

        [SQLite.Column("service_name")]
        [SQLite.NotNull]
        public string? Name { get; set; }

        [SQLite.Column("service_cost")]
        public float? Cost { get; set; }
    }
}
