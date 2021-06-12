using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MeshGenerator : MonoBehaviour {

    public SquareGrid squareGrid;
    List<Vector3> vertices;
    List<int> triangles;

    public void GenerateMesh(int[,] map, float squareSize) {
        squareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x ++) {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y ++) {
                TriangulateSquare(squareGrid.squares[x,y]);
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }


    void TriangulateSquare(Square square) {

	    Dictionary<int, List<MeshGenerator.Node>> dict = new Dictionary<int, List<MeshGenerator.Node>>();
		dict.Add(0, new List<MeshGenerator.Node>(new MeshGenerator.Node[] { square.centreBottom, square.bottomLeft, square.centreLeft }));
		dict.Add(1, new List<MeshGenerator.Node>(new MeshGenerator.Node[] { square.centreRight, square.bottomRight, square.centreBottom }));
		dict.Add(2, new List<MeshGenerator.Node>(new MeshGenerator.Node[] { square.centreTop, square.topRight, square.centreRight }));
		dict.Add(3, new List<MeshGenerator.Node>(new MeshGenerator.Node[] { square.centreLeft, square.topLeft, square.centreTop }));

		int configTracker = square.configuration;
		var nodeList = new List<MeshGenerator.Node>();

		for(int i = 0; i < 4; i++) {
			if( ((configTracker>>i) & 1) == 0b_1 ) {
				var union = dict[i].Union(nodeList);
				var intersect = nodeList.Intersect(dict[i]);
				nodeList = union.Except(intersect).ToList();
			}
		}
		MeshFromPoints(nodeList.ToArray());
    }

    void MeshFromPoints(params Node[] points) {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5) 
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
    }

    void AssignVertices(Node[] points) {
        for (int i = 0; i < points.Length; i ++) {
            if (points[i].vertexIndex == -1) {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c) {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
    }

    void OnDrawGizmos() {
        /*
        if (squareGrid != null) {
            for (int x = 0; x < squareGrid.squares.GetLength(0); x ++) {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y ++) {
                    Gizmos.color = (squareGrid.squares[x,y].topLeft.active)?Color.black:Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x,y].topLeft.position, Vector3.one * .4f);
                    Gizmos.color = (squareGrid.squares[x,y].topRight.active)?Color.black:Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x,y].topRight.position, Vector3.one * .4f);
                    Gizmos.color = (squareGrid.squares[x,y].bottomRight.active)?Color.black:Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x,y].bottomRight.position, Vector3.one * .4f);
                    Gizmos.color = (squareGrid.squares[x,y].bottomLeft.active)?Color.black:Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x,y].bottomLeft.position, Vector3.one * .4f);
                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreTop.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreRight.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreBottom.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreLeft.position, Vector3.one * .15f);
                }
            }
        }
        */
    }

    public class SquareGrid {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize) {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

            for (int x = 0; x < nodeCountX; x ++) {
                for (int y = 0; y < nodeCountY; y ++) {
                    Vector3 pos = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, 0, -mapHeight/2 + y * squareSize + squareSize/2);
                    controlNodes[x,y] = new ControlNode(pos,map[x,y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX -1,nodeCountY -1];
            for (int x = 0; x < nodeCountX-1; x ++) {
                for (int y = 0; y < nodeCountY-1; y ++) {
                    squares[x,y] = new Square(controlNodes[x,y+1], controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y]);
                }
            }

        }
    }

    public class Square {

        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centreTop, centreRight, centreBottom, centreLeft;
        public int configuration;

        public Square (ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft) {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;

            if (topLeft.active)
                configuration += 0b_1000;
            if (topRight.active)
                configuration += 0b_0100;
            if (bottomRight.active)
                configuration += 0b_0010;
            if (bottomLeft.active)
                configuration += 0b_0001;
        }
    }

    public class Node {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos) {
            position = _pos;
        }
    }

    public class ControlNode : Node {

        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos) {
            active = _active;
            above = new Node(position + Vector3.forward * squareSize/2f);
            right = new Node(position + Vector3.right * squareSize/2f);
        }
    }
}