using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Data.Collections;
using Newtonsoft.Json;
using Microsoft.ServiceFabric.Data;

namespace Web1.Controllers
{

    public class ValuesController : Controller
    {
        string flavor;
        int quantity;

        /// <summary>
        /// This method is used to call the Stateful1.cs addToDic method which adds items to the user's cart.
        /// </summary>
        /// <param name="flavor">Flavor of the specified cookie the user wants to add to the cart</param>
        /// <param name="quantity">The quantity of cookies the user wants to add to the cart</param>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>"This user hasn't created a cart yet. Please try to add something to the cart." if there is no dictionary for the given user</returns>
        
        [Route("api/[controller]/AddToCart")]
        [HttpGet]
        public async Task<string> AddToCart(string flavor, int quantity, string userID)
        {
            this.flavor = flavor;
            this.quantity = quantity;
            string proxyResponse = "Not changed yet";

           IMyService flavService = ServiceProxy.Create<IMyService>(new Uri("fabric:/Application8/Stateful1"), new ServicePartitionKey(0));
            try
            {
                proxyResponse = await flavService.addToDict(flavor, quantity, userID);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return e.ToString();
            }
            string updateFlavNQuan = "Stateless Service hit | "  + proxyResponse; 
            return updateFlavNQuan;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method takes what is currently in inventory and returns it as a string that is later parsed to display on the HTML
        /// </summary>
        /// <returns>Returns what is in inventory as a string(Returns the items in inventory and their quantities as a string in the following 
        /// format: MM Madness: 5000 | Chocolate Java Chip: 5000 | LRU BLU-berry: 5000 | Peanut Butter Jam: 5000)</returns>
        [Route("api/[controller]/inventoryToString")]
        public async Task<string> inventoryToString()
        {
            string inventoryString;
            IMyService inventoryService = ServiceProxy.Create<IMyService>(new Uri("fabric:/Application8/Stateful1"), new ServicePartitionKey(0));
            try
            {
                inventoryString = await inventoryService.getInventoryString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return e.ToString();
            }
            return inventoryString;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// This method "places" the user's order by calling the deleteUser Stateful1.cs function which deletes the user from the user dictionary and deletes the user's cart dictionary.
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>Returns either the user's cart items that were deleted in a string format or "" if the cart is empty/the user has already been deleted (see getCartString for other possible messages)</returns>
        [Route("api/[controller]/placeOrder")]
        public async Task<string> placeOrder(string userID)
        {
            string confirmString;
            IMyService placeOrderService = ServiceProxy.Create<IMyService>(new Uri("fabric:/Application8/Stateful1"), new ServicePartitionKey(0));
            try
            {
                confirmString = await placeOrderService.deleteUser(userID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return e.ToString();
            }
            if(confirmString == "User hasn't been initialized in the user dictionary -- user must go to the cookie cache page/add something to the cart")
            {
                confirmString = "";
            }else if(confirmString == "no dictionary" || confirmString == "Your Cart is empty! (No user set)" || confirmString == "Cart is empty" || confirmString == "Your cart list is empty")
            {
                confirmString = "";
            }
            return confirmString;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///This methods gets the orderDictionary as a string where the data is separated by a comma. The first thing in the string is the number of flavors ordered.
        ///This methods also gets the priceDictionary that holds all the prices for the different kinds of cookies. This data is again separated by a comma. The two strings are separated
        ///by a | and the orderDictionary is the first part of the string and the priceDictionary is the second part (i.e. orderDictionary string|priceDIsctionary string)
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>A string where all the contents are separated by commas. The first part of the string contains what is in the user's cart. The second part is the price information
        /// that is used to calculate the total prices of all the items(i.e. orderDictionary string|priceDIsctionary string)</returns>
        [Route("api/[controller]/updateCart")]
        public async Task<string> updateCart(string userID)
        {
            string returnString;
            IMyService cartService = ServiceProxy.Create<IMyService>(new Uri("fabric:/Application8/Stateful1"), new ServicePartitionKey(0));
            try
            {
               returnString = await cartService.getCartString(userID);
                returnString = returnString + "|" + await cartService.getPriceString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return e.ToString();
            }
            return returnString; 
        }

        //----------------------------------------------------------------------Template Methods Below-------------------------------------------------------------------------------------------
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public int Get(int id)
        {
            return id;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
