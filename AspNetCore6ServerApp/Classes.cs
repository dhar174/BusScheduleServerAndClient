namespace Classes;
using System.Collections;
using System.Linq;
using System.Data;
using System.Collections.Generic;

public class Routes
{
    public bool first_o_day = true;
    public int? id { get; set; }
    public int? next_stop_id { get; set; }

    public LinkedListNode<Stops> first_stop { get; set; }

    public LinkedListNode<Stops> current_stop { get; set; }

    public LinkedListNode<Stops> next_stop { get; set; }
    public HashSet<Tuple<int, String>> stops_times_dict { get; set; }

    public LinkedList<Stops> stops_list_linked { get; set; } = new LinkedList<Stops>();

    public Queue stop_queue = new Queue();
    public String? next_Arrival_time;

    public LinkedListNode<Stops> GetNextStop()
    {

        if (first_o_day == true)
        {
            first_o_day = false;
            first_stop = this.current_stop;
            stops_list_linked.RemoveFirst();
            stops_list_linked.AddLast(this.current_stop);
            return first_stop;
        }
        else
        {
            next_stop = this.current_stop;
            stops_list_linked.RemoveFirst();
            stops_list_linked.AddLast(this.current_stop);

            return next_stop;
        }


    }

    public LinkedList<Stops> GetStops(int num_stops)
    {
        for (int i = 0; i < num_stops; i++)
        {
            Stops stop = Program.static_stops_list[i];
            this.stops_list_linked.AddLast(stop);
        }
        this.first_stop = this.stops_list_linked.First;
        return this.stops_list_linked;
    }

    public void populate_Arrival_times_by_route(int id)
    {
        var offset = new TimeSpan(0, (2 * (id - 1)), 0);

        var timeSpan = new TimeSpan(24, 0, 0);
        var increment = new TimeSpan(0, 15, 0);
        var totalIncrements = timeSpan.Ticks / increment.Ticks;
        var startDate = DateTime.Now.Date;
        startDate = DateTime.Parse(startDate.ToString("HH:mm:ss"));
        startDate = startDate.Add(offset);
        Console.WriteLine("startDate for route" + id + ": " + startDate.ToString("HH:mm:ss"));
        var endDate = DateTime.Now.Date;
        endDate = endDate.Add(timeSpan + offset);
        Console.WriteLine("endDate for route" + id + ": " + endDate.ToString("HH:mm:ss"));

        var nextArrival = startDate.ToString("HH:mm:ss");

        for (int i = 0; i < totalIncrements; i++)
        {
            if (i == 0)
            {
                this.stop_queue.Enqueue(nextArrival);
            }
            else
            {
                nextArrival = DateTime.Parse(nextArrival).Add(increment).ToString("HH:mm:ss");
                if (DateTime.Parse(nextArrival) > endDate || DateTime.Parse(nextArrival) < startDate)
                {
                    continue;
                }
                if (DateTime.Parse(nextArrival) > startDate)
                {

                    this.stop_queue.Enqueue(nextArrival);
                }
            }

        }

        var dates = Enumerable.Range(0, this.stop_queue.Count).Select(i => (String)this.stop_queue.Peek()).ToList();
        this.Arrival_times_route = new LinkedList<String>(dates as IEnumerable<String>);
        var now_timestamp = DateTime.Parse(DateTime.Now.ToString("HH:mm:ss"));

        this.next_Arrival_time = ScheduleRetrievalHelper.GetNextArrivalOnRoute(this, now_timestamp);

        this.stops_times_dict = new HashSet<Tuple<int, String>>();
        foreach (var stop in this.stops_list_linked)
        {
            this.stops_times_dict.Add(new Tuple<int, String>(stop.id, this.next_Arrival_time));
        }
    }

    public LinkedList<String>? Arrival_times_route { get; set; } = new LinkedList<String>();

    public Routes(int id)
    {

        this.stops_list_linked = this.GetStops(Program.static_stops_list.Count);
        this.first_stop = new LinkedListNode<Stops>(this.stops_list_linked.ToArray()[0]);
        this.populate_Arrival_times_by_route(id);

        this.current_stop = this.first_stop;

        this.next_stop = this.GetNextStop();

        this.next_stop_id = this.first_stop.Value.id;

        this.id = id;

    }
}

public class Stops
{
    public int id { get; set; }
    public int order { get; set; }
    public int? route_id { get; set; }


    public Queue stop_queue = new Queue();
    public String? next_Arrival_time;

    public LinkedList<String>? Arrival_times_stop { get; set; } = new LinkedList<String>();

    public LinkedListNode<Stops> first_stop { get; set; }

    public LinkedListNode<Stops> current_stop { get; set; }

    public LinkedListNode<Stops> next_stop { get; set; }
    public HashSet<Tuple<int, String>> stops_times_dict { get; set; }

    public LinkedList<Stops> stops_list_linked { get; set; } = new LinkedList<Stops>();
    public int? next_stop_id { get; set; }

    public bool first_o_day = true;

    public Stops(int id, int order)
    {
        this.populate_Arrival_times_for_stop(id);

        this.stops_list_linked = this.GetStops(10);
        this.first_stop = new LinkedListNode<Stops>(this.stops_list_linked.ToArray()[0]);

        this.current_stop = this.first_stop;

        this.next_stop = this.GetNextStop();

        this.next_stop_id = this.first_stop.Value.id;

        this.next_Arrival_time = this.Arrival_times_stop.First.Value;
        this.id = id;
        this.order = order;
    }
    public LinkedListNode<Stops> GetNextStop()
    {





        if (first_o_day == true)
        {
            first_o_day = false;
            first_stop = this.current_stop;
            stops_list_linked.RemoveFirst();
            stops_list_linked.AddLast(this.current_stop);
            return first_stop;
        }
        else
        {
            next_stop = this.current_stop;
            stops_list_linked.RemoveFirst();
            stops_list_linked.AddLast(this.current_stop);

            return next_stop;
        }


    }

    public LinkedList<Stops> GetStops(int num_stops)
    {
        for (int i = 0; i < num_stops; i++)
        {
            this.stops_list_linked.AddLast(this);
        }
        this.first_stop = this.stops_list_linked.First;
        return this.stops_list_linked;
    }

    public void populate_Arrival_times_for_stop(int id)
    {
        var offset = new TimeSpan(0, (2 * (id - 1)), 0);
        var found_in_stop = 0;
        var timeSpan = new TimeSpan(24, 0, 0);
        var increment = new TimeSpan(0, 15, 0);
        var totalIncrements = timeSpan.Ticks / increment.Ticks;
        var startDate = DateTime.Now.Date;
        startDate = DateTime.Parse(startDate.ToString("HH:mm:ss"));
        startDate = startDate.Add(offset);
        Console.WriteLine("startDate for route" + id + ": " + startDate.ToString("HH:mm:ss"));
        var endDate = DateTime.Now.Date;
        endDate = endDate.Add(timeSpan + offset);
        Console.WriteLine("endDate for route" + id + ": " + endDate.ToString("HH:mm:ss"));
        Console.WriteLine();

        var nextArrival = startDate.ToString("HH:mm:ss");

        for (int i = 0; i < totalIncrements; i++)
        {
            if (i == 0)
            {
                this.stop_queue.Enqueue(nextArrival);
            }
            else
            {
                nextArrival = DateTime.Parse(nextArrival).Add(increment).ToString("HH:mm:ss");
                if (DateTime.Parse(nextArrival) > endDate || DateTime.Parse(nextArrival) < startDate)
                {
                    continue;
                }
                if (DateTime.Parse(nextArrival) > startDate)
                {

                    this.stop_queue.Enqueue(nextArrival);
                }
            }


            this.next_Arrival_time = nextArrival;
        }


        var dates = Enumerable.Range(0, this.stop_queue.Count).Select(i => (String)this.stop_queue.Peek()).ToList();
        this.Arrival_times_stop = new LinkedList<String>(dates as IEnumerable<String>);
        this.stops_times_dict = new HashSet<Tuple<int, String>>();
        foreach (var stop in this.Arrival_times_stop)
        {
            this.stops_times_dict.Add(new Tuple<int, String>(this.id, this.next_Arrival_time));
        }
        var now_timestamp = DateTime.Parse(DateTime.Now.ToString("HH:mm:ss"));

        if (Program.routes.Count != 0 && Program.static_stops_list.Count != 0)
        {
            (this.next_Arrival_time, found_in_stop) = ScheduleRetrievalHelper.GetNextArrivalByStop(Program.static_stops_list[id], now_timestamp);
        }



    }

}