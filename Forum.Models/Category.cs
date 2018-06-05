using System;
using System.Collections;
using System.Collections.Generic;

namespace Forum.Models
{
    public class Category
    {
        public Category(int id, string name, IEnumerable<int> postIds)
        {
            this.Id = id;
            this.Name = name;
            this.PostsIds=new List<int>(postIds);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<int> PostsIds { get; set; }

    }
}
