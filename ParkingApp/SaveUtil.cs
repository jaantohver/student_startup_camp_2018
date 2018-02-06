using System;
using System.IO;
using System.Threading.Tasks;

namespace ParkingApp
{
    public class SaveUtil
    {
        readonly string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        const string fileRental = "rental.txt";
        const string fileTrailer = "trailer.txt";
        const string fileRentalPoint = "rentalPoint.txt";

        readonly string rentalPath;
        readonly string trailerPath;
        readonly string rentalPointPath;

        public SaveUtil()
        {
            rentalPath = Path.Combine(basePath, fileRental);
            trailerPath = Path.Combine(basePath, fileTrailer);
            rentalPointPath = Path.Combine(basePath, fileRentalPoint);
        }

        public async Task SaveJpg(byte[] img)
        {
            string path = Path.Combine(
                Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path,
                "TikiTreiler");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string jpgFilename = Path.Combine(path, DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss-ffff") + ".jpg");

            // Doing it the C# way threw a security exception :S.
            Java.IO.FileOutputStream outStream = new Java.IO.FileOutputStream(jpgFilename);
            try
            {
                outStream.Write(img);
            }
            catch (Java.Lang.Exception)
            {
                throw new Java.Lang.IllegalArgumentException();
            }
            finally
            {
                outStream.Close();
            }
        }

        public async Task<Java.IO.File> SavePdf(byte[] pdf)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + "pdf";
            Directory.CreateDirectory(path);
            string filename = Path.Combine(path, "regDoc.pdf");
            File.WriteAllBytes(filename, pdf);
            return new Java.IO.File(filename);
        }
    }
}