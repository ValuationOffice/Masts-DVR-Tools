using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.Types
{
    public struct Mast: IVOAType
    {
        public string Address1 { get; set; }
        public string VOAAddressLine2 { get; set; }
        public string VOAAddressLine3 { get; set; }
        public string VOAAddressLine4 { get; set; }
        public string VOAEffectiveFrom { get; set; }
        public string VOABARefNumber { get; set; }
        public string VOABAName { get; set; }
        public string VOAMastOperator { get; set; }
        public string VOAShared { get; set; }
        public string VOASharedWith { get; set; }
        public string VOAOccupier { get; set; }
        public string VOASiteRef { get; set; }
        public string VOACtil { get; set; }
        public string VOAEastings { get; set; }
        public string VOANorthings { get; set; }
        public string VOASiteType { get; set; }
        public string VOACellType { get; set; }
        public string VOAMastStructureType { get; set; }
        public string VOAMastHeight { get; set; }
        public string VOASiteArea { get; set; }
        public string VOAM25 { get; set; }
        public string VOARateableValue { get; set; }
        
    }
}
