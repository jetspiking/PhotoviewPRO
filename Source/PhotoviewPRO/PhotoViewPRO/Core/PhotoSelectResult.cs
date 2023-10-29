using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoViewPRO.Core
{
    public class PhotoSelectResult
    {
        public String FilePath { get; set; }
        public String ParentDirectory { get; set; }
        public String[] Extensions { get; set; }
    }
}
