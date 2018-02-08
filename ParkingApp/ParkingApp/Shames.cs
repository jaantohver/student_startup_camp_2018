using System.Collections.Generic;

namespace ParkingApp
{
    public static class Shames
    {
        public static readonly List<Shame> List = new List<Shame> ();

        public static void Add (Shame s)
        {
            List.Add (s);
        }
    }
}