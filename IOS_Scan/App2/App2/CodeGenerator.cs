using CoreGraphics;
using CoreImage;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace App2
{
    public class CodeGenerator
    {
        //生成二维码
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
        //生成条形码
        public CIImage CreatCodeImage(NSString content)
        {
            NSData n_content = content.Encode(NSStringEncoding.ASCIIStringEncoding);
            CIFilter cfi = CIFilter.FromName("CICode128BarcodeGenerator");
            cfi.SetValueForKey(n_content, (NSString)"inputMessage");
            cfi.SetValueForKey((NSNumber)0, (NSString)"inputQuietSpace");
            CIImage img = cfi.OutputImage;

            return img;
        }
        //图像清晰化处理
        public UIImage DealFuzzyImage(CIImage img, float size)
        {
            CGRect ctr = img.Extent;

            float scale = (float)Math.Min(size / ctr.Width, size / ctr.Height);

            double s_width = ctr.Width * scale;
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
    }
}
