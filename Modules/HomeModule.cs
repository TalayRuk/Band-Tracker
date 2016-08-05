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
      Patch["/edit/venue/{id}"]=parameters=>{

        Venue newVenue= Venue.Find(parameters.id);
        newVenue.Update(Request.Form["edit-venue-name"]);
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };

      Post["/venue/addBands/{id}"]=parameters=>{
        Venue newVenue= Venue.Find(parameters.id);
        Band newBand= Band.Find(Request.Form["addBands-id"]);

        newVenue.AddBand(newBand);
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };

      Get["/view/venue/{id}"]=parameters=>{
        Venue newVenue= Venue.Find(parameters.id);
        List<Band> bandsofVenue=newVenue.GetBands();
        Dictionary<string,object> info = new Dictionary<string,object>{};
        info.Add("venue",newVenue);
        info.Add("bands",bandsofVenue);

        return View["bandsofVenue.cshtml", info];

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

      Patch["/edit/band/{id}"]=parameters=>{

        Band newBand= Band.Find(parameters.id);
        newBand.Update(Request.Form["edit-band-name"],Request.Form["edit-band-date"]);
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };

      Post["/add/band/addVenue/{id}"]=parameters=>{
        Band newBand= Venue.Find(parameters.id);
        Venue newVenue= Venue.Find(Request.Form["addVenues-id"]);

        newBand.AddVenue(newVenue);
        Dictionary<string,object> model=Member.AllData("Welcome");
        return View["index.cshtml", model];
      };

      Get["/view/bands/{id}"]=parameters=>{
        Band newBand= Band.Find(parameters.id);
        List<Venue> venueOfBand=newBand.GetVenues();
        Dictionary<string,object> info = new Dictionary<string,object>{};
        info.Add("band",newBand);
        info.Add("venues",venueOfBand);

        return View["venueOfBand.cshtml", info];

      };

    }
  }
}
