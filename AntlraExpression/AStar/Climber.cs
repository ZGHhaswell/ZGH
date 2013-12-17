using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar
{
    public class Climber
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Climber(int row, int column)
        {
            Row = row;
            Column = column;
        }


        public List<Climber> DefaultChildren
        {
            get
            {
                return new List<Climber>()
                    {
                        new Climber(Row- 1, Column),
                        new Climber(Row, Column - 1),
                        new Climber(Row+ 1, Column),
                        new Climber(Row, Column + 1),
                    };
            }
        }

        public List<Climber> GetChildren(Map map)
        {
            var list = new List<Climber>();
            var eum = from climber in DefaultChildren
                      where map.IsLegal(climber)
                      select climber;
            list.AddRange(eum);
            return list;
        }

        public static bool operator ==(Climber a, Climber b)
        {
            return (a.Row == b.Row) && (a.Column == b.Column);
        }

        public static bool operator !=(Climber a, Climber b)
        {
            return !(a == b);
        }
    }
}
