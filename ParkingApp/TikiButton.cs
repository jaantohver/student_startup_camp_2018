using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace ParkingApp
{
    public class TikiButton : Button
    {
        public TikiButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Gravity = GravityFlags.Center;
        }
    }
}