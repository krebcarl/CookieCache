using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Data;
using System.Runtime.Remoting.Contexts;

namespace Stateful1
{
    
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Stateful1 : StatefulService, IMyService
    {
        private IReliableDictionary<string, int> orderDictionary;
        private IReliableDictionary<string, int> inventoryDictionary;
        private IReliableDictionary<string, string> userDictionary;
        private CancellationToken token;
        private IReliableDictionary<string, double> priceDictionary;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method adds a specified cookie and a specified quantity of that cookie to the user's shopping cart (cart dictionary). This method is called by the ValuesController.cs file.
        /// </summary>
        /// <param name="flavor">Flavor of the specified cookie the user wants to add to the cart</param>
        /// <param name="quantity">The quantity of cookies the user wants to add to the cart</param>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>Returns a message that indicated the success or failure of adding something to their cart. </returns>
        public async Task<string> AddToDict(string flavor, int quantity, string userID)
        {
            //gets or creates a user dictionary based on the userID
            string userOrderDictionaryPointerName = await GetOrCreateUserDictionaryName(userID);
            
            IReliableDictionary<string, int> userOrderDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, int>>(userOrderDictionaryPointerName);
            
            using(var txn = this.StateManager.CreateTransaction())
            {
                //check the quantity against the inventory logic below. Remove that quantity of inventory then add it to the cart
                int inventoryLeft = await SubtractInventory(flavor, quantity);

                //logic that checks to see whether the item can be added to the cart or not based on the results of subtract inventory
                if (inventoryLeft > -1)
                {
                    await userOrderDictionary.AddOrUpdateAsync(txn, flavor, quantity, (key, oldValue) => oldValue + quantity);
                    string returnMessage = "Thank you! " + quantity + " " + flavor + " cookies have been added to your cart!";
                    await txn.CommitAsync();
                    return returnMessage;
                }
                else
                {
                    string returnMessage = "Sorry, Cookie Cache currently doesn't have the inventory to supply your order -- please try back later!";
                    ConditionalValue<int> cookiesLeft = await inventoryDictionary.TryGetValueAsync(txn, flavor);
                    returnMessage = returnMessage + " There are currently " + cookiesLeft.Value + " " + flavor + " cookies left in inventory.";
                    await txn.CommitAsync();
                    return returnMessage;
                }   
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// This method is used to delete the user from the active user dictionary and then uses the helper method deleteUserCart to delete the user's cart.
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>"This user hasn't created a cart yet. Please try to add something to the cart." if there is no dictionary for the given user
        /// </returns>
        public async Task<string> DeleteUser(string userID)
        {
            string returnResult = await GetCartString(userID);
            string userCartResult = await DeleteUserCart(userID);

            if(userCartResult == "no dictionary")
            {
                return "This user hasn't created a cart yet. Please try to add something to the cart.";
            }
            using (var txn = this.StateManager.CreateTransaction())
            {
                await userDictionary.TryRemoveAsync(txn, userID);
                await txn.CommitAsync();
            }
            return returnResult;
                //"You have successfully placed the order!(deleted the user, etc.)";
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method should try and delete the user's dictionary
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>Returns a message that indicated the success (returns "success") or failure (returns "no dictionary") of deleting the user's cart dictionary</returns>
        public async Task<string> DeleteUserCart(string userID)
        {
            string userOrderDictionaryPointerName = await GetOrCreateUserDictionaryName(userID);
            if(userOrderDictionaryPointerName == ("Sorry a dictionary for user: " + userID + " cannot be found. Please add the user to the userDictionary"))
            {
                return "no dictionary";
            }

            //gets the dictionary for the given user
            IReliableDictionary<string, int> userOrderDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, int>>(userOrderDictionaryPointerName);
            using (var txn = this.StateManager.CreateTransaction())
            {
                await userOrderDictionary.TryRemoveAsync(txn, "Chocolate Java Chip");
                await userOrderDictionary.TryRemoveAsync(txn, "MM Madness");
                await userOrderDictionary.TryRemoveAsync(txn, "LRU BLU-berry");
                await userOrderDictionary.TryRemoveAsync(txn, "Peanut Butter Jam");
                await txn.CommitAsync();
            }
            return "success";
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

            //I think that this is a repeated method and isn't actually used

        /// <summary>
        /// Gets the cart dictionary contents for the given userID
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>Returns the items in the user's cart as a string where items are separated by commas and the respective quantity for each flavor is listed right after the flavor
        /// (i.e. Chocolate Java Chip,55,Peanut Butter Jam,22)
        /// </returns>
        public async Task<IReliableDictionary<string, int>> GetCartItems(string userID)
        {
            string userOrderDictionaryPointerName = await GetOrCreateUserDictionaryName(userID);
            IReliableDictionary<string, int> userOrderDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, int>>(userOrderDictionaryPointerName);
            return userOrderDictionary;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method takes the inventory dictionary and sends it as a string 
        /// </summary>
        /// <returns>Returns the items in inventory and their quantities as a string in the following format: MM Madness: 5000 | Chocolate Java Chip: 5000 | LRU BLU-berry: 5000 | Peanut Butter Jam: 5000
        /// </returns>
        public async Task<string> GetInventoryString()
        {
            string inventoryString;
            using (var txn = this.StateManager.CreateTransaction())
            {
                inventoryString = "MM Madness: " + (await inventoryDictionary.TryGetValueAsync(txn, "MM Madness")).Value + " | Chocolate Java Chip: " + (await inventoryDictionary.TryGetValueAsync(txn, "Chocolate Java Chip")).Value;
                inventoryString = inventoryString + " | LRU BLU-berry: " + (await inventoryDictionary.TryGetValueAsync(txn, "LRU BLU-berry")).Value + " | Peanut Butter Jam: " + (await inventoryDictionary.TryGetValueAsync(txn, "Peanut Butter Jam")).Value;
                await txn.CommitAsync();
            }
            return inventoryString;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method takes the price dictionary and sends it as a string 
        /// </summary>
        /// <returns>Returns the items and their prices as a string in the following format: MM Madness,3.1,Chocolate Java Chip,3,LRU BLU-berry,3.85,Peanut Butter Jam,3.15
        /// </returns>
        public async Task<string> GetPriceString()
        {
            string priceString;
            using (var txn = this.StateManager.CreateTransaction())
            {
                priceString = "MM Madness," + (await priceDictionary.TryGetValueAsync(txn, "MM Madness")).Value + ",Chocolate Java Chip," + (await priceDictionary.TryGetValueAsync(txn, "Chocolate Java Chip")).Value;
                priceString = priceString + ",LRU BLU-berry," + (await priceDictionary.TryGetValueAsync(txn, "LRU BLU-berry")).Value + ",Peanut Butter Jam," + (await priceDictionary.TryGetValueAsync(txn, "Peanut Butter Jam")).Value;
                await txn.CommitAsync();
            }
            return priceString;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method creates/initializes a new reliable dictionary to serve as the user's cart dictionary
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>Returns a message that indicated the success (returns the user's cart dictionary name) or failure (returns "User exists and should have a dictionary already created.") 
        /// of creating cart dictionary</returns>
        public async Task<string> CreateNewUserDictionary(string userID)
        {
            if(DoesUserExist(userID).Result == 1)
            {
                return "User exists and should have a dictionary already created.";
            }
            else 
            {
                Guid id = Guid.NewGuid();
                string userOrderDictionaryPointerName = id.ToString();
                IReliableDictionary<string, int> thingName = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, int>>(userOrderDictionaryPointerName);
                return userOrderDictionaryPointerName;
            }  
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method adds a user to the user dictionary indicating they are an "active" user
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        public async Task AddUser(string userID)
        {
            using (var txn = this.StateManager.CreateTransaction())
            {
                if((await DoesUserExist(userID) ) == 0)
                {
                    await userDictionary.GetOrAddAsync(txn, userID, (await CreateNewUserDictionary(userID)));
                    await txn.CommitAsync();
                }
                return;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// This method is a helper method used within Stateful.cs that determines if the user is in the user dictionary or not.
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>This method returns 1 if the user is already in the userDictionary and returns 0 if the user is not in the userDictionary.</returns>
        public async Task<int> DoesUserExist(string userID)
        {
            using (var txn = this.StateManager.CreateTransaction())
            {
               ConditionalValue<string> exists = await userDictionary.TryGetValueAsync(txn, userID);
                if (exists.HasValue)
                {
                    return 1;
                }
                else 
                {
                    return 0;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// This method gets the name of the user's cart dictionary or creates a cart dictionary if one doesn't exist.
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>returns "Sorry a dictionary for user: " + userID + " cannot be found" if the user dictionary can't be found or returns the name of the specific users dictionary</returns>
        public async Task<string> GetOrCreateUserDictionaryName(string userID)
        {
            using (var txn = this.StateManager.CreateTransaction())
            {
                if (DoesUserExist(userID).Result > 0)
                {
                    return (await userDictionary.TryGetValueAsync(txn, userID)).Value;
                }
                else
                {
                    AddUser(userID);
                    return await GetOrCreateUserDictionaryName(userID);
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        //
        /// <summary>
        /// This method takes the user's cart dictionary and sends it as a string
        /// </summary>
        /// <param name="userID">GUID that is passed from the javaScript layer that identifies the user</param>
        /// <returns>Returns various faliure messages based on differnt scenarios that describe the failure. On success it returns the cart string 
        /// in this format: Chocolate Java Chip,3,LRU BLU-berry,3</returns>
        public async Task<string> GetCartString(string userID)
        {
            if(DoesUserExist(userID).Result == 0)
            {
                return "User hasn't been initialized in the user dictionary -- user must go to the cookie cache page/add something to the cart";
            }

            string userOrderDictionaryPointerName = await GetOrCreateUserDictionaryName(userID);

            if (userOrderDictionaryPointerName == ("Sorry a dictionary for user: " + userID + " cannot be found. Please add the user to the userDictionary"))
            {
                return "no dictionary";
            }
            IReliableDictionary<string, int> userOrderDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, int>>(userOrderDictionaryPointerName);
                
            if(userID == "")
            {
                return "Your Cart is empty! (No user set)";
            }

            if(userOrderDictionary == null)
            {
                return "Cart is empty";
            }    

            IList<string> cartList = new List<string>();
            using (var txn = this.StateManager.CreateTransaction())
            {
                //this takes the IReliable Dictionary and turns it into a IList
                IAsyncEnumerator<KeyValuePair<string, int>> enumerator = (await userOrderDictionary.CreateEnumerableAsync(txn)).GetAsyncEnumerator();
                while (await enumerator.MoveNextAsync(token))
                {
                   cartList.Add(enumerator.Current.Key);
                   cartList.Add((enumerator.Current.Value).ToString());
                }
                await txn.CommitAsync();
            }

            if(cartList.Count == 0)
            {
                return "Your cart list is empty";
            }

            //this takes the list and converts it into a string 
            int numOfFlavs = (cartList.Count) / 2;
            string returnString = numOfFlavs + "," + cartList[0];
            for (int i = 1; i < cartList.Count; i++)
            {
               returnString = returnString + "," + cartList[i];
            }

            return returnString;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        public Stateful1(StatefulServiceContext context)
            : base(context)
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[] { new ServiceReplicaListener(context =>this.CreateServiceRemotingListener(context)) };
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// This method is used by the subract inventory method to verify that there is enough inventory to supply the order
        /// </summary>
        /// <param name="flavor">Flavor of the specified cookie to be checked in inventory</param>
        /// <param name="quantity">The quantity of cookies the user wants to add to the cart to be checked against inventory</param>
        /// <returns>Returns true if their is enough inventory and returns false if there isn't enough inventory availible</returns>
        async Task<bool> EnoughInventory(string flavor, int quantity)
        {
            using(var txn2 = this.StateManager.CreateTransaction())
            {
               ConditionalValue<int> inventoryValue = await inventoryDictionary.TryGetValueAsync(txn2, flavor); //do I want LockMode here?
               if(inventoryValue.Value < quantity)
                {
                    await txn2.CommitAsync();
                    return false;
                }
                else
                {
                    return true;
                }
            }  
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method makes a change to the inventory dictionary depending on what you want to subtract and returns the remaining inventory 
        /// </summary>
        /// <param name="flavor">Flavor of the specified cookie to be subtracted from inventory</param>
        /// <param name="quantity">The quantity of cookies the user wants to add to the cart to be taken out of inventory</param>
        /// <returns>Returns -1 if there isn't enough inventory left for the order quantity or returns the amount of product left if the order can be supplied</returns>
        async Task<int> SubtractInventory(string flavor, int quantity)
        {
           bool enoughIn = await EnoughInventory(flavor, quantity);
           if (enoughIn)
            {
                using (var txn3 = this.StateManager.CreateTransaction())
                {
                    await inventoryDictionary.AddOrUpdateAsync(txn3, flavor, quantity, (key, oldValue) => oldValue - quantity);
                    ConditionalValue<int> returnIn = await inventoryDictionary.TryGetValueAsync(txn3, flavor);
                    await txn3.CommitAsync();
                    return returnIn.Value;
                }         
            }
            else //there is not enough inventory in stock for the order to be placed
            {
                return -1;
            }
         
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method initializes the values that are held in inventory (both the flavor(s) and quantities) 
        /// </summary>
        /// <param name="inventoryDict">This is the IReliableDictionary that holds the inventory information</param>
        async void InitializeInventory(IReliableDictionary<string, int> inventoryDict)
        {
            using(var txn1 = this.StateManager.CreateTransaction())
            {
                if((await inventoryDict.TryGetValueAsync(txn1, "MM Madness")).HasValue)
                {
                    return;
                }
                await inventoryDict.AddAsync(txn1, "MM Madness", 5000);
                await inventoryDict.AddAsync(txn1, "Chocolate Java Chip", 5000);
                await inventoryDict.AddAsync(txn1, "LRU BLU-berry", 5000);
                await inventoryDict.AddAsync(txn1, "Peanut Butter Jam", 5000);
                await txn1.CommitAsync();
            }
              
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This method initializes the values that are held in the price dictionary for the cookies offered (both the flavor(s) and prices)
        /// </summary>
        /// <param name="priceDict">This is the IReliableDictionary that holds the price information</param>
        async void InitializePrices(IReliableDictionary<string, double> priceDict)
        {
            using (var txn1 = this.StateManager.CreateTransaction())
            {
                if ((await priceDict.TryGetValueAsync(txn1, "MM Madness")).HasValue)
                {
                    return;
                }
                await priceDict.AddAsync(txn1, "MM Madness", 3.10);
                await priceDict.AddAsync(txn1, "Chocolate Java Chip", 3.00);
                await priceDict.AddAsync(txn1, "LRU BLU-berry", 3.85);
                await priceDict.AddAsync(txn1, "Peanut Butter Jam", 3.15);
                await txn1.CommitAsync();
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            this.token = cancellationToken;
            //inventory dictionary should be updated here
            inventoryDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, int>>("inventoryDictionaryName");
            priceDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, double>>("priceDictionaryName");
            userDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, string>>("userDictionaryName");
            InitializeInventory(inventoryDictionary);
            InitializePrices(priceDictionary);


        }
    }
}
