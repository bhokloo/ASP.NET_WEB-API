namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MyHoney")]
    public partial class MyHoney
    {
        public int id { get; set; }

        public string name { get; set; }

        public int? age { get; set; }
    }
}
