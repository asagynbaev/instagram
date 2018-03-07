using Instagram.Models;
using System.Collections.Generic;

namespace Instagram.Classes
{
    public class AccountImagesModel
    {
        public Account Account { get; set; }
        public List<Images> Images { get; set; }
        public int countOfImages { get; set; }
    }
}