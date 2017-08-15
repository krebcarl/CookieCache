# Cookie Cache Design Document

## Design Overview
Cookie Cache is a multi-service web application. This application consists of a Stateless Web Service and a Stateful Service that serves as the backend of the application. The basic architecture can be used for a variety of different simple web applications.

![alt text](https://github.com/krebcarl/CookieCache/blob/carly1/Design%20Flow%20Diagram.JPG "Log Title Text 2")

## Front End
HTML, JavaScript, and CSS are used for the front end in this application. 

#### HTML
The HTML serves as a way for the user to input information (i.e. type of cookies they are looking to order, quantity of cookie, order information, etc.). The core HTML files used in this project are: 

- HomePage.html - This is the home page for the Cookie Cache web application. This page doesn't have much functionality, but it allows users to navigate to other parts of the web application. One important thing to note is that there is a GUID in blue on the top of this page and others that you navigate to. This is the userID that identifies the user and is used to identify the correct dictionary that hold the user's cart contents. The userID is a associated with a cookie that is set when the page is loaded.

- CookieCache.html - This page serves as the ordering page. This page has information about each cookie that is for sale online, it has forms where users can add product to the cart by simply selecting a flavor, picking a quantity, and finally press a button to add it to the cart. The user will then see a pop-up that confirms that the product was added to the cart. 

- CartPage.html - This page is the cart page. This is the page where all the users cart contents are displayed (i.e. the products that the user adds to their cart). This page also allows a user to place the order. Currently the contents are just removed from inventory and the user is reset. 

***SF Challenge:*** Add on functionality that has the user input their shipping and billing information and then sends an email to the customer and the manufacturer informing them of the order. Framework for the HTML pages needed are included in the Web1/wwwroot folder (OrderConfirmationPage.html and CheckoutPAge.html)

#### JavaScript
The JavaScript serves as a means to get information out of the HTML and then talk with the Stateless Web Service/Backend. There are two main JavaScript files that each serve a different purpose.

- site.js - This is the main JavaScript file that is in charge of getting all the information needed out of the HTML. This file initially identifies the user by always making sure there is a cookie set for each user's ID. The file sends HTTP requests to talk to the Stateless Web Service in the backend. See section below on HTTP Requests for more information. 

- test.js - This file contains tests for the application to ensure that things are working correctly when changes are being made to the code. To run tests, just navigate to *http://localhost:8742/test.html* and click the "Let's test it!" button. 

## Back End
C# and Service Fabric Reliable Collections are used for the backend of this application. The two main files that serve as the backend in this application are ValuesController.cs and Stateful1.cs.

#### Stateless Web Service
Cookie Cache has one Stateless Web Service called ValuesController.cs. This file contains the code that is used to talk to the Stateful Service through Service Proxies (see below for more information on Service Proxy). 

Some key design features to note in this file are that in the AddToCart() method there is a "Stateless Service hit" message that is sent everytime the method is called. This is to help see where the information is being passed and how the flow works for the project.

#### Stateful Service
Cookie cache has one Stateful Service called Stateful1.cs. This file contains the majority of the functionality for the application including maintaining the data for each user's cart, managing inventory, and managing all of the active users. 

##### Reliable Dictionary
To store all the information needed, Reliable Dictionaries are used. There are three main reliable dictionaries used in this application:

- inventoryDictionary - this dictionary keeps track of the product inventory which is initialized in the backend once the application is started. The key for this dictionary is the flavor of the cookie and the value is the quantity in inventory. (i.e. KEY: Chocolate Java Chip VALUE: 5000)

- userDictionary - this dictionary keeps track of all the *active* users who have a shopping cart. A user is assigned a web cookie when they navigate to the website initially (this can be seen on the top of any of the main pages in a light blue/teal color) -- this cookie ID is not saved until the user adds something to their shopping cart. It is only then when they are added to the active userDictionary and have a shopping cart created and initialized for them. The key for this dictionary is the GUID cookie and the value is a random string that serves as a pointer to the user's shopping cart dictionary and a time stamp that is used to clean up shoppping carts when the orders are not placed. (i.e. KEY: 7513752d-e7fd-4a44-8c9a-80364e98b2e9 VALUE: 0f8fad5b-d9cb-469f-a165-70867728950e|5/1/2008 8:30:52 AM)

- priceDictionary - this dictioanry is 


## Communication Between Services
Since there are multiple different programming languages that are used in this web application, there are certain protocols used to talk between the different parts (these are the lines that are drawn and identified in the diagram above).

#### HTTP Requests
An HTTP Request is used to talk between JavaScript and the different services in an application. In this application, HTTP requests are used in the site.js file to talk to and call methods in the Stateless Web Service ValuesController.c file. Below is a general implementation of an HTTP request that is used in the site.js file:

```javascript
   var http = new XMLHttpRequest();
    http.onreadystatechange = function(){
        if (http.readyState === 4) {
           //write your logic/code here 
        }
    };

    http.open("GET", "http://localhost:8742/api/values/AddToCart/?flavor=" + flavor + "&quantity=" + quantity + "&userID=" + userID, true); // true for asynchronous 
    http.send(null);
```
For more information about using HTTP requests check out https://stackoverflow.com/questions/247483/http-get-request-in-javascript 

#### Service Proxy
Service Proxies are used to talk between services. In this application, a service proxy is required whenever the Stateless Web Service needs information or wants to call a method that lives in the Stateful Service. Below is a code snip-it that shows the most general implementation of a Service Proxy in a method in Web1/Controllers/ValuesController.cs:

```javascript
IMyService inventoryService = ServiceProxy.Create<IMyService>(new Uri("fabric:/Application8/Stateful1"), new ServicePartitionKey(0));
            try
            {
                inventoryString = await inventoryService.GetInventoryString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return e.ToString();
            }
```
For more information about using Service Proxies check out https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicefabric.services.remoting.client.serviceproxy?view=azure-dotnet .

## Scenario: Adding product to the Cart 
Below is a diagram showing the flow when a user presses the "Add To Cart" button on the CookieCache.html page to add something to their cart.
![alt text](https://github.com/krebcarl/CookieCache/blob/carly1/Add%20to%20Cart%20Design%20Flow%20Diagram.JPG "Log Title Text 2")

## Misc. Design Details
#### Stateless Service Hit
#### Inventory Display
#### Application Manifest/ Manifest Files
