using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder2 : MonoBehaviour {

	public PathFind.Grid grid;
	public float mazeScale;
	public float mazeOffsetY;

	public int gridScale = 10;

	float halfGrid;

	public float gFactor;
	public float mFactor;

	// Use this for initialization
	void Start () {
		halfGrid = gridScale / 2;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMazeScale(float ms){
		mazeScale = ms;
		mFactor = 1f * mazeScale / gridScale;
		gFactor = 1f * gridScale / mazeScale;

		/*print ("grid: 0_0 to: "+Grid2Maze (new PathFind.Point(0,0)));
		print ("grid: 269_0 to: "+Grid2Maze (new PathFind.Point(269,0)));
		print ("grid: 0_269 to: "+Grid2Maze (new PathFind.Point(0,269)));
		print ("grid: 0_269 to: "+Grid2Maze (new PathFind.Point(269,269)));*/
	}

	public List<Vector3> FindPath(Vector3 from, Vector3 to){

		//print ("from: "+from+" to: "+to);

		Vector2 f = Maze2Grid (from);
		Vector2 t = Maze2Grid (to);

		//print ("from: "+from+" f: "+f);

		// create source and target points
		PathFind.Point _from = new PathFind.Point(Mathf.RoundToInt(f.x),Mathf.RoundToInt(f.y));
		PathFind.Point _to = new PathFind.Point(Mathf.RoundToInt(t.x),Mathf.RoundToInt(t.y));

		//print ("f: "+f+" from: "+Grid2Maze (_from));

		// get path
		// path will either be a list of Points (x, y), or an empty list if no path is found.
		List<PathFind.Point> path = PathFind.Pathfinding.FindPath(grid, _from, _to);
		//print ("path from: "+f+" to: "+t+" = "+ path.Count);

		List<Vector3> result = new List<Vector3> ();

		foreach (PathFind.Point p in path) {
			result.Add (Grid2Maze (p));
			//print ("point: " + Grid2Maze (p));
		}

		return result;
	}


	Vector2 Maze2Grid(Vector3 p){
		return new Vector2 (p.x * gFactor + halfGrid , (((p.y * -1) + mazeOffsetY) * gFactor) + halfGrid);
	}

	Vector3 Grid2Maze(PathFind.Point p){
		return new Vector3 ((p.x-halfGrid)*mFactor,(((p.y-halfGrid)*mFactor)-mazeOffsetY)*-1,0f);
	}
}
