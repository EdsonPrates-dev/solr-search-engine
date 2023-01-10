using System;

namespace SolrSearchSystem.Extensions
{
    public static class CompareExtension
    {
        public static bool IsWithin(this decimal value, decimal min, decimal max) =>
            value >= min && value <= max;
    }
}
