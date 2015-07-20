namespace GeoNewsScreenScraperCLI.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Journalist> Journalists { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<NewsItem> NewsItems { get; set; }
        public virtual DbSet<Paragraph> Paragraphs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Journalist>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Journalist>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Journalist>()
                .HasMany(e => e.NewsItems)
                .WithMany(e => e.Journalists)
                .Map(m => m.ToTable("NewsItemToJournalistMaps").MapLeftKey("JournalistId").MapRightKey("NewsItemId"));

            modelBuilder.Entity<Location>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.NewsItems)
                .WithMany(e => e.Locations)
                .Map(m => m.ToTable("NewsItemToLocationMaps").MapLeftKey("LocationId").MapRightKey("NewsItemId"));

            modelBuilder.Entity<NewsItem>()
                .Property(e => e.HeadLine)
                .IsUnicode(false);

            modelBuilder.Entity<NewsItem>()
                .HasMany(e => e.Paragraphs)
                .WithRequired(e => e.NewsItem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Paragraph>()
                .Property(e => e.Text)
                .IsUnicode(false);
        }
    }
}
