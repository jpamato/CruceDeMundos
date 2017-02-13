using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

public class MazeGenerator : MonoBehaviour 
{
    
	public Material wallIn,wallOut;

    public int _width, _height;
	float step = 3f;

    public VisualCell visualCellPrefab;

    public Cell[,] cells;

	public GameObject player;

    private Vector2 _randomCellPos; //Position de la cellule aleatoire qui va commencer la generation.
    private VisualCell visualCellInst;

    private List<CellAndRelativePosition> neighbors;

	private List<VisualCell> visualCells;
    
	public bool saveJson = true;
	public string jsonName = "maze03";

    void Start ()
    {
		step = visualCellPrefab.transform.localScale.x;
		cells = new Cell[_width, _height];
		visualCells = new List<VisualCell> ();
		if(saveJson)
			Init(); 
		else
			Import (jsonName);        
    }

    void Init ()
    {
        for(int i = 0; i < _width; i++)
        {

            for(int j = 0; j < _height; j++)
            {

                cells[i, j] = new Cell(false, false, false, false, false);
                cells[i, j].xPos = i;
                cells[i, j].yPos = j;
            }
        }
        RandomCell();

        InitVisualCell();
    }

    void RandomCell ()
    {
        //Recupere une position X et Y aleatoire
        _randomCellPos = new Vector2((int)UnityEngine.Random.Range(0, _width), (int)UnityEngine.Random.Range(0, _height));

        //Lance la fonction GenerateMaze avec la positions X et Y aleatoire.
        GenerateMaze((int)_randomCellPos.x, (int)_randomCellPos.y); 
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
		//Debug.Log (N ["Maze"][0]["east"].AsBool);

		for (int i = 0; i < N ["Maze"].Count; i++) {
			string[] s = (N ["Maze"] [i] ["id"]as string).Split ('_');
			int xPos = int.Parse (s [0]);
			int yPos = int.Parse (s [1]);

			visualCellInst = Instantiate(visualCellPrefab, new Vector3(xPos * step, _height * 1f - yPos * step, 0f), Quaternion.identity) as VisualCell;
			visualCellInst.transform.parent = transform;
			if (N ["Maze"] [i] ["north"]+""== ("fire")) {
				visualCellInst._North.gameObject.SetActive (true);
				visualCellInst._North.FindChild ("fire").gameObject.SetActive (true);
			}else if (N ["Maze"] [i] ["north"]+""== ("portal")) {
				visualCellInst._North.gameObject.SetActive (true);
				visualCellInst._North.FindChild ("portal").gameObject.SetActive (true);
			} else {
				visualCellInst._North.gameObject.SetActive (!N ["Maze"] [i] ["north"].AsBool);
			}
			if (N ["Maze"] [i] ["south"]+""== ("fire")) {
				visualCellInst._South.gameObject.SetActive (true);
				visualCellInst._South.FindChild ("fire").gameObject.SetActive (true);
			}else if (N ["Maze"] [i] ["south"]+""== ("portal")) {
				visualCellInst._South.gameObject.SetActive (true);
				visualCellInst._South.FindChild ("portal").gameObject.SetActive (true);
			} else {
				visualCellInst._South.gameObject.SetActive (!N ["Maze"] [i] ["south"].AsBool);
			}
			
			if (N ["Maze"] [i] ["east"]+""=="fire") {				
				visualCellInst._East.gameObject.SetActive (true);
				visualCellInst._East.FindChild ("fire").gameObject.SetActive (true);
			} else if (N ["Maze"] [i] ["east"]+""=="portal") {				
				visualCellInst._East.gameObject.SetActive (true);
				visualCellInst._East.FindChild ("portal").gameObject.SetActive (true);
			}else {				
				visualCellInst._East.gameObject.SetActive (!N ["Maze"] [i] ["east"].AsBool);
			}

			if (N ["Maze"] [i] ["west"]+""== ("fire")) {
				visualCellInst._West.gameObject.SetActive (true);
				visualCellInst._West.FindChild ("fire").gameObject.SetActive (true);
			} else if (N ["Maze"] [i] ["west"]+""== ("portal")) {
				visualCellInst._West.gameObject.SetActive (true);
				visualCellInst._West.FindChild ("portal").gameObject.SetActive (true);
			} else {
				visualCellInst._West.gameObject.SetActive (!N ["Maze"] [i] ["west"].AsBool);
			}

			if (N ["Maze"] [i] ["wallIn"] != null) {
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
			}

			if (N ["Maze"] [i] ["wallOut"] != null) {
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
			}

			visualCellInst.transform.name = N ["Maze"] [i] ["id"];
		}
		Data.Instance.freeTrail = true;
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
		string json = "{Maze:[";
		// Initialise mes cellules visuel et detruit les murs en fonction des cellules virtuel
		int index=0;
		foreach(VisualCell visualCellInst in gameObject.GetComponentsInChildren<VisualCell>())
		{
			//Debug.Log (cell.xPos.ToString () + "_" + cell.zPos.ToString ()+": Norte: "+cell._North+": Este: "+cell._Est+": Sur: "+cell._South+": Oeste: "+cell._West);

			json += "\n{id:"+ visualCellInst.transform.name + ",";
			/*json += "north:" + cell._North+",";
			json += "east:" + cell._East+",";
			json += "south:" + cell._South+",";
			json += "west:" + cell._West+"}";*/

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
	}
 
	public void RandomizeMaze(){
		if (saveJson) {
			foreach (VisualCell vc in visualCells)
				Destroy (vc.gameObject);
			visualCells.Clear ();
			cells = new Cell[_width, _height];
			Init ();
		}
	}
}
