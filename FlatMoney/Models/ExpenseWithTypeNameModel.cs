using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatMoney.Models
{
    public class ExpenseWithTypeNameModel : ExpenseModel
    {
        public string TypeName { get; set; }
        public DateTime Date { get; set; }
        public float Cost { get; set; }
    }
}
