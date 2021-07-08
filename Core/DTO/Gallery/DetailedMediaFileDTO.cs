using Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class DetailedMediaFileDTO : MediaFileDTO
    {
       public BasicFileInfo PhotoInfo { get; set; }
    }
}
