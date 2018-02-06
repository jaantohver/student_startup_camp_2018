using UIKit;
using CoreGraphics;

namespace ParkingApp.iOS
{
    public class CategoriesView : UIView
    {
        UICollectionViewFlowLayout layout;

        UICollectionView grid;

        public CategoriesView ()
        {
            layout = new UICollectionViewFlowLayout ();

            grid = new UICollectionView (CGRect.Empty, layout);
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            grid.Frame = Bounds;
        }
    }
}