using System;

namespace Instagram.Models
{
    public class Account
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public byte[] avatar { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public int gender { get; set; }
        public string description { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}