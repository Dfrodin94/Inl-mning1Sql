using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Inlämning1Sql
{
    class GenealogyCRUD
    {
        private DatabaseSql db = new DatabaseSql(); 
        public string DatabaseName { get; set; } = "Genealogy";

        /// <summary>
        /// Skapar en person i SQL databasen
        /// </summary>
        /// <param name="person">vi använder oss av personobjektets Propertys för värden till SQL </param>
        public void Create (Person person)
        {
            if(!DoesPersonExist(person.FirstName)) 
            {
                db.DatabaseName = DatabaseName;
                db.ExecuteSQL(@$"INSERT INTO People (firstName, lastName, birthDate, deathDate, momID, dadID, 
                                birthPlace, deathPlace)
                                VALUES(@firstName, @lastName, @birthDate, @deathDate, @momID, @dadID, @birthPlace, @deathPlace)",
                                ("@firstName", person.FirstName),
                                ("@lastName", person.LastName),
                                ("@birthDate", person.BirthDate),
                                ("@deathDate", person.DeathDate),
                                ("@momID",person.MomID),
                                ("@dadID",person.DadID),
                                ("@birthPlace",person.BirthPlace),
                                ("@deathPlace",person.DeathPlace));                        
            }
            else
            {
                Console.WriteLine($"A person with the name: {person.FirstName} already exists");

            }

        }

        /// <summary>
        /// Raderar en person i SQL databasen genom dess ID
        /// </summary>
        /// <param name="person">vi använder oss av personobjektets ID(person.Id)  </param>
        public void Delete (Person person)
        {

            db.DatabaseName = DatabaseName;
            db.ExecuteSQL(@$"DELETE FROM people WHERE ID = @ID",
                ("@ID",person.Id));

        }

        /// <summary>
        /// Retunerar en bool baserat på om personen finns i databasen eller inte 
        /// </summary>
        /// <param name="firstName">vi använder oss av firstName då ID instansieras i SQL </param>
        /// (Detta kan ge stora problem om flera har samma förnamn, vilket är vanligt i familjer). 
        /// Fråga till Marcus (hur löser man detta på ett snyggt sett i vanliga fall?) 
        /// <returns>bool</returns>
        public bool DoesPersonExist(string firstName)
        {
            db.DatabaseName = DatabaseName;
            List<Person> personList = List("firstName",firstName);
           
            // TODO ändra till personList[0] sen
            // Fråga mackan om hur man löser det bäst 
            foreach (Person p in personList)
            {
                if(p.FirstName.Equals(firstName)) 
                    {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

            return false;

            

           
        } // funkar, men ej snygg lösning


        /// <summary>
        /// Retunerar ett personobjekts pappa baserat på dess DadID
        /// </summary>
        /// <param name="person">vi använder objektets person.DadID för att retunera pappan </param>
        /// <returns>Person objekt</returns>
        public Person GetFather(Person person)
        {
            db.DatabaseName = DatabaseName;
            List<Person> personList = List("ID", person.DadID.ToString());
            var aPerson = new Person();

            aPerson = personList[0];
            return aPerson;


        }

        /// <summary>
        /// Retunerar ett personobjekts mamma baserat på dess MomID
        /// </summary>
        /// <param name="person">vi använder objektets person.MomID för att retunera mamman </param>
        /// <returns>Person objekt</returns>
        public Person GetMother(Person person)
        {

            db.DatabaseName = DatabaseName;
            List<Person> personList = List("ID", person.MomID.ToString());
            var aPerson = new Person();

            aPerson = personList[0];
            return aPerson;

        }


        /// <summary>
        /// Retunerar ett person objekt från databasen 
        /// </summary>
        /// <param name="firstName">Person objekt vi söker, firstName då ID instansieras i SQL</param>
        /// <returns>Person objekt</returns>
        public Person Read (string firstName)
        {
            db.DatabaseName = DatabaseName;
            DataTable dt;

            dt = db.GetDataTable(@"SELECT TOP 1 * FROM People WHERE firstName =@firstName",
                                ("@firstName", firstName));

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            return GetPersonObject(dt.Rows[0]);
             
        }

        /// <summary>
        /// Uppdaterar en person i databasen utifrån ett person-objekt 
        /// </summary>
        /// <param name="person"> objekt som innehåller allt värde </param>
        public void Update(Person person)
        {
            db.DatabaseName = DatabaseName;
          
            db.ExecuteSQL(@"UPDATE People SET
                            firstName=@firstName, lastName=@lastName,birthDate=@birthDate, deathDate=@deathDate,
                            dadID=@dadID, momID=@momID, birthPlace=@birthPlace, deathPlace=@deathPlace WHERE ID = @ID",
                            ("@firstName",person.FirstName),
                            ("@lastName",person.LastName),
                            ("@birthDate",person.BirthDate),
                            ("@deathDate", person.DeathDate),
                            ("@dadID", person.DadID),
                            ("@momID", person.MomID),
                            ("@ID", person.Id),
                            ("@birthPlace", person.BirthPlace),
                            ("@deathPlace", person.DeathPlace));   
                            


        }

        /// <summary>
        /// Skapar en databas  
        /// </summary>
        /// <param name="name"> namnet på databasen </param>
        internal void CreateDatabase(string name)
        {
            string sqlCmd = ("CREATE DATABASE " + name);

            if (!DoesDatabaseExist(name))
            {
                db.ExecuteSQL(sqlCmd);
            }
        }

        /// <summary>
        /// Kollar om namnet på en databas existerar i Master-branschen i SQL
        /// </summary>
        /// <param name="databaseName"> namnet på databasen som skapas </param>
        /// <returns>bool</returns>
        internal bool DoesDatabaseExist(string databaseName)
        {
            string doesExist = "";

            var sqlCmd = "SELECT COUNT(*) AS data FROM master.dbo.sysdatabases WHERE name=@database";

            var files = db.GetDataTable(sqlCmd, ("@database",databaseName));

            foreach (DataRow row in files.Rows)
            {
                doesExist = $"{row["data"]}";
            }

            if (doesExist == "1")

            /* Detta dör dock om databasen har flera tabeller, då får man konvertera till int, samt try catch på det osv.
             * Då är det bättre att använda sqlCmd.ExecuteScalar
             * Som det står om här: https://stackoverflow.com/questions/50368114/check-if-database-exists-mssql-c-sharp
             */

            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Lägger till en nvarchar kolumn i tabellen People 
        /// </summary>
        /// <param name="name"> namnet på kolumnen som ska läggas till </param>
        internal void AddColumnVarchar(string name)
        {

            db.DatabaseName = DatabaseName;
            db.ExecuteSQL($"ALTER TABLE People ADD {name} [nvarchar](255) NULL;");
        }

        /// <summary>
        /// Tar bort en kolumn från tabellen People 
        /// </summary>
        /// <param name="name"> namnet på kolumnen som ska läggas till </param>
        internal void DeleteColumn(string name)
        {
            db.DatabaseName = DatabaseName;
            db.ExecuteSQL($"ALTER TABLE People DROP COLUMN {name};");

        }

        /// <summary>
        /// Skapar en tabell People med olika kolumner 
        /// </summary>
        internal void CreateTablePeople()
        {
            db.ExecuteSQL(@"USE [Genealogy]
                            CREATE TABLE [dbo].[People](
                                [ID] [int] IDENTITY(1,1) NOT NULL,
                                [firstName] [nvarchar](255) NULL,
                                [lastName] [nvarchar](255) NULL,
                                [birthDate] [nvarchar](255) NULL,
                                [deathDate] [nvarchar](255) NULL,
                                [dadID] [int] NULL,
                                [momID] [int] NULL
                            ) ON [PRIMARY]");


        }


        /// <summary>
        /// Retunerar en lista fylld med person objekt utifrån en SQL query, används internt inom klassen 
        /// </summary>
        /// <param name="condition"> den kolumnen som ska filtreras </param>
        /// <param name="value"> värdet på det som filtreras ex firstName = 'David' </param>
        /// <returns>List Person</returns>

        public List<Person> List(string condition, string value)
        {
            db.DatabaseName = DatabaseName;
            string sqlParamter = "@";
            sqlParamter += condition;

            var sqlCmd = @$"SELECT * FROM People
                           WHERE {condition} = {sqlParamter}";

            var data = db.GetDataTable(sqlCmd, (sqlParamter, value));
            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        /// <summary>
        /// Retunerar en lista fylld med person objekt utifrån det första namnets bokstav, för användare 
        /// </summary>
        /// <param name="letter"> första bokstaven i namnet </param>
        /// <returns>List Person</returns>
        public List<Person> UserListFirstLetter(string letter)
        {
            db.DatabaseName = DatabaseName;

            var sqlCmd = @"SELECT * FROM People
                           WHERE firstName LIKE @firstName + '%' ";

            var data = db.GetDataTable(sqlCmd, ("@firstName", letter));
            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        /// <summary>
        /// Retunerar en lista fylld med person objekt utifrån ett WHERE statement, för användare 
        /// </summary>
        /// <param name="condition"> den kolumnen som ska sökas </param>
        /// <param name="value"> värdet på det som söks </param>
        /// <returns>List Person</returns>
        public List<Person> UserListWhere(string condition, string value)
        {
            db.DatabaseName = DatabaseName;
            string sqlParamter = "@";
            sqlParamter += condition;

            var sqlCmd = @$"SELECT * FROM People
                           WHERE {condition} = {sqlParamter}";

            var data = db.GetDataTable(sqlCmd, (sqlParamter, value));
            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        /// <summary>
        /// Retunerar en lista fylld med person objekt utifrån ett WHERE och OR statement, för användare 
        /// </summary>
        /// <param name="condition1"> den första kolumnen som ska sökas </param>
        /// <param name="value1"> värdet på det första villkoret</param>
        /// <returns>List Person</returns>
        public List<Person> UserListWhereOr(string condition1, string value1, string condition2, string value2)
        {
            db.DatabaseName = DatabaseName;
            string sqlParamter1 = "@";
            sqlParamter1 += condition1;

            string sqlParamter2 = "@";
            sqlParamter2 += condition2;

            var sqlCmd = @$"SELECT * FROM People
                           WHERE {condition1} = {sqlParamter1} OR {condition2} = {sqlParamter2}";

            var data = db.GetDataTable(sqlCmd,
                (sqlParamter1, value1),
                (sqlParamter2, value2));
            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        /// <summary>
        /// Retunerar en lista fylld med person objekt sorterat alfabetiskt utifrån födelsstad, de utan data för föd
        /// de utan data för födelseland tas inte med. 
        /// </summary>
        /// <returns>List Person</returns>
        public List<Person> UserListOrderbyBirthPlace()
        {
            db.DatabaseName = DatabaseName;
          

            var sqlCmd = @"SELECT* FROM People
                           WHERE birthPlace != 'noinfo'
                         ORDER BY birthPlace";

            var data = db.GetDataTable(sqlCmd);
            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }


        /// <summary>
        /// Retunerar ett personobjekt utifrån en Datarow i en datatable 
        /// </summary>
        /// <param name="row"> en datarow från en SQL query </param>
        /// <returns>Person Object</returns>
        private static Person GetPersonObject(DataRow row)
        {
            return new Person
            {
                FirstName = row["firstName"].ToString(),
                LastName = row["lastName"].ToString(),
                BirthDate = row["birthDate"].ToString(),
                DeathDate = row["deathDate"].ToString(),
                DadID = (int)row["dadID"],
                MomID = (int)row["momID"],
                Id = (int)row["ID"],
                BirthPlace = row["birthPlace"].ToString(),
                DeathPlace = row["deathPlace"].ToString()
                
            };

       

    }


    }
}
