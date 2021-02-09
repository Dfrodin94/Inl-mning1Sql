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


        internal string ConnectionString { get; set; } = @"Data Source=.\SQLExpress;Integrated Security=true;database={0}";
        public string DatabaseName { get; set; } = "Genealogy";
        public int MaxRows { get; set; } = 10;
        public string OrderBy { get; set; } = "surName";
        public void Create (Person person)
        {
            if(DoesPersonExist(person.FirstName)) 
            {
                Console.WriteLine($"A person with the name {person.FirstName} already exists"); 
            }
            else
            {
                db.DatabaseName = DatabaseName;
                db.ExecuteSQL(@$"INSERT INTO People (firstName, lastName, birthDate, deathDate, dadID, momID)
                                VALUES('{person.FirstName}','{person.LastName}','{person.BirthDate}',
                                        '{person.DeathDate}',{person.DadID}, {person.MomID})");
            }

        }
        public void Delete (Person person)
        {

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

        /* public List<Person> List (string filter = "firstName LIKE @input", string paramValue)
        {
            List<Person> list = new List<Person>();
            return list; 

        }
        */

        public Person Read (string name)
        {

            Person p = new Person();
            return p;
        }

        public void Update(Person person)
        {

        }

        internal void CreateDatabase(string name, bool OpenNewDatabase = false)
        {
            db.DatabaseName = DatabaseName;

            db.ExecuteSQL("CREATE DATABASE " + name);
            if (OpenNewDatabase) DatabaseName = name;
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
