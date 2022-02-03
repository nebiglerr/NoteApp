using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime CratedOn { get; set; } // olusturma tarihi

        [Required]
        public DateTime ModifiedOn { get; set; } // düzenleme tarihi

        [Required, StringLength(30)]
        public string ModifiedUsername { get; set; } // kimin yaptığı

    }
}
