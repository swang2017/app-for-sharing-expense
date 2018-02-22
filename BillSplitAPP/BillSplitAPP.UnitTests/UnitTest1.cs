using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Splitting;
using System.Collections;
using System.Diagnostics;

namespace BillSplitAPP.UnitTests
{
    [TestClass]
    public class HelperTests
    {

        [TestMethod]
        public void GetFileName_InputFileNameEndWithTXT_ShouldReturnNewFileNameEndWithOut()
        {
            //Arrange
            string expectedName = "expenses.txt.out";
            //Act

            var ActualResult = Helper.GetFileName("/Users/senwang/Projects/BillSplitAPP/BillSplitAPP/expenses.txt");

            //Assert
            Assert.AreEqual(expectedName, ActualResult);
        }

        [TestMethod]
        public void RawDataProcessFunction_DataPatternFollowExpensesDotTXTExample_ShouldReturnAListOfString()
        {
            //Arrange

            List<string> expectedStringList = new List<string> { "3", "2", "10.00", "20.00", "4", "15.00", "15.01", "3.00", "3.01", "3", "5.00", "9.00", "4.00", "2", "2", "8.00", "6.00", "2", "9.20", "6.75", "0" };
           
            //Act

            List<string> ActualResultList = Helper.RawDataProcessFunciton("/Users/senwang/Projects/BillSplitAPP/BillSplitAPP/expenses.txt");

            //Assert
            CollectionAssert.AreEqual(expectedStringList, ActualResultList, StructuralComparisons.StructuralComparer);
        }

    }


    [TestClass]
    public class TripTests
    {
        
        [TestMethod]
        public void LoadTripInfoPersonInfoTripExepnseSummary_DataPatternFollowExpensesDotTXTExample_ShouldLoadAllInfoToListOfTrips()
        {
            //Arrange

            List<string> resultList = Helper.RawDataProcessFunciton("/Users/senwang/Projects/BillSplitAPP/BillSplitAPP/expenses.txt");

            //Act

            Trip.LoadTripInfo(resultList);
            Person.LoadPersonInfo(resultList);
            Trip.LoadTripExpenseSummary();

            //Assert
            Assert.AreEqual(2, Helper.ListOfTrips.Count);
            Assert.AreEqual(1, Helper.ListOfTrips[0].TripID);
            Assert.AreEqual(2, Helper.ListOfTrips[1].TripID);
            Assert.AreEqual(3, Helper.ListOfTrips[0].NumPeople);
            Assert.AreEqual(2, Helper.ListOfTrips[1].NumPeople);
            Assert.AreEqual(0, Helper.ListOfTrips[0].TripStart);
            Assert.AreEqual(12, Helper.ListOfTrips[0].TripEnd);
            Assert.AreEqual(13, Helper.ListOfTrips[1].TripStart);
            Assert.AreEqual(20, Helper.ListOfTrips[1].TripEnd);

            Assert.AreEqual(84.02.ToString(), Helper.ListOfTrips[0].TripExpenseSum.ToString());
            Assert.AreEqual(29.95.ToString(), Helper.ListOfTrips[1].TripExpenseSum.ToString());
            Assert.AreEqual(28.01.ToString(), Helper.ListOfTrips[0].TripExpenseAverage.ToString());
            Assert.AreEqual(14.98.ToString(), Helper.ListOfTrips[1].TripExpenseAverage.ToString());


            Assert.AreEqual(1, Helper.ListOfTrips[0].IndividualData[0].PersonID);
            Assert.AreEqual(2, Helper.ListOfTrips[0].IndividualData[1].PersonID);
            Assert.AreEqual(3, Helper.ListOfTrips[0].IndividualData[2].PersonID);
            Assert.AreEqual(1, Helper.ListOfTrips[1].IndividualData[0].PersonID);
            Assert.AreEqual(2, Helper.ListOfTrips[1].IndividualData[1].PersonID);

            Assert.AreEqual(2, Helper.ListOfTrips[0].IndividualData[0].NumExpenses);
            Assert.AreEqual(4, Helper.ListOfTrips[0].IndividualData[1].NumExpenses);
            Assert.AreEqual(3, Helper.ListOfTrips[0].IndividualData[2].NumExpenses);
            Assert.AreEqual(2, Helper.ListOfTrips[1].IndividualData[0].NumExpenses);
            Assert.AreEqual(2, Helper.ListOfTrips[1].IndividualData[1].NumExpenses);

            Assert.AreEqual(2, Helper.ListOfTrips[0].IndividualData[0].PersonExpenseStart);
            Assert.AreEqual(3, Helper.ListOfTrips[0].IndividualData[0].PersonExpenseEnd);
            Assert.AreEqual(5, Helper.ListOfTrips[0].IndividualData[1].PersonExpenseStart);
            Assert.AreEqual(8, Helper.ListOfTrips[0].IndividualData[1].PersonExpenseEnd);
            Assert.AreEqual(10, Helper.ListOfTrips[0].IndividualData[2].PersonExpenseStart);
            Assert.AreEqual(12, Helper.ListOfTrips[0].IndividualData[2].PersonExpenseEnd);
            Assert.AreEqual(15, Helper.ListOfTrips[1].IndividualData[0].PersonExpenseStart);
            Assert.AreEqual(16, Helper.ListOfTrips[1].IndividualData[0].PersonExpenseEnd);
            Assert.AreEqual(18, Helper.ListOfTrips[1].IndividualData[1].PersonExpenseStart);
            Assert.AreEqual(19, Helper.ListOfTrips[1].IndividualData[1].PersonExpenseEnd);

            Assert.AreEqual((float)10.00, Helper.ListOfTrips[0].IndividualData[0].ItemizedExpense[0]);
            Assert.AreEqual((float)20.00, Helper.ListOfTrips[0].IndividualData[0].ItemizedExpense[1]);
            Assert.AreEqual((float)15.00, Helper.ListOfTrips[0].IndividualData[1].ItemizedExpense[0]);
            Assert.AreEqual((float)15.01, Helper.ListOfTrips[0].IndividualData[1].ItemizedExpense[1]);
            Assert.AreEqual((float)3.00, Helper.ListOfTrips[0].IndividualData[1].ItemizedExpense[2]);
            Assert.AreEqual((float)3.01, Helper.ListOfTrips[0].IndividualData[1].ItemizedExpense[3]);
            Assert.AreEqual((float)5.00, Helper.ListOfTrips[0].IndividualData[2].ItemizedExpense[0]);
            Assert.AreEqual((float)9.00, Helper.ListOfTrips[0].IndividualData[2].ItemizedExpense[1]);
            Assert.AreEqual((float)4.00, Helper.ListOfTrips[0].IndividualData[2].ItemizedExpense[2]);
            Assert.AreEqual((float)8.00, Helper.ListOfTrips[1].IndividualData[0].ItemizedExpense[0]);
            Assert.AreEqual((float)6.00, Helper.ListOfTrips[1].IndividualData[0].ItemizedExpense[1]);
            Assert.AreEqual((float)9.20, Helper.ListOfTrips[1].IndividualData[1].ItemizedExpense[0]);
            Assert.AreEqual((float)6.75, Helper.ListOfTrips[1].IndividualData[1].ItemizedExpense[1]);

            Assert.AreEqual((float)30.00, Helper.ListOfTrips[0].IndividualData[0].PersonSum);
            Assert.AreEqual((float)36.02, Helper.ListOfTrips[0].IndividualData[1].PersonSum);
            Assert.AreEqual((float)18.00, Helper.ListOfTrips[0].IndividualData[2].PersonSum);
            Assert.AreEqual((float)14.00, Helper.ListOfTrips[1].IndividualData[0].PersonSum);
            Assert.AreEqual((float)15.95, Helper.ListOfTrips[1].IndividualData[1].PersonSum);
        }




    }






}






