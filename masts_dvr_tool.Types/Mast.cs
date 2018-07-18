using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Types
{
    public struct Mast : IVOAType
    {
        public string VOAAddressLine1 { get; set; }
        public string VOAAddressLine2 { get; set; }
        public string VOAAddressLine3 { get; set; }
        public string VOAAddressLine4 { get; set; }
        public string VOACounty { get; set; }
        public string VOAPostcode { get; set; }
        public string VOAEffectiveFrom { get; set; }
        public string VOAMastOperator { get; set; }
        public string VOAShared { get; set; }
        public string VOASharedWith { get; set; }
        public string VOAOccupier { get; set; }
        public string VOARateableValue { get; set; }

    }
}
