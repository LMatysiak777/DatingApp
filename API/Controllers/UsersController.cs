using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.interfaces;
using AutoMapper;
using API.DTOs;
using System.Security.Claims;

namespace API.Controllers
{

    // [ApiController]
    // [Route("api/[controller]")]

    // every controller inherits from controllerbase:
    // ApiController and Route properties now inherited from BaseApiController class:
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;

        }


        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await this.userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        { 
            return await this.userRepository.GetMemberAsync(username);
        }
        //put method to update data to server

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto) {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await this.userRepository.GetUserByUsernameAsync(username);
            this.mapper.Map(memberUpdateDto, user);
            this.userRepository.Update(user);

            if(await this.userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("failed to update user");
            
        }
    }
}