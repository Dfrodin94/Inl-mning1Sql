using System;
using System.Collections.Generic;

namespace Inlämning1Sql
{
    class Program
    {
        static void Main(string[] args)
        {
            GenealogyCRUD crud = new GenealogyCRUD();
            // crud.CreateDatabase("Genealogy");
            // crud.CreateTablePeople();
          
            Person david = new Person("David", "Frödin", "1994", "alive", 0, 0);
            Person theo = new Person("Theo", "Frödin", "2010", "alive", 0, 0);
            Person natalia = new Person("Natalia", "Frödin", "1974", "alive", 0, 0);
            Person ulf = new Person("Ulf", "Johansson", "1954", "alive", 0, 0);
            Person nadja = new Person("Nadja", "Johansson", "1954", "alive", 0, 0);
            Person kjell = new Person("Kjell", "Frödin", "1947", "alive", 0, 0);
            Person shawn = new Person("Shawn", "Yermain", "1962", "alive", 0, 0);
            Person josef = new Person("Josef", "Yermain", "1931", "alive", 0, 0);
            Person kaye = new Person("Kaye", "Yermain", "1934", "1984", 0, 0);
            Person serguej = new Person("Serguej", "Rodisevich", "1957", "alive", 0, 0);
            Person galina = new Person("Galina", "Rolåva", "1934", "2021", 0, 0);
            Person vladimir = new Person("Vladimir", "Safranov", "1932", "1995", 0, 0);

            /*crud.Create(david);
            crud.Create(theo);
            crud.Create(natalia);
            crud.Create(ulf);
            crud.Create(nadja);
            crud.Create(kjell);
            crud.Create(shawn);
            crud.Create(josef);
            crud.Create(kaye);
            crud.Create(serguej);
            crud.Create(galina);
            crud.Create(vladimir);
            */

            david = crud.Read(david.FirstName);
            theo = crud.Read(theo.FirstName);
            natalia = crud.Read(natalia.FirstName);
            ulf = crud.Read(ulf.FirstName);
            nadja = crud.Read(nadja.FirstName);
            kjell = crud.Read(kjell.FirstName);
            shawn = crud.Read(shawn.FirstName);
            josef = crud.Read(josef.FirstName);
            kaye = crud.Read(kaye.FirstName);
            serguej = crud.Read(serguej.FirstName);
            galina = crud.Read(galina.FirstName);
            vladimir = crud.Read(vladimir.FirstName);

            david.DadID = shawn.Id;
            david.MomID = natalia.Id;
            crud.Update(david);

            theo.DadID = ulf.Id;
            theo.MomID = natalia.Id;
            crud.Update(theo);

            natalia.DadID = serguej.Id;
            natalia.MomID = nadja.Id;
            crud.Update(natalia);

            shawn.DadID = josef.Id;
            shawn.MomID = kaye.Id;
            crud.Update(shawn);

            nadja.MomID = galina.Id;
            nadja.DadID = vladimir.Id;
            crud.Update(nadja);

            List<Person> people = crud.List("firstName LIKE 'D%'", "");
            foreach(Person p in people)
            {
                Console.WriteLine(p.ToString());
            }

            people = crud.List("birthDate = '1994'", "");
            foreach (Person p in people)
            {
                Console.WriteLine(p.ToString());
            }

            people = crud.List("momID = 0 OR dadID = 0", "");
            foreach (Person p in people)
            {
                Console.WriteLine(p.ToString());
            }

            Person momDavid = crud.GetMother(david);
            Console.WriteLine(momDavid.ToString());

            people = crud.List($"momID = {natalia.Id}", "");
            foreach (Person p in people)
            {
                Console.WriteLine(p.ToString());
            }



        }
}
}
