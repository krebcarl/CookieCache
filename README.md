
---
services: service-fabric
platforms: dotnet
author: cakreb
---

# Service Fabric Retail Sample Application - Cookie Cache

This repository contains an introductory sample project for [Microsoft Azure Service Fabric](https://azure.microsoft.com/services/service-fabric/) highlighting the use of Reliable Collections. The sample project contains a single application with two services (one stateful and one stateless) demonstrating the basic concepts needed to get you started building highly-available, scalable, distributed applications.

## About this sample application
Cookie Cache is a sample retail application developed using Azure Service Fabric that demonstrates the features Service Fabric offers and showcases their uses. The sample retail application will allow customers to place and purchase orders to demonstrate the ease and benefits of using Service Fabric to create a web application. Cookie Cache will be an example of how ecommerce web applications can look when built on Service Fabric. Since it is easier to build out a retail application that has a specific purpose, this sample will be for an arbitrary company that will sell cookies. The company name is “Cookie Cache”. To learn more about the design choices and implementation it is encouraged you read the DesignDoc.md file.
### Web Service
This is a stateless front-end web service using [ASP.NET Core in a Reliable Service](https://docs.microsoft.com/azure/service-fabric/service-fabric-reliable-services-communication-aspnetcore). This service demonstrates a basic front-end service that acts as a gateway for users into your application. It presents a multi-page application UI and an HTTP API to interact with the rest of the application. This is the only service that exposes an endpoint to the Internet for users to interact with, and all user ingress to the application comes through this service.
#### Key concepts
 - Stateless public-facing ASP.NET Core service using WebListener
 - Communicating with other services in a variety of ways:
 - Over HTTP to another service using the Service Fabric Reverse Proxy
 
### Stateful Backend Service
This is a stateful back-end service using ASP.NET Core. This service demonstrates the use of [Reliable Collections](https://docs.microsoft.com/azure/service-fabric/service-fabric-reliable-services-reliable-collections) in a stateful service. It presents an internal-only HTTP API for other services to interact with.

#### Key concepts
 - Stateful internal-only ASP.NET Core service using Kestrel
 - Reliable Collections
 - Dependency injection of IReliableStateManager for use in MVC controllers

## Building and deploying

[Set up your development environment](https://docs.microsoft.com/azure/service-fabric/service-fabric-get-started) with [Visual Studio 2017](https://www.visualstudio.com/vs/). Make sure you have at least version **15.1** of Visual Studio 2017 installed.

This sample application can be built and deployed immediately using Visual Studio 2017. When opening Visual Studio, right click on the application image to "Run as Administrator". When Visual Studio is open, locate and click on File -> Open -> Project/Solution.. (Ctrl + Shift + O) and locate the sample application folder. To open the project, double click on the file with the .sln ending. The application is ready to be ran locally by clicking on the Start button with the green arrow at the top of the page.

To deploy on the local cluster, you can simply hit F5 to debug the sample. If you'd like to try publishing the sample to an Azure cluster:
1. Right-click on the application project in Solution Explorer and choose Publish.
2. Sign-in to the Microsoft account associated with your Azure subscription.
3. Choose the cluster you'd like to deploy to.

## Other useful resources
 
 - Reading List
 - DesignDoc.md 

---
*This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.*
