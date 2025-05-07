using System;
using Microsoft.AspNetCore.SignalR;

namespace Chatting_Service_Backend.Hubs
{
    public class ChatHub : Hub
    {
        // Method to send a message to all clients in a specific chatroom
        public async Task SendMessageToChatroom(string chatroomId, string senderName, string messageText)
        {
            await Clients.Group(chatroomId).SendAsync("ReceiveMessage", senderName, messageText);
        }

        // Method to join a chatroom group
        public async Task JoinChatroom(string chatroomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatroomId);
        }

        // Method to leave a chatroom group
        public async Task LeaveChatroom(string chatroomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatroomId);
        }
    }
}