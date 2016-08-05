using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BandTracker
{
  public class BandTest : IDisposable
  {
    public BandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange,Act
      int result = Band.GetAll().Count;
      //Assert
      Assert.Equal(0,result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      DateTime testTime = new DateTime(2016, 1, 1);
      Band firstBand = new Band("A",testTime);
      Band secondBand = new Band("A",testTime);
      Assert.Equal(firstBand,secondBand);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      DateTime testTime = new DateTime(2016, 1, 1);
      Band testBand = new Band("A",testTime);

      testBand.Save();
      List<Band> result = Band.GetAll();
      List<Band> testList = new List<Band>{testBand};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      DateTime testTime = new DateTime(2016, 1, 1);

      Band testBand=new Band("A",testTime);

      testBand.Save();
      Band savedBand = Band.GetAll()[0];
      int result=savedBand.GetId();
      int testId=testBand.GetId();

      Assert.Equal(testId,result);
    }

    [Fact]
    public void Test_Find_FindsBandInDatabase()
    {
      DateTime testTime = new DateTime(2016, 1, 1);

      Band testBand = new Band("A",testTime);
      testBand.Save();

      Band foundBand = Band.Find(testBand.GetId());

      Assert.Equal(testBand, foundBand);
    }

    [Fact]
    public void Test_AddVenue_AddsVenueToBand()
    {
      DateTime testTime = new DateTime(2016, 1, 1);

      //Arrange
      Band testBand = new Band("A",testTime);
      testBand.Save();

      Venue testVenue = new Venue("Hello");
      testVenue.Save();

      //Act
      testBand.AddVenue(testVenue);
      List<Venue> result = testBand.GetVenues();
      List<Venue> testList = new List<Venue> {testVenue};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]

    public void Test18_GetVenues_ReturnsAllBandVenues()
    {
      DateTime testTime = new DateTime(2016, 1, 1);

      Band testBand = new Band("A",testTime);
      testBand.Save();

      Venue testVenue1 = new Venue("Hello");
      testVenue1.Save();

      Venue testVenue2 = new Venue(" Stuff");
      testVenue2.Save();

      testBand.AddVenue(testVenue1);
      List<Venue> result = testBand.GetVenues();
      List<Venue> testList= new List<Venue>{testVenue1};

      Assert.Equal(testList,result);
    }

    [Fact]
    public void Test_Delete_DeletesBandAssociationsFromDatabase()
    {
      //Arrange
      Venue testVenue = new Venue("Hello");
      testVenue.Save();
      DateTime testTime = new DateTime(2016, 1, 1);

      string testName = "A";
      Band testBand = new Band(testName,testTime);
      testBand.Save();

      //Act
      testBand.AddVenue(testVenue);
      testBand.Delete();

      List<Band> resultVenueBands = testVenue.GetBands();
      List<Band> testVenueBands = new List<Band> {};

      //Assert
      Assert.Equal(testVenueBands, resultVenueBands);
    }

    [Fact]
    public void Test_UpDateInDatabase()
    {
      //Arrange
      DateTime testTime = new DateTime(2016, 1, 1);
      string testName = "A";
      Band testBand = new Band(testName,testTime);
      testBand.Save();
      testBand.Update("B",testTime);
      //Act
      //Assert
      Assert.Equal("B", testBand.GetName());
    }


    public void Dispose()
    {
      Band.DeleteAll();
    }
  }
}
