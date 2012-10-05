namespace Areas.Lib.CodeFirst
{
    using System.Data.Entity;

    ////initializer
    public class Init : DropCreateDatabaseIfModelChanges<CodeContext>
    {
        protected override void Seed(CodeContext context)
        {
            base.Seed(context);

            context.Attendees.Add(new Attendee { FirstName = "Asif", LastName = "Ashraf" });

            context.SaveChanges();
        }
    }
}