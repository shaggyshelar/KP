using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models.Core
{
    public class AppModuleDto : BaseDto
    {
        public Guid Id { get; set; }
        public string MenuText { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}