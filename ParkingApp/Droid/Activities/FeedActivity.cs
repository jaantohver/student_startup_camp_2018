using System;
using System.Collections.Generic;

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
            HivescaleDB.Instance = new HivescaleDB (GetDbPath ());

            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.ActivityFeed);

            list = FindViewById<ListView> (Resource.Id.list);
            list.ItemClick += ItemSelected;
            list.ItemLongClick += RemoveContent;

            FloatingActionButton fab = FindViewById<FloatingActionButton> (Resource.Id.fab);
            fab.AttachToListView (list);
            fab.Click += AddContent;

            SetUpActionBar ();

            List<Shame> shames = HivescaleDB.Instance.GetShames ();

            foreach (Shame s in shames) {
                Shames.Add (s);
            }
        }

        string GetDbPath ()
        {
            string dbName = "hivescale.db";
            string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
            return System.IO.Path.Combine (documentsPath, dbName);
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            list.Adapter = new FeedAdapter (this, 0, Shames.List);
            (list.Adapter as FeedAdapter).Upvoted += (sender, e) =>
            {
                int index = (int)sender;

                Shame s = Shames.List[index];

                if (s.Upvoted) {
                    s.Score -= 1;
                    s.Upvoted = false;
                } else if (s.Downvoted) {
                    s.Score += 2;
                    s.Upvoted = true;
                } else {
                    s.Score += 1;
                    s.Upvoted = true;
                }

                s.Downvoted = false;

                HivescaleDB.Instance.UpdateShame (s);
            };
            (list.Adapter as FeedAdapter).Downvoted += (sender, e) =>
            {
                int index = (int)sender;

                Shame s = Shames.List[index];

                if (s.Downvoted) {
                    s.Score += 1;
                    s.Downvoted = false;
                } else if (s.Upvoted) {
                    s.Score -= 2;
                    s.Downvoted = true;
                } else {
                    s.Score -= 1;
                    s.Downvoted = true;
                }

                s.Upvoted = false;

                HivescaleDB.Instance.UpdateShame (s);
            };
        }

        void ItemSelected (object sender, AdapterView.ItemClickEventArgs e)
        {
            Console.WriteLine ("Selected");
        }

        void AddContent (object sender, EventArgs e)
        {
            StartActivity (typeof (AddActivity));
        }

        void RemoveContent (object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Shame s = Shames.List[e.Position];

            HivescaleDB.Instance.RemoveShame (s);

            Shames.List.RemoveAt (e.Position);

            (list.Adapter as FeedAdapter).NotifyDataSetChanged ();
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