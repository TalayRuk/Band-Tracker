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
    }
  }
}
