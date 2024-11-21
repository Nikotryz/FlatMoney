using System.ComponentModel;
using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.EXPENSE_TYPES_TABLE_NAME)]
    public class ExpenseTypeModel : BaseTable
    {
        [SQLite.Column("PK_expense_type_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id {  get; set; } 

        [SQLite.Column("expense_type_name")]
        [SQLite.NotNull, SQLite.Unique]
        public string Name { get; set; }
    }
}
