This is a client based in Flask. It can be run in a Docker container.

TO USE:
To run from the command line, in the app directory, set the following env variables:

 $env:FLASK_ENV="development"
 $env:FLASK_APP="app"      
 $env:FLASK_DEBUG="true"     
 
 Then the application can be run with:
 py -m flask run -h localhost -p 8081
 
 This will open a page on localhost:8080 that will host the client side of the application.
