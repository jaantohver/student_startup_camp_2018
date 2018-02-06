using Android.OS;
using Android.App;
using Android.Widget;

namespace ParkingApp
{
    [Activity(Label = "CategoriesActivity", MainLauncher = true)]
    public class CategoriesActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Categories);

            GridView grid = FindViewById<GridView>(Resource.Id.grid);
            grid.Adapter = new CategoriesAdapter(this);
            grid.ItemClick += CategorySelected;
        }

        private void CategorySelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            StartActivity(typeof(ReportActivity));
        }
    }
}