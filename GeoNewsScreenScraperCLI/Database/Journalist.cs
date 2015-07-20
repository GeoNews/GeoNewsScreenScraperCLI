namespace GeoNewsScreenScraperCLI.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Journalist
    {
        public Journalist()
        {
            NewsItems = new HashSet<NewsItem>();
        }

        public int JournalistId { get; set; }

        [Required]
        [StringLength(75)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(75)]
        public string LastName { get; set; }

        public virtual ICollection<NewsItem> NewsItems { get; set; }
    }
}
