
using Android.Util;
using Android.Content;
using Android.Support.V4.Content;

namespace ParkingApp
{
    public class TikiButtonBlue : TikiButton
    {
        public TikiButtonBlue(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetTextColor(new Android.Graphics.Color(ContextCompat.GetColor(context, Android.Resource.Color.White)));
        }
    }
}