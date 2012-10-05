namespace Areas.Lib.CodeFirst
{
    using System;
    using System.ComponentModel.DataAnnotations;

    ////Poco
    //strings are nullable by default, we need to decorate with Required attribute
    //Other column types are not nullable unless we put ? with the type declaration

    public class Attendee
    {
        public int AttendeeID { get; set; }

        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}