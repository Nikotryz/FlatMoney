using System.ComponentModel;

namespace FlatMoney.Models
{
    public class BaseTable
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
    }
}
