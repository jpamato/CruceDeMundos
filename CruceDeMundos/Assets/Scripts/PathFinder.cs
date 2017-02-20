using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

	public List<Vector2> path;
	public List<Vector2> lastTrace;
	private int traceCant;
	public int lastPointIndex;

	// Use this for initialization
	void Start () {
		path = new List<Vector2> ();
		traceCant = lastTrace.Count;	
	}

	public void AddTrace(Vector2 lastPoint){
		/*Vector2[] copy = new Vector2[lastTrace.Length];
		Array.Copy (lastTrace, 1, copy, 0, copy.Length - 1);
		copy [lastTrace.Length - 1] = lastPoint;
		lastTrace = copy;*/

		lastTrace.Add (lastPoint);
		lastTrace.RemoveAt (0);
	}

	public void TraceReset(){
		/*Vector2[] copy = new Vector2[lastTrace.Length];
		Array.Copy (lastTrace, 1, copy, 0, copy.Length - 1);
		copy [lastTrace.Length - 1] = lastPoint;
		lastTrace = copy;*/

		lastTrace.RemoveRange (0, lastTrace.Count - traceCant);

		/*lastTrace.Clear ();
		lastTrace.Add (pos);
		lastTrace.Add (pos);
		lastTrace.Add (pos);*/
	}
}
