using CoreFoundation;
using System;

using UIKit;

namespace App2
{
    public partial class CreateQC : UIViewController
    {
        UIImageView barCode;
        UIButton generator_btn;
        UITextField content_utf;
      
        UIAlertView alt;
        public CreateQC() : base("CreateQC", null)
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
            CodeGenerator cg = new CodeGenerator();
            View.AutoresizingMask = UIViewAutoresizing.All;
            View.Frame = new CoreGraphics.CGRect(0, 20, 300, 600);

         
            
             NavigationItem.Title = "生成二维码";
          




            content_utf = new UITextField();
            content_utf.AutoresizingMask = UIViewAutoresizing.All;
            content_utf.Frame = new CoreGraphics.CGRect(20, 100, 100, 40);
            content_utf.Layer.BorderWidth = 1;
            content_utf.Layer.BorderColor = UIColor.FromRGBA(0, 0, 0, 0.3f).CGColor;
            content_utf.Layer.CornerRadius = 4;
            //content_utf.EnablesReturnKeyAutomatically = true;
            content_utf.ReturnKeyType = UIReturnKeyType.Done;
            content_utf.ShouldReturn = (u) => {
                u.ResignFirstResponder();
                return true;
            };


            View.AddSubview(content_utf);


            generator_btn = new UIButton(UIButtonType.RoundedRect);
            generator_btn.SetTitle("生成",UIControlState.Normal);
            generator_btn.AutoresizingMask = UIViewAutoresizing.All;
            //generator_btn.BackgroundColor = UIColor.FromRGBA(0,0,0,0.3f);
            generator_btn.Frame = new CoreGraphics.CGRect(150,100,40,40);
            //generator_btn.Layer.BorderWidth = 1;
            generator_btn.TouchUpInside += (sender,args) => {
                if (content_utf.IsFirstResponder)
                {
                    content_utf.ResignFirstResponder();
                }
                if (!string.IsNullOrEmpty(content_utf.Text))
                {
                    barCode.Image = cg.DealFuzzyImage(cg.CreateQC((Foundation.NSString)content_utf.Text),200);
                }else
                {
                    if (alt == null)
                    {
                        alt = new UIAlertView("","转换内容不能为空",null,"确定",null);
                    }
                  
                  
                    alt.Show();
                }
            };
            View.AddSubview(generator_btn);

          

            UIBarButtonItem save_btn = new UIBarButtonItem();
            save_btn.Title = "保存至相册";
            save_btn.Clicked += (sender, args) => {
                barCode.Image.SaveToPhotosAlbum((ui,err)=> {
                    if (err == null)
                    {
                        if (alt == null)
                        {
                            alt = new UIAlertView("","保存成功",null,"确定",null);
                        }
                        
                        alt.Show();
                    }
                });
            };

            NavigationItem.RightBarButtonItem = save_btn;
            barCode = new UIImageView();
            barCode.AutoresizingMask = UIViewAutoresizing.All;
            barCode.Frame = new CoreGraphics.CGRect(50,180,200,250);
            View.AddSubview(barCode);
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }
    }
}