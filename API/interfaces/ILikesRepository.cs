using System.Threading.Tasks    ;
using API.Entities;
using API.DTOs;
using System.Collections.Generic;
using API.Helpers;

namespace API.interfaces

{
    public interface ILikesRepository
    { 
        Task<UserLike> GetUserLike(int SourceUserId, int LikedUserId); 
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes (LikesParams likesParams);

    }
}