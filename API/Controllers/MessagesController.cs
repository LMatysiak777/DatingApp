using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Helpers;
using System.Threading.Tasks;
using API.interfaces;
using API.DTOs;
using API.Extensions;
using API.Entities;
using AutoMapper;

namespace API.Controllers
{
    [Authorize]

    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;

        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("you cannot send messages to yourself");

            var sender = await this.userRepository.GetUserByUsernameAsync(username);
            var recipient = await this.userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                Content = createMessageDto.Content

            };

            this.messageRepository.AddMessage(message);
            if (await this.messageRepository.SaveAllAsync()) return Ok(this.mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message"); 


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername() ;
            var messages = await this.messageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesThread (string username ) 
        {
            var currentUsername = User.GetUsername() ; 
            return Ok(await this.messageRepository.GetMessageThread(currentUsername, username)); 
        }

        [HttpDelete ("{id}")]

        public async Task<ActionResult> DeleteMessage (int id)
        {
            var username = User.GetUsername();
            var message = await this.messageRepository.GetMessage(id);
            if (message.Sender.UserName != username && message.Recipient.UserName !=username) return Unauthorized();

            if (message.Sender.UserName == username ) message.SenderDeleted = true; 

            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted) this.messageRepository.DeleteMessage(message);

            if (await this.messageRepository.SaveAllAsync()) return Ok();


            return BadRequest("problem deleting message");
        }
 


}
}
