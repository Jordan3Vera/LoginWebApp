using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using WebApiRest.Model.DTOs;

namespace WebApiRest.Helper
{
    public class StoreHub : Hub<IStoreHub> 
    {
        const string Customer = "Customer";
        const string Employee = "Employee";
        const string Store = "Store";

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Store);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Store);
            await base.OnDisconnectedAsync(ex);
        }

        public async Task SubscribeTogroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        //public async Task UnsubscribeFromGroup(string group)
        //{        //public async Task UnsubscribeFromGroup(string group)
        //{
            /*De acuerdo a la notificación de SignalR
             * No debería eliminar manualmente el usuario del grupo cuando el usuario está desconectado
             * Esta acción es automáticamente realizada por el framework SignalR
             * await Groups.RemoveFromGroupAsync(Context.CnnectionId, group)
             **/
        //}

        public async Task PostNewInfo(CustomerOrderDTO order, string group)
        {
            await Clients.Group(group).NotifyNewInfo(order);
        }
    }
}
