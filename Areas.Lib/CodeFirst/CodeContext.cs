namespace Areas.Lib.CodeFirst
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;

    public class CodeContext : DbContext
    {
         private ObjectContext objectContext;
        public ObjectContext ObjectContext
        {
            get
            {
                if(objectContext.IsNull())
                {
                    objectContext = ((IObjectContextAdapter)this).ObjectContext;
                }
                return objectContext;
            }
            set
            {
                this.objectContext = value;
            }
        }

         public CodeContext(string connectionString)
             : base(connectionString)
        {
        }

        public DbSet<Attendee> Attendees { get; set; }

        //overrides
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Attendee>().Property(p => p.LastName).IsRequired().HasMaxLength(50);
        }
    }
}