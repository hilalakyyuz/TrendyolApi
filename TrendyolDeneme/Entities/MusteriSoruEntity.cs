using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolDeneme
{
    public partial class EntityContent
    {
        public Content[] Content { get; set; }
        public long Page { get; set; }
        public long Size { get; set; }
        public long TotalElements { get; set; }
        public long TotalPages { get; set; }
    }

    public partial class Content
    {
        public Answer Answer { get; set; }
        public string AnsweredDateMessage { get; set; }
        public long CreationDate { get; set; }
        public long CustomerId { get; set; }
        public long Id { get; set; }
        public Uri ImageUrl { get; set; }
        public string ProductName { get; set; }
        public bool Public { get; set; }
        public bool ShowUserName { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public Uri WebUrl { get; set; }
        public string ProductMainId { get; set; }
    }

    public partial class Answer
    {
        public long Id { get; set; }
        public long CreationDate { get; set; }
        public string Text { get; set; }
    }
}
