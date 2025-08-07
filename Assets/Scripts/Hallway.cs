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

        public Vector2Int StartPositionAbsolute => _startPosition +_startRoom.Area.position;
        public Vector2Int EndPositionAbsolute => _endPosition +_endRoom.Area.position;

        public HallwayDirection StartDirection => _startDirection;

        public HallwayDirection EndDirection
        {
            get => _endDirection; 
            set => _endDirection = value;
        }


        public Vector2Int StartPosition
        {
            get => _startPosition;
            set => _startPosition = value;
        }

        public Vector2Int EndPosition
        {
            get => _endPosition;
            set => _endPosition = value;
        }

        public RectInt Area
        {
            get
            {
                int x =Mathf.Min(StartPositionAbsolute.x, EndPositionAbsolute.x);
                int y = Mathf.Min(StartPositionAbsolute.y, EndPositionAbsolute.y);
                int width = Mathf.Max(1, Mathf.Abs(StartPositionAbsolute.x - EndPositionAbsolute.x));
                int height = Mathf.Max(1, Mathf.Abs(StartPositionAbsolute.y - EndPositionAbsolute.y));

                if (StartPositionAbsolute.x == EndPositionAbsolute.x)
                {
                    y++;
                    height--;
                }

                if (StartPositionAbsolute.y == EndPositionAbsolute.y)
                {
                    x++;
                    width--;
                }
                return new RectInt(x,y,width,height);
            }
        }
        
        public Hallway(HallwayDirection startDirection, Vector2Int startPosition, Room startRoom = null)
        {
            this._startDirection = startDirection;
            this._startPosition = startPosition;
            this._startRoom = startRoom;
        }
    }

