namespace Inlämning1Sql
{
    internal class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string DeathDate { get; set; }
        public int DadID { get; set; }
        public int MomID { get; set; }

        public Person(string firstName, string surName, string birthDate, string deathDate, int dadID, int momID)
        {
            this.FirstName = firstName;
            this.LastName = surName;
            this.BirthDate = birthDate;
            this.DeathDate = deathDate;
            this.DadID = dadID;
            this.MomID = momID;
        }

        public Person()
        {
        }

        public override string ToString()
        {
            return @$"Name: {FirstName} {LastName} ID: {Id}";
        }
    }
}