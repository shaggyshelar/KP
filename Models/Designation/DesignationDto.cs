using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models {
    public class DesignationDto : BaseDto {
        public Guid DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string DesignationCode { get; set; }
    }
}