using System.ComponentModel;
using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.FLATS_TABLE_NAME)]
    public class FlatModel : BaseTable
    {
        [SQLite.Column("PK_flat_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id {get; set;}

        [SQLite.Column("flat_name")]
        [SQLite.NotNull, SQLite.Unique]
        public string Name { get; set; }

        [SQLite.Column("flat_type")]
        [SQLite.NotNull]
        public string Type { get; set; }



        [SQLite.Column("rent_cost")]
        public float RentCost { get; set; }

        [SQLite.Column("rent_date")]
        public DateTime RentStartDate { get; set; }

        [SQLite.Column("rent_interval")]
        public int RentInterval { get; set; }

        [SQLite.Column("rent_autopay")]
        public bool RentAutopay { get; set; }



        [SQLite.Column("internet_cost")]
        public float InternetCost { get; set; }

        [SQLite.Column("internet_date")]
        public DateTime InternetStartDate { get; set; }

        [SQLite.Column("internet_interval")]
        public int InternetInterval { get; set; }

        [SQLite.Column("internet_autopay")]
        public bool InternetAutopay { get; set; }



        [SQLite.Column("address")]
        public string Address { get; set; }
    }
}
