using UnityEngine;

[CreateAssetMenu(fileName = "New LayoutConfig",
    menuName = "Custom/Procedural Generation/Room Level Layout Configuration")]
public class RoomLevelLayoutConfigurationSo : ScriptableObject
{
    [Header("Map")] 
    [SerializeField] private int width = 64;
    [SerializeField] private int lenght = 64;

    [Header("Room")] 
    [SerializeField] private int roomWidthMin = 3;
    [SerializeField] private int roomWidthMax = 5;
    [SerializeField] private int roomLenghtMin = 3;
    [SerializeField] private int roomLenghtMax = 5;
    [SerializeField] private int maxRoomCount = 10;
    
    [Header("Hallway")]
    [SerializeField] private int hallwayLengthMin = 2;
    [SerializeField] private int hallwayLengthMax = 5;
    [SerializeField] private int doorDistanceFromEdge = 1;
    [SerializeField] private int minRoomDistance = 1;
    
    public int Width => width;
    public int Lenght => lenght;
    public int RoomWidthMin => roomWidthMin;
    public int RoomWidthMax => roomWidthMax;
    public int RoomLenghtMin => roomLenghtMin;
    public int RoomLenghtMax => roomLenghtMax;
    public int MaxRoomCount => maxRoomCount;
    public int HallwayLengthMin => hallwayLengthMin;
    public int HallwayLengthMax => hallwayLengthMax;
    public int DoorDistanceFromEdge => doorDistanceFromEdge;
    public int MinRoomDistance => minRoomDistance;



}