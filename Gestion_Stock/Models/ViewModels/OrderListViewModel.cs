using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Stock.Models.ViewModels
{
    public class OrderListViewModel
    {
        public IList<OrderDetailsViewModel> Orders { get; set; }
    }
}
