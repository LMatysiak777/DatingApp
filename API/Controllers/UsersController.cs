using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using API.Helpers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.interfaces;
using AutoMapper;
using API.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API.Extensions;

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
        private readonly IPhotoService photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this.photoService = photoService;
            this.mapper = mapper;
            this.userRepository = userRepository;

        }


        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams )
        {

            //returning opposite gender in member list as default
            var user= await this.userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = user.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female" : "male" ;
            var users = await this.userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await this.userRepository.GetMemberAsync(username);
        }
        //put method to update data to server

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await this.userRepository.GetUserByUsernameAsync(User.GetUsername());
            this.mapper.Map(memberUpdateDto, user);
            this.userRepository.Update(user);

            if (await this.userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("failed to update user");

        }
        //below attribute defines endpoint for addphoto method as .../add-photo
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await this.userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await this.photoService.AddPhotoAsync(file);
            //handle cloudinary or other error related to adding photo 
            if (result.Error!=null) return BadRequest(result.Error.Message);
            //creating photo basing on parameters returned by cloudinary 
            var photo = new Photo {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
            //check for users first photo upload and making it main photo
            if (user.Photos.Count ==0)
            {
                photo.IsMain =true;
            }
            user.Photos.Add(photo); 
            if (await this.userRepository.SaveAllAsync()) {
                // return this.mapper.Map<PhotoDto>(photo);
                return CreatedAtRoute("GetUser",new {Username = user.UserName},this.mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("problem adding photo ...");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId) 
        {
            var user= await this.userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo= user.Photos.FirstOrDefault(x=>x.Id ==photoId);
            if(photo.IsMain) return BadRequest("This is already your main photo");
            var currentMain = user.Photos.FirstOrDefault(x=>x.IsMain==true);
            if (currentMain != null) currentMain.IsMain = false; 
            photo.IsMain =true;

            if(await this.userRepository.SaveAllAsync()) return NoContent(); 
            return BadRequest("Failed to set main photo");

        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await this.userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x=>x.Id == photoId);

            if(photo==null) return NotFound();
            if(photo.IsMain) return BadRequest("you cannot delete your main photo");
            if(photo.PublicId != null) {
                var result = await this.photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if (await this.userRepository.SaveAllAsync()) return Ok();
            return BadRequest ("Failed to delete foto");
        }

    }
}