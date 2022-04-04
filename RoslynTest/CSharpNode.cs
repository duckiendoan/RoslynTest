using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTest
{
    public class CSharpNode
    {
        public int Id { get; internal set; }

        public string Name { get; }
        public IReadOnlyList<CSharpNode> Children => _children;

        public CSharpNode Parent { get; set; }

        private List<CSharpNode> _children;

        public CSharpNode(string name)
        {
            Name = name;
            _children = new List<CSharpNode>();
        }

        public void AddChild(CSharpNode child)
            => _children.Add(child);

        public void AddChildren(IEnumerable<CSharpNode> children)
            => _children.AddRange(children);
    }
}
