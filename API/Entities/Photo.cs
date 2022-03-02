using System.ComponentModel.DataAnnotations.Schema;
namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        // publicId will be used for deleting image from cloudinary
        public string PublicId { get; set; } 
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}