using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DlyaTexOtcheta.data
{
    public class ChatMessage
    {
        public int Id { get; set; }                  
        public string Sender { get; set; }           
        public string Text { get; set; }             
        public string MediaPath { get; set; }    
        public DateTime Timestamp { get; set; }      
    }
}
