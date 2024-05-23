using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Persistence.Configurations;

public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.ToTable("TMAgent","dbo");

        builder.HasKey(t=>t.AgentID);

        builder.Property(t => t.AgentID)
            .HasColumnName("sAgentID")
            .HasColumnType("nvarchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t=>t.UserName)
            .HasColumnName("sUserName")
            .HasColumnType("nvarchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.UserImage)
            .HasColumnName("iUserImage")
            .HasColumnType("image");
                    
    }
}
