using System;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using Android.Support.V4.App;
using Java.IO;
using Java.Nio;

namespace ParkingApp.Droid
{
    [Activity (
        Theme = "@style/TikiGenericActivityTheme",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class CameraActivity : FragmentActivity
    {
        NonSwipeableViewPager _pager;
        CameraStatePagerAdapter _adapter;
        public const int RequestCameraPermission = 1337;
        public const int RequestWriteExternalStoragePermission = 1338;
        Bitmap _lastBitMap; // To enforce recycle and dispose.

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.ActivityCamera);

            _pager = FindViewById<NonSwipeableViewPager> (Resource.Id.pagerCamera);
            _adapter = new CameraStatePagerAdapter (SupportFragmentManager, OnTakePhoto, OnPhotoDecoded);
            _pager.Adapter = _adapter;

            SetUpActionBar ();
        }

        public override bool OnOptionsItemSelected (IMenuItem item)
        {
            switch (item.ItemId) {
            case Android.Resource.Id.Home:
                PreviousFragment ();
                return true;
            }
            return base.OnOptionsItemSelected (item);
        }

        void SetUpActionBar ()
        {
            Toolbar toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
            toolbar.Title = "Take photo";
            toolbar.SetTitleTextColor (Color.White);
            toolbar.SetBackgroundColor (new Color (25, 25, 25));

            SetActionBar (toolbar);

            ActionBar.SetDisplayHomeAsUpEnabled (true);
            ActionBar.SetHomeButtonEnabled (true);
        }

        public override void OnRequestPermissionsResult (int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode) {
            case RequestCameraPermission:
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted) {
                    _adapter.TakePhotoFragment.TryStartCamera ();
                } else {
                    UIUtil.ShowDialog (
                        this,
                        "Strings.PermissionNeeded",
                        "Strings.CameraPermissionNeeded",
                        "Strings.Cancel", (sender, e) => Finish (),
                        "Strings.OK", (sender, e) => _adapter.TakePhotoFragment.TryStartCamera ()
                    );
                }
                break;
            case RequestWriteExternalStoragePermission:
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted) {
                    _adapter.PhotoPreviewFragment.TrySavePhoto ();
                } else {
                    UIUtil.ShowDialog (
                        this,
                        "Strings.PermissionNeeded",
                        "Strings.WriteExternalStoragePermissionNeeded",
                        "Strings.Cancel", (sender, e) =>
                        {
                            AddActivity.PhotoData = null;
                            Finish ();
                        },
                        "Strings.OK", (sender, e) => _adapter.PhotoPreviewFragment.TrySavePhoto ()
                    );
                }
                break;
            }
        }

        void PreviousFragment ()
        {
            // If PreviousFragment() was called the save button wasn't clicked.
            AddActivity.PhotoData = null;
            if (_pager.CurrentItem == (int)Constants.CameraFragments.PhotoPreview) {
                _pager.SetCurrentItem (0, true);
                if (_lastBitMap != null) {
                    _lastBitMap.Recycle ();
                    _lastBitMap.Dispose ();
                    _lastBitMap = null;
                }
            } else {
                Finish ();
            }
        }

        public override void OnBackPressed ()
        {
            PreviousFragment ();
        }

        void OnTakePhoto ()
        {
            _pager.SetCurrentItem ((int)Constants.CameraFragments.PhotoPreview, true);
            _adapter.PhotoPreviewFragment.ShowLoading ();
        }

        void OnPhotoDecoded (byte[] bytes)
        {
            Bitmap bmp = BitmapFactory.DecodeByteArray (bytes, 0, bytes.Length);

            float aspectRatio = (float)bmp.Height / bmp.Width;
            int height = _adapter.PhotoPreviewFragment.PreviewImageView.Height;
            int width = (int)Math.Round (height / aspectRatio);

            Bitmap scaled = Bitmap.CreateScaledBitmap (bmp, width, height, false);
            bmp.Recycle ();
            bmp.Dispose ();

            if (width > height) {
                scaled = RotateBitmap90 (scaled);
            }
            _lastBitMap = scaled;
            _adapter.PhotoPreviewFragment.PreviewImageView.SetImageBitmap (_lastBitMap);
            _adapter.PhotoPreviewFragment.ShowImage ();


            //wtfblock
            var stream = new System.IO.MemoryStream ();
            scaled.Compress (Bitmap.CompressFormat.Png, 0, stream);
            byte[] byteArray = stream.ToArray ();
            AddActivity.PhotoData = byteArray;
            //wtfblock


            Finish ();
        }

        Bitmap RotateBitmap90 (Bitmap bitmap)
        {
            Matrix matrix = new Matrix ();
            matrix.PostRotate (90);
            Bitmap rotated = Bitmap.CreateBitmap (bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
            bitmap.Recycle ();
            bitmap.Dispose ();
            return rotated;
        }
    }
}