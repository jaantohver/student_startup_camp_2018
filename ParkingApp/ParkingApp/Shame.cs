using System;

using SQLite;

namespace ParkingApp
{
    public enum ShameType
    {
        Image,
        Description,
        ImageAndDescription
    }

    [Table (nameof (Shame))]
    public class Shame
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        byte[] photoData;

        [Ignore]
        public byte[] PhotoData
        {
            get {
                if (photoData == null && !string.IsNullOrEmpty (base64)) {
                    photoData = Convert.FromBase64String (base64);
                }

                return photoData;
            }
            set => photoData = value;
        }

        string base64;
        public string Base64
        {
            get {
                if (photoData == null) {
                    return string.Empty;
                }

                return Convert.ToBase64String (photoData);
            }
            set => base64 = value;
        }

        public int Score { get; set; }

        public ShameType Type { get; set; }

        public bool Upvoted { get; set; }

        public bool Downvoted { get; set; }
    }
}