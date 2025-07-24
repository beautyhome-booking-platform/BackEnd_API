using Domain.Constrans;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class FlawlessDBContext : IdentityDbContext<UserApp>
    {
        public FlawlessDBContext(DbContextOptions<FlawlessDBContext> options) : base(options) { }

        public DbSet<UserApp> UserApps { get; set; }
        public DbSet<ServiceCategories> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceOption> ServiceOptions { get; set; }
        public DbSet<Appointment> Appointments { get; set; }   
        public DbSet<AppointmentDetail> AppointmentDetails { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ArtistAvailability> ArtistsAvailabilitys { get; set; }
        public DbSet<ArtistProgress> ArtistsProgresss { get; set; }
        public DbSet<BankInfo> BankInfos { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<HistoryRefund> HistoryRefunds { get; set; }
        public DbSet<Notification> Notifications { get; set; }  
        public DbSet<Post> Posts { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<UserProgress> UserProgresss { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<InformationArtist> InformationArtists { get; set; } 
        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<ChatHistory> ChatHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Service Categories
            builder.Entity<ServiceCategories>()
                .Property(sc => sc.Id)
                .ValueGeneratedOnAdd();

            // Service
            builder.Entity<Service>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Service>()
                .HasOne(s => s.ServiceCategory)
                .WithMany(sc => sc.Services)
                .HasForeignKey(s => s.ServiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Service Option
            builder.Entity<ServiceOption>()
                .Property(so => so.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<ServiceOption>()
                .HasOne(so => so.Service)  
                .WithMany(s => s.ServiceOptions)  
                .HasForeignKey(so => so.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ServiceOption>()
                .Property(so => so.Price)
                .HasColumnType("decimal(18,2)");
            
            // Appoinment 
            builder.Entity<Appointment>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Appointment>()
                .HasMany(ad => ad.AppointmentsDetails)
                .WithOne(a => a.Appointment)
                .HasForeignKey(a => a.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Appointment>()
                .Property(a => a.TotalAmount)
                .HasColumnType("decimal(18,2)");
            builder.Entity<Appointment>()
                .Property(a => a.AmountToPayForArtist)
                .HasColumnType("decimal(18,2)");
            builder.Entity<Appointment>()
                .Property(a => a.DepositForApp)
                .HasColumnType("decimal(18,2)");
            builder.Entity<Appointment>()
                .Property(a => a.TotalAmountAfterDiscount)
                .HasColumnType("decimal(18,2)");
            builder.Entity<Appointment>()
                .Property(a => a.TotalDiscount)
                .HasColumnType("decimal(18,2)");

            // Appointment - Customer Relationship
            builder.Entity<Appointment>()
                .HasOne(a => a.Customer)  // Liên kết với Customer
                .WithMany()  // Một Customer có thể có nhiều Appointment
                .HasForeignKey(a => a.CustomerId)  // CustomerId là khóa ngoại
                .OnDelete(DeleteBehavior.Restrict);  // Không cho phép Cascade Delete khi xóa Customer

                    // Appointment - ArtistMakeup Relationship
            builder.Entity<Appointment>()
                .HasOne(a => a.ArtistMakeup)  // Liên kết với ArtistMakeup
                .WithMany()  // Một ArtistMakeup có thể có nhiều Appointment
                .HasForeignKey(a => a.ArtistMakeupId)  // ArtistMakeupId là khóa ngoại
                .OnDelete(DeleteBehavior.Restrict);  // Không cho phép Cascade Delete khi xóa ArtistMakeup

            // Appoinment Detail
            builder.Entity<AppointmentDetail>()
                .Property(ap => ap.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<AppointmentDetail>()
                .Property(ap => ap.UnitPrice)
                .HasColumnType("decimal(18.2)");

            // Artist Availiability
            builder.Entity<ArtistAvailability>()
                .Property(aa => aa.Id)
                .ValueGeneratedOnAdd();

            //Artist Progress
            builder.Entity<ArtistProgress>()
                .Property(ap => ap.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<ArtistProgress>()
                .Property(ap => ap.TotalReceive)
                .HasColumnType("decimal(18.2)");


            // User Progress
            builder.Entity<UserProgress>()
                .Property(up => up.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<UserProgress>()
                .Property(ap => ap.TotalSpent)
                .HasColumnType("decimal(18.2)");

            // BankInfo
            builder.Entity<ArtistProgress>()
                .Property(bi => bi.Id)
                .ValueGeneratedOnAdd();

            // Commission
            builder.Entity<Commission>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Commission>()
                .Property(c => c.Rate)
                .HasColumnType("decimal(5,2)");
            builder.Entity<Commission>()
                .Property(c => c.Amount)
                .HasColumnType("decimal(18,2)");
            // Conversation
            builder.Entity<Conversation>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Conversation>()
                .HasMany(c => c.Messages) 
                .WithOne(m => m.Conversation) 
                .HasForeignKey(m => m.ConversationId);

                        // Conversation - User1 Relationship
            builder.Entity<Conversation>()
                .HasOne(c => c.User1)  // Liên kết với Customer
                .WithMany()  // Một Customer có thể có nhiều Appointment
                .HasForeignKey(c => c.User1Id)  // CustomerId là khóa ngoại
                .OnDelete(DeleteBehavior.Restrict);  // Không cho phép Cascade Delete khi xóa Customer

                        // Conversation - User2 Relationship
            builder.Entity<Conversation>()
                .HasOne(c => c.User2)  // Liên kết với ArtistMakeup
                .WithMany()  // Một ArtistMakeup có thể có nhiều Appointment
                .HasForeignKey(c => c.User2Id)  // ArtistMakeupId là khóa ngoại
                .OnDelete(DeleteBehavior.Restrict);  // Không cho phép Cascade Delete khi xóa ArtistMakeup


            // Message
            builder.Entity<Message>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Message>()
                .HasOne(m => m.Sender)              
                .WithMany()                         
                .HasForeignKey(r => r.SenderId)     
                .OnDelete(DeleteBehavior.Restrict); 


            builder.Entity<Message>()
                .HasOne(m => m.Receiver)           
                .WithMany()                        
                .HasForeignKey(m => m.ReceiverId)  
                .OnDelete(DeleteBehavior.Restrict);


            // History Refund
            builder.Entity<HistoryRefund>()
                .Property(hr => hr.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<HistoryRefund>()
                .Property(hr => hr.RefundAmount)
                .HasColumnType("decimal(18.2)");

            // Post
            builder.Entity<Post>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // Report
            builder.Entity<Report>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Report>()
                .HasOne(r => r.Reporter)            
                .WithMany()                         
                .HasForeignKey(r => r.ReporterId)  
                .OnDelete(DeleteBehavior.Restrict); 

                            
            builder.Entity<Report>()
                .HasOne(r => r.ReportedUser)           
                .WithMany()                            
                .HasForeignKey(r => r.ReportedUserId)  
                .OnDelete(DeleteBehavior.Restrict);

            // Area
            builder.Entity<Area>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();
            


            builder.Entity<Area>()
                .HasIndex(a => new { a.City, a.District })
                .IsUnique();

            // InformationArtist
            builder.Entity<InformationArtist>()
                .Property(ia => ia.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<InformationArtist>()
                .Property(ia => ia.MinPrice)
                .HasColumnType("decimal(18.2)");
            builder.Entity<InformationArtist>()
                .Property(ia => ia.MaxPrice)
                .HasColumnType("decimal(18.2)");
            


            // Transaction
            builder.Entity<Transaction>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Transaction>()
                .Property(hr => hr.Amount)
                .HasColumnType("decimal(18.2)");

            builder.Entity<ArtistProgress>()
                .HasOne(ap => ap.InformationArtist)
                .WithOne(ia => ia.ArtistProgress)
                .HasForeignKey<InformationArtist>(ia => ia.ArtistProgressId)
                .OnDelete(DeleteBehavior.Cascade);  // hoặc Restrict tuỳ ý mày

            // Voucher
            builder.Entity<Voucher>()
                .Property(v => v.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Voucher>()
                .Property(hr => hr.DiscountValue)
                .HasColumnType("decimal(18.2)");
            builder.Entity<Voucher>()
                .Property(hr => hr.MinTotalAmount)
                .HasColumnType("decimal(18.2)");
            // Notification
            builder.Entity<Notification>()
                .Property(n => n.Id)
                .ValueGeneratedOnAdd();

            // Feedback
            builder.Entity<Notification>()
                .Property(n => n.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Certificate>()
                .Property(n => n.Id)
                .ValueGeneratedOnAdd();

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                var now = DateTime.UtcNow;


                if (entry.State == EntityState.Added)
                {
                    entity.CreateAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdateAt = now;
                }

                // Nếu là xóa, bạn có thể cập nhật thêm các trường liên quan đến xóa
                if (entry.State == EntityState.Deleted)
                {
                    entity.DeleteAt = now;
                    entity.IsDeleted = true;
                }
            }
        }
    }
    
}
