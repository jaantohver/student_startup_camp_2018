using Android.Util;
using Android.Widget;
using Android.Content;

namespace ParkingApp
{
    public class SquareImageView : ImageView
    {
        public SquareImageView(Context context) : base(context)
        {
        }

        public SquareImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public SquareImageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            SetMeasuredDimension(MeasuredWidth, MeasuredWidth);
        }
    }
}