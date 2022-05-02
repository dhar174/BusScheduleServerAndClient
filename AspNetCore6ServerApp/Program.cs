using Classes;

public class Program
{

    public static Routes? route1;
    public static Routes? route2;
    public static Routes? route3;

    public static List<Routes> routes = new List<Routes>();

    public static List<Routes> stops = new List<Routes>();

    public static List<Stops> static_stops_list = new List<Stops>();


    public static void Main()
    {
        CompileStaticStopsList(10);

        route1 = new Routes(1);
        route2 = new Routes(2);
        route3 = new Routes(3);


        routes.Add(route1);
        routes.Add(route2);
        routes.Add(route3);
        // Populate the static stops list first

        var args = new WebApplicationOptions
        {
            ApplicationName = typeof(Program).Assembly.FullName,
            ContentRootPath = Directory.GetCurrentDirectory(),
            EnvironmentName = Environments.Development,
            WebRootPath = "Test"
        };
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.AllowSynchronousIO = true;
            serverOptions.Limits.MaxRequestBodySize = null;
            serverOptions.Limits.MinRequestBodyDataRate = null;
            serverOptions.Limits.MinResponseDataRate = null;
            serverOptions.ApplicationServices.CreateAsyncScope();
            serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(1);
            serverOptions.Limits.Http2.KeepAlivePingDelay = TimeSpan.FromSeconds(1);
        });


        Console.WriteLine($"Application Name: {builder.Environment.ApplicationName}");
        Console.WriteLine($"Environment Name: {builder.Environment.EnvironmentName}");
        Console.WriteLine($"ContentRoot Path: {builder.Environment.ContentRootPath}");
        Console.WriteLine($"WebRootPath: {builder.Environment.WebRootPath}");

        Console.WriteLine("Starting WebApplication");


        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:8080",
                                                      "http://localhost:8081", "http://localhost:*3000")
                                                      .AllowAnyHeader()
                                                      .AllowAnyMethod();
                              });
        });

        builder.Services.AddMemoryCache();
        builder.Services.AddMvcCore();
        builder.Services.AddLogging((loggingBuilder) =>
        {
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();
        });


        builder.Services.AddRouting();
        builder.Services.AddResponseCompression();


        builder.Services.AddRouting();
        builder.Services.AddSingleton<Program>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddResponseCaching();
        var app = builder.Build();



        var current_timestamp = DateTime.Parse(DateTime.Now.ToString("HH:mm:ss"));

        var y = ScheduleRetrievalHelper.GetNextArrivalByStop(7, current_timestamp);

        var time_span = ScheduleRetrievalHelper.GetTimeUntilNextArrival(current_timestamp, y.Item1);

        var result_string = string.Format("Next Arrival at Stop {0} will be at: {1} in {2} minutes and {3} seconds", 7, y.Item1, Math.Abs(time_span) / 60, ((Math.Abs(time_span) - ((Math.Abs(time_span) / 60) * 60))));



        app.UseRouting();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Test")),
            RequestPath = "/Test",
            ServeUnknownFileTypes = true
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("default", "{controller=Test}/{action=GetNextArrivalTime}/{type?}/{id?}");
            endpoints.MapRazorPages();
        });
        app.UseFileServer(new FileServerOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Test")),
            RequestPath = "/Test",
            EnableDirectoryBrowsing = true
        });
        app.UseResponseCaching();
        app.UseWebSockets();
        app.ConfigureAwait(true);
        //app.UseSession();

        app.MapControllers();

        app.MapPost("/Test", () => result_string);
        app.MapRazorPages();
        //app.MapDefaultControllerRoute();
        app.UsePathBase("/Test");
        //app.StartAsync();
        // app.Use(async (context, next) =>
        // {
        //     await context.Response.WriteAsync("Hello World!");
        //     await next.Invoke();
        // });


        app.Run(url: "https://localhost:8080");


    }

    public static void CompileStaticStopsList(int num_stops)
    {
        for (int i = 0; i < num_stops; i++)
        {
            Stops stop = new Stops(i, i);
            Program.static_stops_list.Add(stop);
        }
    }

    public static String AcceptRequests(String request_type, int object_id)
    {


        var current_timestamp = DateTime.Parse(DateTime.Now.ToString("HH:mm:ss"));
        switch (request_type)
        {
            case "route":
                var route = routes[object_id];
                if (route != null)
                {
                    var y = ScheduleRetrievalHelper.GetNextArrivalOnRoute(route, current_timestamp);

                    var time_span = ScheduleRetrievalHelper.GetTimeUntilNextArrival(current_timestamp, y);
                    var next_on_route = string.Format("Next Arrival at Route {0} will be at: {1} in {2} minutes and {3} seconds", object_id, y, Math.Abs(time_span) / 60, ((Math.Abs(time_span) - ((Math.Abs(time_span) / 60) * 60))));
                    return next_on_route;

                }
                else
                {
                    return string.Format(("Route " + object_id + " does not exist."));
                }
            case "stop":
                {
                    var y = ScheduleRetrievalHelper.GetNextArrivalByStop(object_id, current_timestamp);

                    var time_span = ScheduleRetrievalHelper.GetTimeUntilNextArrival(current_timestamp, y.Item1);

                    var result_string = string.Format("Next Arrival at Stop {0} will be at: {1} in {2} minutes and {3} seconds", object_id, y.Item1, Math.Abs(time_span) / 60, ((Math.Abs(time_span) - ((Math.Abs(time_span) / 60) * 60))));
                    return result_string;
                }
            default:
                return string.Format("Invalid request type."); ;
        }


    }


}



