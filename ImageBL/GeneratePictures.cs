using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Rabbit.Enums;
using System.Drawing.Drawing2D;

namespace ImageBL
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class GeneratePictures
    {
        #region 函数

        /// <summary>
        /// 图片保存根目录
        /// </summary>
        private string _saveFile { get; set; }

        /// <summary>
        /// 大图片保存根目录
        /// </summary>
        private string _bigSaveFile { get; set; }

        /// <summary>
        /// 小图片保存根目录
        /// </summary>
        private string _smallSaveFile { get; set; }

        /// <summary>
        /// 社区id
        /// </summary>
        private int _communityId { get; set; }

        /// <summary>
        ///门id
        /// </summary>
        private int _doorId { get; set; }

        /// <summary>
        /// 社区名称
        /// </summary>
        private string _communityName { get; set; }

        /// <summary>
        /// 门名称
        /// </summary>
        private string _doorName { get; set; }

        /// <summary>
        /// 图片后缀名
        /// </summary>
        private string _imgExtension { get; set; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        private string _serverPath { get; set; }


        /// <summary>
        /// 大图片模板图片地址
        /// </summary>
        private string _bigBackPath { get; set; }


        /// <summary>
        /// 小图片模板图片地址
        /// </summary>
        private string _smallBackPath { get; set; }


        /// <summary>
        /// 字体
        /// </summary>
        private string fontType = "微软雅黑";

        /// <summary>
        /// 微信logo地址
        /// </summary>
        private string _wechatLog { get; set; }
        #endregion

        #region 构造函数


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="saveFile">保存根目录</param>
        /// <param name="communityId">_communityId</param>
        /// <param name="doorId">门id</param>
        /// <param name="communityName">社区名称</param>
        /// <param name="doorName">门名称</param>
        /// <param name="serverPath"   >服务器地址</param>
        /// <param name="bigBackPath">大图片模板图片地址</param>
        /// <param name="smallBackPath">小图片模板图片地址</param>
        public GeneratePictures(string saveFile, int communityId, int doorId, string communityName, string doorName, string serverPath, string bigBackPath, string smallBackPath)
        {
            _wechatLog = saveFile;
            _saveFile = saveFile;
            _communityId = communityId;
            _communityName = communityName;
            _doorId = doorId;
            _doorName = doorName;
            _serverPath = serverPath;
            _bigBackPath = bigBackPath;
            _smallBackPath = smallBackPath;
        }


        #endregion

        #region 方法


        /// <summary>
        /// 合成图片
        /// </summary>
        /// <param name="imgUrl">二维码图片地址</param>
        /// <param name="wqt">二维码类型</param>
        /// <returns></returns>
        public Dictionary<string, string> Generate(string imgUrl, WechatQrCodeType wqt)
        {
            _bigSaveFile = _smallSaveFile = _saveFile = _saveFile + _communityId + @"\";
            var smallPath = DownOriginalImg(imgUrl, _doorId.ToString(), _saveFile, "small", wqt);
            _imgExtension = Path.GetExtension(smallPath);
            Dictionary<string, string> d = new Dictionary<string, string>
            {
                {"original",smallPath.Replace(@"D:\AllWeb",_serverPath) },
                { "small",Small(smallPath,wqt).Replace(@"D:\AllWeb",_serverPath) },
                { "big", Big(DownOriginalImg(imgUrl, _doorId.ToString(), _saveFile,"big",wqt),wqt).Replace(@"D:\AllWeb",_serverPath) }
            };
            return d;
        }



        /// <summary>
        /// 大图片模板
        /// </summary>
        private string Big(string originalImgPath, WechatQrCodeType wqt)
        {
            string imgBackPath = _bigBackPath;
            string imgBackExtension = Path.GetExtension(originalImgPath);
            Image imgBack = Image.FromFile(imgBackPath);
            Image img = Image.FromFile(originalImgPath);
            Bitmap bmp = CombinImage(imgBack, img, 574, 468, 340, 340);
            Image wechatLogo = Image.FromFile(_wechatLog + @"wechatLog.png");
            bmp = CombinImageBig(bmp, wechatLogo, 705, 599, 80, 80);
            bmp = AddText(bmp, fontType, 35f, System.Configuration.ConfigurationManager.AppSettings["werchatQRTitle"], 575, 400, 340, 200, true);
            bmp = AddText(bmp, fontType, 28f, _doorName, 570, 820, 340, 200, true);
            bmp = AddText(bmp, fontType, 23f, _communityName, 680, 1290, 300, 100, false);
            bmp = AddText(bmp, fontType, 23f, System.Configuration.ConfigurationManager.AppSettings["communityNameNextText"], 680, 1330, 300, 100, false);
            bmp = PicSized(bmp, 1122,1674);
            bmp = UpdateResolution(bmp, 300f, 300f);
            MemoryStream ms = new MemoryStream();
            Directory.CreateDirectory(_bigSaveFile + @"big\");
            _bigSaveFile = _bigSaveFile + @"big\" + wqt.ToString() + "-" + _doorId + _imgExtension;
            bmp.Save(_bigSaveFile, ImageFormat.Jpeg);
            return _bigSaveFile;
        }



        /// <summary>
        /// 小图片模板
        /// </summary>
        /// <param name="originalImgPath"></param>
        /// <returns></returns>
        private string Small(string originalImgPath, WechatQrCodeType wqt)
        {
            string imgBackPath = _smallBackPath;
            string imgBackExtension = Path.GetExtension(originalImgPath);
            Image imgBack = Image.FromFile(imgBackPath);
            Image img = Image.FromFile(originalImgPath);
            Bitmap bmp = CombinImage(imgBack, img, 37, 155, 555, 555);
            Image wechatLogo = Image.FromFile(_wechatLog + @"wechatLog.png");
            bmp = CombinImageBig(bmp, wechatLogo, 257, 373, 120, 120);
            bmp = AddText(bmp, fontType, 40f, System.Configuration.ConfigurationManager.AppSettings["werchatSmallTitle"], 38, 720, 555, 100, true);
            bmp = AddText(bmp, fontType, 50f, _communityName, 35, 50, 555, 100, true);
            bmp = UpdateResolution(bmp, 300f, 300f);
            MemoryStream ms = new MemoryStream();
            Directory.CreateDirectory(_smallSaveFile + @"small\");
            _smallSaveFile = _smallSaveFile + @"small\" + wqt.ToString() + "-" + _doorId + _imgExtension;
            bmp.Save(_smallSaveFile, ImageFormat.Jpeg);
            return _smallSaveFile;
        }



        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="imgBack"></param>
        /// <param name="img"></param>
        /// <param name="xDeviation"></param>
        /// <param name="yDeviation"></param>
        /// <returns></returns>
        private Bitmap CombinImage(Image imgBack, Image img, int xDeviation = 0, int yDeviation = 0, int weight = 0, int height = 0)
        {
            Bitmap bmp = new Bitmap(imgBack.Width, imgBack.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);

            g.DrawImage(img, xDeviation, yDeviation, weight, height);
            GC.Collect();
            return bmp;
        }


        /// <summary>
        /// 合并图片 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="img"></param>
        /// <param name="xDeviation"></param>
        /// <param name="yDeviation"></param>
        /// <param name="weight"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Bitmap CombinImageBig(Bitmap bitmap, Image img, int xDeviation = 0, int yDeviation = 0, int weight = 0, int height = 0)
        {
            Bitmap bmp = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);

            g.DrawImage(img, xDeviation, yDeviation, weight, height);
            GC.Collect();
            return bmp;
        }

        /// <summary>
        /// 下载原始二维码图片
        /// </summary>
        /// <param name="url">二维码地址</param>
        /// <param name="name">id</param>
        /// <param name="savePath">保存基地址</param>
        /// <param name="firstName">名称前缀</param>
        /// <param name="wqt">二维码类型</param>
        /// <returns></returns>
        private string DownOriginalImg(string url, string name, string savePath, string firstName, WechatQrCodeType wqt)
        {
            WebClient myWebClient = new WebClient();
            Directory.CreateDirectory(savePath + @"Original\");
            var path = savePath + @"Original\" + firstName + "-" + wqt.ToString() + "-" + name + ".jpg";
            myWebClient.DownloadFile(new Uri(url), path);
            myWebClient.Dispose();
            return path;
        }


        /// <summary>
        /// 添加文字
        /// </summary>
        /// <param name="bitmap">图片流</param>
        /// <param name="fontType">字体</param>
        /// <param name="size">大小</param>
        /// <param name="text">要添加的文字</param>
        /// <param name="x">文字x轴</param>
        /// <param name="y">文字y轴</param>
        /// <param name="isCenter">是否局中</param>
        /// <returns></returns>
        private Bitmap AddText(Bitmap bitmap, string fontType, float size, string text, int x, int y, int weight, int height, bool isCenter)
        {
            Font theFont = new Font(fontType, size, FontStyle.Bold);
            Graphics g = Graphics.FromImage(bitmap);
            SolidBrush sbrush = new SolidBrush(Color.White);
            if (isCenter)
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center
                };
                g.DrawString(text, theFont, sbrush, new Rectangle(x, y, weight, height), sf);
            }
            else
            {

                StringFormat sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(text, theFont, sbrush, new Rectangle(x, y, weight, height), sf);
            }
            GC.Collect();
            return bitmap;
        }

        /// <summary>
        /// 修改DPI
        /// </summary>
        /// <param name="bitmap">图片流</param>
        /// <param name="xDPI">x坐标dpi</param>
        /// <param name="yDPI">y坐标dpi</param>
        /// <returns></returns>
        private Bitmap UpdateResolution(Bitmap bitmap, float xDPI, float yDPI)
        {
            bitmap.SetResolution(xDPI, yDPI);
            return bitmap;
        }


        /// <summary>
        /// 修改大小
        /// </summary>
        /// <param name="bitmap">原始图片流</param>
        /// <param name="xWight">想要的宽</param>
        /// <param name="xHeight">想要的高</param>
        /// <returns></returns>
        private Bitmap UpdateSize(Bitmap bitmap,int xWight,int xHeight)
        {
            Graphics g = Graphics.FromImage(bitmap);

            // 插值算法的质量
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(bitmap, new Rectangle(0, 0, xWight, xHeight), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            g.Dispose();
            return bitmap;
        }


        /// <summary>
        /// 剪裁
        /// </summary>
        /// <param name="b"></param>
        /// <param name="StartX"></param>
        /// <param name="StartY"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public static Bitmap KiCut(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;

            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }

            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }

            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);

                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();

                return bmpOut;
            }
            catch
            {
                return null;
            }
        }



        /// <summary> 
        /// 放大缩小图片尺寸 
        /// </summary> 
        /// <param name="bitmap">原始图片</param> 
        /// <param name="w">目标宽度</param>
        /// <param name="h">目标高度</param>
        public Bitmap PicSized(Bitmap bitmap,int w,int h)
        {
            Bitmap resizedBmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(resizedBmp);
            //设置高质量插值法   
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度   
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            //消除锯齿 
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(bitmap, new Rectangle(0, 0, w, h), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            g.Dispose();
            return resizedBmp;
        }

        #endregion
    }
}
