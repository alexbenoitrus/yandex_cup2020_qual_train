using System.Threading.Tasks;

namespace Yandex.Cup2020.Qual.Train.C.InterestingGame
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    class Program
    {
        private const string InputFile = "input.txt";
        private const string OutputFile = "output.txt";

        private static Graph Graph;
        private static List<DownloadRequest> Requests;

        static void Main(string[] args)
        {
            GetInput();
            Console.WriteLine(Process());
        }

        private static string Process()
        {
            var result = new StringBuilder();

            foreach (var request in Requests)
            {
                var confirmServers = request.ConfirmServers;

                var taskCount = request.ContenderServers.Length;
                var tasks = new Task[taskCount];

                for (int i = 0; i < taskCount; i++)
                {
                    var storage = request.ContenderServers[i];
                    tasks[i] = Task.Factory.StartNew(() => FindPath(request.To, storage, confirmServers));
                }

                Task.WaitAll(tasks);

                result.AppendLine(request.CanDownload
                    ? $"{confirmServers.Count} {string.Join(' ', confirmServers)}"
                    : "0");
            }

            return result.ToString();
        }

        private static void FindPath(string target, string storageServer, List<string> goodCollection)
        {
            var pathManager = new PathManager(Graph);
            bool hasPath = pathManager.HasPath(storageServer, target);
            if (hasPath) goodCollection.Add(storageServer);
        }

        private static void GetInput()
        {
            Graph = new Graph();
            Requests = new List<DownloadRequest>();

            var input = ReadInput().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            int pairsCount = int.Parse(input[0]);
            for (int i = 1; i <= pairsCount; i++)
            {
                var pairServers = input[i].Split(' ');
                var serverA = pairServers[0];
                var serverB = pairServers[1];

                if (Graph.FindVertex(serverA) == null) Graph.AddVertex(serverA);
                if (Graph.FindVertex(serverB) == null) Graph.AddVertex(serverB);

                Graph.AddEdge(serverA, serverB);
            }

            int requestCountsPosition = pairsCount + 1;
            int requestCounts = int.Parse(input[requestCountsPosition]);

            for (int i = requestCountsPosition + 1; i < requestCountsPosition + 1 + requestCounts * 2; i++)
            {
                Requests.Add(new DownloadRequest
                {
                    To = input[i].Split(' ')[0],
                    ContenderServers = input[++i].Split(' ')
                });
            }
        }

        private static string ReadInput()
        {
            using (var file = File.Open(InputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }

        private static void WriteOutput(object value)
        {
            WriteOutput(value.ToString());
        }

        private static void WriteOutput(string value)
        {
            using (var writer = new StreamWriter(OutputFile, true))
            {
                writer.Write(value);
            }
        }

        private class DownloadRequest
        {
            public DownloadRequest()
            {
                this.ConfirmServers = new List<string>();
            }

            public string To { get; set; }

            public bool CanDownload => this.ConfirmServers.Any();

            public List<string> ConfirmServers { get; set; }

            public string[] ContenderServers { get; set; }
        }
    }

    /// <summary>
    /// Ребро графа
    /// </summary>
    public class Edge
    {
        public Edge(Vertex connectedVertex)
        {
            this.ConnectedVertex = connectedVertex;
        }
        public Vertex ConnectedVertex { get; }

    }

    /// <summary>
    /// Вершина графа
    /// </summary>
    public class Vertex
    {
        public Vertex(string vertexName)
        {
            this.Name = vertexName;
            this.Edges = new List<Edge>();
        }

        public string Name { get; }

        public List<Edge> Edges { get; }

        public void AddEdge(Edge edge)
        {
            this.Edges.Add(edge);
        }

        public void AddEdge(Vertex vertex)
        {
            this.AddEdge(new Edge(vertex));
        }

        public override string ToString() => this.Name;
    }

    public class Graph
    {
        public Graph()
        {
            this.Vertices = new List<Vertex>();
        }
        public List<Vertex> Vertices { get; }

        public void AddVertex(string vertexName)
        {
            this.Vertices.Add(new Vertex(vertexName));
        }

        public Vertex FindVertex(string name)
        {
            return this.Vertices.FirstOrDefault(v => v.Name.Equals(name));
        }

        public void AddEdge(string firstName, string secondName)
        {
            var vertex1 = this.FindVertex(firstName);
            var vertex2 = this.FindVertex(secondName);

            if (vertex2 != null && vertex1 != null)
            {
                vertex1.AddEdge(vertex2);
                vertex2.AddEdge(vertex1);
            }
        }
    }

    /// <summary>
    /// Информация о вершине
    /// </summary>
    public class VertexInfo
    {
        public VertexInfo(Vertex vertex)
        {
            this.Vertex = vertex;
            this.IsVisited = true;
        }

        public Vertex Vertex { get; set; }

        public bool IsVisited { get; set; }
    }

    public class PathManager
    {
        public PathManager(Graph graph)
        {
            this.Graph = graph;
        }

        private Graph Graph { get; }

        private List<VertexInfo> Infos { get; set; }

        public bool HasPath(string startName, string finishName)
        {
            InitInfo();
            this.ResetVisits();

            return HasPath(Graph.FindVertex(startName), Graph.FindVertex(finishName));
        }

        private bool HasPath(Vertex startVertex, Vertex finishVertex)
        {
            var first = GetVertexInfo(startVertex);
            if (first.IsVisited) return false;

            first.IsVisited = true;

            var nextVertices = first.Vertex.Edges
                .Select(e => e.ConnectedVertex)
                .ToList();

            if (!nextVertices.Any()) return false;
            if (nextVertices.Any(v => v == finishVertex)) return true;

            return nextVertices.Any(nextVertex => HasPath(nextVertex, finishVertex));
        }

        private void ResetVisits()
        {
            this.Infos.ForEach(i => i.IsVisited = false);
        }

        private VertexInfo GetVertexInfo(Vertex vertex)
        {
            return this.Infos.FirstOrDefault(i => i.Vertex.Equals(vertex));
        }

        private void InitInfo()
        {
            Infos = Graph.Vertices
                .Select(v => new VertexInfo(v))
                .ToList();
        }
    }
}