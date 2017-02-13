using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VisualCell))]
public class VisualCellEditor : Editor {

	VisualCell.WallState lastEastState;
	VisualCell.WallState lastWestState;
	VisualCell.WallState lastNorthState;
	VisualCell.WallState lastSouthState;

	public override void OnInspectorGUI(){
		VisualCell visualCell = (VisualCell)target;
		DrawDefaultInspector ();
		//visualCell.eastState = (VisualCell.WallState)EditorGUILayout.EnumPopup("EastState", visualCell.eastState);

		if (lastEastState != visualCell.eastState) {			
			lastEastState = visualCell.eastState;
			visualCell.OnEditorWallState (visualCell._East, visualCell.eastState);
		}

		if (lastWestState != visualCell.westState) {
			lastWestState = visualCell.westState;
			visualCell.OnEditorWallState (visualCell._West, visualCell.westState);
		}

		if (lastNorthState != visualCell.northState) {
			lastNorthState = visualCell.northState;
			visualCell.OnEditorWallState (visualCell._North, visualCell.northState);
		}

		if (lastSouthState != visualCell.southState) {
			lastSouthState = visualCell.southState;
			visualCell.OnEditorWallState (visualCell._South, visualCell.southState);
		}


	}
	
}
