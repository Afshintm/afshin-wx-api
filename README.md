# README

### What is this repository for?

This is a .net core 3.1 webapi to submit the WooliesX Technical test excercise

### Api endpoint to run the tests

http://atwxapi-1471568992.ap-southeast-2.elb.amazonaws.com/api

### Project structure

1- Web Api

3- Integration tests

### How to build and run

You will have to install .net core 3.1 runtime which can be downloaded from

    https://dotnet.microsoft.com/download/dotnet-core/3.1 to run the project.

## To build the project you need to to the following:

1- clone the repo to your local environment and cd to repository directory

2- dotnet build ./afshin-wx-api.sln

3- dotnet run --project ./src/At.Wx.Api/At.Wx.Api.csproj

the api will run in development environment on https://localhost:5091

Alternatively, open afshin-wx-api.sln using visual studio 2019 or Intelij Rider then Build and run it.

### To run Integration tests:

dotnet test ./src/At.Wx.Api.Integration.Tests

### Api Endpoints:

In Summary:

### Get Name/token status

`https://localhost:5091/api/user`

### Get Sorted products using sortOption:

If you donot pass any sort options Low will be the default

"Low" - Low to High Price

"High" - High to Low Price

"Ascending" - A - Z sort on the Name

"Descending" - Z - A sort on the Name

"Recommended" Will return the items based on popularity

`https://localhost:5091/api/sort?sortOption=Low`

### Get the minumum total cost of the trolley

Post `https://localhost:5091/api/trolleyTotal`

### See it in action: The original api link is deployed to AWS Ecs

http://atwxapi-1471568992.ap-southeast-2.elb.amazonaws.com/api

### Swagger and sort endpoints

http://atwxapi-1471568992.ap-southeast-2.elb.amazonaws.com/swagger/index.html

http://atwxapi-1471568992.ap-southeast-2.elb.amazonaws.com/api/sort
