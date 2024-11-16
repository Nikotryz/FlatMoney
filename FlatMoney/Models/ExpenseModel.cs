using System.ComponentModel;
using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.EXPENSES_TABLE_NAME)]
    public class ExpenseModel : BaseTable
    {
        [DisplayName("ID")]
        [SQLite.Column("PK_expense_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id { get; set; }

        [DisplayName("Type ID")]
        [SQLite.Column("FK_expense_type_id")]
        public int TypeId { get; set; }

        [DisplayName("Date")]
        [SQLite.Column("expense_date")]
        [SQLite.NotNull]
        public DateTime Date { get; set; }

        [DisplayName("Cost")]
        [SQLite.Column("expense_cost")]
        [SQLite.NotNull]
        public float Cost { get; set; }
    }
}
