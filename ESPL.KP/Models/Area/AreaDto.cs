using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models {
    public class AreaDto : BaseDto {
        public Guid AreaID { get; set; }
        public string AreaName { get; set; }
        public string AreaCode { get; set; }
        public string PinCode { get; set; }
    }
}