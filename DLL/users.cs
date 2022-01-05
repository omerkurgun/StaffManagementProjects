namespace DLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class users
    {
        public int id { get; set; }

        [Required]
        [StringLength(150)]
        public string uname { get; set; }

        [Required]
        [StringLength(150)]
        public string pass { get; set; }
    }
}
