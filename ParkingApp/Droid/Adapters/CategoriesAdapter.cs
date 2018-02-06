using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

namespace ParkingApp.Droid
{
    class CategoriesAdapter : BaseAdapter
    {
        Context context;

        public CategoriesAdapter (Context context)
        {
            this.context = context;
        }


        public override Java.Lang.Object GetItem (int position)
        {
            return position;
        }

        public override long GetItemId (int position)
        {
            return position;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            CategoriesAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as CategoriesAdapterViewHolder;

            if (holder == null) {
                holder = new CategoriesAdapterViewHolder ();
                var inflater = context.GetSystemService (Context.LayoutInflaterService).JavaCast<LayoutInflater> ();

                view = inflater.Inflate (Resource.Layout.CategoryItem, parent, false);

                holder.Text = view.FindViewById<TextView> (Resource.Id.text);
                holder.Image = view.FindViewById<ImageView> (Resource.Id.image);

                view.Tag = holder;
            }

            holder.Text.Text = "Item number " + position;

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get {
                return 6;
            }
        }
    }

    class CategoriesAdapterViewHolder : Java.Lang.Object
    {
        public TextView Text { get; set; }
        public ImageView Image { get; set; }
    }
}