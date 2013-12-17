using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar
{
    public class Map
    {
        private int[,] mapInfo;

        private int _row;
        public int Row
        {
            get { return _row; }
        }

        private int _column;
        public int Column 
        {
            get { return _column; }
        }

        public Map(int row = 10, int column = 10)
        {
            _row = row;
            _column = column;
            MapInit();
        }

        private void MapInit()
        {
            mapInfo = new int[,]
                {
                    {0,1,0,0,1,0,0,1,0,0,},
                    {0,0,1,0,0,0,0,0,0,0,},
                    {0,0,0,1,0,1,1,0,0,0,},
                    {0,0,0,0,1,0,0,1,0,0,},
                    {0,0,0,0,0,1,0,0,0,0,},
                    {0,0,0,0,0,0,1,1,1,0,},
                    {0,0,0,0,0,1,0,0,0,0,},
                    {0,0,1,1,1,0,0,0,0,0,},
                    {0,0,1,0,0,0,0,0,0,0,},
                    {0,0,0,0,0,0,0,0,0,0,},
                };
        }

        public bool IsLegal(Climber climber)
        {
            if (climber.Row >= 0 && climber.Row < Row &&
                climber.Column >= 0 && climber.Column < Column)
            {
                if (mapInfo[climber.Row, climber.Column] != 1)
                {
                    return true;
                }
            }
            return false;
        }

         

        public void Show()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Console.Write(mapInfo[i, j] + " ");
                }
                Console.Write(Environment.NewLine);
            }
        }



        public static double Distance(Climber a, Climber b)
        {
            return Math.Sqrt(Math.Pow((b.Row - a.Row), 2) + Math.Pow((b.Column - a.Column), 2));
        }

        public void Press(Climber climber)
        {
            mapInfo[climber.Row, climber.Column] = 2;
        }
    }
}
