using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Messages
{
     public class MessageContainer
    {
        //lists will be containing 3 items first item will be registerer email, second messagfer email third will be the message
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string receiver {  get; set; }
        [Required]
        public string messager { get; set; }
        [Required]
        public string message { get; set; }
        [Required]
        public DateTime createdtime { get; set; } = DateTime.Now;

        [Required]
        public bool accepted { get; set; } = false;

        [Required]
        public string subject { get; set; }


    }
}
