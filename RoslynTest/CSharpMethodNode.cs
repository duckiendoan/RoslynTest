using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTest
{
    public class CSharpMethodNode : CSharpNode
    {
        public CSharpMethodNode(string name) : base(name)
        {
        }

        public CSharpType ReturnType { get; set; }
        public bool IsConstructor { get; set; }
    }
}
