using System;
using System.Collections.Generic;

using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Graphics;

namespace ParkingApp.Droid
{
    class FeedAdapter : ArrayAdapter<Shame>
    {
        static readonly Color selected = Color.ParseColor ("#33f3cc");
        static readonly Color notSelected = Color.LightSlateGray;

        readonly Context context;
        readonly int resource;
        readonly List<Shame> items;

        public event EventHandler Upvoted, Downvoted;

        public FeedAdapter (Context context, int resource, List<Shame> items) : base (context, resource, items)
        {
            this.context = context;
            this.resource = resource;
            this.items = items;
        }

        public override long GetItemId (int position)
        {
            return position;
        }

        public override int GetItemViewType (int position)
        {
            return (int)items[position % items.Count].Type;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            ViewHolder holder = null;

            if (view != null) {
                holder = view.Tag as ViewHolder;
            }

            if (holder == null) {
                var inflater = context.GetSystemService (Context.LayoutInflaterService).JavaCast<LayoutInflater> ();

                holder = new ViewHolder ();

                switch (items[position % items.Count].Type) {
                case ShameType.Description:
                    view = inflater.Inflate (Resource.Layout.ShameItemDescription, parent, false);

                    holder.Text = view.FindViewById<TextView> (Resource.Id.text);

                    break;
                case ShameType.Image:
                    view = inflater.Inflate (Resource.Layout.ShameItemImage, parent, false);

                    holder.Image = view.FindViewById<ImageView> (Resource.Id.image);

                    break;
                case ShameType.ImageAndDescription:
                    view = inflater.Inflate (Resource.Layout.ShameItemImageAndDescription, parent, false);

                    holder.Text = view.FindViewById<TextView> (Resource.Id.text);
                    holder.Image = view.FindViewById<ImageView> (Resource.Id.image);

                    break;
                }

                holder.Name = view.FindViewById<TextView> (Resource.Id.name);
                holder.Score = view.FindViewById<TextView> (Resource.Id.score);

                view.Tag = holder;
            }

            Shame s = items[position % items.Count];

            if (string.IsNullOrWhiteSpace (s.Name)) {
                holder.Name.Text = "Anonymous";
            } else {
                holder.Name.Text = s.Name;
            }

            if (holder.Text != null) {
                holder.Text.Text = s.Description;
            }

            if (holder.Image != null) {
                Bitmap bmp = BitmapFactory.DecodeByteArray (s.PhotoData, 0, s.PhotoData.Length);

                holder.Image.SetImageBitmap (bmp);
            }

            holder.Score.Text = s.Score.ToString ();

            ImageButton upvote = view.FindViewById<ImageButton> (Resource.Id.upvote);
            ImageButton downvote = view.FindViewById<ImageButton> (Resource.Id.downvote);

            if (s.Upvoted) {
                upvote.SetColorFilter (selected);
            } else {
                upvote.SetColorFilter (notSelected);
            }

            upvote.Click += delegate
            {
                if (s.Upvoted) {
                    holder.Score.Text = (int.Parse (holder.Score.Text) - 1).ToString ();
                    upvote.SetColorFilter (notSelected);
                } else if (s.Downvoted) {
                    holder.Score.Text = (int.Parse (holder.Score.Text) + 2).ToString ();
                    upvote.SetColorFilter (selected);
                } else {
                    holder.Score.Text = (int.Parse (holder.Score.Text) + 1).ToString ();
                    upvote.SetColorFilter (selected);
                }

                downvote.SetColorFilter (notSelected);

                Upvoted?.Invoke (position, EventArgs.Empty);
            };

            if (s.Downvoted) {
                downvote.SetColorFilter (selected);
            } else {
                downvote.SetColorFilter (notSelected);
            }

            downvote.Click += delegate
            {
                if (s.Downvoted) {
                    holder.Score.Text = (int.Parse (holder.Score.Text) + 1).ToString ();
                    downvote.SetColorFilter (notSelected);
                } else if (s.Upvoted) {
                    holder.Score.Text = (int.Parse (holder.Score.Text) - 2).ToString ();
                    downvote.SetColorFilter (selected);
                } else {
                    holder.Score.Text = (int.Parse (holder.Score.Text) - 1).ToString ();
                    downvote.SetColorFilter (selected);
                }

                upvote.SetColorFilter (notSelected);

                Downvoted?.Invoke (position, EventArgs.Empty);
            };

            return view;
        }

        public override int Count
        {
            get => items.Count;// * 100;
        }

        public override int ViewTypeCount
        {
            get => 3;
        }
    }

    class ViewHolder : Java.Lang.Object
    {
        public TextView Score { get; set; }
        public TextView Name { get; set; }
        public TextView Text { get; set; }
        public ImageView Image { get; set; }
    }
}