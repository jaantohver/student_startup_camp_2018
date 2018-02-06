using Android.Views;
using Android.Widget;

namespace ParkingApp.Droid
{
    public class ProgressBarManager
    {
        public ProgressBar Bar { private set; get; }
        int _inProgressShowRequests;

        public ProgressBarManager (ProgressBar progressbar)
        {
            Bar = progressbar;
        }

        public void ShowBar ()
        {
            if (!Bar.IsShown) {
                Bar.Visibility = ViewStates.Visible;
            }
            _inProgressShowRequests++;
        }

        public void HideBar ()
        {
            _inProgressShowRequests--;
            if (_inProgressShowRequests < 0) {
                _inProgressShowRequests = 0;
            }
            if (_inProgressShowRequests == 0 && Bar.IsShown) {
                Bar.Visibility = ViewStates.Invisible;
            }
        }

        public void AdjustFrameLayoutProgressBarGravity ()
        {
            FrameLayout.LayoutParams lp = new FrameLayout.LayoutParams (Bar.LayoutParameters);
            lp.Gravity = GravityFlags.Bottom;
            Bar.LayoutParameters = lp;
        }
    }
}