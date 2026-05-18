namespace WP.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de la entidad OutboxMessage para Entity Framework.
/// </summary>
public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    /// <summary>
    /// Configura la entidad OutboxMessage.
    /// </summary>
    /// <param name="builder">El constructor de la entidad.</param>
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id");

        builder.Property(o => o.Type)
            .HasColumnName("type")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(o => o.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(o => o.ProcessedAt)
            .HasColumnName("processed_at");

        builder.Property(o => o.Error)
            .HasColumnName("error");
    }
}
