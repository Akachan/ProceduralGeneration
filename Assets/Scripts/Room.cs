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
    
    

}
