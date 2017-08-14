## Cookixce Cache Design Document

## Design Overview
Cookie Cache is a multi-service web application. This application consists of a Stateless Web Service and a Stateful Service that serves as the backend of the application. The basic architecture can be used for a variety of different simple web applications.

![alt text](https://github.com/krebcarl/CookieCache/blob/carly1/Design%20Flow%20Diagram.JPG "Log Title Text 2")

## Front End
HTML, JavaScript, and CSS are used for the front end in this application. 

#### HTML
The HTML serves as a way for the user to input information (i.e. type of cookies they are looking to order, quantity of cookie, order information, etc.). The core HTML files used in this project are: 

- HomePage.html - This is the home page for the Cookie Cache web application. This page doesn't have much functionality, but it allows users to navigate to other parts of the web application. One important thing to note is that there is a GUIID in blue on the top of this page and others that you navigate to. This is the userID that identifies the user and is used to identify the correct dictionary that hold the user's cart contents. The userID is a associated with a cookie that is set when the page is loaded.

- CookieCache.html - This page serves as the ordering page. This page has information about each cookie that is for sale online, it has forms where users can add product to the cart by simply selecting a flavor, picking a quantity, and finally press a button to add it to the cart. The user will then see a pop-up that confirms that the product was added to the cart. 

- CartPage.html - This page is the cart page. This is the page where all the users cart contents are displayed (i.e. the products that the user adds to their cart). This page also allows a user to place the order. Currently the contents are just removed from inventory and the user is reset. 

**SF Challenge:** Add on functionality that has the user input their shipping and billing information and then sends an email to the customer and the manufacturer informing them of the order. Framework for the HTML pages needed are included in the Web1/wwwroot folder (OrderConfirmationPage.html and CheckoutPAge.html)

#### JavaScript
The JavaScript serves as a means to get information out of the HTML and then talk with the Stateless Web Service/Backend. There are two main JavaScript files that each serve a different purpose.

- site.js - This is the main JavaScript file that is in charge of getting all the information needed out of the HTML. The file sends HTTP requests

## Back End
C# and Service Fabric Reliable Collections are used for the backend of this application. 

## Communication Between Services
Since there is new
#### HTTP Requests

#### Service Proxy
Service Proxies are used to talk between services. In this application, a service proxy is required whenever the Stateless Web Service needs information or wants to call a method that lives in the Stateful Service. Below is a code snip-it that shows th
'''javascript
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
'''

## Scenario: Adding product to the Cart 
Below is a diagram showing the flow when a user presses the "Add To Cart" button on the CookieCache.html page to add something to their cart.
![alt text](https://github.com/krebcarl/CookieCache/blob/carly1/Add%20to%20Cart%20Design%20Flow%20Diagram.JPG "Log Title Text 2")

## Misc. Design Details
