using System;
using AVFoundation;
using UIKit;
using Foundation;
using CoreFoundation;
using CoreImage;
using CoreGraphics;
namespace App2
{
    public partial class ViewController1 : UIViewController
    {
        static AVCaptureSession session;
        static UILabel scanres;
        static CoreAnimation.CALayer Layer;
        UIImageView img;
        UIImageView img2;
        UIView area;

        public ViewController1() : base("ViewController1", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            if (session == null)
            {
                //StartScanWithSize(200);
            }else
            {
                //session.StartRunning();
            }
        }
        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            View.AutoresizingMask = UIViewAutoresizing.All;
            View.Frame = new CoreGraphics.CGRect(0, 20, 300, 600);

            
            area = new UIView();
            area.AutoresizingMask = UIViewAutoresizing.All;
            area.Frame = new CoreGraphics.CGRect(30, 180, 240, 240);
            area.Layer.BorderWidth = 1;
            area.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0.3f);

            //View.AddSubview(area);
            img = new UIImageView();
            UIImage iim = DealFuzzyImage(CreateQC((NSString)"www.baidu.com"),250);
            //iim.SaveToPhotosAlbum(null);


            iim.SaveToPhotosAlbum(null);
            img.Image = iim;
            img.AutoresizingMask = UIViewAutoresizing.All;
            Console.WriteLine("Height:{0},Width:{1}",img.Image.Size.Height,img.Image.Size.Width); 
            img.Frame = new CGRect(50,100,200,260);
            img.Layer.BorderWidth = 1;
            View.AddSubview(img);

            img2 = new UIImageView();
            UIImage iim2 = DealFuzzyImage(CreatCodeImage((NSString)"www.baidu.com"), 250);
            img2.Image = iim2;
            img2.AutoresizingMask = UIViewAutoresizing.All;
            Console.WriteLine("Height:{0},Width:{1}", img.Image.Size.Height, img.Image.Size.Width);
            img2.Frame = new CGRect(50, 400, 200, 40);

            View.AddSubview(img2);

            Layer = View.Layer;
            scanres = new UILabel();
            scanres.AutoresizingMask = UIViewAutoresizing.All;
            scanres.Frame = new CoreGraphics.CGRect(22,502,202,32);
            scanres.Layer.BorderWidth = 1;
            scanres.AdjustsFontSizeToFitWidth = true;
            //View.AddSubview(scanres);

            // Perform any additional setup after loading the view, typically from a nib.
        }

       
        public void StartScanWithSize(float size)
        {
            //��ȡ�����豸
            AVCaptureDevice device = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            
            //����������
            NSError error = null;
            AVCaptureDeviceInput input = AVCaptureDeviceInput.FromDevice(device,out error);

            //�ж��������Ƿ����
            if (input!=null)
            {
                //���������
                AVCaptureMetadataOutput output = new AVCaptureMetadataOutput();
                var OutPutObj = new OutputObjDelegate();

                output.SetDelegate(OutPutObj, DispatchQueue.MainQueue);

                nfloat x = area.Frame.Y / View.Frame.Height;
                nfloat y = area.Frame.X / View.Frame.Width;
                nfloat width = area.Frame.Height / View.Frame.Height;
                nfloat height = area.Frame.Width / View.Frame.Width;
                Console.WriteLine("����" + output.RectOfInterest);
                output.RectOfInterest = new CoreGraphics.CGRect(x,y,width,height);
                
                //��ʼ�����Ӷ���
                session = new AVCaptureSession();

                //���ø������ɼ���0
                session.SessionPreset = AVCaptureSession.PresetHigh;
                session.AddInput(input);
                session.AddOutput(output);

                ////ɨ�������С����(����,��Ժ������Ͻ�)

              
                //����ɨ��֧�ֵı����ʽ
                output.MetadataObjectTypes = AVMetadataObjectType.QRCode | AVMetadataObjectType.EAN13Code | AVMetadataObjectType.EAN8Code | AVMetadataObjectType.Code128Code;

               
                AVCaptureVideoPreviewLayer layer = new AVCaptureVideoPreviewLayer(session);
                layer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;

                layer.Frame = View.Layer.Bounds;
               
                View.Layer.InsertSublayer(layer,0);

                session.StartRunning();


                
            }
        }
        //���ɶ�ά��
        public CIImage CreateQC(NSString content)
        {


            NSData n_content = content.Encode(NSStringEncoding.UTF8);
            CIFilter cfi = CIFilter.FromName("CIQRCodeGenerator");
            cfi.SetDefaults();
            cfi.SetValueForKey(n_content, (NSString)"inputMessage");
            
            cfi.SetValueForKey((NSString)"M", (NSString)"inputCorrectionLevel");


            CIImage img = cfi.OutputImage;
            
            
            return img;
        }
        //����������
        public CIImage CreatCodeImage(NSString content)
        {
            NSData n_content = content.Encode(NSStringEncoding.ASCIIStringEncoding);
            CIFilter cfi = CIFilter.FromName("CICode128BarcodeGenerator");
            cfi.SetValueForKey(n_content, (NSString)"inputMessage");
            cfi.SetValueForKey((NSNumber)0, (NSString)"inputQuietSpace");
            CIImage img = cfi.OutputImage;

            return img;
        }
        //ͼ������������
        public UIImage DealFuzzyImage(CIImage img,float size)
        {
            CGRect ctr = img.Extent;

            float scale = (float)Math.Min(size / ctr.Width, size / ctr.Height);

            double s_width =  ctr.Width * scale;
            double s_height = ctr.Height * scale;

            CGColorSpace color_space = CGColorSpace.CreateDeviceGray();
            CGContext cg_context = new CGBitmapContext(null, (int)s_width, (int)s_height, 8, 0, color_space, CGImageAlphaInfo.None);
            CIContext context = CIContext.FromOptions(null);
            CGImage bitmap_imge = context.CreateCGImage(img, ctr);

            cg_context.InterpolationQuality = CGInterpolationQuality.None;
            cg_context.ScaleCTM(scale, scale);
            cg_context.DrawImage(ctr, bitmap_imge);

            CGImage scaleImg = ((CGBitmapContext)cg_context).ToImage();

            return UIImage.FromImage(scaleImg);       
        }
        public class OutputObjDelegate: AVCaptureMetadataOutputObjectsDelegate
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
                        scanres.Text = metaDataObj.StringValue;

                        //AVCaptureVideoPreviewLayer layer = (AVCaptureVideoPreviewLayer)Layer.Sublayers[0];
                        //layer.RemoveFromSuperLayer();

                        UIResponder responer = scanres.NextResponder;
                        do
                        {
                            if (responer is UIViewController)
                            {
                                //var nav = responer as UIViewController;
                                ((UIViewController)responer).NavigationController.PushViewController(new Scan(), true);
                                break;
                            }
                            responer = responer.NextResponder;
                        } while (responer!=null);



                    }
                }
            }
        }
    }
}