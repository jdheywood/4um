# README #

* This is an example forum application, consisting of .NET API, MongoDb doc store and JavaScript framework UI
* 0.1 work in progress

## How do I get set up? ##

* Clone this repo
* Download MongoDb, for reviewing, running and developing this API you need windows so select the appropriate Windows version 
[https://www.mongodb.org/downloads](https://www.mongodb.org/downloads)
* Install MongoDb, installing as a service is optional but saves you starting the service when you boot up 
[Install MongoDb as Windows Service](http://docs.mongodb.org/manual/tutorial/install-mongodb-on-windows/#run-the-mongodb-service)
* Open Forum.sln in Visual Studio (2012 or above)
* Set Forum.Api project as start up
* hit F5 to see API running
* Set up the Api in IIS under the url localhost/forum-api (or whatever you want to configure it as)
* Set Forum.Web project as start up
* hit f5 to see the UI, this should interact with your local toast API, check your config if it's not playing nicely


### TODO: ###
* Review js frameworks for integration with API
* Build SPA
* Integrate API and SPA


### Contribution guidelines ###
* TDD via NUnit, NSubstitute and AutoFixture please
* Issue PRs to me please if you want to contribute


### Who do I talk to? ###
* Repo owner: jdheywood@gmail.com

