using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Inlämning1Sql
{
    class GenealogyCRUD
    {
        private DatabaseSql db = new DatabaseSql(); // kanske bättre att bara göra denna statisk? 
        public string DatabaseName { get; set; } = "Genealogy";

        public void Create (Person person)
        {
            if(!DoesPersonExist(person.FirstName)) 
            {
                db.DatabaseName = DatabaseName;
                db.ExecuteSQL(@$"INSERT INTO People (firstName, lastName, birthDate, deathDate, momID, dadID)
                                VALUES(@firstName, @lastName, @birthDate, @deathDate, @momID, @dadID)",
                                ("@firstName", person.FirstName),
                                ("@lastName", person.LastName),
                                ("@birthDate", person.BirthDate),
                                ("@deathDate", person.DeathDate),
                                ("@momID",person.MomID),
                                ("@dadID",person.DadID));                        
            }
            else
            {
                Console.WriteLine($"A person with the name: {person.FirstName} already exists");

            }

        }

        public void Delete (Person person)// TODO: ändra till ID sen
        {

            db.DatabaseName = DatabaseName;
            db.ExecuteSQL(@$"DELETE FROM people WHERE firstName = @firstName",
                ("@firstName",person.FirstName));

        }

        public bool DoesPersonExist(string firstName)
        {
            db.DatabaseName = DatabaseName;
            List<Person> personList = List($"firstName = '{firstName}'");
           
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

        public bool DoesPersonExist(int id)
        {
            db.DatabaseName = DatabaseName;
            List<Person> personList = List($"ID = '{id}'");

            foreach (Person p in personList)
            {
                if (p.Id.Equals(id))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            return false;
        }

        public Person GetFather(Person person)
        {
            db.DatabaseName = DatabaseName;
            List<Person> personList = List($"ID = '{person.DadID}'");
            var aPerson = new Person();

            foreach (Person p in personList)
            {
                if (p.Id.Equals(person.DadID))
                {
                    aPerson = p;
                }

            }

            return aPerson;



        }

        public Person GetMother(Person person)
        {

            db.DatabaseName = DatabaseName;
            List<Person> personList = List($"ID = '{person.MomID}'");
            var aPerson = new Person();


            foreach (Person p in personList)
            {
                if (p.Id.Equals(person.MomID))
                {
                    aPerson = p;
                }

            }

            return aPerson;

        }

        public List<Person> GetChildren (Person person)
        {

            db.DatabaseName = DatabaseName;
            List<Person> personList = List($"momID = '{person.Id}' OR dadID = '{person.Id}'");
            return personList;

        }

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

        public void Update(Person person)
        {
            db.DatabaseName = DatabaseName;
            List($"firstName = '{person.FirstName}'");

            db.ExecuteSQL(@"UPDATE People SET
                            firstName=@firstName, lastName=@lastName,birthDate=@birthDate, deathDate=@deathDate,
                            dadID=@dadID, momID=@momID WHERE ID = @ID",
                            ("@firstName",person.FirstName),
                            ("@lastName",person.LastName),
                            ("@birthDate",person.BirthDate),
                            ("@deathDate", person.DeathDate),
                            ("@dadID", person.DadID),
                            ("@momID", person.MomID),
                            ("@ID", person.Id));


        }

        internal void CreateDatabase(string name)
        {
            string sqlCmd = ("CREATE DATABASE " + name);

            if (!DoesDatabaseExist(name))
            {
                db.ExecuteSQL(sqlCmd);
            }
        }

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

        internal void AddColumn(string name, string field)
        {

            db.DatabaseName = DatabaseName;
            db.ExecuteSQL($"ALTER TABLE People ADD {name} {field};");
        }

        internal void DeleteColumn(string name)
        {
            db.DatabaseName = DatabaseName;
            db.ExecuteSQL($"ALTER TABLE People DROP COLUMN {name};");

        }

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

        public List<Person> List(string filter = "", string orderby = "")
        {
            db.DatabaseName = DatabaseName;

            var sqlCmd = "SELECT";
            sqlCmd += "* FROM People";
            if (filter != "") sqlCmd += " WHERE " + filter;
            if (orderby != "") sqlCmd += " ORDER BY " + orderby;

            var data = db.GetDataTable(sqlCmd);
            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        public List<Person> List2(string filter = "", string orderby = "")
        {
            db.DatabaseName = DatabaseName;

            // TODO: Här är det kaoz, måste ha parameterar, fixa sen! 
            var sqlCmd = "SELECT";
            sqlCmd += "* FROM People";
            if (filter != "") sqlCmd += " WHERE firstName = @firstName";
            if (orderby != "") sqlCmd += " ORDER BY " + orderby;

            Console.WriteLine(sqlCmd); 
            
            var data = db.GetDataTable(sqlCmd, ("@firstName", filter));
            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

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
                Id = (int)row["ID"]
            };

       

    }


    }
}
