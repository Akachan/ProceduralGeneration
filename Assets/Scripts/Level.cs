using System.Collections.Generic;
using UnityEngine;
    public class Level
    {
        private int _width;
        private int _lenght;

        public int Width {get{return _width;}}
        public int Lenght {get{return _lenght;}}
        
        private List<Room> _rooms;
        private List<Hallway> _hallways;
        
        public List<Room> Rooms {get{return _rooms;}}
        public List<Hallway> Hallways {get{return _hallways;}}

        public Level(int width, int lenght)
        {
            this._width = width;
            this._lenght = lenght;
            _rooms = new List<Room>();
            _hallways = new List<Hallway>();
            
        }
        
        
        
        
    }
