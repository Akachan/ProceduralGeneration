using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class LevelGeneratorRooms : MonoBehaviour
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

        AddRoom();
        
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
        
        //Calcula la posición de la nueva room en base al largo del pasillo, la posicion absoluta de la entrada (al pasillo)
        //y la posición relativa de la salida (del pasillo) hacia la nueva room.
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

    private Room ConstructAdjacentRoom(Hallway selectedEntryway)
    {
        //inicializamos un rectangulo con ancho y alto random
        RectInt roomCandidateRect = new RectInt                     
        {
            width = _random.Next(roomWidthMin, roomWidthMax),
            height = _random.Next(roomLenghtMin, roomLenghtMax)
        };
        
        //elegimos un pasillo random (del nuevo rectangulo) que esté del lado donde queremos
        Hallway selectedExit = SelectHallwayCandidate(roomCandidateRect, selectedEntryway);
        if (selectedExit == null) { return null; }
        
        Debug.Log(selectedExit.StartPosition);
        Debug.Log(selectedExit.StartDirection);
        
        //Calculamos la posición de la nueva room, en base al pasillo
        int distance = _random.Next(hallwayLengthMin, hallwayLengthMax);
        Vector2Int roomCandidatePosition = CalculateRoomPosition(selectedEntryway,
                                                                roomCandidateRect.width, 
                                                                roomCandidateRect.height,
                                                                distance,
                                                                selectedExit.StartPosition);
        
        //posicionamos el rectangulo en su pos final
        roomCandidateRect.position = roomCandidatePosition;

        if (!IsRoomCandidateValid(roomCandidateRect))
        {
            return null;
        }
        
        //creamos la room, con ese rectangulo
        Room newRoom = new Room(roomCandidateRect);
        
        //Al pasillo le definimos cual es su salida y a que room te lleva
        selectedEntryway.EndRoom = newRoom;
        selectedEntryway.EndPosition = selectedExit.StartPosition;
        
        return newRoom;
    }

    private void AddRoom()
    {
        while (_openDoorways.Count > 0 && _level.Rooms.Length < maxRoomCount)
        {
            //Selecciona  una entrada random de todas las entradas disponibles
            Hallway selectedEntryway = _openDoorways[_random.Next(0, _openDoorways.Count)];

            //creo una room que salga por esa entrada
            Room newRoom = ConstructAdjacentRoom(selectedEntryway);

            //si no se puede creear una room en ese hallway, retira esa entrada del pool de objetos para que
            //no se vuelva a seleccionar
            if (newRoom == null)
            {
                _openDoorways.Remove(selectedEntryway);
                continue;
            }
            
            //si tuvimos exito agrego la room y el hallway al nivel y las correspondientes relaciones
            _level.AddRoom(newRoom);
            _level.AddHallway(selectedEntryway);
            selectedEntryway.EndRoom = newRoom;
            
            //Calculo las nuevos pasillos disponibles de la nueva room
            List<Hallway> newOpenHallways = newRoom.CalculateAllPosibleDoorways(newRoom.Area.width, newRoom.Area.height, 1);
            
            //Asigna a todos los nuevos pasillos, la nueva room como origen.
            newOpenHallways.ForEach(hallway => hallway.StartRoom = newRoom);
            
            //retiro del pool, la entrada usada y agrego todos los nuevos objetos
            _openDoorways.Remove(selectedEntryway);
            _openDoorways.AddRange(newOpenHallways);
            
            
        }
        
    }

    bool IsRoomCandidateValid(RectInt roomCandidateRect)
    {
        //FIX: SALIDA DEL MAPA**************************
        //hace un rectangulo levemente mas chico que el mapa
        RectInt levelRect = new RectInt(1, 1, width -2,lenght - 2);
        
        //se fija si la room nueva, está contenida dentro del mapa  y si no overlapea con otras rooms (ver funcion)
        return levelRect.Contains(roomCandidateRect) && !CheckRoomOverlap(roomCandidateRect, _level.Rooms, _level.Hallways, 1);
    }

    bool CheckRoomOverlap(RectInt roomCandidateRect, Room[] rooms, Hallway[] hallways, int minRoomDistance)
    {
        //nueva room con margen
        RectInt paddedRoomRect = new RectInt
        { 
            //agreganding margen
            x = roomCandidateRect.x - minRoomDistance,
            y = roomCandidateRect.y - minRoomDistance,
            width = roomCandidateRect.width + 2 * minRoomDistance,
            height = roomCandidateRect.height + 2 * minRoomDistance
        };
        
        //revisa si la nueva room se overlapea con las rooms existentes
        foreach (Room room in rooms)
        {
            if(paddedRoomRect.Overlaps(room.Area))
            {
                return true;
            }
        }

        foreach (var hallway in hallways)
        {
            if(paddedRoomRect.Overlaps(hallway.Area))
            {
                return true;
            }
        }
        return false;
    }

}



