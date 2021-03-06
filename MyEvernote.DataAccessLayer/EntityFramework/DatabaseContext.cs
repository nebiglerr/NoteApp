using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
     public class DatabaseContext : DbContext
    {
        // veritabanına karsılık gelen clasları dbset oalrak burada tanımlıyoruz 
        public DbSet<EvernoteUser> EvernoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likeds { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        #region Override
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // FluentAPI EF Mimarisi ile DatabaseContext oluşurken ezerek veritabanı bağlantısındaki cascade işlemlerini EF mimarisi ile yapımı
        //    İlişkili tablolarda x tablosuna bağlı olan y tablosundaki verileride sil anlamıno taşır 

        //    modelBuilder.Entity<Note>()
        //        .HasMany(n => n.Comments)
        //        .WithRequired(c => c.Note)
        //        .WillCascadeOnDelete(true);

        //    modelBuilder.Entity<Note>()
        //        .HasMany(n => n.Likes)
        //        .WithRequired(c => c.Note)
        //        .WillCascadeOnDelete(true);
        //}
        #endregion
    }
}
