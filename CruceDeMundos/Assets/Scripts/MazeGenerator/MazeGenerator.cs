﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

public class MazeGenerator : MonoBehaviour 
{
    
	public Material wallIn,wallOut;

    public int _width, _height;
	public int blockWidth, blockHeight;
	float step = 3f;

    public VisualCell visualCellPrefab;

    public Cell[,] cells;

	public GameObject player;

    private Vector2 _randomCellPos; //Position de la cellule aleatoire qui va commencer la generation.
    private VisualCell visualCellInst;

    private List<CellAndRelativePosition> neighbors;

	private List<VisualCell> visualCells;
    
	public bool saveJson = true;
	public bool importJson = true;
	public string jsonName = "maze03";

	float[,] tilesmap;

	PathFind.Grid grid;

    void Start ()
    {
		step = visualCellPrefab.transform.localScale.x;
		cells = new Cell[_width, _height];
		visualCells = new List<VisualCell> ();
		if (!importJson) 			
			Init (); 
		else
			Import (jsonName);		

		Events.OnFalseTrail += OnFalseTrail;
		Events.GetVisistedTrail += GetVisistedTrail;
    }

	void OnDestroy(){
		Events.OnFalseTrail -= OnFalseTrail;
		Events.GetVisistedTrail -= GetVisistedTrail;
	}

    void Init ()
    {
        for(int i = 0; i < _width; i++)
        {

            for(int j = 0; j < _height; j++)
            {
				if (blockWidth>0 && blockHeight>0){
					if (i % (blockWidth * 2) < blockWidth && j % (blockHeight * 2) < blockHeight) {
						cells [i, j] = new Cell (true, true, true, true, true);
						cells [i, j].xPos = i;
						cells [i, j].yPos = j;
					}else {
						cells [i, j] = new Cell (false, false, false, false, false);
						cells [i, j].xPos = i;
						cells [i, j].yPos = j;
					}
				} else {
					cells [i, j] = new Cell (false, false, false, false, false);
					cells [i, j].xPos = i;
					cells [i, j].yPos = j;
				}
            }
        }
        RandomCell();

        InitVisualCell();
    }

    void RandomCell ()
    {
        //Recupere une position X et Y aleatoire
        _randomCellPos = new Vector2((int)UnityEngine.Random.Range(0, _width), (int)UnityEngine.Random.Range(0, _height));
		Cell currentCell = cells[(int)_randomCellPos.x, (int)_randomCellPos.y];
		if (currentCell._visited == false)
        //Lance la fonction GenerateMaze avec la positions X et Y aleatoire.
        GenerateMaze ((int)_randomCellPos.x, (int)_randomCellPos.y);
		else
			RandomCell ();
    }

    void GenerateMaze (int x, int y)
    {
        //	Debug.Log("Doing " + x + " " + y);
        Cell currentCell = cells[x, y]; //Definit la cellule courante
        neighbors = new List<CellAndRelativePosition>(); //Initialise la liste
        if(currentCell._visited == true) return;
        currentCell._visited = true;

        if(x + 1 < _width && cells[x + 1, y]._visited == false)
        { //Si on est pas a la largeur limite max du laby et que la cellule de droite n'est pas visiter alors on peut aller a droite
            neighbors.Add(new CellAndRelativePosition(cells[x + 1, y], CellAndRelativePosition.Direction.East)); //Ajoute la cellule voisine de droite dans la liste des voisins
        }

        if(y + 1 < _height && cells[x, y + 1]._visited == false)
        { //Si on est pas a la longueur limite du laby et que la cellule du bas n'est pas visiter alors on peut aller en bas
            neighbors.Add(new CellAndRelativePosition(cells[x, y + 1], CellAndRelativePosition.Direction.South)); //Ajoute la cellule voisine du bas dans liste des voisins
        }

        if(x - 1 >= 0 && cells[x - 1, y]._visited == false)
        { //Si on est pas a la largeur limite mini du laby et que la cellule de gauche n'est pas visiter alors on peut aller a gauche
            neighbors.Add(new CellAndRelativePosition(cells[x - 1, y], CellAndRelativePosition.Direction.West)); //Ajoute la cellule voisine de gauche dans la liste des voisins
        }

        if(y - 1 >= 0 && cells[x, y - 1]._visited == false)
        { //Si on est pas a la longueur limite mini du laby et que la cellule du haut n'est pas visiter alors on peut aller en haut
            neighbors.Add(new CellAndRelativePosition(cells[x, y - 1], CellAndRelativePosition.Direction.North)); //Ajoute la cellule voisine du haut dans la liste des voisins
        }

        if(neighbors.Count == 0) return;  // Si il y a 0 voisins dans la liste on sort de la méthode.

        neighbors.Shuffle(); // Melange la liste de voisins

        foreach(CellAndRelativePosition selectedcell in neighbors)
        {
            if(selectedcell.direction == CellAndRelativePosition.Direction.East)
            { // A droite
                if(selectedcell.cell._visited) continue;
                currentCell._East = true; //Detruit le mur de droite de la cellule courante
                selectedcell.cell._West = true; //Detruit le mur de gauche de la cellule voisine choisie
                GenerateMaze(x + 1, y); //Relance la fonction avec la position de la cellule voisine
            }

            else if(selectedcell.direction == CellAndRelativePosition.Direction.South)
            { // En bas
                if(selectedcell.cell._visited) continue;
                currentCell._South = true; //Detruit le mur du bas de la cellule courante
                selectedcell.cell._North = true; //Detruit le mur du haut de la cellule voisine choisie
                GenerateMaze(x, y + 1); //Relance la fonction avec la position de la cellule voisine
            }
            else if(selectedcell.direction == CellAndRelativePosition.Direction.West)
            { // A gauche
                if(selectedcell.cell._visited) continue;
                currentCell._West = true; //Detruit le mur de gauche de la cellule courante
                selectedcell.cell._East = true; //Detruit le mur de droite de la cellule voisine choisie
                GenerateMaze(x - 1, y); //Relance la fonction avec la position de la cellule voisine
            }
            else if(selectedcell.direction == CellAndRelativePosition.Direction.North)
            { // En haut
                if(selectedcell.cell._visited) continue;
                currentCell._North = true; //Detruit le mur du haut de la cellule courante
                selectedcell.cell._South = true; //Detruit le mur du bas de la cellule voisine choisie
                GenerateMaze(x, y - 1); //Relance la fonction avec la position de la cellule voisine
            }
        }


    }

	void Import(string file){

		string filePath = "Maze/" + file.Replace(".json", "");
		TextAsset text = Resources.Load<TextAsset>(filePath);

		//Debug.Log (text.text);
		var N = JSON.Parse(text.text);

		string nnode = N ["Maze"] [N ["Maze"].Count - 1] ["id"];
		string[] ss = nnode.Split ('_');
		int size_x = int.Parse (ss [0]) + 1;
		int size_y = int.Parse (ss [1]) + 1;
		if (Game.Instance != null) {
			int gridScale = Game.Instance.pathfinder2.gridScale;
			tilesmap = new float[size_x * gridScale, size_y * gridScale];
			//SetTileMap (0, 0, size_x * gridScale, size_y * gridScale, true,false);
			SetAllTileMap (0, 0, size_x * gridScale, size_y * gridScale, gridScale);
		}


		//Debug.Log (N ["Maze"][0]["east"].AsBool);
		_height = N ["height"].AsInt;
		for (int i = 0; i < N ["Maze"].Count; i++) {
			string nodeName = N ["Maze"] [i] ["id"];
			string[] s = nodeName.Split ('_');
			int xPos = int.Parse (s [0]);
			int yPos = int.Parse (s [1]);
			//print (nodeName);

			visualCellInst = Instantiate (visualCellPrefab, new Vector3 (xPos * step, N ["height"].AsInt * 1f - yPos * step, 0f), Quaternion.identity) as VisualCell;
			visualCellInst.transform.parent = transform;
			if (N ["Maze"] [i] ["north"] + "" == "FIRE") {
				visualCellInst._North.gameObject.SetActive (true);
				visualCellInst._North.Find ("fire").gameObject.SetActive (true);
				visualCellInst.northState = VisualCell.WallState.FIRE;
			} else if (N ["Maze"] [i] ["north"] + "" == "PORTAL") {
				visualCellInst._North.gameObject.SetActive (true);
				visualCellInst._North.Find ("portal").gameObject.SetActive (true);
				visualCellInst.northState = VisualCell.WallState.PORTAL;
			} else if (N ["Maze"] [i] ["north"] + "" == "IN") { 
				player.transform.position = visualCellInst.transform.position;
				visualCellInst._North.Find("tronWall").gameObject.SetActive(false);

				GameObject exit = visualCellInst.transform.Find ("exit").gameObject;
				exit.SetActive (true);
				exit.transform.Find ("South").gameObject.SetActive (true);

				visualCellInst.isFirst = true;
				visualCellInst.northState = VisualCell.WallState.IN;
			} else if (N ["Maze"] [i] ["north"] + "" == "OUT") { 				
				/*Renderer r = visualCellInst._North.GetComponent<Renderer> ();
				r.material = wallOut;*/
				visualCellInst._North.Find("tronWall").gameObject.SetActive(false);
				GameObject exit = visualCellInst.transform.Find ("exit").gameObject;
				exit.SetActive (true);
				exit.transform.Find ("North").gameObject.SetActive (true);
				visualCellInst._North.gameObject.AddComponent<OnObjectiveCollider> ();
				visualCellInst.northState = VisualCell.WallState.OUT;
			} else if (N ["Maze"] [i] ["north"] + "" == "SOLID") {		
				visualCellInst._North.gameObject.SetActive (true);
				visualCellInst.northState = VisualCell.WallState.SOLID;
				SetTileMap (xPos * 10, yPos * 10, 10, 2, 0,false);
			} else if (N ["Maze"] [i] ["north"] + "" == "OBSTACLE") {		
				visualCellInst._North.gameObject.SetActive (true);
				visualCellInst.northState = VisualCell.WallState.OBSTACLE;
				visualCellInst.SetObstacle (visualCellInst._North);
				SetTileMap (xPos * 10, yPos * 10, 10, 2, 0,false);
			} else if (N ["Maze"] [i] ["north"] + "" == "INVISIBLE") { 	
				AddPathPoint (visualCellInst._North);
				visualCellInst._North.gameObject.SetActive (false);
				visualCellInst.northState = VisualCell.WallState.INVISIBLE;
			} else {
				visualCellInst._North.gameObject.SetActive (!N ["Maze"] [i] ["north"].AsBool);
				visualCellInst.northState = N ["Maze"] [i] ["north"].AsBool ? VisualCell.WallState.INVISIBLE : VisualCell.WallState.SOLID;
				if(visualCellInst.northState==VisualCell.WallState.SOLID)
					SetTileMap (xPos * 10, yPos * 10, 10, 2, 0,false);
			}

			if (N ["Maze"] [i] ["south"] + "" == "FIRE") {
				visualCellInst._South.gameObject.SetActive (true);
				visualCellInst._South.Find ("fire").gameObject.SetActive (true);
				visualCellInst.southState = VisualCell.WallState.FIRE;
			} else if (N ["Maze"] [i] ["south"] + "" == "PORTAL") {
				visualCellInst._South.gameObject.SetActive (true);
				visualCellInst._South.Find ("portal").gameObject.SetActive (true);
				visualCellInst.southState = VisualCell.WallState.PORTAL;
			} else if (N ["Maze"] [i] ["south"] + "" == "IN") { 
				player.transform.position = visualCellInst.transform.position;
				/*Renderer r = visualCellInst._South.GetComponent<Renderer> ();
				r.material = wallIn;*/
				visualCellInst._South.Find ("tronWall").gameObject.SetActive (false);
				GameObject exit = visualCellInst.transform.Find ("exit").gameObject;
				exit.SetActive (true);
				exit.transform.Find ("North").gameObject.SetActive (true);

				visualCellInst.isFirst = true;
				visualCellInst.southState = VisualCell.WallState.IN;
			} else if (N ["Maze"] [i] ["south"] + "" == "OUT") { 				
				/*Renderer r = visualCellInst._South.GetComponent<Renderer> ();
				r.material = wallOut;*/
				visualCellInst._South.Find ("tronWall").gameObject.SetActive (false);
				GameObject exit = visualCellInst.transform.Find ("exit").gameObject;
				exit.SetActive (true);
				exit.transform.Find ("South").gameObject.SetActive (true);

				visualCellInst._South.gameObject.AddComponent<OnObjectiveCollider> ();
				visualCellInst.southState = VisualCell.WallState.OUT;
			} else if (N ["Maze"] [i] ["south"] + "" == "SOLID") { 				
				visualCellInst._South.gameObject.SetActive (true);
				visualCellInst.southState = VisualCell.WallState.SOLID;
				SetTileMap (xPos * 10, (yPos * 10) + 8, 10, 2, 0, false);
			} else if (N ["Maze"] [i] ["south"] + "" == "OBSTACLE") {		
				visualCellInst._South.gameObject.SetActive (true);
				visualCellInst.southState = VisualCell.WallState.OBSTACLE;
				visualCellInst.SetObstacle (visualCellInst._South);
				SetTileMap (xPos * 10, (yPos * 10) + 8, 10, 2, 0, false);
			} else if (N ["Maze"] [i] ["south"] + "" == "INVISIBLE") { 	
				AddPathPoint (visualCellInst._South);
				visualCellInst._South.gameObject.SetActive (false);
				visualCellInst.southState = VisualCell.WallState.INVISIBLE;
			} else {
				visualCellInst._South.gameObject.SetActive (!N ["Maze"] [i] ["south"].AsBool);
				visualCellInst.southState = N ["Maze"] [i] ["south"].AsBool ? VisualCell.WallState.INVISIBLE : VisualCell.WallState.SOLID;
				if(visualCellInst.southState==VisualCell.WallState.SOLID)
					SetTileMap (xPos * 10, (yPos * 10)+8, 10, 2, 0,false);
			}
			
			if (N ["Maze"] [i] ["east"] + "" == "FIRE") {				
				visualCellInst._East.gameObject.SetActive (true);
				visualCellInst._East.Find ("fire").gameObject.SetActive (true);
				visualCellInst.eastState = VisualCell.WallState.FIRE;
			} else if (N ["Maze"] [i] ["east"] + "" == "PORTAL") {				
				visualCellInst._East.gameObject.SetActive (true);
				visualCellInst._East.Find ("portal").gameObject.SetActive (true);
				visualCellInst.eastState = VisualCell.WallState.PORTAL;
			}else if (N ["Maze"] [i] ["east"] + "" == "IN") { 
				player.transform.position = visualCellInst.transform.position;
				/*Renderer r = visualCellInst._East.GetComponent<Renderer> ();
				r.material = wallIn;*/
				visualCellInst._East.Find("tronWall").gameObject.SetActive(false);
				GameObject exit = visualCellInst.transform.Find ("exit").gameObject;
				exit.SetActive (true);
				exit.transform.Find ("West").gameObject.SetActive (true);

				visualCellInst.isFirst = true;
				visualCellInst.eastState = VisualCell.WallState.IN;
			} else if (N ["Maze"] [i] ["east"] + "" == "OUT") { 				
				//Renderer r = visualCellInst._East.GetComponent<Renderer> ();
				//r.material = wallOut;
				visualCellInst._East.Find("tronWall").gameObject.SetActive(false);
				GameObject exit = visualCellInst.transform.Find ("exit").gameObject;
				exit.SetActive (true);
				exit.transform.Find ("East").gameObject.SetActive (true);

				visualCellInst._East.gameObject.AddComponent<OnObjectiveCollider> ();
				visualCellInst.eastState = VisualCell.WallState.OUT;
			} else if (N ["Maze"] [i] ["east"] + "" == "SOLID") { 				
				visualCellInst._East.gameObject.SetActive (true);
				visualCellInst.eastState = VisualCell.WallState.SOLID;
				SetTileMap ((xPos * 10)+8, yPos * 10, 2, 10, 0,false);
			} else if (N ["Maze"] [i] ["east"] + "" == "OBSTACLE") {		
				visualCellInst._East.gameObject.SetActive (true);
				visualCellInst.eastState = VisualCell.WallState.OBSTACLE;
				visualCellInst.SetObstacle (visualCellInst._East);
				SetTileMap ((xPos * 10)+8, yPos * 10, 2, 10, 0,false);
			} else if (N ["Maze"] [i] ["east"] + "" == "INVISIBLE") { 
				AddPathPoint (visualCellInst._East);
				visualCellInst._East.gameObject.SetActive (false);
				visualCellInst.eastState = VisualCell.WallState.INVISIBLE;
			} else {				
				visualCellInst._East.gameObject.SetActive (!N ["Maze"] [i] ["east"].AsBool);
				visualCellInst.eastState = N ["Maze"] [i] ["east"].AsBool ? VisualCell.WallState.INVISIBLE : VisualCell.WallState.SOLID;
				if(visualCellInst.eastState==VisualCell.WallState.SOLID)
					SetTileMap ((xPos * 10)+8, yPos * 10, 2, 10, 0,false);
			}

			if (N ["Maze"] [i] ["west"] + "" == "FIRE") {
				visualCellInst._West.gameObject.SetActive (true);
				visualCellInst._West.Find ("fire").gameObject.SetActive (true);
				visualCellInst.westState = VisualCell.WallState.FIRE;
			} else if (N ["Maze"] [i] ["west"] + "" == "PORTAL") {
				visualCellInst._West.gameObject.SetActive (true);
				visualCellInst._West.Find ("portal").gameObject.SetActive (true);
				visualCellInst.westState = VisualCell.WallState.PORTAL;
			}else if (N ["Maze"] [i] ["west"] + "" == "IN") { 
				player.transform.position = visualCellInst.transform.position;
				//Renderer r = visualCellInst._West.GetComponent<Renderer> ();
				//r.material = wallIn;
				visualCellInst._West.Find("tronWall").gameObject.SetActive(false);
				GameObject exit = visualCellInst.transform.Find ("exit").gameObject;
				exit.SetActive (true);
				exit.transform.Find ("East").gameObject.SetActive (true);

				visualCellInst.isFirst = true;
				visualCellInst.westState = VisualCell.WallState.IN;
			}else if (N ["Maze"] [i] ["west"] + "" == "OUT") { 				
				/*Renderer r = visualCellInst._West.GetComponent<Renderer> ();
				r.material = wallOut;*/
				visualCellInst._West.Find("tronWall").Find("tronWall").gameObject.SetActive(false);
				GameObject exit = visualCellInst.transform.Find ("exit").gameObject;
				exit.SetActive (true);
				exit.transform.Find ("West").gameObject.SetActive (true);

				visualCellInst._West.gameObject.AddComponent<OnObjectiveCollider> ();
				visualCellInst.westState = VisualCell.WallState.OUT;
			} else if (N ["Maze"] [i] ["west"] + "" == "SOLID") { 				
				visualCellInst._West.gameObject.SetActive (true);
				visualCellInst.westState = VisualCell.WallState.SOLID;
				SetTileMap (xPos * 10, yPos * 10, 2, 10, 0,false);
			} else if (N ["Maze"] [i] ["west"] + "" == "OBSTACLE") {		
				visualCellInst._West.gameObject.SetActive (true);
				visualCellInst.westState = VisualCell.WallState.OBSTACLE;
				visualCellInst.SetObstacle (visualCellInst._West);
				SetTileMap (xPos * 10, yPos * 10, 2, 10, 0,false);
			} else if (N ["Maze"] [i] ["west"] + "" == "INVISIBLE") { 
				AddPathPoint (visualCellInst._West);
				visualCellInst._West.gameObject.SetActive (false);
				visualCellInst.westState = VisualCell.WallState.INVISIBLE;
			} else {
				visualCellInst._West.gameObject.SetActive (!N ["Maze"] [i] ["west"].AsBool);
				visualCellInst.westState = N ["Maze"] [i] ["west"].AsBool ? VisualCell.WallState.INVISIBLE : VisualCell.WallState.SOLID;
				if(visualCellInst.westState==VisualCell.WallState.SOLID)
					SetTileMap (xPos * 10, yPos * 10, 2, 10, 0,false);
			}

			/*if (N ["Maze"] [i] ["wallIn"] != null) {
				//Debug.Log (player.transform.position+" -  "+visualCellInst.transform.position);
				player.transform.position = visualCellInst.transform.position;
				Renderer r = null;
				if ((N ["Maze"] [i] ["wallIn"]as string).Equals("north"))
					r = visualCellInst._North.GetComponent<Renderer> ();				
				else if ((N ["Maze"] [i] ["wallIn"]as string).Equals("east"))					
					r = visualCellInst._East.GetComponent<Renderer> ();
				else if ((N ["Maze"] [i] ["wallIn"]as string).Equals("south"))
					r = visualCellInst._South.GetComponent<Renderer> ();
				else if ((N ["Maze"] [i] ["wallIn"]as string).Equals("west"))
					r = visualCellInst._West.GetComponent<Renderer> ();

				r.material = wallIn;
				visualCellInst.isFirst = true;
			}*/

			/*if (N ["Maze"] [i] ["wallOut"] != null) {
				Renderer r = null;
				if ((N ["Maze"] [i] ["wallOut"]as string).Equals ("north")) {
					r = visualCellInst._North.GetComponent<Renderer> ();
					visualCellInst._North.gameObject.AddComponent<OnObjectiveCollider> ();
				} else if ((N ["Maze"] [i] ["wallOut"]as string).Equals ("east")) {
					r = visualCellInst._East.GetComponent<Renderer> ();
					visualCellInst._East.gameObject.AddComponent<OnObjectiveCollider> ();
				} else if ((N ["Maze"] [i] ["wallOut"]as string).Equals ("south")) {
					r = visualCellInst._South.GetComponent<Renderer> ();
					visualCellInst._South.gameObject.AddComponent<OnObjectiveCollider> ();
				} else if ((N ["Maze"] [i] ["wallOut"]as string).Equals ("west")) {
					r = visualCellInst._West.GetComponent<Renderer> ();
					visualCellInst._West.gameObject.AddComponent<OnObjectiveCollider> ();
				}

				r.material = wallOut;
			}*/

			visualCellInst.deadEnd = N ["Maze"] [i] ["deadEnd"].AsBool;
			visualCellInst.transform.name = N ["Maze"] [i] ["id"];
			visualCells.Add (visualCellInst);
		}
	if(Game.Instance!=null)Game.Instance.traceManager.freeTrail = true;

		if (Game.Instance != null) {
			int gridScale = Game.Instance.pathfinder2.gridScale;
			//ShowTileMap (size_x * gridScale, size_y * gridScale);
			Game.Instance.pathfinder2.grid = new PathFind.Grid (size_x * gridScale, size_y * gridScale, tilesmap);
			Game.Instance.pathfinder2.mazeOffsetY = _height;
			Game.Instance.pathfinder2.SetMazeScale (step);
		}
	}

	void SetTileMap(int from_x, int from_y, int size_x, int size_y, float val, bool debug){
	/*if (from_x > 2)from_x-=3;
	if (from_y > 2)from_y-=3;
	if (from_x + size_x < tilesmap.GetLength (0) - 3)size_x+=3;
	if (from_y + size_y < tilesmap.GetLength (1) - 3)size_y+=3;*/
		if (Game.Instance != null) {
			for (int i = from_x; i < from_x + size_x; i++) {		
				for (int j = from_y; j < from_y + size_y; j++) {
					tilesmap [i, j] = val;
					if (debug)
						print (i + "," + j + ":" + val);
				}
			}
		}
	}

	void SetAllTileMap(int from_x, int from_y, int size_x, int size_y, int squareSize){
		float halfSZ = 0.5f*squareSize;
		float invHalf = 1 / halfSZ;
		for (int i = from_x; i < from_x + size_x; i++) {		
			for (int j = from_y; j < from_y + size_y; j++) {			
			tilesmap [i, j] = 0.001f+new Vector2 (((i % squareSize) - halfSZ)*invHalf, ((j % squareSize) - halfSZ)*invHalf).magnitude;
			tilesmap [i, j]=tilesmap [i, j]*tilesmap [i, j]*tilesmap [i, j]*tilesmap [i, j];
			}
		}
	}

	void ShowTileMap(int size_x, int size_y){

		Texture2D tex = new Texture2D(size_x, size_y, TextureFormat.RGB24, false);
		
		for(int i = 0; i < size_x; i++){		
			for(int j = 0; j < size_y; j++){
				
				/*if (tilesmap [i, j]) {
					tex.SetPixel (i, j, Color.white);
				} else {
					tex.SetPixel (i, j, Color.black);
				}*/

				float v = tilesmap [i, j];
				if (v > 0f)
					tex.SetPixel (i, j, new Color (1f - v, 0, 0));
				else
				tex.SetPixel (i, j, Color.black);
			}
		}

		tex.Apply ();

		byte[] bytes = tex.EncodeToJPG ();

		System.IO.File.WriteAllBytes ("C:\\Documentos\\Unity\\Proyectos\\navMap.jpg",bytes);


		/*using (FileStream fs = new FileStream ("Assets/Resources/Maze/test.txt", FileMode.Create)) {
			using (StreamWriter writer = new StreamWriter (fs)) {
				writer.Write (x);
			}
		}*/

	}


    void InitVisualCell ()
    {
		string json = "{Maze:[";
        // Initialise mes cellules visuel et detruit les murs en fonction des cellules virtuel
		int index=0;
        foreach(Cell cell in cells)
        {

			visualCellInst = Instantiate(visualCellPrefab, new Vector3(cell.xPos * step, _height * 1f - cell.yPos * step, 0f), Quaternion.identity) as VisualCell;
            visualCellInst.transform.parent = transform;
            visualCellInst._North.gameObject.SetActive(!cell._North);
            visualCellInst._South.gameObject.SetActive(!cell._South);
            visualCellInst._East.gameObject.SetActive(!cell._East);
            visualCellInst._West.gameObject.SetActive(!cell._West);

			visualCellInst.northState = cell._North ? VisualCell.WallState.INVISIBLE : VisualCell.WallState.SOLID;
			visualCellInst.southState = cell._South ? VisualCell.WallState.INVISIBLE : VisualCell.WallState.SOLID;
			visualCellInst.eastState = cell._East ? VisualCell.WallState.INVISIBLE : VisualCell.WallState.SOLID;
			visualCellInst.westState = cell._West ? VisualCell.WallState.INVISIBLE : VisualCell.WallState.SOLID;

            visualCellInst.transform.name = cell.xPos.ToString() + "_" + cell.yPos.ToString();
			visualCells.Add (visualCellInst);

			//Debug.Log (cell.xPos.ToString () + "_" + cell.zPos.ToString ()+": Norte: "+cell._North+": Este: "+cell._Est+": Sur: "+cell._South+": Oeste: "+cell._West);

			json += "\n{id:"+ cell.xPos.ToString () + "_" + cell.yPos.ToString () + ",";
			json += "north:" + cell._North+",";
			json += "east:" + cell._East+",";
			json += "south:" + cell._South+",";
			json += "west:" + cell._West+"}";

			if(index<cells.Length-1)
				json += ",";

			index++;
        }
			json += "]}";

				//Debug.Log(json);


		if(saveJson)
			using (FileStream fs = new FileStream("Assets/Resources/Maze/"+jsonName+".json", FileMode.Create)){
			using (StreamWriter writer = new StreamWriter(fs)){
				writer.Write(json);
			}
		}
		#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh ();
		#endif

    }

	public void SaveJson(){
	string json = "{\"height\":"+_height+",\n\"Maze\":[";
		// Initialise mes cellules visuel et detruit les murs en fonction des cellules virtuel
		int index=0;
		foreach(VisualCell visualCellInst in visualCells)
		{
			//Debug.Log (visualCellInst.name);

			json += "\n{\"id\":\""+ visualCellInst.transform.name + "\",";
			json += "\"north\":\"" + visualCellInst.northState.ToString() +"\",";
			json += "\"east\":\"" + visualCellInst.eastState.ToString()+"\",";
			json += "\"south\":\"" + visualCellInst.southState.ToString()+"\",";
			json += "\"west\":\"" + visualCellInst.westState.ToString()+"\",";
			json += "\"deadEnd\":\"" + visualCellInst.deadEnd+"\"}";
			
			if(index<visualCells.Count-1)
				json += ",";

			index++;
		}
		json += "]}";

		Debug.Log(json);


		using (FileStream fs = new FileStream("Assets/Resources/Maze/"+jsonName+".json", FileMode.Create)){
			using (StreamWriter writer = new StreamWriter(fs)){
				writer.Write(json);
			}
		}
		#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh ();
		#endif
	}
 
	public void RandomizeMaze(){		
		foreach (VisualCell vc in visualCells)
			Destroy (vc.gameObject);
		visualCells.Clear ();
		cells = new Cell[_width, _height];
		Init ();
	}

	void AddPathPoint(Transform t){
		//Instantiate(point,t.transform.position,Quaternion.Euler(Vector3.zero));
		//if(Game.Instance!=null)Game.Instance.pathfinder.path.Add(new Vector2(t.transform.position.x,t.transform.position.y));
	}

	void OnFalseTrail(VisualCell cellCheck, VisualCell cellStop){
		List<VisualCell>vcells = visualCells.FindAll(x => x.cameFrom == cellCheck);
		foreach (VisualCell vc in vcells) {
			if (vc != null && vc != cellStop) {
				if (vc.visited) {
					vc.SetVisited (false);
					OnFalseTrail (vc, cellStop);
				}
			}
		}
	}

	void GetVisistedTrail(){
		List<VisualCell>vcells = visualCells.FindAll(x => x.visited == true);
		string result = "";
		foreach (VisualCell vc in vcells)
			result += vc.name+";";

		List<VisualCell>vcells2 = visualCells.FindAll(x => x.deadEnd == true);
		string result2 = "";
		foreach (VisualCell vc in vcells2)
		result2 += "{\"id\":\""+vc.name+"\",\"times\":\""+vc.visitTimes+"\"};";

		Game.Instance.levelMetrics.trail = result;
		Game.Instance.levelMetrics.deadEnds = result2;
		
	}
	
}
