using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellAndRelativePosition 
{
    #region Attributs
    public Cell cell;           // Cellule.
    public Direction direction; // Direction à prendre lors de la suppression de mur.
    #endregion

    #region Propriétés
    #endregion

    #region Enums
    public enum Direction
    {
        North,
        South,
        East,
        West
    }
	#endregion
	
	#region Constructeur
    public CellAndRelativePosition (Cell cell, Direction direction)
    {
        this.cell = cell;
        this.direction = direction;
    }
	#endregion
}
