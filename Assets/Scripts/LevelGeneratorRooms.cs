using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorRooms : MonoBehaviour
{
    [SerializeField] private int width = 64;
    [SerializeField] private int lenght = 64;
    
    [SerializeField] private int roomWidthMin = 3;
    [SerializeField] private int roomWidthMax = 5;
    [SerializeField] private int roomLenghtMin = 3;
    [SerializeField] private int roomLenghtMax = 5;

    private System.Random _random;
    
    [ContextMenu("Generate Level Layout")]
    public void GenerateLevel()
    {   
        _random = new System.Random();
        var roomRect = GetStartRoomRect();
        Debug.Log(roomRect);
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
}

