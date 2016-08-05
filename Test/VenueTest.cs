using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BandTracker
{
  public class VenueTest : IDisposable
  {
    public  VenueTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void T1_VenuesEmptyAtFirst()
    {
      // Arrange , Act
      int result = Venue.GetAll().Count;

      // Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void T2_Equal_ReturnsTrueForSameName()
    {
      // Arrange, act
      Venue firstVenue = new Venue("A");
      Venue secondVenue = new Venue("A");

      // Assert
      Assert.Equal(firstVenue, secondVenue);
    }

    [Fact]
    public void T3_Save_SavesVenueToDatabase()
    {
      // Arrange
      Venue testVenue = new Venue("A");
      testVenue.Save();

      // Act
      List<Venue> result = Venue.GetAll();
      List<Venue> testList = new List<Venue>{testVenue};

      // Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void T4_Save_AssignsIdToCagetoryObject()
    {
      //Arrange

      Venue testVenue = new Venue("A");
      testVenue.Save();

      //Act
      Venue savedVenue = Venue.GetAll()[0];

      int result = savedVenue.GetId();
      int testId = testVenue.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void T5_Find_FindsVenueInDatabase()
    {
      //Arrange
      Venue testVenue = new Venue("A");
      testVenue.Save();

      //Act
      Venue foundVenue = Venue.Find(testVenue.GetId());

      //Assert
      Assert.Equal(testVenue, foundVenue);
    }


    [Fact]
    public void T7_Update_UpdatesVenueInDatabase()
    {
      //Arrange
      string name = "A place";
      Venue testVenue = new Venue(name);
      testVenue.Save();
      string newName = "B place";

      //Act
      testVenue.Update(newName);

      string result = testVenue.GetName();

      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void T8_Delete_DeletesVenueFromDatabase()
    {
      //Arrange
      string name1 = "A place";
      Venue testVenue1 = new Venue(name1);
      testVenue1.Save();

      string name2 = "B place";
      Venue testVenue2 = new Venue(name2);
      testVenue2.Save();

      //Act
      testVenue1.Delete();
      List<Venue> resultVenues = Venue.GetAll();
      List<Venue> testVenueList = new List<Venue> {testVenue2};

      //Assert
      Assert.Equal(testVenueList, resultVenues);
    }

    [Fact]
    public void Test_AddBand_AddsBandToVenue()
    {
      //Arrange
      Venue testVenue = new Venue("A");
      testVenue.Save();
      DateTime testTime = new DateTime(2016, 1, 1);

      Band testBand = new Band("A",testTime);
      testBand.Save();

      Band testBand2 = new Band("B",testTime);
      testBand2.Save();

      //Act
      testVenue.AddBand(testBand);
      testVenue.AddBand(testBand2);

      List<Band> result = testVenue.GetBands();
      List<Band> testList = new List<Band> {testBand, testBand2};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetBands_ReturnAllVenueBands()
    {
      Venue testVenue = new Venue("place");
      testVenue.Save();
      DateTime testTime = new DateTime(2016, 1, 1);

      Band testBand1= new Band("A",testTime);
      testBand1.Save();

      Band testBand2 = new Band("C",testTime);
      testBand2.Save();

      testVenue.AddBand(testBand1);
      List<Band> savedBands = testVenue.GetBands();
      List<Band> testList= new List<Band>{testBand1};
      Assert.Equal(testList,savedBands);
    }

    [Fact]
    public void Test_Delete_DeletesVenueAssociationsFromDatabase()
    {
      //Arrange
      DateTime testTime = new DateTime(2016, 1, 1);

      Band testBand = new Band("A",testTime);
      testBand.Save();

      string testName = "Home stuff";
      Venue testVenue = new Venue(testName);
      testVenue.Save();

      //Act
      testVenue.AddBand(testBand);
      testVenue.Delete();

      List<Venue> resultBandVenues = testBand.GetVenues();
      List<Venue> testBandVenues = new List<Venue> {};
      List<Band> resultsBands= Band.GetAll();
      List<Band> testBands= new List<Band>{testBand};

      //Assert
      Assert.Equal(testBandVenues, resultBandVenues);
      Assert.Equal(testBands, resultsBands);

    }

    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }
  }
}
