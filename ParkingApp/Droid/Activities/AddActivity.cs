using System;

using Android.OS;
using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using Android.Text.Style;

using Refractored.Fab;

namespace ParkingApp.Droid
{
    [Activity (
        Theme = "@style/TikiGenericActivityTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        WindowSoftInputMode = SoftInput.AdjustNothing
    )]
    public class AddActivity : Activity
    {
        public static byte[] PhotoData;

        bool _showConfirmButton = true;

        EditText name, description;
        FloatingActionButton cameraButton;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.ActivityAdd);

            name = FindViewById<EditText> (Resource.Id.name);
            name.Hint = "Name";

            description = FindViewById<EditText> (Resource.Id.description);
            description.Hint = "Description";

            cameraButton = FindViewById<FloatingActionButton> (Resource.Id.fab);
            cameraButton.Click += delegate
            {
                StartActivity (typeof (CameraActivity));
            };

            SetUpActionBar ();
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            if (PhotoData != null && PhotoData.Length > 0) {
                Bitmap bmp = BitmapFactory.DecodeByteArray (PhotoData, 0, PhotoData.Length);

                FindViewById<ImageView> (Resource.Id.image).SetImageBitmap (bmp);
            }
        }

        public override void Finish ()
        {
            PhotoData = null;

            base.Finish ();
        }

        public override bool OnCreateOptionsMenu (IMenu menu)
        {
            MenuInflater.Inflate (Resource.Menu.MenuConfirm, menu);

            return true;
        }

        public override bool OnPrepareOptionsMenu (IMenu menu)
        {
            if (!_showConfirmButton) {
                menu.FindItem (Resource.Id.action_confirm).SetVisible (false);
            }

            return true;
        }

        public override bool OnMenuItemSelected (int featureId, IMenuItem item)
        {
            switch (item.ItemId) {
            case Android.Resource.Id.Home:

                Finish ();

                return true;
            case Resource.Id.action_confirm:

                Shame s = new Shame ();

                if (!string.IsNullOrWhiteSpace (name.Text)) {
                    s.Name = name.Text;
                }

                if (!string.IsNullOrWhiteSpace (description.Text)) {
                    s.Description = description.Text;
                }

                if (PhotoData != null && PhotoData.Length > 0) {
                    s.PhotoData = PhotoData;
                }

                if (!string.IsNullOrWhiteSpace (s.Description) && s.PhotoData != null && s.PhotoData.Length > 0) {
                    s.Type = ShameType.ImageAndDescription;
                } else if (!string.IsNullOrWhiteSpace (s.Description)) {
                    s.Type = ShameType.Description;
                } else if (s.PhotoData != null && s.PhotoData.Length > 0) {
                    s.Type = ShameType.Image;
                } else {
                    return false;
                }

                Shames.List.Insert (0, s);

                Finish ();

                return true;
            }

            return base.OnMenuItemSelected (featureId, item);
        }

        void OnCameraButtonClicked (object src, EventArgs e)
        {
            if (PhotoData == null) {
                StartActivity (typeof (CameraActivity));
            } else {
                UIUtil.ShowDialog (
                   this,
                   "Strings.DiscardCurrentPhotoTitle",
                   "Strings.DiscardCurrentPhotoMessage",
                   "Strings.Cancel", (sender, ev) => { },
                   "Strings.OK", (sender, ev) => StartActivity (typeof (CameraActivity))
               );
            }
        }

        void SetUpActionBar ()
        {
            Toolbar toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
            toolbar.Title = "Post";
            toolbar.SetTitleTextColor (Color.White);
            SetActionBar (toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled (true);
            ActionBar.SetHomeButtonEnabled (true);
        }

        void ShowConfirmButton ()
        {
            _showConfirmButton = true;
            InvalidateOptionsMenu ();
        }

        void HideConfirmButton ()
        {
            _showConfirmButton = false;
            InvalidateOptionsMenu ();
        }
    }
}