using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Splitting
{


    public static class Helper
    {

        // create a global list to contain the trips from the TXT file
        public static List<Trip> ListOfTrips = new List<Trip>();

        public static string GetFileName(String pathToTxtFile)
        {
            String OutputFileName = Path.GetFileName(pathToTxtFile) + ".out";
            return OutputFileName;
        }

        public static List<string> RawDataProcessFunciton(String pathToTxtFile)
        {
            int LineCounter = 0;
            String LineValue;
            StreamReader sr = new StreamReader(pathToTxtFile);
            List<String> CampInfoSumArray = new List<String>();
            LineValue = sr.ReadLine();

            while (LineValue != null)
            {
                if (LineValue.Length != 0)
                {
                    CampInfoSumArray.Add(LineValue);
                    LineCounter++;
                }
                LineValue = sr.ReadLine();
            }
            sr.Close();

            return CampInfoSumArray;
        }

    }

    public class Trip
    {
        public int TripID;
        public int TripEnd;
        public int NumPeople;
        public int TripStart;
        public float TripExpenseSum;
        public float TripExpenseAverage;
        public List<Person> IndividualData = new List<Person>();


        public static void LoadTripInfo(List<string> CampInfoSumArray)
        {
            int TripCounter = 0;
            for (int i = 0; i < CampInfoSumArray.Count() - 1; i++)
            {
                if (CampInfoSumArray[i].Length < 3 && CampInfoSumArray[i + 1].Length < 3)
                {
                    Trip Trip = new Trip();
                    Helper.ListOfTrips.Add(Trip);
                    Trip.TripID = TripCounter + 1;
                    Trip.TripStart = i;
                    Trip.NumPeople = Int32.Parse(CampInfoSumArray[i]);
                    TripCounter++;
                }

            }

            //  load end point of each trip

            for (int i = 0; i < Helper.ListOfTrips.Count() - 1; i++)
            {
                Helper.ListOfTrips[i].TripEnd = Helper.ListOfTrips[i + 1].TripStart - 1;
            }
            Helper.ListOfTrips[Helper.ListOfTrips.Count() - 1].TripEnd = CampInfoSumArray.Count() - 1;
        }



        public static void LoadTripExpenseSummary()
        {
            for (int i = 0; i < Helper.ListOfTrips.Count(); i++)
            {
                float TempTripSum = 0;
                for (int j = 0; j < Helper.ListOfTrips[i].IndividualData.Count(); j++)
                {
                    TempTripSum += Helper.ListOfTrips[i].IndividualData[j].PersonSum;
                }
                Helper.ListOfTrips[i].TripExpenseSum = TempTripSum;

                float temporaryNumber = Helper.ListOfTrips[i].TripExpenseSum / Helper.ListOfTrips[i].NumPeople;
                Helper.ListOfTrips[i].TripExpenseAverage = (float)(Math.Round((double)temporaryNumber, 2));
            }
        }

    }


    public class Person
    {
        public int PersonID;
        public int NumExpenses;
        public float PersonSum;
        public int PersonExpenseEnd;
        public int PersonExpenseStart;
        public List<float> ItemizedExpense = new List<float>();



        public static void LoadPersonInfo(List<string> CampInfoSumArray)
        {
            for (int i = 0; i < Helper.ListOfTrips.Count(); i++)
            {
                int NumPersonCounter = 0;
                for (int j = Helper.ListOfTrips[i].TripStart; j <= Helper.ListOfTrips[i].TripEnd - 1; j++)
                {

                    if (CampInfoSumArray[j].Length < 3 && CampInfoSumArray[j + 1].Length >= 3)
                    {
                        Person Person = new Person();

                        Person.PersonID = NumPersonCounter + 1;
                        Person.NumExpenses = Int32.Parse(CampInfoSumArray[j]);
                        Person.PersonExpenseStart = j + 1;
                        Person.PersonExpenseEnd = Person.NumExpenses + Person.PersonExpenseStart - 1;
                        for (int n = Person.PersonExpenseStart; n <= Person.PersonExpenseEnd; n++)
                        {
                            Person.ItemizedExpense.Add(float.Parse(CampInfoSumArray[n]));
                        }
                        Person.PersonSum = Person.ItemizedExpense.Sum();

                        NumPersonCounter++;

                        Helper.ListOfTrips[i].IndividualData.Add(Person);

                    }

                }

            }

        }

    }


   



    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string NewFileName = Helper.GetFileName("/Users/senwang/Projects/BillSplitAPP/BillSplitAPP/expenses.txt");
                List<string> resultList = Helper.RawDataProcessFunciton("/Users/senwang/Projects/BillSplitAPP/BillSplitAPP/expenses.txt");
                Trip.LoadTripInfo(resultList);
                Person.LoadPersonInfo(resultList);
                Trip.LoadTripExpenseSummary();


                String fmt = "$#,##0.00;($#,##0.00)";
                using (StreamWriter writer = new StreamWriter(NewFileName))
                {
                    for (int i = 0; i < Helper.ListOfTrips.Count(); i++)
                    {
                        for (int j = 0; j < Helper.ListOfTrips[i].IndividualData.Count(); j++)
                        {
                            writer.WriteLine((Helper.ListOfTrips[i].TripExpenseAverage - Helper.ListOfTrips[i].IndividualData[j].PersonSum).ToString(fmt));
                        }
                        writer.WriteLine();
                    }
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }

        }
    }
}
