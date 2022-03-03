using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{ 
    public class UserParams 
    {  
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } =1; 
        public int _pageSize { get; set; } = 5;
        public int PageSize { 
            get => _pageSize; 
            set => _pageSize = (value>PageSize) ? MaxPageSize :value ; } 

        public string CurrentUsername { get; set; }
    public string Gender { get; set; }

    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 150;
    public string OrderBy { get; set; } = "lastActive";
    

}
}