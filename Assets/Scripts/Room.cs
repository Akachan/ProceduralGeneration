using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private RectInt _area;
    public RectInt Area { get {return _area;} }

    public Room(RectInt area)
    {
       this._area = area; 
    }

    public List<Hallway> CalculateAllPosibleDoorways(int widht, int lenght, int minDistanceFromEdge)
    {
        List<Hallway> hallwaysCandidates = new List<Hallway>();
        hallwaysCandidates.Add(new Hallway(new Vector2Int(0,0)));
        hallwaysCandidates.Add(new Hallway(new Vector2Int(widht,lenght)));
        return hallwaysCandidates;
    }
    

}
