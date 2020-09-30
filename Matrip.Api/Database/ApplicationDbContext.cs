using Matrip.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Matrip.Web.Database
{
    public class ApplicationDbContext : IdentityDbContext<ma01user, ma37role, int>
    {
        private readonly string _sufixo = "ma01";
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ma01user>().ToTable("ma01user");
            builder.Entity<ma01user>().HasKey(t => t.Id);
            builder.Entity<ma01user>().Property(p => p.LockoutEnd).HasColumnName(_sufixo + "LockoutEnd");
            builder.Entity<ma01user>().Property(p => p.TwoFactorEnabled).HasColumnName(_sufixo + "TwoFactorEnabled");
            builder.Entity<ma01user>().Property(p => p.PhoneNumberConfirmed).HasColumnName(_sufixo + "PhoneNumberConfirmed");
            builder.Entity<ma01user>().Property(p => p.PhoneNumber).HasColumnName(_sufixo + "PhoneNumber");
            builder.Entity<ma01user>().Property(p => p.ConcurrencyStamp).HasColumnName(_sufixo + "ConcurrencyStamp");
            builder.Entity<ma01user>().Property(p => p.SecurityStamp).HasColumnName(_sufixo + "SecurityStamp");
            builder.Entity<ma01user>().Property(p => p.PasswordHash).HasColumnName(_sufixo + "PasswordHash");
            builder.Entity<ma01user>().Property(p => p.EmailConfirmed).HasColumnName(_sufixo + "EmailConfirmed");
            builder.Entity<ma01user>().Property(p => p.NormalizedEmail).HasColumnName(_sufixo + "NormalizedEmail");
            builder.Entity<ma01user>().Property(p => p.Email).HasColumnName(_sufixo + "Email");
            builder.Entity<ma01user>().Property(p => p.NormalizedUserName).HasColumnName(_sufixo + "NormalizedUserName");
            builder.Entity<ma01user>().Property(p => p.UserName).HasColumnName(_sufixo + "UserName");
            builder.Entity<ma01user>().Property(p => p.Id).HasColumnName(_sufixo + "Iduser");
            builder.Entity<ma01user>().Property(p => p.LockoutEnabled).HasColumnName(_sufixo + "LockoutEnabled");
            builder.Entity<ma01user>().Property(p => p.AccessFailedCount).HasColumnName(_sufixo + "AccessFailedCount");

        }

        public DbSet<ma02profile> ma02profile { get; set; }
        public DbSet<ma03language> ma03language { get; set; }
        public DbSet<ma04guide> ma04guide { get; set; }
        public DbSet<ma05trip> ma05trip { get; set; }
        public DbSet<ma06tripcategory> ma06tripcategory { get; set; }
        public DbSet<ma07country> ma07country { get; set; }
        public DbSet<ma08uf> ma08uf { get; set; }
        public DbSet<ma09city> ma09city { get; set; }
        public DbSet<ma10userlanguages> ma10userlanguages { get; set; }
        public DbSet<ma11service> ma11service { get; set; }
        public DbSet<ma12SubtripGuide> ma12SubtripGuide { get; set; }
        public DbSet<ma13tripphoto> ma13tripphoto { get; set; }
        public DbSet<ma14subtrip> ma14subtrip { get; set; }
        public DbSet<ma15subtripphoto> ma15subtripphoto { get; set; }
        public DbSet<ma16subtripschedule> ma16subtripschedule { get; set; }
        public DbSet<ma17SubtripValue> ma17SubtripValue { get; set; }
        public DbSet<ma18tripitemshoppingcart> ma18tripitemshoppingcart { get; set; }
        public DbSet<ma19SubTripItemShoppingCart> ma19SubTripItemShoppingCart { get; set; }
        public DbSet<ma20ServiceItemShoppingCart> ma20ServiceItemShoppingCart { get; set; }
        public DbSet<ma21saleTrip> ma21saleTrip { get; set; }
        public DbSet<ma22subtripsale> ma22subtripsale { get; set; }
        public DbSet<ma23servicesale> ma23servicesale { get; set; }
        public DbSet<ma24payment> ma24payment { get; set; }
        public DbSet<ma25partner> ma25partner { get; set; }
        public DbSet<ma26PartnerGuide> ma26PartnerGuide { get; set; }
        public DbSet<ma27AgeDiscount> ma27AgeDiscount { get; set; }
        public DbSet<ma28SaleTourist> ma28SaleTourist { get; set; }
        public DbSet<ma29TouristShoppingCart> ma29TouristShoppingCart { get; set; }
        public DbSet<ma30GuideSubtripShoppingCart> ma30GuideSubtripShoppingCart { get; set; }
        public DbSet<ma31SubtripSaleGuide> ma31SubtripSaleGuide { get; set; }
        public DbSet<ma32sale> ma32sale { get; set; }
        public DbSet<ma33UserAddress> ma33UserAddress { get; set; }
        public DbSet<ma34TransferencePendencies> ma34TransferencePendencies { get; set; }
        public DbSet<ma35cityphoto> ma35cityphoto { get; set; }
        public DbSet<ma36SubtripGroup> ma36SubtripGroup { get; set; }

        public DbSet<ma38InfluencerDiscount> ma38InfluencerDiscount { get; set; }

    }
}
