
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Com.Google.Android.Cameraview;
using Java.IO;
using Java.Lang;

namespace ParkingApp
{
    public class TakePhotoFragment : Fragment
    {
        TikiButtonBlue _cameraButton;
        CameraView _cameraView;
        Action _onTakePhoto;
        Action<Bitmap> _onImageDecoded;

        public TakePhotoFragment(Action onTakePhoto, Action<Bitmap> onImageDecoded)
        {
            _onTakePhoto = onTakePhoto;
            _onImageDecoded = onImageDecoded;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.FragmentTakePhoto, container, false);

            _cameraButton = v.FindViewById<TikiButtonBlue>(Resource.Id.tikiButtonBlueTakePhoto);
            _cameraButton.Text = "Strings.TakeThePhoto";
            _cameraView = v.FindViewById<CameraView>(Resource.Id.cameraView);
            _cameraView.AddCallback(new CameraViewCallback(_onImageDecoded));

            return v;
        }

        public override void OnResume()
        {
            base.OnResume();
            TryStartCamera();
            _cameraButton.Click += TakePhoto;
        }

        public override void OnPause()
        {
            StopCamera();
            _cameraButton.Click -= TakePhoto;
            base.OnPause();
        }

        public void TryStartCamera()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (Activity.CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted)
                {
                    StartCamera();
                }
                else
                {
                    Activity.RequestPermissions(new string[] { Manifest.Permission.Camera }, CameraActivity.RequestCameraPermission);
                }
            }
            else
            {
                StartCamera();
            }
        }

        void StartCamera()
        {
            if (!_cameraView.IsCameraOpened)
            {
                _cameraView.Start();
            }
        }

        void StopCamera()
        {
            _cameraView.Stop();
        }

        void TakePhoto(object src, EventArgs e)
        {
            _cameraView.TakePicture();
            _onTakePhoto();
        }

        class CameraViewCallback : CameraView.Callback
        {
            Action<Bitmap> _onDecoded;

            public CameraViewCallback(Action<Bitmap> onDecoded)
            {
                _onDecoded = onDecoded;
            }

            public override async void OnPictureTaken(CameraView p0, byte[] p1)
            {
                ReportActivity.PhotoData = p1;
                _onDecoded(BitmapFactory.DecodeByteArray(p1, 0, p1.Length));
            }
        }
    }
}