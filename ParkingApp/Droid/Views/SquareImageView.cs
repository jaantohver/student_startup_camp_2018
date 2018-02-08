using Android.Util;
using Android.Widget;
using Android.Content;

namespace ParkingApp
{
    public class SquareImageView : ImageView
    {
        public SquareImageView (Context context) : base (context)
        {
            SetScaleType (ScaleType.CenterCrop);
        }

        public SquareImageView (Context context, IAttributeSet attrs) : base (context, attrs)
        {
            SetScaleType (ScaleType.CenterCrop);
        }

        public SquareImageView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
        {
            SetScaleType (ScaleType.CenterCrop);
        }

        protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure (widthMeasureSpec, heightMeasureSpec);

            SetMeasuredDimension (MeasuredWidth, MeasuredWidth);
        }
    }
}