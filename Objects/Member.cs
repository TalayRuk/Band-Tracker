using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace BandTracker
{
  public class Member
  {

    public static Dictionary<string,object> AllData(string message)
    {
      List<Band> AllBand = Band.GetAll();
      List<Venue> AllVenue = Venue.GetAll();
      Dictionary<string,object> model= new Dictionary<string,object>{};
      model.Add("band",AllBand);
      model.Add("venue",AllVenue);
      model.Add("message",message);
      return model;
    }


  }

}
