public class Cell 
{
    #region Attributs
    public bool _West, _North, _East, _South;
    public bool _visited;

    public int xPos, yPos;
    #endregion

    #region Propriétés
    #endregion

    #region Enums
    #endregion

    #region Constructeur
    // Constructeur.
    public Cell (bool west, bool north, bool east, bool south, bool visited)
    {
        this._West = west;
        this._North = north;
        this._East = east;
        this._South = south;
        this._visited = visited;
    }
    #endregion

}
