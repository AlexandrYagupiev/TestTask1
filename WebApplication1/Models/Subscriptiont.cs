using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public partial class Subscriptiont
    {
        public int id { get; set; }
        public string url { get; set; }
        public string mail { get; set; }
    }
}
