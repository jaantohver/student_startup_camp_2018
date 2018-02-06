using UIKit;

namespace ParkingApp.iOS
{
    public class CategoriesController : UIViewController
    {
        CategoriesView contentView;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            Title = "Categories";

            View = contentView = new CategoriesView ();
        }
    }
}