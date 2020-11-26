using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mtama.Models
{
    public class AggregatorAssociationModel
    {
        public int Id { get; set; }
        public string AggregatorId { get; set; }
        public string FarmerId { get; set; }
    }
}
