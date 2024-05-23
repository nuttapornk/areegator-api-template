using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Persistence.Configurations;

public class AgentAddressConfiguration : IEntityTypeConfiguration<AgentAddress>
{
    public void Configure(EntityTypeBuilder<AgentAddress> builder)
    {
        builder.ToTable("TMAgentAddress", "dbo");

        builder.HasKey(t => t.AgentAddressID);

        builder.Property(t => t.AgentAddressID)
            .HasColumnType("int")
            .IsRequired();
    }
}
