namespace DLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("staff")]
    public partial class staff
    {
        public int id { get; set; }

        public int? country_id { get; set; }

        public int? city_id { get; set; }

        public int? department_id { get; set; }

        [Required]
        [StringLength(150)]
        public string name_surname { get; set; }//

        [Required]
        [StringLength(150)]
        public string nationality { get; set; }

        [Required]
        [StringLength(11)]
        public string identity_number { get; set; }

        [StringLength(150)]
        public string mission { get; set; }

        public bool gender { get; set; }

        public DateTime? birth_date { get; set; }

        [StringLength(150)]
        public string birth_place { get; set; }

        [StringLength(50)]
        public string blood_type { get; set; }

        [StringLength(150)]
        public string last_graduated_school { get; set; }

        [StringLength(150)]
        public string email { get; set; }

        [StringLength(100)]
        public string mobile_phone { get; set; }

        [StringLength(100)]
        public string home_phone { get; set; }

        [StringLength(500)]
        public string home_address { get; set; }

        [StringLength(500)]
        public string image_path { get; set; }

        public virtual city city { get; set; }

        public virtual country country { get; set; }

        public virtual department department { get; set; }

        [NotMapped]
        public virtual double? birthDateTimestamp { get; set; }
    }

    public partial class staffFormModel
    {
        public int? staffId { get; set; }

        public string name_surname { get; set; }

        public string nationality { get; set; }

        public string identity_number { get; set; }

        public int? department_id { get; set; }

        public string mission { get; set; }

        public int? country_id { get; set; }

        public int? city_id { get; set; }

        public string last_graduated_school { get; set; }

        public string mobile_phone { get; set; }

        public string email { get; set; }

        public string birth_place { get; set; }

        public string home_phone { get; set; }

        public bool? gender { get; set; }

        public double? birth_dateTimeStamp { get; set; }

        public string home_address { get; set; }

        public string operationName { get; set; }

        public string blood_type { get; set; }
    }
}
