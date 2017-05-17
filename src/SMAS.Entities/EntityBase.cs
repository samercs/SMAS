using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMAS.Entities
{
    public abstract class EntityBase
    {
        public bool IsDeleted { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
        public DateTime? DeletedUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
