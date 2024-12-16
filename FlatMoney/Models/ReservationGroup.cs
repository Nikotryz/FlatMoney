using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatMoney.Models
{
    public class ReservationGroup : List<ReservationModel>
    {
        public string Status { get; set; }

        public ReservationGroup(string status, IEnumerable<ReservationModel> reservations) : base(reservations) 
        {
            Status = status;
        }
    }
}
