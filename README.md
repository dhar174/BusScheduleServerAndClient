# BusScheduleServerAndClient
This is a sample/demo application that retrieves theoretical arrival times for theoretical bus stops.

NOTE: Please make sure to use the latest version. Critical updates were made in the afternoon of May 2nd, 2022.
With minor changes, this could be compiled to run on any hardware. For this demo version, use Windows 10/11.

TO USE (demo version):

1. Clone repository
2. (On Windows) Navigate to {root folder}\BusSchedServerCore\bin\Debug\net6.0\publish\ snf run BusSchedServerCore.exe
3. Next, run the client using the following Powershell commands in the Flask Client folder:
          1. $env:FLASK_ENV="development"
          2. $env:FLASK_APP="app"
          3.  $env:FLASK_DEBUG="true"    
          4.  py -m flask run -h localhost -p 8081
4. Navigate to http://localhost:8081/

5. Note: Use "Refresh" and "Back" to update info. 

6. Note: Data can also be gathered with simple Get commands WITHOUT the Flask client app. Simply send a GET request to the server at /Test/GetNextArrivalTime/stop/{stop number} for the next arrival at the indicated stop, or /Test/GetNextArrivalTime/route/{route number} for the next stop on the route.

Example: https://localhost:8080/Test/GetNextArrivalTime/stop/6



Known Issues and Improvements:
This application is basic and would benefit from multiple improvements that could be made were this actually headed to production and not a demo. 
For instance: automatic refresh (an easy addition), aesthetic improvements, more data, realtime telemetry data, etc. as well as more ways to compile and ditribute the client other than running only as a basic web application. 

NOTE: Route numbers start at zero. This needs to be fixed, but just note it for now.
