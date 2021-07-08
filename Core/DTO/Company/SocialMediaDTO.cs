using Core.Entity;
using System;

namespace Core.DTO
{
    public class SocialMediaDTO
    {
        public int SocialMediaId { get; set; }

        public string Url { get; set; }

        public SocialMediaType Type { get; set; }
    }
}
