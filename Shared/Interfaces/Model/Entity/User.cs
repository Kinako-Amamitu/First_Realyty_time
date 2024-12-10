﻿using MessagePack;
using System;

namespace MasicOnionServer00.Model.Entity
{
    [MessagePackObject]
    public class User
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Password { get; set; }
        [Key(3)]
        public string Token { get; set; }
        [Key(4)]
        public DateTime Created_at { get; set; }
        [Key(5)]
        public DateTime Updated_at { get; set; }
    }


}
