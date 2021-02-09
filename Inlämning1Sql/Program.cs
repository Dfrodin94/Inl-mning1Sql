using System;
using System.Collections.Generic;

namespace Inlämning1Sql
{
    class Program
    {
        static void Main(string[] args)
        {

            // skapar databas
            /*
            crud.DatabaseName = "Master";
            crud.ExecuteSQL("CREATE Database Genealogy");
            crud.DatabaseName = "Genealogy";
            */

            // skapar tabell 
            /*
            crud.ExecuteSQL(@"USE [Genealogy]
                            CREATE TABLE [dbo].[People](
                                [ID] [int] IDENTITY(1,1) NOT NULL,
                                [firstName] [nvarchar](255) NULL,
                                [lastName] [nvarchar](255) NULL,
                                [birthDate] [nvarchar](255) NULL,
                                [deathDate] [nvarchar](255) NULL,
                                [dadID] [int] NULL,
                                [momID] [int] NULL
                            ) ON [PRIMARY]");
            */

            //lägger till personer i databas
            /* crud.ExecuteSQL(@"INSERT INTO People (firstName, lastName, birthDate, deathDate, dadID, momID)
                                 VALUES('David','Frödin','1994', '2122', 2, 3)");
            */

            GenealogyCRUD crud = new GenealogyCRUD();

            Person Theo = new Person("theo", "frödin", "1994", "aldrig", 4, 6);

            crud.Create(Theo);


           







}
}
}
