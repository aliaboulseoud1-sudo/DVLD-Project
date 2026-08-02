using DVLD_Buisness;
using System;
using System.Data;

namespace DVLD_Test
{
    internal class Program
    {
        /*
        static void testAddNewPerson()
        {
            clsPerson Person1 = new clsPerson();

            Person1.NationalNo = "N12346";
            Person1.FirstName = "Ali";
            Person1.SecondName = "Waheed";
            Person1.ThirdName = "Aboul-Seoud";
            Person1.LastName = "Test";
            Person1.DateOfBirth = new DateTime(2000, 1, 1);
            Person1.Gender = clsPerson.enGender.Male;
            Person1.Address = "Menoufia, Egypt";
            Person1.PhoneNumber = "01000000000";
            Person1.Email = "ali@test.com";
            Person1.CountryID = 1;
            Person1.ImagePath = "";


            Console.WriteLine("Saving the person to the database...");


            if (Person1._AddNewPerson())
            {
                Console.WriteLine($"Successfully added! The new PersonID is: {Person1.PersonID}");
            }
            else
            {
                Console.WriteLine("The addition operation failed! Check the log file to find out the reason..");
            }
        }        
        */

        /*
        static void testUpdatePerson()
        {          
            clsPerson Person1 = new clsPerson(1046, "N90", "Ali", "Waheed", "",
                                              "Aboulseoud", DateTime.Now, 0, "Street1",
                                              "011232323", "test@gmail.com", 1, "");

            Console.WriteLine("Updating this person to the database...");


            if (Person1._UpdatePerson())
            {
                Console.WriteLine($"Successfully Updated! PersonID is: {Person1.PersonID}");
            }
            else
            {
                Console.WriteLine("The Updating operation failed! Check the log file to find out the reason..");
            }
        }
        */
        
        /*
       static void testFindPerson(int PersonID)
       {          
           clsPerson person = clsPerson.Find(PersonID);

           Console.WriteLine("Finding this person in the database...");

            if (person != null)
            {
                Console.WriteLine("\n---------------------------------------------------------");
                Console.WriteLine("                    PERSON DETAILS                       ");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($" Person ID   : {person.PersonID}");
                Console.WriteLine($" National No : {person.NationalNo}");
                Console.WriteLine($" Full Name   : {person.FirstName} {person.SecondName} {person.ThirdName} {person.LastName}".Replace("  ", " "));
                Console.WriteLine($" Gender      : {(person.Gender == clsPerson.enGender.Male ? "Male" : "Female")}");
                Console.WriteLine($" Date Of Birth: {person.DateOfBirth:dd/MM/yyyy}");
                Console.WriteLine($" Phone       : {person.PhoneNumber}");
                Console.WriteLine($" Email       : {(string.IsNullOrWhiteSpace(person.Email) ? "[N/A]" : person.Email)}");
                Console.WriteLine($" Address     : {person.Address}");
                Console.WriteLine($" Country ID  : {person.CountryID}");
                Console.WriteLine($" Image Path  : {(string.IsNullOrWhiteSpace(person.ImagePath) ? "[No Image]" : person.ImagePath)}");
                Console.WriteLine("---------------------------------------------------------\n");
            }
            else
            {
                Console.WriteLine("\n[!] Person record was not found.\n");
            }
        }

        static void testFindPerson(string NationalNo)
        {
            clsPerson person = clsPerson.Find(NationalNo);

            Console.WriteLine("Finding this person in the database...");

            if (person != null)
            {
                Console.WriteLine("\n---------------------------------------------------------");
                Console.WriteLine("                    PERSON DETAILS                       ");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($" Person ID   : {person.PersonID}");
                Console.WriteLine($" National No : {person.NationalNo}");
                Console.WriteLine($" Full Name   : {person.FirstName} {person.SecondName} {person.ThirdName} {person.LastName}".Replace("  ", " "));
                Console.WriteLine($" Gender      : {(person.Gender == clsPerson.enGender.Male ? "Male" : "Female")}");
                Console.WriteLine($" Date Of Birth: {person.DateOfBirth:dd/MM/yyyy}");
                Console.WriteLine($" Phone       : {person.PhoneNumber}");
                Console.WriteLine($" Email       : {(string.IsNullOrWhiteSpace(person.Email) ? "[N/A]" : person.Email)}");
                Console.WriteLine($" Address     : {person.Address}");
                Console.WriteLine($" Country ID  : {person.CountryID}");
                Console.WriteLine($" Image Path  : {(string.IsNullOrWhiteSpace(person.ImagePath) ? "[No Image]" : person.ImagePath)}");
                Console.WriteLine("---------------------------------------------------------\n");
            }
            else
            {
                Console.WriteLine("\n[!] Person record was not found.\n");
            }
        }
        */

        /*
        static void testSave()
        {
            clsPerson person = new clsPerson();

            person.NationalNo = "N12346";
            person.FirstName = "Ali";
            person.SecondName = "Waheed";
            person.ThirdName = "Aboul-Seoud";
            person.LastName = "Test";
            person.DateOfBirth = new DateTime(2000, 1, 1);
            person.Gender = clsPerson.enGender.Male;
            person.Address = "Menoufia, Egypt";
            person.PhoneNumber = "01000000000";
            person.Email = "ali@test.com";
            person.CountryID = 1;
            person.ImagePath = "";

            Console.WriteLine("Saving the person to the database...");


            if (person.Save())
            {
                Console.WriteLine($"Successfully added! The new PersonID is: {person.PersonID}");
            }
            else
            {
                Console.WriteLine("The addition operation failed! Check the log file to find out the reason..");
            }

            person.FirstName = "Aly";

            Console.WriteLine("Updating this person...");


            if (person.Save())
            {
                Console.WriteLine($"Successfully Updated! The updated PersonID is: {person.PersonID}");
            }
            else
            {
                Console.WriteLine("The Updating operation failed! Check the log file to find out the reason..");
            }
        }
        */

        /*
        static void testDelete(int PersonID)
        {
            if (clsPerson.DeletePerson(PersonID))
                Console.WriteLine($"Person with ID = [{PersonID}] deleted successfuly");
            else
                Console.WriteLine($"Person with ID = [{PersonID}] NOT deleted successfuly");
        }
        */

        /*
        static void testGetAllPeople()
        {
            Console.WriteLine("\n--- Testing GetAllPeople() ---");

            DataTable dtPeople = clsPerson.GetAllPeople();

            if (dtPeople != null && dtPeople.Rows.Count > 0)
            {
                Console.WriteLine($"[Success] Retrieved {dtPeople.Rows.Count} person(s) from database.\n");

                Console.WriteLine($"{"ID",-5} | {"National No",-12} | {"Full Name",-30} | {"Phone",-15}");
                Console.WriteLine(new string('-', 70));

                foreach (DataRow row in dtPeople.Rows)
                {
                    int personID = Convert.ToInt32(row["PersonID"]);
                    string nationalNo = row["NationalNo"].ToString();

                    string fullName = $"{row["FirstName"]} {row["SecondName"]} {row["ThirdName"]} {row["LastName"]}";
                    string phone = row["Phone"].ToString();

                    Console.WriteLine($"{personID,-5} | {nationalNo,-12} | {fullName,-30} | {phone,-15}");
                }
            }
            else
            {
                Console.WriteLine("[Notice] No people found in the database or an error occurred.");
            }
        }
        */

        /*
        static void testIsPersonExist(int PersonID)
        {
            if(clsPerson.IsPersonExist(PersonID))
                Console.WriteLine("EXIST");
            else
                Console.WriteLine("NOT EXIST");
        }

        static void testIsPersonExist(string NationalNo)
        {
            if (clsPerson.IsPersonExist(NationalNo))
                Console.WriteLine("EXIST");
            else
                Console.WriteLine("NOT EXIST");
        }
        */



        static void Main(string[] args)
        {

            //testAddNewPerson();
            //testUpdatePerson();
            //testFindPerson(null);
            //testSave();
            //testDelete(1);
            //testGetAllPeople();
            //testIsPersonExist(" ");



        }
    }
}