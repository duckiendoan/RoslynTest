using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynTest
{
    public class TestParser
    {
        public CSharpNode ParseProject(string path)
        {
            if (!Directory.Exists(path))
                return null;
            var directoryName = Path.GetFileName(path);
            var root = new CSharpNode(directoryName);
            foreach (var directory in Directory.GetDirectories(path))
            {
                var node = ParseDirectory(directory);
                node.Parent = root;
                root.AddChild(node);
            }
            return root;
        }

        public CSharpNode ParseDirectory(string path)
        {
            var root = new CSharpNode(Path.GetFileName(path));

            foreach (var directory in Directory.GetDirectories(path))
            {
                var node = ParseDirectory(directory);
                node.Parent = root;
                root.AddChild(node);
            }

            foreach (var file in Directory.GetFiles(path, "*.cs"))
            {
                var nodes = ParseFile(file);
                foreach (var node in nodes)
                {
                    node.Parent = root;
                    root.AddChild(node);
                }
            }
            return root;
        }

        public List<CSharpNode> ParseFile(string path)
        {
            var nodes = new List<CSharpNode>();
            SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(path));
            var root = tree.GetRoot();
            var cds = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            nodes.AddRange(cds.Select(ParseClass));
            return nodes;
        }

        private CSharpClassNode ParseClass(ClassDeclarationSyntax classDeclarationSyntax)
        {
            var classNode = new CSharpClassNode(classDeclarationSyntax.Identifier.Text);
            if (classDeclarationSyntax.BaseList != null)
                foreach (var baseType in classDeclarationSyntax.BaseList.Types)
                    classNode.AddBaseType(new CSharpType(baseType.Type.ToString()));

            foreach (var member in classDeclarationSyntax.Members)
            {
                if (member is PropertyDeclarationSyntax)
                    classNode.AddChild(ParseProperty(classNode, member as PropertyDeclarationSyntax));

                else if (member is FieldDeclarationSyntax)
                    classNode.AddChildren(ParseField(classNode, member as FieldDeclarationSyntax));

                else if (member is MethodDeclarationSyntax)
                    classNode.AddChild(ParseMethod(classNode, member as MethodDeclarationSyntax));

                else if (member is ConstructorDeclarationSyntax)
                    classNode.AddChild(ParseConstructor(classNode, member as ConstructorDeclarationSyntax));
            }
            return classNode;
        }

        private CSharpPropertyNode ParseProperty(CSharpNode parent, PropertyDeclarationSyntax propertyDeclaration)
        {
            var propertyNode = new CSharpPropertyNode(propertyDeclaration.Identifier.Text);
            propertyNode.Type = new CSharpType(propertyDeclaration.Type.ToString());
            propertyNode.Parent = parent;
            return propertyNode;
        }

        private List<CSharpFieldNode> ParseField(CSharpNode parent, FieldDeclarationSyntax fieldDeclaration)
        {
            var fieldNodes = new List<CSharpFieldNode>();
            foreach (var variable in fieldDeclaration.Declaration.Variables)
            {
                var fieldNode = new CSharpFieldNode(variable.Identifier.Text);
                fieldNode.Parent = parent;
                fieldNode.Type = new CSharpType(fieldDeclaration.Declaration.Type.ToString());
            }
            
            return fieldNodes;
        }

        private CSharpMethodNode ParseMethod(CSharpNode parent, MethodDeclarationSyntax methodDeclaration)
        {
            var methodNode = new CSharpMethodNode(methodDeclaration.Identifier.Text);
            methodNode.ReturnType = new CSharpType(methodDeclaration.ReturnType.ToString());
            methodNode.Parent = parent;

            foreach (var parameter in methodDeclaration.ParameterList.Parameters)
            {
                var parameterNode = new CSharpParameterNode(parameter.Identifier.Text);
                parameterNode.Parent = methodNode;
                parameterNode.ReturnType = new CSharpType(parameter?.Type?.ToString());
                parameterNode.DefaultValue = parameter?.Default?.Value?.ToString();
                methodNode.AddChild(parameterNode);
            }
            return methodNode;
        }

        private CSharpMethodNode ParseConstructor(CSharpNode parent, ConstructorDeclarationSyntax constructorDeclaration)
        {
            var constructorNode = new CSharpMethodNode(constructorDeclaration.Identifier.Text);
            constructorNode.Parent = parent;

            foreach (var parameter in constructorDeclaration.ParameterList.Parameters)
            {
                var parameterNode = new CSharpParameterNode(parameter.Identifier.Text);
                parameterNode.Parent = constructorNode;
                parameterNode.ReturnType = new CSharpType(parameter?.Type?.ToString());
                parameterNode.DefaultValue = parameter?.Default?.Value?.ToString();
                constructorNode.AddChild(parameterNode);
            }
            return constructorNode;
        }
    }
}
