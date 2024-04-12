using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrickHouse.Models
{
    [Table("ProdRecs")] // Replace with your actual table name as it is in the database
    public class ProdRec // Replace with your desired class name
    {
        [Column("column1")] // Match the name with the actual column name if it's to be included
        public byte Column1 { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("product_ID")]
        public int ProductId { get; set; }

        [Column("mean_rating")]
        public double MeanRating { get; set; }

        [Column("times_rated")]
        public short TimesRated { get; set; }
    }
}
