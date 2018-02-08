namespace ParkingApp
{
    public enum ShameType
    {
        Image,
        Description,
        ImageAndDescription
    }

    public class Shame
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] PhotoData { get; set; }

        public ShameType Type { get; set; }
    }
}