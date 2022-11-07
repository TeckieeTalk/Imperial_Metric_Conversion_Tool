using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialMetricConversion.Models
{
    public class Container
    {
        public decimal Metric_Height { get; set; }
        public decimal Metric_Width { get; set; }
        public decimal Metric_length { get; set; }
        public int Imperial_Width { get; set; }
        public int Imperial_length { get; set; }
        public int Imperial_Foot { get; set; }
        public int Imperial_Inch { get; set; }
        public int Imperial_One16 { get; set; }


    }
}
