using Domain.Nucleus.Entities.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insfraestructure.PrincipalContext.Data.Configurations.Application
{
    public class LogRequestAndResponseConfiguration : IEntityTypeConfiguration<LogRequestAndResponse>
    {
        public void Configure(EntityTypeBuilder<LogRequestAndResponse> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("LogRequestAndResponse", "application");

            builder.Property(e => e.ResponseDate).HasColumnType("datetime");

            builder.Property(e => e.RequestDate).HasColumnType("datetime");

            builder.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");
        }
    }
}
