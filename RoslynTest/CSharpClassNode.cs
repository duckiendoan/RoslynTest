using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTest
{
    public class CSharpClassNode : CSharpNode
    {
        public CSharpClassNode(string name) : base(name)
        {
            _baseTypes = new List<CSharpType>();
        }

        public IReadOnlyList<CSharpType> BaseTypes => _baseTypes;

        private List<CSharpType> _baseTypes;

        public void AddBaseType(CSharpType type) => _baseTypes.Add(type);
    }
}
