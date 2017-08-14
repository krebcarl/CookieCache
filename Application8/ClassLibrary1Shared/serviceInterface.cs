using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;

public interface IMyService : IService
{
    Task<string> AddToDict(string flavor, int quantity, string userID);
    Task<IReliableDictionary<string, int>> GetCartItems(string userID);
    Task<string> GetInventoryString();
    Task<string> GetCartString(string userID);
    Task<string> GetPriceString();
    Task<string> GetOrCreateUserDictionaryName(string userID);
    Task<string> CreateNewUserDictionary(string userID);
    Task<string> DeleteUser(string userID);
    Task<string> DeleteUserCart(string userID);
}
