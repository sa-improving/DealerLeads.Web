using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class Vehicle
    {
        [Key]
        [Column("VehicleId")]
        public int Id { get; set; }

        [Column("ModelId")]
        public int Model { get; set; }

        [Column("MSRP")]
        public decimal MSRP { get; set; }

        [Column("StockNumber")]
        public string StockNumber { get; set; }

        [Column("Color")]
        public string Color { get; set; }

        [Column("DealershiplId")]
        public int Dealership { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? SaleDate { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreateDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }
    }
}
