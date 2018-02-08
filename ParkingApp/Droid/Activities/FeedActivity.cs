using System;

using Android.OS;
using Android.App;
using Android.Widget;
using Android.Graphics;

using Refractored.Fab;

namespace ParkingApp.Droid
{
    [Activity (
        Theme = "@style/TikiGenericActivityTheme",
        Label = "Wall of Shame",
        MainLauncher = true
    )]
    public class FeedActivity : Activity
    {
        ListView list;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.ActivityFeed);

            list = FindViewById<ListView> (Resource.Id.list);
            list.ItemClick += ItemSelected;

            FloatingActionButton fab = FindViewById<FloatingActionButton> (Resource.Id.fab);
            fab.AttachToListView (list);
            fab.Click += AddContent;

            SetUpActionBar ();
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            list.Adapter = new FeedAdapter (this, 0, Shames.List);
        }

        void ItemSelected (object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine ("Selected");
        }

        void AddContent (object sender, EventArgs e)
        {
            StartActivity (typeof (AddActivity));
        }

        void SetUpActionBar ()
        {
            Toolbar toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
            toolbar.Title = "Feed";
            toolbar.SetTitleTextColor (Color.White);
            toolbar.SetBackgroundColor (new Color (25, 25, 25));

            SetActionBar (toolbar);

            ActionBar.SetDisplayHomeAsUpEnabled (false);
            ActionBar.SetHomeButtonEnabled (false);
        }
    }
}