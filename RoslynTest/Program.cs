using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTest;
/*
 * References:
 * https://sankarsan.wordpress.com/2011/11/20/roslyn-ctpa-walk-through-the-syntax-treepart-i/
 * http://www.swat4net.com/roslyn-you-part-iii-playing-with-syntax-trees/
 * https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
 * C# 8.0 in a Nutshell: The Definitive Reference Chapter 27
 */
class Program
{
    static void Main(string[] args)
    {
        string path = Console.ReadLine();
        var parser = new TestParser();
        var root = parser.ParseProject(path);
        GenerateIdDFS(root);
        //BFS(root);
        var classNodes = new List<CSharpClassNode>();
        GetClasses(classNodes, root);
        Console.WriteLine($"Found {classNodes.Count} classes");
        foreach (var classNode in classNodes)
        {
            Console.WriteLine($"[{classNode.Id}] Class {classNode.Name} has {classNode.Children.Count} children");
        }
        Console.ReadLine();
    }

    static void BFS(CSharpNode node)
    {
        var queue = new Queue<CSharpNode>();
        queue.Enqueue(node);
        var currentNode = node;
        while (queue.TryDequeue(out currentNode))
        {
            Console.WriteLine($"[{currentNode.Id}] {currentNode.Name}: {currentNode.GetType()}");
            foreach (var child in currentNode.Children)
                queue.Enqueue(child);
        }
    }
    static int currentId = 0;
    static void GenerateIdDFS(CSharpNode node)
    {
        var stack = new Stack<CSharpNode>();
        stack.Push(node);
        var currentNode = node;
        while (stack.TryPop(out currentNode))
        {
            currentNode.Id = currentId;
            currentId++;
            foreach (var child in currentNode.Children)
                stack.Push(child);
        }
    }

    static void GetClasses(List<CSharpClassNode> nodes, CSharpNode root)
    {
        if (root.Children.Count == 0) return;
        if (root is CSharpClassNode)
        {
            nodes.Add(root as CSharpClassNode);
            return;
        }    
        foreach (var child in root.Children)
            GetClasses(nodes, child);
    }
}