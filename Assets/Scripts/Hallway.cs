using UnityEngine;


    public class Hallway
    {
        private Vector2Int _startPosition;
        private Vector2Int _endPosition;
        
        private HallwayDirection _startDirection;
        private HallwayDirection _endDirection;

        private Room _startRoom;
        private Room _endRoom;

        public Room StartRoom
        {
            get { return _startRoom; }
            set { _startRoom = value; }
        }
        public Room EndRoom
        {
            get { return _endRoom; }
            set { _endRoom = value; }
        }

        public Vector2Int StartPositionAbsolute { get {return _startPosition +_startRoom.Area.position;} }
        public Vector2Int EndPositionAbsolute { get {return _endPosition +_endRoom.Area.position;} }

        public HallwayDirection StartDirection {get {return _startDirection;}}
        public HallwayDirection EndDirection {get {return _endDirection; } set {_endDirection = value;}}
        
        public Hallway(HallwayDirection startDirection, Vector2Int startPosition, Room startRoom = null)
        {
            this._startDirection = startDirection;
            this._startPosition = startPosition;
            this._startRoom = startRoom;
        }
    }

