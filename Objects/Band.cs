using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace BandTracker
{
  public class Band
  {
    private int _id;
    private string _name;
    private DateTime _createDate;
    public Band(string Name, DateTime CreateDate, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _createDate = CreateDate;
    }

    public override bool Equals(System.Object otherBand)
    {
        if (!(otherBand is Band))
        {
          return false;
        }
        else
        {
          Band newBand = (Band) otherBand;
          bool idEquality = this.GetId() == newBand.GetId();
          bool nameEquality = this.GetName() == newBand.GetName();
          bool createDateEquality = this.GetCreateDate()==newBand.GetCreateDate();
          return (idEquality && nameEquality && createDateEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }

    public DateTime GetCreateDate()
    {
      return _createDate;
    }

    public static List<Band> GetAll()
    {
      List<Band> AllBands = new List<Band>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        DateTime bandCreateDate = rdr.GetDateTime(2);
        Band newBand = new Band(bandName, bandCreateDate, bandId);
        AllBands.Add(newBand);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllBands;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands (name,create_date) OUTPUT INSERTED.id VALUES (@BandName,@createDate);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@BandName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);


      SqlParameter createDateParameterName = new SqlParameter();
      createDateParameterName.ParameterName = "@createDate";
      createDateParameterName.Value = this.GetCreateDate();

      cmd.Parameters.Add(createDateParameterName);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM bands; DELETE FROM bands_venues;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Band Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId;", conn);
      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = id.ToString();
      cmd.Parameters.Add(bandIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundBandId = 0;
      string foundBandName = null;
      DateTime foundBandCreateDate = new DateTime(1000, 1, 1);

      while(rdr.Read())
      {
        foundBandId = rdr.GetInt32(0);
        foundBandName = rdr.GetString(1);
        foundBandCreateDate = rdr.GetDateTime(2);
      }
      Band foundBand = new Band(foundBandName, foundBandCreateDate,foundBandId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBand;
    }
    public void AddVenue(Venue newVenue)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (venue_id, band_id) VALUES (@VenueId, @BandId);", conn);

      SqlParameter venueIdParameter = new SqlParameter();
      venueIdParameter.ParameterName = "@VenueId";
      venueIdParameter.Value = newVenue.GetId();
      cmd.Parameters.Add(venueIdParameter);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bandIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Venue> GetVenues()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT venues.* From bands JOIN bands_venues ON ( bands.id=bands_venues.band_id) Join venues ON (venues.id=bands_venues.venue_id) WHERE bands.id= @BandId",conn);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName= "@BandId";
      bandIdParameter.Value=this.GetId();
      cmd.Parameters.Add(bandIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      List<Venue> allVenues= new List<Venue>{};
      while(rdr.Read())
      {
        int thisVenueId = rdr.GetInt32(0);
        string venueName = rdr.GetString(1);
        Venue foundVenue = new Venue(venueName, thisVenueId);
        allVenues.Add(foundVenue);
      }

      if(rdr !=null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
      return allVenues;
    }

        public void Update(string newName, DateTime newDate)
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("UPDATE bands SET name = @NewName,create_date = @NewDate OUTPUT INSERTED.name, INSERTED.create_date WHERE id = @BandId;", conn);

          SqlParameter newNameParameter = new SqlParameter();
          newNameParameter.ParameterName = "@NewName";
          newNameParameter.Value = newName;
          cmd.Parameters.Add(newNameParameter);

          SqlParameter newDateParameter = new SqlParameter();
          newDateParameter.ParameterName = "@NewDate";
          newDateParameter.Value = newDate;
          cmd.Parameters.Add(newDateParameter);

          SqlParameter bandIdParameter = new SqlParameter();
          bandIdParameter.ParameterName = "@BandId";
          bandIdParameter.Value = this.GetId();
          cmd.Parameters.Add(bandIdParameter);
          SqlDataReader rdr = cmd.ExecuteReader();

          while(rdr.Read())
          {
            this._name = rdr.GetString(0);
            this._createDate = rdr.GetDateTime(1);

          }

          if (rdr != null)
          {
            rdr.Close();
          }

          if (conn != null)
          {
            conn.Close();
          }
        }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @BandId; DELETE FROM bands_venues WHERE band_id = @BandId;", conn);
      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();

      cmd.Parameters.Add(bandIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

  }
}
