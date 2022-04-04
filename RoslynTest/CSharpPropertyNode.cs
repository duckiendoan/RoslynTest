using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTest
{
    public class CSharpPropertyNode : CSharpNode
    {
        public CSharpPropertyNode(string name) : base(name)
        {
        }

        public CSharpType Type { get; set; }
    }

    public class CSharpFieldNode : CSharpNode
    {
        public CSharpFieldNode(string name) : base(name)
        {
        }

        public CSharpType Type { get; set; }

        public string DefaultValue { get; set; }
    }
}
