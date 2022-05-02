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

    public static (String, int) GetNextArrivalByStop(Stops stop, DateTime current_time)
    {
        var current_time_string = current_time.ToString("HH:mm:ss");

        int found_in_route = 0;

        next_arrival_time = (String)stop.stop_queue.Peek();
        var Time_span = GetTimeUntilNextArrival(current_time, next_arrival_time);
        Console.WriteLine("Time_span in routes: " + Time_span);
        while (Time_span > 0)
        {
            next_arrival_time = (String)stop.stop_queue.Dequeue();
            Time_span = GetTimeUntilNextArrival(current_time, next_arrival_time);

            if (Time_span < (15 * 60) && DateTime.Compare(DateTime.ParseExact(next_arrival_time, "HH:mm:ss", null), current_time) > 0)
            {

                return (next_arrival_time, found_in_route);
            }
            continue;
        }
        return (next_arrival_time, found_in_route);

    }

    public static long GetTimeUntilNextArrival(DateTime current_time, string next_arrival_time)
    {
        DateTimeOffset current_unix_epoch = new DateTimeOffset(current_time, TimeSpan.Zero);
        DateTimeOffset arrival_unix_epoch = new DateTimeOffset(DateTime.ParseExact(next_arrival_time, "HH:mm:ss", null), TimeSpan.Zero);
        var Time_span = current_unix_epoch.ToUnixTimeSeconds() - arrival_unix_epoch.ToUnixTimeSeconds();
        return Time_span;
    }

}
