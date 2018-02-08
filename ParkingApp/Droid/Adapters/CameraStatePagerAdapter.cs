using System;

using Android.Graphics;
using Android.Support.V4.App;

using static ParkingApp.Constants;

namespace ParkingApp.Droid
{
    public class CameraStatePagerAdapter : FragmentStatePagerAdapter
    {
        Fragment[] _fragments;
        public override int Count => _fragments.Length;
        public TakePhotoFragment TakePhotoFragment { get; }
        public PhotoPreviewFragment PhotoPreviewFragment { get; }

        public CameraStatePagerAdapter (FragmentManager fm, Action onTakePhoto, Action<byte[]> onDecoded) : base (fm)
        {
            _fragments = new Fragment[Enum.GetNames (typeof (CameraFragments)).Length];
            _fragments[(int)CameraFragments.TakePhoto] = TakePhotoFragment = new TakePhotoFragment (onTakePhoto, onDecoded);
            _fragments[(int)CameraFragments.PhotoPreview] = PhotoPreviewFragment = new PhotoPreviewFragment ();
        }

        public override Fragment GetItem (int position)
        {
            return _fragments[position];
        }
    }
}