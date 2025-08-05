using System.Collections.Generic;
using UnityEngine;
    public class Level
    {
        private int _width;
        private int _lenght;

        public int Width => _width;
        public int Lenght => _lenght;
        
        private List<Room> _rooms;
        private List<Hallway> _hallways;

        public Room[] Rooms => _rooms.ToArray();
        public Hallway[] Hallways => _hallways.ToArray();

        public Level(int width, int lenght)
        {
            this._width = width;
            this._lenght = lenght;
            _rooms = new List<Room>();
            _hallways = new List<Hallway>();
            
        }

        public void AddRoom(Room newRoom) => _rooms.Add(newRoom);
        public void AddHallway(Hallway newHallway) => _hallways.Add(newHallway);
        
        
        
    }
