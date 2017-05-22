using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities
{
    [NotMapped]
    public class LookUpItem
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        
    }
}