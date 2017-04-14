using AVFoundation;
using CoreFoundation;
using Foundation;
using System;

using UIKit;
using CoreGraphics;

namespace App2
{
    public partial class Scan : UIViewController
    {
        static AVCaptureSession session;
        static UILabel scanres;
        static CoreAnimation.CALayer Layer;
        ShelterView area;
        CGRect clear_area;
        UIView clear_area_view;
        UILabel title;
        UIView scan_line;
       
        public Scan() : base("Scan", null)
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

            base.ViewDidLoad();
            View.AutoresizingMask = UIViewAutoresizing.All;
            View.Frame = new CoreGraphics.CGRect(0, 20, 300, 600);

            scanres = new UILabel();


            NavigationItem.Title = "ɨ��������/��ά��";

            clear_area = new CGRect(60, 120, 200, 200);

            area = new ShelterView(clear_area,View.Bounds);
            area.AutoresizingMask = UIViewAutoresizing.All;
            area.Layer.BorderWidth = 1;
            clear_area_view = new UIView();
            //clear_area_view.Frame = new CGRect(60, 140, 200, 200);
            clear_area_view.Frame = clear_area;
            //clear_area_view.AutoresizingMask = UIViewAutoresizing.All;
            clear_area_view.Layer.BorderColor = UIColor.White.CGColor;
            clear_area_view.Layer.BorderWidth = 2;
            View.AddSubview(clear_area_view);

            scan_line = new UIView();
            
            scan_line.Frame = new CGRect(0,0,clear_area.Width,3);
            scan_line.BackgroundColor = UIColor.FromRGBA(0,100,0,0.4f);

          


            clear_area_view.AddSubview(scan_line);
           
            scanres = new UILabel();
            scanres.Frame = new CGRect(0,400,100,30);
            scanres.AutoresizingMask = UIViewAutoresizing.All;
            View.AddSubview(scanres);
            View.AddSubview(area);

            Console.WriteLine(View.Bounds.Width);
            Console.WriteLine(View.Bounds.Height);

            //�����Ƿ���HOME��������򣨴�ʱ���ᴥ��ViewWillDisAppear������
            //NSNotificationCenter.DefaultCenter.AddObserver((NSString)"UIApplicationWillResignActiveNotification", AppplicationEnterBackground);

            //�����Ƿ����½������(��ʱ���ᴥ��ViewWillDisAppear����)
            NSNotificationCenter.DefaultCenter.AddObserver((NSString)"UIApplicationDidBecomeActiveNotification",ApplicationEnterAgain);
            // Perform any additional setup after loading the view, typically from a nib.

            //StartScanWithSize(200);
        }

        private void ApplicationEnterAgain(NSNotification obj)
        {
            scan_line.Frame = new CGRect(0, 0, clear_area.Width, 3);
            ScanAnimation();
        }

        public void AppplicationEnterBackground(NSNotification n)
        {
           
        }
        public override void ViewWillAppear(bool animated)
        {
            if (session == null)
            {
                StartScanWithSize(200);
            }
            else
            {
                if (!session.Running)
                {
                    session.StartRunning();
                }
            }

            scan_line.Frame = new CGRect(0, 0, clear_area.Width, 3);
        }

        public override void ViewWillDisappear(bool animated)
        {
            //if (session.Running)
            //    session.StopRunning();
        }
        public void ScanAnimation()
        {
            UIView.Animate(2.5, 0, UIViewAnimationOptions.Repeat, () =>
            {
                scan_line.Frame = new CGRect(0, clear_area.Height - 1, clear_area.Width, 3);
            }, null);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ScanAnimation();

            
            
        }
        public void StartScanWithSize(float size)
        {
            AVAuthorizationStatus status = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

            if(status == AVAuthorizationStatus.Denied|status == AVAuthorizationStatus.Restricted)
            {
                Console.WriteLine("Ӧ�����Ȩ�����ޣ��뵽����������");
            }
            //��ȡ�����豸
            AVCaptureDevice device = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);

            //����������
            NSError error = null;
            AVCaptureDeviceInput input = AVCaptureDeviceInput.FromDevice(device, out error);

            if (error != null)
            {
                Console.WriteLine("���������");
            }
            //�ж��������Ƿ����
            if (input != null)
            {
                //���������
                AVCaptureMetadataOutput output = new AVCaptureMetadataOutput();
                var OutPutObj = new OutputObjDelegate();

                output.SetDelegate(OutPutObj, DispatchQueue.MainQueue);

                //ɨ�������С����(����,��Ժ������Ͻ�)
                nfloat x = clear_area.Y / View.Frame.Height;
                nfloat y = clear_area.X / View.Frame.Width;
                nfloat width = clear_area.Height / View.Bounds.Height;
                nfloat height = clear_area.Width / View.Bounds.Width;

                output.RectOfInterest = new CoreGraphics.CGRect(x, y, width, height);

                //output.RectOfInterest = View.Bounds;
                //��ʼ�����Ӷ���
                session = new AVCaptureSession();

                //���ø������ɼ���0
                session.SessionPreset = AVCaptureSession.PresetHigh;
                session.AddInput(input);
                session.AddOutput(output);

                //����ɨ��֧�ֵı����ʽ
                output.MetadataObjectTypes = AVMetadataObjectType.QRCode | AVMetadataObjectType.EAN13Code | AVMetadataObjectType.EAN8Code | AVMetadataObjectType.Code128Code;


                AVCaptureVideoPreviewLayer layer = new AVCaptureVideoPreviewLayer(session);
                layer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;

                layer.Frame = View.Layer.Frame;

                View.Layer.InsertSublayer(layer, 0);

                //����ɨ��
                session.StartRunning();
            }
        }

        public class OutputObjDelegate : AVCaptureMetadataOutputObjectsDelegate
        {
            public override void DidOutputMetadataObjects(AVCaptureMetadataOutput captureOutput, AVMetadataObject[] metadataObjects, AVCaptureConnection connection)
            {
                //base.DidOutputMetadataObjects(captureOutput, metadataObjects, connection);
                if (metadataObjects.Length > 0)
                {
                    //��ȡ����Ϣ��ֹͣɨ��
                    session.StopRunning();

                    AVMetadataMachineReadableCodeObject metaDataObj = metadataObjects[0] as AVMetadataMachineReadableCodeObject;
                    if (metadataObjects != null)
                    {
                       // scanres.Text = metaDataObj.StringValue;

                        //AVCaptureVideoPreviewLayer layer = (AVCaptureVideoPreviewLayer)Layer.Sublayers[0];
                        //layer.RemoveFromSuperLayer();

                        UIResponder responer = scanres.NextResponder;
                        do
                        {
                            if (responer is UIViewController)
                            {
                                var nav = responer as UIViewController;
                                ScanResult sc = new ScanResult();
                                sc.scan_res = metaDataObj.StringValue;
                                ((UIViewController)responer).NavigationController.PushViewController( sc, true);
                                
                                break;
                            }
                            responer = responer.NextResponder;
                        } while (responer != null);



                    }
                }
            }
        }

        public class ShelterView : UIView
        {
            private CGRect clearArea;
            public ShelterView(CGRect ClearArea,CGRect frame):base(frame)
            {
                this.BackgroundColor = UIColor.Clear;
                this.Opaque = false;
                this.Alpha = 1;
                this.clearArea = ClearArea;
                
            }


            public override void Draw(CGRect rect)
            {
                var context = UIGraphics.GetCurrentContext();

                UIColor.FromRGBA(0, 0, 0, 0.3f).SetFill();
                context.FillRect(rect);

                context.ClearRect(clearArea);
               
              
            }
        }

        
    }
}