using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTest
{
    public struct CSharpType
    {
        public string Name { get; set; }

        public CSharpType(string name) => Name = name;
    }
}
