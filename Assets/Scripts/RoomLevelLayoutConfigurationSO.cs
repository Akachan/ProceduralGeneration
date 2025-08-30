using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New LayoutConfig",
    menuName = "Custom/Procedural Generation/Room Level Layout Configuration")]
public class RoomLevelLayoutConfigurationSo : ScriptableObject
{
    [Header("Map")] 
    [SerializeField] private int width = 64;
    [SerializeField] private int lenght = 64;

    [Header("Room")] 
  
    
    [SerializeField] private RoomTemplate[] roomTemplates;
    [SerializeField] private int maxRoomCount = 10;
    
    [Header("Hallway")]
    [SerializeField] private int hallwayLengthMin = 2;
    [SerializeField] private int hallwayLengthMax = 5;
    [SerializeField] private int doorDistanceFromEdge = 1;
    [SerializeField] private int minRoomDistance = 1;
    
    public int Width => width;
    public int Lenght => lenght;

    
    public  RoomTemplate[] RoomTemplates => roomTemplates;
    public int MaxRoomCount => maxRoomCount;
    public int HallwayLengthMin => hallwayLengthMin;
    public int HallwayLengthMax => hallwayLengthMax;
    public int DoorDistanceFromEdge => doorDistanceFromEdge;
    public int MinRoomDistance => minRoomDistance;

    public Dictionary<RoomTemplate, int> GetAvailableRooms()
    {
        var availableRooms = new Dictionary<RoomTemplate, int>();
        for (int i = 0; i < roomTemplates.Length; i++)
        {
            availableRooms.Add(roomTemplates[i], roomTemplates[i].NumberOfRooms);
        }
        //Esta linea devuelve los Templates que uan le queden Rooms disponibles para tomar.
        availableRooms = availableRooms.Where(kvp => kvp.Value > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        
        
        return availableRooms;
    }


}