namespace GeoNewsScreenScraperCLI.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NewsItem
    {
        public NewsItem()
        {
            Paragraphs = new HashSet<Paragraph>();
            Journalists = new HashSet<Journalist>();
            Locations = new HashSet<Location>();
        }

        public int NewsItemId { get; set; }

        public DateTime NewsItemDate { get; set; }

        [Required]
        public string HeadLine { get; set; }

        public virtual ICollection<Paragraph> Paragraphs { get; set; }

        public virtual ICollection<Journalist> Journalists { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}
