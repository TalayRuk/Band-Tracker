using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace BandTracker
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };
      Post["/venue/new"]=_=>{
        Venue newVenue= new Venue(Request.Form["new-venue-name"]);
        newVenue.Save();
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };
      Delete["/delete/venue/{id}"]=parameters=>{
        Venue newVenue= Venue.Find(parameters.id);
        newVenue.Delete();
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };


      Post["/band/new"]=_=>{
        Band newBand= new Band(Request.Form["new-band-name"],Request.Form["new-band-date"]);
        newBand.Save();
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };
      Delete["/delete/band/{id}"]=parameters=>{
        Band newBand= Band.Find(parameters.id);
        newBand.Delete();
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };
    }
  }
}
