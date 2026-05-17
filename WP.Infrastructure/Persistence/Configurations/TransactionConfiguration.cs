namespace WP.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de la entidad Transaction para Entity Framework Core.
/// </summary>
public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    /// <summary>
    /// Configura la entidad Transaction.
    /// </summary>
    /// <param name="builder">Constructor de la entidad.</param>
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(t => t.AccountId)
            .HasColumnName("account_id")
            .IsRequired();

        builder.OwnsOne(t => t.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("amount")
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasConversion(
                    currency => currency.Code,
                    code => Currency.FromTrustedSource(code))
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(t => t.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(t => t.TransferId)
            .HasColumnName("transfer_id");

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.OwnsMany(t => t.Tags, tag =>
        {
            tag.ToTable("transaction_tags");
            tag.WithOwner().HasForeignKey("transaction_id");
            tag.Property(t => t.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();
        });
    }
}
