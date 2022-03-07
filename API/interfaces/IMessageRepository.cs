using System.Threading.Tasks    ;
using API.Entities;
using API.DTOs;
using System.Collections.Generic;
using API.Helpers;
using API.Helpers;

namespace API.interfaces

{
    public interface IMessageRepository
    {  
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        //method for getting individual message: return Task of type Message ... generic
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams); 
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string  RecipientUsername);
        Task<bool> SaveAllAsync();

    }
}