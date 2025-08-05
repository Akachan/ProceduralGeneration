using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private RectInt _area;
    public RectInt Area  => _area;

    public Room(RectInt area)
    {
       this._area = area; 
    }

    public List<Hallway> CalculateAllPosibleDoorways(int widht, int lenght, int minDistanceFromEdge)
    {
        List<Hallway> hallwaysCandidates = new List<Hallway>();
        
        int top = lenght - 1;
        int minX = minDistanceFromEdge;
        int maxX = widht - minDistanceFromEdge;

        for (int x = minX; x < maxX; x++)
        {
            hallwaysCandidates.Add(new Hallway(HallwayDirection.Bottom, new Vector2Int(x, 0)));
            hallwaysCandidates.Add(new Hallway(HallwayDirection.Top, new Vector2Int(x, top)));
        }
        
        int right = widht - 1;
        int minY = minDistanceFromEdge;
        int maxY = lenght - minDistanceFromEdge;

        for (int y = minY; y < maxY; y++)
        {
            hallwaysCandidates.Add(new Hallway(HallwayDirection.Left, new Vector2Int(0, y)));
            hallwaysCandidates.Add(new Hallway(HallwayDirection.Right, new Vector2Int(right, y)));
        }
        
  
        return hallwaysCandidates;
    }
    

}
