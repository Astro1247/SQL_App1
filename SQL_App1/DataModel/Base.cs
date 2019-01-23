using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_App1.DataModel
{
    // Класс модели данных CodeFirst

    public class Users
    {
        [Required]
        [Index]
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        [ConcurrencyCheck]
        public string Username { get; set; }
        [Required]
        [MaxLength(255)]
        [ConcurrencyCheck]
        public string Password { get; set; }
        public ICollection<Keys> Keys { get; set; }
    }

    public class Chats
    {
        [Required]
        [Index]
        [Key]
        public int Id { get; set; }
        [Required]
        public int Creator { get; set; }
        [Required]
        public int member { get; set; }
        [Range(0, Double.MaxValue)]
        public ICollection<Messages> Messages { get; set; }
        public ChatOptions ChatOptions { get; set; }
    }

    public class ChatOptions
    {
        [Required] [Index] [Key]
        public int Id { get; set; }
        public bool AnnounceOnly { get; set; }
        public bool InviteOnly { get; set; }
        public bool IsHidden { get; set; }
    }

    public class ConferenceOptions : ChatOptions
    {
        public int Period { get; set; }
        public bool NotifyOnMemberJoin { get; set; }
    }
    public class AdminOptions : ConferenceOptions
    {
        public int MaxAdminCount { get; set; }
        public bool AdminAllRights { get; set; }
    }


    public class Keys
    {
        [Required]
        [Index]
        public int Id { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Key { get; set; }
        public ICollection<Users> User { get; set; }
    }

    public class Messages
    {
        [Required]
        [Index]
        [Key]
        public int Id { get; set; }
        [Required]
        public int Sender { get; set; }
        [Required]
        public int ChatId { get; set; }

        public string Message { get; set; }
    }
}
