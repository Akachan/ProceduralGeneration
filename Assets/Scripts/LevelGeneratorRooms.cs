using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class LevelGeneratorRooms : MonoBehaviour
{
    [SerializeField] private int width = 64;
    [SerializeField] private int lenght = 64;
    
    [SerializeField] private int roomWidthMin = 3;
    [SerializeField] private int roomWidthMax = 5;
    [SerializeField] private int roomLenghtMin = 3;
    [SerializeField] private int roomLenghtMax = 5;

    [SerializeField] private GameObject levelLayoutDisplay;
    [SerializeField] private List<Hallway> _openDoorways;
    
    private System.Random _random;
    private Level _level;
    
    [ContextMenu("Generate Level Layout")]
    public void GenerateLevel()
    {   
        _random = new System.Random();
        _openDoorways = new List<Hallway>();
        _level = new Level(width, lenght);
        var roomRect = GetStartRoomRect();
        Debug.Log(roomRect);
        Room room = new Room(roomRect);
        
        //Calcula las salidas de una room
        List<Hallway> hallways = room.CalculateAllPosibleDoorways(  room.Area.width,           
                                                                    room.Area.height, 
                                                                    1);
        hallways.ForEach(hallway => hallway.StartRoom = room);                      //Lambda Expression for Foreach
        hallways.ForEach(hallway => _openDoorways.Add(hallway));
        
        _level.AddRoom(room);
        
        Hallway selectedEntryway = _openDoorways[_random.Next(_openDoorways.Count)];
        Hallway selectedExit = SelectHallwayCandidate(new RectInt(0, 0, 5, 7), selectedEntryway);
        Debug.Log(selectedExit.StartPosition);
        Debug.Log(selectedExit.StartDirection);
        Vector2Int roomCandidatePosition = CalculateRoomPosition(selectedEntryway, 5,7,3, selectedExit.StartPosition);
        Room secondRoom = new Room(new RectInt(roomCandidatePosition.x, roomCandidatePosition.y, 5, 7));
        selectedEntryway.EndRoom = secondRoom;
        selectedEntryway.EndPosition = selectedExit.StartPosition;
        _level.AddRoom(secondRoom);
        _level.AddHallway(selectedEntryway);
        DrawLayout(selectedEntryway, roomRect);
        
        
        //testing
    
        //end Testing
        
        //DrawLayout(roomRect);
    }

    private RectInt GetStartRoomRect()
    {
        //datos previos: Se considera que no se puede construir en un margen de width/4 es decir que
        //              es construible en un ancho de width/2 (el centro)
        
        int roomWidth = _random.Next(roomWidthMin, roomWidthMax);       //calcula el ancho de la room
        
        int availableWidthX = width / 2 - roomWidth;                    //calcula el espacio en donde
                                                                        //se puede mover ese rectangulo
                                                                        
        int randomX = _random.Next(0, availableWidthX);                 //Tira un numero random
        
        int roomX = randomX + (width/4);                                //Calcula la posicion sumando al márgen la posicion
                                                                        //dentro del espacio disponible
        
        //Esto es lo mismo pero para la altura :) 
        int roomLenght = _random.Next(roomLenghtMin, roomLenghtMax);
        int availableLenghtY = lenght / 2 - roomLenght;
        int randomY = _random.Next(0, availableLenghtY);
        int roomY = randomY + (lenght / 4);

        return new RectInt(roomX, roomY, roomWidth, roomLenght);
    }

    private void DrawLayout(Hallway selectedEntry = null, RectInt roomCandidate = new RectInt())
    {
        var renderer = levelLayoutDisplay.GetComponent<Renderer>();                 //toma el renderer

        var layoutTexture = (Texture2D)renderer.sharedMaterial.mainTexture;         //toma la textura 2d

        layoutTexture.Reinitialize(width, lenght);                            //le da el tamaño a la textura
        levelLayoutDisplay.transform.localScale = new Vector3(width, lenght, 1f);   //le da tamaño del mapa
        layoutTexture.FillWithColor(Color.black);                                //lo pinta de negro
        
        Array.ForEach(_level.Rooms, room => layoutTexture.DrawRectangle(room.Area, Color.white));
        Array.ForEach(_level.Hallways, hallway => layoutTexture.DrawLine(hallway.StartPositionAbsolute, hallway.EndPositionAbsolute, Color.white));
        
        layoutTexture.DrawRectangle(roomCandidate, Color.blue);                  //Dibuja un rectangulo cyan

     
        foreach (var hallway in _openDoorways)
        {
            layoutTexture.SetPixel(hallway.StartPositionAbsolute.x, hallway.StartPositionAbsolute.y, hallway.StartDirection.GetColor());
        }
        if (selectedEntry != null)
        {
            layoutTexture.SetPixel(selectedEntry.StartPositionAbsolute.x, selectedEntry.StartPositionAbsolute.y, Color.red);
        }

        
        layoutTexture.SaveAsset();                                                //guarda el asset??
        
        
    }

    private Hallway SelectHallwayCandidate(RectInt roomCandidateRect, Hallway entryway)
    {
        Room room = new Room(roomCandidateRect);
        List<Hallway> candidates = room.CalculateAllPosibleDoorways(room.Area.width, room.Area.height, 1);
        HallwayDirection requiredDirection = entryway.StartDirection.GetOppositeDirection();  
        List <Hallway> filteredHallwayCandidates = candidates.Where(hallwayCandidate => hallwayCandidate.StartDirection == requiredDirection).ToList();
        return filteredHallwayCandidates.Count > 0 ? filteredHallwayCandidates[_random.Next(filteredHallwayCandidates.Count)] : null;
    }

    private Vector2Int CalculateRoomPosition(Hallway entryway, int roomWidth, int roomLenght, int distance, Vector2Int endPosition)
    {
        Vector2Int roomPosition = entryway.StartPositionAbsolute;
        switch (entryway.StartDirection)
        {
            case HallwayDirection.Left:
                roomPosition.x -= distance + roomWidth;
                roomPosition.y -= endPosition.y;
                break;
            case HallwayDirection.Top:
                roomPosition.x -= endPosition.x;
                roomPosition.y += distance + 1;
                break;
            case HallwayDirection.Right:
                roomPosition.x += distance + 1;
                roomPosition.y -= endPosition.y;
                break;
            case HallwayDirection.Bottom:
                roomPosition.x -= endPosition.x;
                roomPosition.y -= distance + roomLenght;
                break;
        }
        return roomPosition;

    }
}

