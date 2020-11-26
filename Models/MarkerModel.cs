using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.Models
{
    public class MarkerModel
    {
        public int Id { get; set; }


        public string LatLng { get; set; }

        public string UserId { get; set; }

        public decimal PlotArea { get; set; }
    }
}
