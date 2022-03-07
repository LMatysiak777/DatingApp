using API.interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using System.Collections.Generic    ;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using API.Helpers;

namespace API.Controllers {

    [Authorize]
    public class LikesController : BaseApiController
    {

        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {

            _likesRepository = likesRepository; 
            _userRepository = userRepository; 
        }

        [HttpPost("{username}")]

        public async Task<ActionResult>AddLike(string username)
        {
            var sourceUserId = User.GetUserId(); 
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == username ) return BadRequest(" You cannot like yourself ");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest ("you already liked this user"); 

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id,
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest ("failed to like user");

        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams) 
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams) ;
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            
            return Ok(users);
        }
    }
}