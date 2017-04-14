using System;

using UIKit;

namespace App2
{

    public partial class ScanResult : UIViewController
    {
        public UILabel res;
        public string scan_res { get; set; }
        public ScanResult() : base("ScanResult", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
           
            base.ViewDidLoad();
            View.AutoresizingMask = UIViewAutoresizing.All;
            View.Frame = new CoreGraphics.CGRect(0,20,300,600);

          

            
            
            res = new UILabel();
            res.AutoresizingMask = UIViewAutoresizing.All;
            res.Frame = new CoreGraphics.CGRect(100,180,100,30);
            
            View.AddSubview(res);

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if(scan_res !=null)
                res.Text = scan_res;
            res.SizeToFit();
        }


    }
}