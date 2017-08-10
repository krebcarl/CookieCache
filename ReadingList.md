
http://www.tutorialspoint.com/http/http_requests.htm# Recommended Reading List for CookieCache

## Azure
Microsoft Azure is a growing collection of integrated cloud services that developers and IT professionals use to build, deploy, and manage applications through our global network of datacenters. With Azure, you get the freedom to build and deploy wherever you want, using the tools, applications, and frameworks of your choice.
https://azure.microsoft.com/en-us/overview/what-is-azure/

## Service Fabric
To get a basic overview of what Service Fabric is, this link provides a pretty good basis on general topics that will help you understand what Service Fabric is.
https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-overview

The link below is for a document that explains some basic service fabric terminology that would be helpful to have an understanding of as these terms are used commonly throughout any other Service Fabric documentation. 
https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-technical-overview
### Reliable Services
This is a good link that talks about both Stateful and Stateless Services. Cookie Cache consists of two services -- one a stateless web service and the other a stateful service.
https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-introduction

## Microservices
Service Fabric is a platform to build microservice applications on, thus it is nice to have a little knowledge on what microservices are.
https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-overview-microservices

## HTTP requests
Http requests are used for talking between the JavaScript layer and the Stateless Web Service.
http://www.tutorialspoint.com/http/http_requests.htm

## Service Proxy
Service Proxys are used for talking between the Stateless Web Service and the Stateful backend that stores the information and does most of the computation.
https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicefabric.services.remoting.client.serviceproxy?view=azure-dotnet

## Reliable Collections
Reliable Collections are Service Fabric's reliable data structures used to store information. For this sample application Reliable Dictionary is the only Reliable collection that is used, but the others are equally useful.
https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-reliable-collections

https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-work-with-reliable-collections

## Reliable Dictionary
Reliable Dictionaries are used to store all the information needed in this sample application. 
https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicefabric.data.collections.ireliabledictionary-2?redirectedfrom=MSDN&view=azure-dotnet#microsoft_servicefabric_data_collections_ireliabledictionary_2
