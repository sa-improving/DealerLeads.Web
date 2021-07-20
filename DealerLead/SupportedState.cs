﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class SupportedState
    {
        [Key]
        [Column("StateAbbreviation")]
        public string Abbreiviation { get; set; }

        [Column("StateName")]
        public string Name { get; set; }
    }
}
