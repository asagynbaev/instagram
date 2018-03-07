using System;

namespace Instagram.Models
{
    public class Images
    {
        public int id { get; set; }
        public byte[] image { get; set; }
        public string description { get; set; }
        public int user_id { get; set; }
        public int likes { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}