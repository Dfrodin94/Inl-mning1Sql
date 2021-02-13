using System;
using System.Collections.Generic;

namespace Inlämning1Sql
{
    class Program
    {
        static void Main(string[] args)
        {
            GenealogyCRUD crud = new GenealogyCRUD();

            Person theo = new Person("king", "frödin", "1994", "aldrig", 1, 7);
            Person momTheo = new Person("Gudinna", "frödin", "1994", "aldrig", 1, 7);
            momTheo.Id = 6;
            crud.Create(momTheo);
            Person dadTheo = new Person();






            //Console.WriteLine(momTheo.ToString());
            //Console.WriteLine(dadTheo.ToString());

            List<Person> aList = crud.List("","firstName");

            foreach(Person p in aList)

            {
                Console.WriteLine(p.ToString());
            }


    




        }
}
}
