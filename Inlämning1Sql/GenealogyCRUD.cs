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
        public int MaxRows { get; set; } = 10;
        public string OrderBy { get; set; } = "surName";
        public void Create (Person person)
        {
            if(!DoesPersonExist(person.FirstName)) 
            {
                db.DatabaseName = DatabaseName;
                db.ExecuteSQL(@$"INSERT INTO People (firstName, lastName, birthDate, deathDate)
                                VALUES(@firstName, @lastName, @birthDate, @deathDate)",
                                ("@firstName", person.FirstName),
                                ("@lastName", person.LastName),
                                ("@birthDate", person.BirthDate),
                                ("@deathDate", person.DeathDate));                        
            }
            else
            {
                Console.WriteLine($"A person with the name: {person.FirstName} already exists");

            }

        }

        public void Delete (Person person)
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
            bool b = true;
            return b;

        }

        public void GetFather(Person person)
        {

        }

        public void GetMother(Person person)
        {

        }

        public Person Read (string name)
        {

            Person p = new Person();
            return p;
        }

        public void Update(Person person)
        {

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

        internal void CreateTable(string name, string fields)
        {

            db.DatabaseName = DatabaseName;

            db.ExecuteSQL($"CREATE TABLE {name} ({fields});");
        }

        public List<Person> List(string filter = "", string orderBy = "lastName", int max = 10)
        {
            var db = new DatabaseSql
            {
                DatabaseName = DatabaseName
            };

            var sql = "SELECT";
            if (max > 0) sql += " TOP " + max.ToString();
            sql += "* From People";
            if (filter != "") sql += " WHERE " + filter;
            if (orderBy != "") sql += " ORDER BY " + orderBy;
            var data = db.GetDataTable(sql);
            var lst = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                lst.Add(GetPersonObject(row));
            }
            return lst;
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
