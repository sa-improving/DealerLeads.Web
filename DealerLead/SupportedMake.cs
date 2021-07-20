using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class SupportedMake
    {
        [Key]
        [Column("MakeId")]
        public int MakeId { get; set; }

        [Column("MakeName")]
        [Display(Name = "Make Name")]
        public string MakeName { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreateDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }
        
        public List<SupportedModel> Models { get; set; }
    }
}
