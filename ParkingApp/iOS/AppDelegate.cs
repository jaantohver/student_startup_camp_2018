using UIKit;
using Foundation;

namespace ParkingApp.iOS
{
    public class AppDelegate : UIApplicationDelegate
    {
        UINavigationController navController;

        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
        {
            navController = new UINavigationController ();

            Window = new UIWindow (UIScreen.MainScreen.Bounds);
            Window.RootViewController = navController;
            Window.MakeKeyAndVisible ();

            return true;
        }

        public override void OnResignActivation (UIApplication application)
        {
        }

        public override void DidEnterBackground (UIApplication application)
        {
        }

        public override void WillEnterForeground (UIApplication application)
        {
        }

        public override void OnActivated (UIApplication application)
        {
        }

        public override void WillTerminate (UIApplication application)
        {
        }
    }
}