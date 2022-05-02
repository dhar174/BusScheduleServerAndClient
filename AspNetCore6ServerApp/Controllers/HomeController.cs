global using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace BusSchedServerCore.Controllers;
using Models;

public class HomeController : Controller
{

    // 
    // GET: /Home/Welcome/ 

    public string Welcome()
    {
        return "This is the Welcome action method...";
    }

    // 
    // GET: /Home/
    public IActionResult Index()
    {
        MyModel model = new MyModel()
        {
            Message = "Hello World!"
        }; return View();
    }
}