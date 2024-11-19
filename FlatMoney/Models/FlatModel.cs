using System.ComponentModel;
using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.FLATS_TABLE_NAME)]
    public class FlatModel : BaseTable
    {
        [DisplayName("ID")]
        [SQLite.Column("PK_flat_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id {get; set;}

        [DisplayName("Name")]
        [SQLite.Column("flat_name")]
        [SQLite.NotNull, SQLite.Unique]
        public string Name { get; set; }

        [DisplayName("Is own")]
        [SQLite.Column("is_own")]
        [SQLite.NotNull]
        public bool IsOwn { get; set; }



        [DisplayName("Rent cost")]
        [SQLite.Column("rent_cost")]
        public float RentCost { get; set; }

        [DisplayName("Rent date")]
        [SQLite.Column("rent_date")]
        public DateTime RentStartDate { get; set; }

        [DisplayName("Rent interval")]
        [SQLite.Column("rent_interval")]
        public int RentInterval { get; set; }

        [DisplayName("Rent autopay")]
        [SQLite.Column("rent_autopay")]
        public bool RentAutopay { get; set; }


        [DisplayName("Internet cost")]
        [SQLite.Column("internet_cost")]
        public float InternetCost { get; set; }

        [DisplayName("Internet date")]
        [SQLite.Column("internet_date")]
        public DateTime InternetStartDate { get; set; }

        [DisplayName("Internet interval")]
        [SQLite.Column("internet_interval")]
        public int InternetInterval { get; set; }

        [DisplayName("Internet autopay")]
        [SQLite.Column("internet_autopay")]
        public bool InternetAutopay { get; set; }



        [DisplayName("Address")]
        [SQLite.Column("address")]
        public string Address { get; set; }
    }
}
