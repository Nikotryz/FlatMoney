﻿using FlatMoney.LocalDataBase;

namespace FlatMoney.Models
{
    [SQLite.Table(Tables.EXPENSES_TABLE_NAME)]
    public class ExpenseModel : BaseTable
    {
        [SQLite.Column("PK_expense_id")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public new int Id { get; set; }

        [SQLite.Column("expense_type_name")]
        public string TypeName { get; set; }

        [SQLite.Column("expense_date")]
        [SQLite.NotNull]
        public DateTime Date { get; set; }

        [SQLite.Column("expense_cost")]
        [SQLite.NotNull]
        public float Cost { get; set; }
    }
}
