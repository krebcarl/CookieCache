# CookieCache
Service Fabric Retail Example Sample Application
This repository contains an introductory sample project for [Microsoft Azure Service Fabric](https://azure.microsoft.com/services/service-fabric/) highlighting the use of Reliable Collections. The sample project contains a single application with two services (one stateful and one stateless) demonstrating the basic concepts needed to get you started building highly-available, scalable, distributed applications.

## Building and deploying

1. [Set up your development environment](https://docs.microsoft.com/azure/service-fabric/service-fabric-get-started) with [Visual Studio 2017](https://www.visualstudio.com/vs/). Make sure you have at least version **15.1** of Visual Studio 2017 installed.

This sample application can be built and deployed immediately using Visual Studio 2017. When opening Visual Studio, right click on the application image to "Run as Administrator".
To deploy on the local cluster, you can simply hit F5 to debug the sample. If you'd like to try publishing the sample to an Azure cluster:

1. Right-click on the application project in Solution Explorer and choose Publish.
2. Sign-in to the Microsoft account associated with your Azure subscription.
3. Choose the cluster you'd like to deploy to.

## Files in the Project
After you have opened the project in Visual Studio, on the 

#### Key concepts
-Stateless Web Applications
-Stateful Service serving as the backend of the application
-Reliable Collections -- specifically Reliable Dictionary
