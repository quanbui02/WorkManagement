using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Work.DataContext.Models;

namespace Work.DataContext
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasOne(u => u.IdentityUser)
                   .WithOne(i => i.UsersNavigation)
                   .HasForeignKey<Users>(u => u.IdentityUserId);

            // Có thể thêm các cấu hình khác nếu cần
        }
    }
}
