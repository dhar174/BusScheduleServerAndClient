using Classes;
using System.Data;
public class ScheduleRetrievalHelper
{
    public static String next_arrival_time = "";



    public static String GetNextArrivalOnRoute(Routes route, DateTime current_time)
    {
        var current_time_string = current_time.ToString("HH:mm:ss");
        if (route.stop_queue.Count == 0)
        {
            Console.WriteLine("No stops in queue");
            return next_arrival_time;
        }
        else
        {
            if (route.stop_queue.Count == 0)
            {
                Console.WriteLine("No stops in queue");
                return next_arrival_time;
            }
            next_arrival_time = (String)route.stop_queue.Peek();
            var Time_span = GetTimeUntilNextArrival(current_time, next_arrival_time);
            Console.WriteLine("Time_span in routes: " + Time_span);
            while (Time_span > 0)
            {
                next_arrival_time = (String)route.stop_queue.Dequeue();
                Time_span = GetTimeUntilNextArrival(current_time, next_arrival_time);

                if (Time_span < (15 * 60) && DateTime.Compare(DateTime.ParseExact(next_arrival_time, "HH:mm:ss", null), current_time) > 0)
                {

                    return next_arrival_time;
                }
                continue;
            }
        }
        return next_arrival_time;
    }

    public static (String, int) GetNextArrivalByStop(int stop_id, DateTime current_time)
    {
        var current_time_string = current_time.ToString("HH:mm:ss");
        var times = new List<DateTime>();

        var routesu = new List<Routes>();
        var stopsu = new List<int>();
        int found_in_route = 0;
        int found_in_stop = 0;

        Console.WriteLine("route count: " + Program.routes.Count);

        foreach (var route in Program.routes)
        {

            found_in_route = (int)route.id;


            if (route.stop_queue.Count == 0)
            {
                Console.WriteLine("No stops in queue");
                return (next_arrival_time, found_in_route);
            }
            else
            {

                //next_arrival_time = route.next_stop.Value.stop_queue.Where(x => x.Contains(stop_id.ToString())).FirstOrDefault();
                next_arrival_time = (String)route.next_stop.Value.stop_queue.Peek();
                Console.WriteLine("next_arrival_time: " + next_arrival_time);


                var Time_span = GetTimeUntilNextArrival(current_time, next_arrival_time);
                Console.WriteLine("Time_span in stops: " + Time_span);
                foreach (var stop in route.stops_list_linked)
                {



                    next_arrival_time = (String)route.stop_queue.Dequeue();


                    Time_span = GetTimeUntilNextArrival(current_time, next_arrival_time);

                    if (Time_span < (15 * 60) && DateTime.Compare(DateTime.ParseExact(next_arrival_time, "HH:mm:ss", null), current_time) > 0)
                    {
                        routesu.Add(route);
                        stopsu.Add(stop_id);
                        times.Add(DateTime.ParseExact(next_arrival_time, "HH:mm:ss", null));
                        found_in_stop = stop_id;
                        break;
                    }
                }


            }


        }


        for (int i = 0; i < times.Count; i++)
        {

            Console.Write("time in set: " + (i + 1) + "  " + times[i].ToString("HH:mm:ss"));
        }


        var closestTime = times.OrderBy(x => Math.Abs(x.Subtract(current_time).TotalSeconds)).First();

        Console.WriteLine("Next arrival time for stop: " + closestTime.ToString("HH:mm:ss"));
        Console.WriteLine("Found in route: " + found_in_route);
        Console.WriteLine("Stop is: " + found_in_stop);
        return (closestTime.ToString("HH:mm:ss"), found_in_route);

    }

    public static long GetTimeUntilNextArrival(DateTime current_time, string next_arrival_time)
    {
        DateTimeOffset current_unix_epoch = new DateTimeOffset(current_time, TimeSpan.Zero);
        DateTimeOffset arrival_unix_epoch = new DateTimeOffset(DateTime.ParseExact(next_arrival_time, "HH:mm:ss", null), TimeSpan.Zero);
        var Time_span = current_unix_epoch.ToUnixTimeSeconds() - arrival_unix_epoch.ToUnixTimeSeconds();
        return Time_span;
    }

}
