using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DlyaTexOtcheta.data
{
    public class Request
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string CommentAuthor { get; set; }
        public string Author { get; set; }
        public string Address { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedOn { get; set; }
    }
}
