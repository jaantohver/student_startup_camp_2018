using System;

using Android.Util;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.View;

namespace ParkingApp
{
    public class NonSwipeableViewPager : ViewPager
    {
        TikiPagerScoller _scroller;

        public NonSwipeableViewPager(Context context) : base(context)
        {
            PostInit();
        }

        public NonSwipeableViewPager(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            PostInit();
        }

        void PostInit()
        {
            IntPtr ViewPagerClass = JNIEnv.FindClass("android/support/v4/view/ViewPager");
            IntPtr mScrollerProperty = JNIEnv.GetFieldID(ViewPagerClass, "mScroller", "Landroid/widget/Scroller;");

            _scroller = new TikiPagerScoller(Context);

            JNIEnv.SetField(Handle, mScrollerProperty, _scroller.Handle);
        }

        public override bool OnInterceptTouchEvent(Android.Views.MotionEvent ev)
        {
            return false;
        }

        public override bool OnTouchEvent(Android.Views.MotionEvent e)
        {
            return false;
        }

        class TikiPagerScoller : Scroller
        {
            public TikiPagerScoller(Context context) : base(context)
            {
            }

            public override void StartScroll(int startX, int startY, int dx, int dy, int duration)
            {
                base.StartScroll(startX, startY, dx, dy, 1000);
            }
        }
    }
}