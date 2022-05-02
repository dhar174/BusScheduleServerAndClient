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

        for (int i = 0; i < Program.routes.Count; i++)
        {


            for (int j = 0; j < Program.routes[i].stops_times_dict.Count; j++)
            {
                Console.WriteLine("count " + Program.routes[i].stops_times_dict.Count);
                Console.WriteLine("j " + j);
                Console.WriteLine("Route " + i + " next arrival time " + (String)Program.routes[i].next_Arrival_time);
                Console.WriteLine("checking match on route " + i + " " + Program.routes[i].stops_times_dict.Where(x => x.Item1 == stop_id).FirstOrDefault().Item2 + " " + (String)Program.routes[i].next_Arrival_time);

                if (Program.routes[i].stops_times_dict.Where(x => x.Item1 == stop_id).FirstOrDefault().Item2.Equals((String)Program.routes[i].next_Arrival_time))
                {
                    Console.WriteLine("time of match? " + Program.routes[i].stops_times_dict.Where(x => x.Item1 == stop_id).FirstOrDefault().Item2);

                    Console.WriteLine("stop id of match " + Program.routes[i].stops_times_dict.Where(x => x.Item1 == stop_id).FirstOrDefault().Item1);
                    Console.WriteLine("time of match " + Program.routes[i].stops_times_dict.Where(x => x.Item1 == stop_id).First().Item2);


                    Console.WriteLine("Found stop " + Program.routes[i].next_stop_id);

                    next_arrival_time = GetNextArrivalOnRoute(Program.routes[i], current_time);
                    times.Add(DateTime.ParseExact(next_arrival_time, "HH:mm:ss", null));
                    routesu.Add(Program.routes[i]);
                    break;
                }


            }

        }
        for (int i = 0; i < times.Count; i++)
        {

            Console.Write("time in set: " + (i + 1) + "  " + times[i].ToString("HH:mm:ss"));
        }


        var closestTime = times.OrderBy(t => Math.Abs((t - current_time).Ticks))
                             .First();

        Console.WriteLine("Next arrival time for stop: " + closestTime.ToString("HH:mm:ss"));
        Console.WriteLine("Found in route: " + routesu[times.IndexOf(closestTime)].id);
        var found_in_route = (int)routesu[times.IndexOf(closestTime)].id;
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
