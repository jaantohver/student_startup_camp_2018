using System;

using Android;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace ParkingApp.Droid
{
    public class PhotoPreviewFragment : Fragment
    {
        public ImageView PreviewImageView;
        ProgressBarManager _pbManager;

        public override bool UserVisibleHint
        {
            get {
                return base.UserVisibleHint;
            }
            set {
                base.UserVisibleHint = value;
                if (!value && View != null) {
                    HideImage ();
                }
            }
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate (Resource.Layout.FragmentPhotoPreview, container, false);

            PreviewImageView = v.FindViewById<ImageView> (Resource.Id.imageViewPreview);
            _pbManager = new ProgressBarManager (v.FindViewById<ProgressBar> (Resource.Id.progressBarPhotoPreview));

            return v;
        }

        public void TrySavePhoto ()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M) {
                if (Activity.CheckSelfPermission (Manifest.Permission.WriteExternalStorage)
                    != Android.Content.PM.Permission.Granted) {
                    Activity.RequestPermissions (new string[] { Manifest.Permission.WriteExternalStorage },
                                               CameraActivity.RequestWriteExternalStoragePermission);
                } else {
                    SavePhoto ();
                }
            } else {
                SavePhoto ();
            }
        }

        async void SavePhoto ()
        {
            ShowLoading (hideImage: false);
            await new SaveUtil ().SaveJpg (AddActivity.PhotoData);
            Activity.Finish ();
        }

        public void ShowLoading (bool hideImage = true)
        {
            if (hideImage) {
                HideImage ();
            }
            _pbManager.ShowBar ();
        }

        void HideLoading ()
        {
            _pbManager.HideBar ();
        }

        public void ShowImage ()
        {
            HideLoading ();
            PreviewImageView.Visibility = ViewStates.Visible;
        }

        void HideImage ()
        {
            PreviewImageView.Visibility = ViewStates.Invisible;
            PreviewImageView.SetImageBitmap (null);
            PreviewImageView.DestroyDrawingCache ();
        }
    }
}