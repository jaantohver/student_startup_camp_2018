using System;

using Android;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using Android.Support.V4.App;

using Com.Google.Android.Cameraview;

using Refractored.Fab;

namespace ParkingApp.Droid
{
    public class TakePhotoFragment : Fragment
    {
        FloatingActionButton _cameraButton;
        CameraView _cameraView;
        Action _onTakePhoto;
        Action<byte[]> _onImageDecoded;

        public TakePhotoFragment (Action onTakePhoto, Action<byte[]> onImageDecoded)
        {
            _onTakePhoto = onTakePhoto;
            _onImageDecoded = onImageDecoded;
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate (Resource.Layout.FragmentTakePhoto, container, false);

            _cameraButton = v.FindViewById<FloatingActionButton> (Resource.Id.fab);
            _cameraView = v.FindViewById<CameraView> (Resource.Id.cameraView);
            _cameraView.AddCallback (new CameraViewCallback (_onImageDecoded));

            return v;
        }

        public override void OnResume ()
        {
            base.OnResume ();
            TryStartCamera ();
            _cameraButton.Click += TakePhoto;
        }

        public override void OnPause ()
        {
            StopCamera ();
            _cameraButton.Click -= TakePhoto;
            base.OnPause ();
        }

        public void TryStartCamera ()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M) {
                if (Activity.CheckSelfPermission (Manifest.Permission.Camera) == Permission.Granted) {
                    StartCamera ();
                } else {
                    Activity.RequestPermissions (new string[] { Manifest.Permission.Camera }, CameraActivity.RequestCameraPermission);
                }
            } else {
                StartCamera ();
            }
        }

        void StartCamera ()
        {
            if (!_cameraView.IsCameraOpened) {
                _cameraView.Start ();
            }
        }

        void StopCamera ()
        {
            _cameraView.Stop ();
        }

        void TakePhoto (object src, EventArgs e)
        {
            _cameraView.TakePicture ();
            _onTakePhoto ();
        }

        class CameraViewCallback : CameraView.Callback
        {
            Action<byte[]> _onDecoded;

            public CameraViewCallback (Action<byte[]> onDecoded)
            {
                _onDecoded = onDecoded;
            }

            public override void OnPictureTaken (CameraView p0, byte[] p1)
            {
                _onDecoded (p1);
            }
        }
    }
}