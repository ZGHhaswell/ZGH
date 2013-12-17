using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar
{
    class Program
    {
        

        static void Main(string[] args)
        {
            var map = new Map();
            map.Show();

            var open = new List<Climber>();
            var close = new List<Climber>();
            var start = new Climber(0, 0);
            var end = new Climber(0, 6);

            if (!map.IsLegal(start) || !map.IsLegal(end))
            {
                Console.WriteLine("Start Or End is Error!");
                return;
            }

            open.Add(start);

            while (open.Count != 0)
            {
                var n = open.GetMinClimber(end);
                if (n == end)
                {
                    break;
                }
                var children = n.GetChildren(map);
                foreach (var climber in children)
                {
                    
                }
                open.Remove(n);
                close.Add(n);
            }

            foreach (var climber in close)
            {
                map.Press(climber);
            }

            Console.WriteLine("After");

            map.Show();
        }
    }

    public static class ApandFuc
    {
        public static Climber GetMinClimber(this List<Climber> list, Climber end)
        {
            if (list.Count > 0)
            {
                var enu = (from climber in list
                          let dis = Map.Distance(climber, end)
                          orderby dis
                          select climber).ToList();

                return enu[0];
            }
            return null;
        }
    }
}
