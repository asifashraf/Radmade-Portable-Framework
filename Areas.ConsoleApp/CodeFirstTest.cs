namespace Areas.ConsoleApp
{
    using System.Data.Entity;
    using System.Linq;
    using Areas.Lib.CodeFirst;

    public class CodeFirstTest : ConsoleTestClass
    {
        public override void InitTest()
        {
            this.Step(() => Database.SetInitializer(new Init()), "Set Initializer");

            Start("Initializing data context");
            using (var ctx = new CodeContext(ConnectionString))
            {
                End("Initialized data context");

                //printing all users
                this.Step(() =>
                    {
                        var attendees = ctx.Attendees.ToList();
                        foreach (var attendee in attendees)
                        {
                            Print("{0}, {1}", attendee.FirstName, attendee.LastName);
                        }
                    }, "All users");

                //Count users
                this.Step(() =>
                {
                    Print(ctx.Attendees.Count());
                }, "Count users"
            );

                this.Read();
            }
        }
    }
}