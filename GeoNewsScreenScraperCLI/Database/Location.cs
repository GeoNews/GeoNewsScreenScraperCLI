namespace GeoNewsScreenScraperCLI.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Location
    {
        public Location()
        {
            NewsItems = new HashSet<NewsItem>();
        }

        public int LocationId { get; set; }

        [Required]
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public virtual ICollection<NewsItem> NewsItems { get; set; }
    }
}
