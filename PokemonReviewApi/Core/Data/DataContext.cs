using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Core.Models;

namespace PokemonReviewApi.Core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        { }

        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Owner> Owners { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }

        public DbSet<Country> Countries { get; set; }


        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pokemon>().ToTable("pokemon");
            modelBuilder.Entity<Review>().ToTable("reviews");
            modelBuilder.Entity<Reviewer>().ToTable("reviewers");
            modelBuilder.Entity<Owner>().ToTable("owners");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<PokemonOwner>().ToTable("pokemon_owners");
            modelBuilder.Entity<PokemonCategory>().ToTable("pokemon_categories");
            modelBuilder.Entity<Country>().ToTable("countries");


            modelBuilder.Entity<PokemonCategory>().HasKey(pc => new { pc.PokemonId, pc.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
                .HasKey(po => new { po.PokemonId, po.OwnerId });

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(po => po.Pokemon)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Owner)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(p => p.OwnerId);



        }

    }

}