namespace Areas.Lib.CodeFirst
{
    using System.Data.Entity.ModelConfiguration;

    public class AttendeeConfiguration : EntityTypeConfiguration<Attendee>
    {
        public AttendeeConfiguration()
        {
            Property(p => p.LastName).IsRequired().HasMaxLength(50);
        }
    }
}