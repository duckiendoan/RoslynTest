using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTest
{
    public class CSharpParameterNode : CSharpNode
    {
        public CSharpParameterNode(string name) : base(name)
        {
        }

        public CSharpType ReturnType { get; set; }

        public string DefaultValue { get; set; }
    }
}
