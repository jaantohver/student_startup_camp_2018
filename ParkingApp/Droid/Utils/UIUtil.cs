using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Support.V4.App;
using Android.Views.InputMethods;

namespace ParkingApp.Droid
{
    public class UIUtil
    {
        public static void ShowDialog (Context ctx, string title, string message,
                                      string negativeText, EventHandler<DialogClickEventArgs> negativeCallback,
                                      string positiveText, EventHandler<DialogClickEventArgs> positiveCallback)
        {
            Android.Support.V7.App.AlertDialog.Builder alertDialog =
                new Android.Support.V7.App.AlertDialog.Builder (ctx);
            alertDialog.SetTitle (title);
            alertDialog.SetMessage (message);
            alertDialog.SetNegativeButton (negativeText, negativeCallback);
            alertDialog.SetPositiveButton (positiveText, positiveCallback);
            alertDialog.Create ().Show ();
        }

        public static void ShowInfoDialog (Context ctx, string title, string message, string buttonMessage)
        {
            Android.Support.V7.App.AlertDialog.Builder alertDialog =
                new Android.Support.V7.App.AlertDialog.Builder (ctx);
            alertDialog.SetTitle (title);
            alertDialog.SetMessage (message);
            alertDialog.SetPositiveButton (buttonMessage, (src, e) => { });
            alertDialog.Create ().Show ();
        }

        public static void ShowInfoDialog (Context ctx, string title, string message, string buttonMessage,
                                           EventHandler<DialogClickEventArgs> action)
        {
            Android.Support.V7.App.AlertDialog.Builder alertDialog =
                new Android.Support.V7.App.AlertDialog.Builder (ctx);
            alertDialog.SetTitle (title);
            alertDialog.SetMessage (message);
            alertDialog.SetPositiveButton (buttonMessage, action);
            alertDialog.Create ().Show ();
        }

        public static void HideKeyboard (FragmentActivity activity, View view)
        {
            InputMethodManager imm = (InputMethodManager)activity.GetSystemService (Context.InputMethodService);
            imm.HideSoftInputFromWindow (view.WindowToken, 0);
        }

        public static void HideKeyboard (Activity activity, View view)
        {
            InputMethodManager imm = (InputMethodManager)activity.GetSystemService (Context.InputMethodService);
            imm.HideSoftInputFromWindow (view.WindowToken, 0);
        }
    }
}