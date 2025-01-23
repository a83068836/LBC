using ImageMagick;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LBC.Class
{
    // 定义枚举类型
    public enum TPixelFormat : byte
    {
        pfDevice,
        pf1bit,
        pf4bit,
        pf8bit,
        pf15bit,
        pf16bit,
        pf24bit,
        pf32bit,
        pfCustom
    }

    // 定义结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct TIMAGE_HEADER
    {
        public TPixelFormat PixelFormat; // 颜色位数
        public byte unKnow1;
        public byte unKnow2;
        public byte boAlp; // 是否有透明通道？
        public ushort nWidth;
        public ushort nHeight;
        public short x;
        public short y;
        public int nCompressSize; // 如果>0则为压缩后的大小，否则未压缩
    }
    public class ImageItem
    {
        public byte[] ImagePath { get; set; }
        public int imagewidth { get; set; }
        public int imageheight { get; set; }
    }
    public class G
    {
        [DllImport("GomPak.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetImageToFile(IntPtr p, int index, string savePath, string png);

        [DllImport("GomPak.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void DeletePtr(IntPtr p);

        [DllImport("GomPak.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateForFileName(string fileName, int type, string passWord);

        [DllImport("GomPak.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetImageToMemory(IntPtr p, int index, ref int size, string png);

        [DllImport("GomPak.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetImageNumber(IntPtr p);

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool VirtualFree(IntPtr lpAddress, int dwSize, int dwFreeType);

        [DllImport("GomPak.dll", CallingConvention = CallingConvention.StdCall)]

        public static extern bool GetImageInfo(IntPtr ptr, int index, out TIMAGE_HEADER imageInfo);
        public static List<ImageItem> GetimagesList(int looks,string pakfilename)
        {
            var list = new List<ImageItem>();
            if (looks >= 10000)
            {
                IntPtr p = CreateForFileName(@"H:\GomPak\尘缘遗忘补丁\data"+@"\"+ pakfilename + (int)(looks/10000) +".pak", 1, "58版本库QQ87329279-58bbk.com");
                if (p != IntPtr.Zero)
                {
                    int size = GetImageNumber(p);
                    int index = looks - (((int)(looks / 10000)) * 10000);
                    if (size > 0 && index < size)
                    {
                        TIMAGE_HEADER imageInfo;
                        bool result = GetImageInfo(p, index, out imageInfo);
                        int imageSize = 0;
                    start:
                        try
                        {
                        
                            IntPtr ptr = GetImageToMemory(p, index, ref imageSize, "png");
                            if (ptr != IntPtr.Zero)
                            {
                                byte[] png = new byte[imageSize];
                                Marshal.Copy(ptr, png, 0, imageSize);
                                list.Add(new ImageItem { ImagePath = png, imagewidth = imageInfo.nWidth, imageheight = imageInfo.nHeight });
                                VirtualFree(ptr, 0, 0x8000); // MEM_RELEASE
                            }
                        }
                        catch (Exception ex)
                        {
                            goto start;
                            ex.ToString();
                        }
                        
                        
                    }
                    DeletePtr(p);
                }
            }
            return list;
        }
        public static List<ImageItem> GetimagesList(int a,int b ,string pakfilename)
        {
            var list = new List<ImageItem>();
            IntPtr p = CreateForFileName(@"H:\GomPak\尘缘遗忘补丁\data" + @"\"+ pakfilename, 1, "58版本库QQ87329279-58bbk.com");
            if (p != IntPtr.Zero)
            {
                int size = GetImageNumber(p);
                for (int i = a; i < a+b; i++)
                {
                    int index = i;
                    TIMAGE_HEADER imageInfo;
                    bool result = GetImageInfo(p, index, out imageInfo);
                    int imageSize = 0;
                start:
                    try
                    {
                        IntPtr ptr = GetImageToMemory(p, index, ref imageSize, "png");
                        if (ptr != IntPtr.Zero)
                        {
                            byte[] png = new byte[imageSize];
                            Marshal.Copy(ptr, png, 0, imageSize);
                            list.Add(new ImageItem { ImagePath = png, imagewidth = imageInfo.nWidth, imageheight = imageInfo.nHeight });
                            VirtualFree(ptr, 0, 0x8000); // MEM_RELEASE
                        }
                    }
                    catch (Exception ex)
                    {
                        goto start;
                        ex.ToString();
                    }
                }
                DeletePtr(p);
            }
            return list;
        }
        public static BitmapImage Getimage(byte[] PatchA1Binary,int width,int height)
        {
            string tempFilePath = System.IO.Path.GetTempFileName().Replace(".tmp", ".png");

            string temppath = System.IO.Path.GetDirectoryName(tempFilePath) + @"\lbc\" + System.IO.Path.GetFileName(tempFilePath);
            // 使用三元运算符检查目录是否存在，不存在则创建
            CheckAndCreateDirectory(System.IO.Path.GetDirectoryName(tempFilePath) + @"\lbc\");
            // 将 byte[] 保存为 PNG 文件
            File.WriteAllBytes(temppath, PatchA1Binary);
            BitmapImage image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.DecodePixelWidth = width;
                image.DecodePixelHeight = height;
                image.UriSource = new Uri(temppath);
                //image.StreamSource = mem;
                image.EndInit();
                image.Freeze(); // 使图像可跨线程访问
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            
            return image;
        }
        public static void CheckAndCreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        public static BitmapImage CreateGifFromBitmapImages(List<BitmapImage> bitmapImages)
        {
            using (MagickImageCollection collection = new MagickImageCollection())
            {
                foreach (BitmapImage bitmapImage in bitmapImages)
                {
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        // 将 BitmapImage 保存到内存流中
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                        encoder.Save(memStream);
                        memStream.Position = 0;

                        // 从内存流中读取图像并添加到 MagickImageCollection
                        MagickImage image = new MagickImage(memStream);
                        collection.Add(image);
                    }
                }

                // 设置每帧的延迟时间（以 1/100 秒为单位）
                collection[0].AnimationDelay = 100;

                // 优化 GIF 文件大小
                collection.Optimize();
                // 将 GIF 图片写入临时文件
                string tempFilePath = System.IO.Path.GetTempFileName().Replace(".tmp", ".gif");

                string temppath = System.IO.Path.GetDirectoryName(tempFilePath) + @"\lbc\" + System.IO.Path.GetFileName(tempFilePath);
                // 使用三元运算符检查目录是否存在，不存在则创建
                CheckAndCreateDirectory(System.IO.Path.GetDirectoryName(tempFilePath) + @"\lbc\");
                collection.Write(temppath);
                // 从临时文件加载 GIF 图片并转换为 BitmapImage
                BitmapImage gifBitmap = new BitmapImage();
                gifBitmap.BeginInit();
                gifBitmap.UriSource = new System.Uri(temppath);
                gifBitmap.CacheOption = BitmapCacheOption.OnLoad; // 确保在加载后立即释放文件
                gifBitmap.EndInit();
                gifBitmap.Freeze(); // 使 BitmapImage 可跨线程访问
                return gifBitmap;
            }
        }
    }
}
