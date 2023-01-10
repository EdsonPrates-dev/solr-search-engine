using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrSearchSystem.Models.Enum
{
    public enum SortEnum
    {
        RELEVANCE = 1,
        PRICELOWERTOHIGHER = 2,
        PRICEHIGHERTOLOWER = 3,
        SCORE = 4
    }
}
