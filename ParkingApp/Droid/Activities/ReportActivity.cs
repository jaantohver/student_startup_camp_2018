using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using Android.Text.Style;

using ZhangHai.Android.MaterialProgressBar;

namespace ParkingApp.Droid
{
    [Activity (
        Theme = "@style/TikiGenericActivityTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        WindowSoftInputMode = SoftInput.AdjustNothing
    )]
    public class ReportActivity : Activity
    {
        public static bool UserClickedSave;
        public static byte[] PhotoData;
        public static string PhotoId;
        public static string Feedback;

        EditText _messageEditText;
        TikiButtonBlue _cameraButton;
        ProgressBarManager _pbManager;
        bool _showConfirmButton = true;
        CancellationTokenSource _uploadCancelTokenSource;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            SetContentView (Resource.Layout.ActivityReport);
            FindViewById<TextView> (Resource.Id.textViewPleaseDescribe).Text = "Strings.DescribeIssue";
            _messageEditText = FindViewById<EditText> (Resource.Id.editTextMessage);
            _messageEditText.Hint = "Strings.EnterMessage";
            if (!string.IsNullOrEmpty (Feedback)) {
                _messageEditText.Append (Feedback);
            }
            _cameraButton = FindViewById<TikiButtonBlue> (Resource.Id.tikiButtonBlueCamera);
            _cameraButton.Text = "Strings.Camera";
            _pbManager = new ProgressBarManager (FindViewById<MaterialProgressBar> (Resource.Id.progressBarReport));
            SetUpActionBar ();
        }

        protected override void OnResume ()
        {
            base.OnResume ();
            if (_cameraButton.Text != "Strings.UploadingPhoto") {
                _cameraButton.Click += OnCameraButtonClicked;
                if (PhotoData == null) {
                    _cameraButton.Text = "Strings.Camera";
                } else {
                    _cameraButton.Text = "Strings.ChangePhoto";
                }
                SetCameraInstructionText (FindViewById<TextView> (Resource.Id.textViewTapOnCamera));
            }
        }

        protected override void OnPause ()
        {
            _cameraButton.Click -= OnCameraButtonClicked;
            base.OnPause ();
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
                if (_uploadCancelTokenSource != null && String.IsNullOrEmpty (PhotoId)) {
                    _uploadCancelTokenSource.Cancel ();
                }
                Finish ();
                return true;
            case Resource.Id.action_confirm:
                SendFeedback ();
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
            toolbar.Title = "Strings.ReportTitle";
            toolbar.SetTitleTextColor (Color.White);
            SetActionBar (toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled (true);
            ActionBar.SetHomeButtonEnabled (true);
        }

        void SetCameraInstructionText (TextView tv)
        {
            StyleSpan boldItalic = new StyleSpan (TypefaceStyle.BoldItalic);
            string pleb = string.Format ("Tap on {0} button if you would like to add a photo to your report.", _cameraButton.Text);
            int boldStart = pleb.IndexOf (_cameraButton.Text);
            int boldEnd = boldStart + _cameraButton.Text.Length;
            SpannableString spannable = new SpannableString (pleb);
            spannable.SetSpan (boldItalic, boldStart, boldEnd, SpanTypes.ExclusiveExclusive);
            tv.TextFormatted = spannable;
        }

        async void SendFeedback ()
        {
            HideConfirmButton ();
            _cameraButton.Click -= OnCameraButtonClicked;
            Feedback = _messageEditText.Text;
            if (PhotoData != null) {
                _pbManager.ShowBar ();
                _cameraButton.Text = "Strings.UploadingPhoto";
                _uploadCancelTokenSource = new CancellationTokenSource ();
                KeyValuePair<bool, string> result = await Networking.UploadImage (PhotoData.ToArray (), _uploadCancelTokenSource.Token);
                _pbManager?.HideBar ();

                if (this != null) {
                    if (result.Key) {
                        PhotoId = result.Value;
                        Finish ();
                    } else {
                        _cameraButton.Text = "Strings.ChangePhoto";
                        _cameraButton.Click += OnCameraButtonClicked;
                        ShowConfirmButton ();

                        UIUtil.ShowDialog (
                            this,
                            "Strings.Error",
                            result.Value,
                            "Strings.Cancel", (sender, e) => { },
                            "Strings.TryAgain", (sender, e) => SendFeedback ()
                        );
                    }
                }
            } else {
                Finish ();
            }
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

        public override void OnBackPressed ()
        {
            if (_uploadCancelTokenSource != null && String.IsNullOrEmpty (PhotoId)) {
                _uploadCancelTokenSource.Cancel ();
            }
            base.OnBackPressed ();
        }
    }
}