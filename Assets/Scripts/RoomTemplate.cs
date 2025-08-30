using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomTemplate 
{
    [SerializeField] private string roomName;
    [SerializeField] private int numberOfRooms;
    
    
    [Header("Room")] 
    [SerializeField] private int roomWidthMin = 3;
    [SerializeField] private int roomWidthMax = 5;
    [SerializeField] private int roomLenghtMin = 3;
    [SerializeField] private int roomLenghtMax = 5;
    
    public int NumberOfRooms => numberOfRooms;
    public int RoomWidthMin => roomWidthMin;
    public int RoomWidthMax => roomWidthMax;
    public int RoomLenghtMin => roomLenghtMin;
    public int RoomLenghtMax => roomLenghtMax;
}
