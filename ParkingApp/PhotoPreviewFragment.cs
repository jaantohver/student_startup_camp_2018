
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ParkingApp
{
    public class PhotoPreviewFragment : Fragment
    {
        public ImageView PreviewImageView;
        ProgressBarManager _pbManager;
        TikiButtonBlue _saveButton;

        public override bool UserVisibleHint {
            get {
                return base.UserVisibleHint;
            }
            set {
                base.UserVisibleHint = value;
                if (!value && View != null)
                {
                    HideImage();
                }
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.FragmentPhotoPreview, container, false);

            PreviewImageView = v.FindViewById<ImageView>(Resource.Id.imageViewPreview);
            _pbManager = new ProgressBarManager(v.FindViewById<ProgressBar>(Resource.Id.progressBarPhotoPreview));
            _saveButton = v.FindViewById<TikiButtonBlue>(Resource.Id.tikiButtonBlueSavePhoto);
            _saveButton.Text = "Save";

            return v;
        }

        public override void OnResume()
        {
            base.OnResume();
            _saveButton.Click += OnSaveClicked;
        }

        public override void OnPause()
        {
            _saveButton.Click -= OnSaveClicked;
            base.OnPause();
        }

        void OnSaveClicked(object src, EventArgs e)
        {
            if (ReportActivity.PhotoData != null)
            {
                TrySavePhoto();
            }
        }

        public void TrySavePhoto()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (Activity.CheckSelfPermission(Manifest.Permission.WriteExternalStorage)
                    != Android.Content.PM.Permission.Granted)
                {
                    Activity.RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage },
                                               CameraActivity.RequestWriteExternalStoragePermission);
                }
                else
                {
                    SavePhoto();
                }
            }
            else
            {
                SavePhoto();
            }
        }

        async void SavePhoto()
        {
            ShowLoading(hideImage: false);
            await new SaveUtil().SaveJpg(ReportActivity.PhotoData);
            Activity.Finish();
        }

        public void ShowLoading(bool hideImage = true)
        {
            if (hideImage)
            {
                HideImage();
            }
            _pbManager.ShowBar();
        }

        void HideLoading()
        {
            _pbManager.HideBar();
        }

        public void ShowImage()
        {
            HideLoading();
            PreviewImageView.Visibility = ViewStates.Visible;
        }

        void HideImage()
        {
            PreviewImageView.Visibility = ViewStates.Invisible;
            PreviewImageView.SetImageBitmap(null);
            PreviewImageView.DestroyDrawingCache();
        }
    }
}