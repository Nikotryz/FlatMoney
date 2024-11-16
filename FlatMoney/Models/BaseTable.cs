using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatMoney.Models
{
    public class BaseTable
    {
        [DisplayName("ID")]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
    }
}
