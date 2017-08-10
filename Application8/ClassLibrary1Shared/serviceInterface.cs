using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;

public interface IMyService : IService
{
    Task<string> addToDict(string flavor, int quantity, string userID);
    Task<IReliableDictionary<string, int>> getCartItems(string userID);
    Task<string> getInventoryString();
    Task<string> getCartString(string userID);
    Task<string> getPriceString();
    Task<string> getOrCreateUserDictionaryName(string userID);
    Task<string> createNewUserDictionary(string userID);
    Task<string> deleteUser(string userID);
    Task<string> deleteUserCart(string userID);




}

//class MyService : StatelessService, IMyService
//{
//    //public MyService(StatelessServiceContext context)
//    //    : base (context)
//    //{
//    //}

//    //public Task HelloWorldAsync()
//    //{
//    //    return Task.FromResult("Hello!");
//    //}

//    //protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
//    //{
//    //    return new[] { new ServiceInstanceListener(context =>            this.CreateServiceRemotingListener(context)) };
//    //}
//}