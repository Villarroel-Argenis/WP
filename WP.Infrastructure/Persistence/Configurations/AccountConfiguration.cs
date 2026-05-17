namespace WP.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de la entidad Account para Entity Framework.
/// </summary>
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    /// <summary>
    /// Configura la entidad Account.
    /// </summary>
    /// <param name="builder">El constructor de la entidad.</param>
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("id");

        builder.Property(a => a.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(a => a.Balance, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("balance_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasConversion(
                    c => c.Code, // Convertir a string para almacenar en la base de datos
                    s => Currency.FromTrustedSource(s) // Convertir de string a Currency al leer de la base de datos
                )
                .HasColumnName("balance_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

    }
}
