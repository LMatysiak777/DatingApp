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
    }
}