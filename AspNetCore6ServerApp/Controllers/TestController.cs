global using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace BusSchedServerCore.Controllers;
using Models;

public class TestController : Controller
{

    // 
    // GET: /test/Welcome/ 

    public string GetNextArrivalTime(string type, int id)
    {
        var result = Program.AcceptRequests(type, id);
        Console.WriteLine(result);
        return result;
    }

    // 
    // GET: /test/
    public IActionResult Index()
    {
        MyModel model = new MyModel()
        {
            Message = "Hello World!"
        }; return View();
    }
}