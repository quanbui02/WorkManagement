using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.DataContext
{
    public class RefreshToken
    {
        public int Id { get; set; }                 
        public string Token { get; set; }            
        public string UserId { get; set; }           
        public DateTime CreatedDate { get; set; } 
        public DateTime ExpiryDate { get; set; }   
        public bool IsRevoked { get; set; }       
        public AppUser User { get; set; } 
    }
}
