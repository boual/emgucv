using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.UI;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Globalization;
using System.Threading;

namespace Emgu.CV.Test
{
   public class Tester
   {
      public void TestRotationMatrix2D()
      {
         RotationMatrix2D<float> mat = new RotationMatrix2D<float>(new PointF(1, 2), 30, 1);
         RotationMatrix2D<double> mat2 = new RotationMatrix2D<double>(new PointF(1, 2), 30, 1);
         //Trace.WriteLine(Emgu.Toolbox.MatrixToString<float>(mat.Data, ", ", ";\r\n"));
      }

      public void GenerateLogo()
      {
         Image<Bgr, Byte> semgu = new Image<Bgr, byte>(160, 72, new Bgr(0, 0, 0));
         Image<Bgr, Byte> scv = new Image<Bgr, byte>(160, 72, new Bgr(0, 0, 0));
         MCvFont f1 = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 1.5, 1.5);
         MCvFont f2 = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX, 1.6, 2.2);
         semgu.Draw("Emgu", ref f1, new Point(6, 50), new Bgr(55, 155, 255));
         semgu._Dilate(1);
         scv.Draw("CV", ref f2, new Point(50, 60), new Bgr(255, 55, 255));
         scv._Dilate(2);
         Image<Bgr, Byte> logoBgr = semgu.Or(scv);
         Image<Gray, Byte> logoA = new Image<Gray, byte>(logoBgr.Width, logoBgr.Height);
         logoA.SetValue(255, logoBgr.Convert<Gray, Byte>());
         logoBgr._Not();
         logoA._Not();
         Image<Gray, Byte>[] channels = logoBgr.Split();
         channels = new Image<Gray, byte>[] { channels[0], channels[1], channels[2], new Image<Gray, Byte>(channels[0].Width, channels[0].Height, new Gray(255.0)) };
         Image<Bgra, Byte> logoBgra = new Image<Bgra, byte>(channels);
         logoBgra.SetValue(new Bgra(0.0, 0.0, 0.0, 0.0), logoA);
         logoBgra.Save("EmguCVLogo.gif");

         Image<Bgr, Byte> bg_header = new Image<Bgr, byte>(1, 92);
         for (int i = 0; i < 92; i++)
            bg_header[i, 0] = new Bgr(210, 210 - i * 0.4, 210 - i * 0.9);
         bg_header.Save("bg_header.gif");
      }

      /*
      public void TestPointPerformance()
      {
         #region test point constructors
         int numberOfPoints = 100000;
         Stopwatch stopwatch = Stopwatch.StartNew();
         Point2D<int>[] pts = new Point2D<int>[numberOfPoints];
         for (int i = 0; i < pts.Length; i++)
            pts[i] = new Point2D<int>();
         stopwatch.Stop();
         Trace.WriteLine("Point2D creation: " + stopwatch.ElapsedMilliseconds + " milliseconds.");
         stopwatch.Reset(); stopwatch.Start();
         System.Drawing.Point[] mpts = new System.Drawing.Point[numberOfPoints];
         stopwatch.Stop();
         Trace.WriteLine("System.Drawing.Point creation: " + stopwatch.ElapsedMilliseconds + " milliseconds.");
         #endregion

         #region System.Drawing.Point example
         System.Drawing.Point p1 = new System.Drawing.Point(0, 0); 

         System.Drawing.Point[] ptsArray = new System.Drawing.Point[1]; //structure (value type) in array are initialized, ptsArray[0] has been allocated to a default point (0,0)
         ptsArray[0] = p1; //ptsArray[0] now contains a copy of p1 (0, 0)
         p1.X = 1; //change the value on p1, now p1 is (1, 0)
         Trace.WriteLine("difference in X: " + (p1.X - ptsArray[0].X)); //ptsArray[0] returns a copy of the point (0,0)
         #endregion

         int numberOfPointsInArray = 1000000;
         int numberOfReadyAccess = 1000;

         stopwatch.Reset(); stopwatch.Start();
         //initialize Point2D array
         Point2D<int>[] pointArray = new Point2D<int>[numberOfPointsInArray];
         for (int i = 0; i < pointArray.Length; i++)
            pointArray[i] = new Point2D<int>();

         for (int j = 0; j < numberOfReadyAccess; j++)
            for (int i = 0; i < pointArray.Length; i++)
            {
               pointArray[i].X = 2;
            }
         stopwatch.Stop();
         Trace.WriteLine("Time to access elements in Point2D<int> array: " + stopwatch.ElapsedMilliseconds);

         stopwatch.Reset(); stopwatch.Start();
         //initialize System.Drawing.Point array
         System.Drawing.Point[] pointStructures = new System.Drawing.Point[numberOfPointsInArray];

         for (int j = 0; j < numberOfReadyAccess; j++)
            for (int i = 0; i < pointStructures.Length; i++)
            {
               pointStructures[i].X = 2;
            }
         stopwatch.Stop();
         Trace.WriteLine("Time to access elements in System.Drawing.Point array: " + stopwatch.ElapsedMilliseconds);

      }*/

      public void TestCvNamedWindow1()
      {
         //The name of the window
         String win1 = "Test Window";

         //Create the window using the specific name
         CvInvoke.cvNamedWindow(win1);

         //Create an image of 400x200 of Blue color
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 200, new Bgr(255, 0, 0))) 
         {
            //Create the font
            MCvFont f = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX, 1.0, 1.0);
            //Draw "Hello, world." on the image using the specific font
            img.Draw("Hello, world", ref f, new Point(10, 80), new Bgr(0, 255, 0));

            //Show the image
            CvInvoke.cvShowImage(win1, img.Ptr);
            //Wait for the key pressing event
            CvInvoke.cvWaitKey(0);
            //Destory the window
            CvInvoke.cvDestroyWindow(win1); 
         }
      }

      public void TestCvNamedWindow2()
      {
         //Create an image of 400x200 of Blue color
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 200, new Bgr(255, 0, 0))) 
         {
            //Create the font
            MCvFont f = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX, 1.0, 1.0);

            //Draw "Hello, world." on the image using the specific font
            img.Draw("Hello, world", ref f, new Point(10, 80), new Bgr(0, 255, 0)); 
            
            //Show the image using ImageViewer from Emgu.CV.UI
            ImageViewer.Show(img, "Test Window");
         }
      }

      public void TestConvert()
      {
         Image<Gray, Single> g = new Image<Gray, Single>(80, 40);
         Image<Gray, Single> g2 = g.Convert<Single>(delegate(Single v, int x, int y) { return System.Convert.ToSingle(Math.Sqrt(0.0 + x * x + y * y)); });
         ImageViewer.Show(g2);
      }

      public void TestHorizontalLine()
      {
         Point p1 = new Point(10, 10);
         Point p2 = new Point(20, 10);
         LineSegment2D l1 = new LineSegment2D(p1, p2);
         Image<Bgr, Byte> img = new Image<Bgr, byte>(200, 400, new Bgr(255, 255, 255));
         img.Draw(l1, new Bgr(0.0, 0.0, 0.0), 1);
         ImageViewer.Show(img);
      }
      
      public void TestRectangle()
      {
         PointF p1 = new PointF(1.1f, 2.2f);
         SizeF p2 = new SizeF(2.2f, 4.4f);
         RectangleF rect = new RectangleF();
         rect.Location = PointF.Empty;
         rect.Size = p2;

         Map<Gray, Byte> map = new Map<Gray, Byte>(new RectangleF(PointF.Empty, new SizeF(4.0f, 8.0f)), new PointF(0.1f, 0.1f), new Gray(255.0));
         map.Draw(rect, new Gray(0.0), 1);

         ImageViewer.Show(map);
      }

      public void TestModuleInfo()
      {
         string pluginName;
         string versionName;
         Emgu.CV.Util.GetModuleInfo(out pluginName, out versionName);
         Trace.WriteLine(String.Format("Plugin: {0}\r\nVersion: {1}", pluginName, versionName ));
      }

      public void TestRandom()
      {
         using (Image<Bgr, byte> img = new Image<Bgr, byte>(200, 200))
         {
            img.SetRandNormal(0xffffffff, new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(50.0, 50.0, 50.0));
            ImageViewer.Show(img);
         }
      }

      public void CameraTest()
      {
         ImageViewer viewer = new ImageViewer(); //create an image viewer
         Capture capture = new Capture(); //create a camera captue
         Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
         {  //run this until application closed (close button click on image viewer)
            viewer.Image = capture.QueryFrame(); //draw the image obtained from camera
         });
         viewer.ShowDialog(); //show the image viewer
      }

      public void CameraTest2()
      {
         ImageViewer viewer = new ImageViewer();
         Capture capture = new Capture();
         Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
         {
            Image<Bgr, Byte> img = capture.QueryFrame();
            img = img.Resize(0.8, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Image<Gray, Byte> gray = img.Convert<Gray, Byte>();
            gray._EqualizeHist();

            viewer.Image = gray;
         });
         viewer.ShowDialog();
      }

      public void TestImageLoader()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         using (Image<Bgr, Single> img = new Image<Bgr, Single>("stuff.jpg"))
         using (Image<Bgr, Single> img2 = img.Resize(100, 100, CvEnum.INTER.CV_INTER_AREA, true))
         {
            Application.Run(new ImageViewer(img2));
            Rectangle r = img2.ROI;
            r.Width >>= 1;
            r.Height >>= 1;
            img2.ROI = r;
            ImageViewer.Show(img2);
         }
      }

      public void TestBgr()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 100, new Bgr(0, 100, 200)))
         {
            Application.Run(new ImageViewer(img));
            Image<Gray, Byte>[] channels = img.Split();
            foreach (Image<Gray, Byte> i in channels)
               Application.Run(new ImageViewer(i));
         }
      }

      public void TestBgra()
      {
         Image<Bgra, Byte> img = new Image<Bgra, byte>(100, 100);
         img.SetValue(new Bgra(255.0, 120.0, 0.0, 120.0));
         Image<Gray, Byte>[] channels = img.Split();
         foreach (Image<Gray, Byte> i in channels)
            Application.Run(new ImageViewer(i));
         Application.Run(new ImageViewer(img));
      }

      public void TestFont()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         using (Image<Gray, Byte> img = new Image<Gray, Byte>(200, 300, new Gray()))
         {
            MCvFont f = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX_SMALL, 1.0, 1.0);
            {
               img.Draw("h.", ref f, new Point(100, 10), new Gray(255.0));
               img.Draw("a.", ref f, new Point(100, 50), new Gray(255.0));
            }
            ImageViewer.Show(img);
         }
      }

      public void TestHistogram()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>("stuff.jpg"))
         using (Image<Hsv, Byte> img2 = img.Convert<Hsv, Byte>())
         {
            Image<Gray, Byte>[] HSVs = img2.Split();

            using (Histogram h = new Histogram(20, new RangeF(0, 180)))
            {
               h.Calculate(new Image<Gray, Byte>[1] { HSVs[0] }, true, null);
               using (Image<Gray, Byte> bpj = h.BackProject(new Image<Gray, Byte>[1] { HSVs[0] }))
                  ImageViewer.Show(bpj);
            }

            foreach (Image<Gray, Byte> i in HSVs) i.Dispose();
         }
      }

      public void TestSplitMerge()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>("stuff.jpg"))
         {
            using (Image<Hsv, Byte> imgHsv = img.Convert<Hsv, Byte>())
            {
               Image<Gray, Byte>[] imgs = imgHsv.Split();
               using (Image<Hsv, Byte> imgHsv2 = new Image<Hsv, Byte>(imgs))
               {
                  using (Image<Bgr, Byte> imageRGB = imgHsv2.Convert<Bgr, Byte>())
                  {
                     LineSegment2D[][] lines = imgHsv2.HoughLines(
                         new Hsv(50.0, 50.0, 50.0), new Hsv(200.0, 200.0, 200.0),
                         1, Math.PI / 180.0, 50, 50, 10);

                     CircleF[][] circles = img.HoughCircles(
                         new Bgr(200.0, 200.0, 200.0), new Bgr(100.0, 100.0, 100.0),
                         4.0, 1.0, 0, 0);

                     for (int i = 0; i < lines[0].Length; i++)
                     {
                        imageRGB.Draw(lines[0][i], new Bgr(255.0, 0.0, 0.0), 1);
                     }

                     for (int i = 0; i < lines[1].Length; i++)
                     {
                        imageRGB.Draw(lines[1][i], new Bgr(0.0, 255.0, 0.0), 1);
                     }

                     for (int i = 0; i < lines[2].Length; i++)
                     {
                        imageRGB.Draw(lines[2][i], new Bgr(0.0, 0.0, 255.0), 1);
                     }

                     foreach (CircleF[] cs in circles)
                        foreach (CircleF c in cs)
                           imageRGB.Draw(c, new Bgr(0.0, 0.0, 0.0), 1);

                     Application.Run(new ImageViewer(imageRGB));

                     bool applied = false;
                     foreach (CircleF[] cs in circles)
                        foreach (CircleF c in cs)
                        {
                           if (!applied)
                           {
                              CircleF cir = c;
                              cir.Radius += 30;
                              using (Image<Gray, Byte> mask = new Image<Gray, Byte>(imageRGB.Width, imageRGB.Height, new Gray(0.0)))
                              {
                                 mask.Draw(cir, new Gray(255.0), -1);

                                 using (Image<Bgr, Byte> res = imageRGB.InPaint(mask, 50))
                                 {

                                 }
                              }
                              applied = true;
                           }
                        }
                  }
               }

               foreach (Image<Gray, Byte> i in imgs)
                  i.Dispose();
            }
         }
      }

      public void TestHaarPerformance()
      {
         HaarCascade face = new HaarCascade(".\\haarcascades\\haarcascade_frontalface_alt2.xml");
         Image<Gray, Byte> img = new Image<Gray, byte>("lena.jpg");
         Stopwatch watch = Stopwatch.StartNew();
         img.DetectHaarCascade(face);
         watch.Stop();
         Trace.WriteLine(String.Format("Detecting face from {0}x{1} image took: {2} milliseconds.", img.Width, img.Height, watch.ElapsedMilliseconds));
      }

      public void TestFaceDetect()
      {
         using (Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg"))
         using (Image<Bgr, Byte> smooth = image.SmoothGaussian(7))
         {
            DateTime t1 = DateTime.Now;

            FaceDetector fd = new FaceDetector();
            Face f = fd.Detect(smooth)[0];
            TimeSpan ts = DateTime.Now.Subtract(t1);
            Trace.WriteLine(ts.TotalMilliseconds);

            Eye e = f.DetectEye()[0];

            //Application.Run(new ImageViewer(e.RGB));

            /*
            Image<Rgb, Byte> res = f.RGB.BlankClone();
            res.Draw(f.SkinContour, new Rgb(255.0, 255.0, 255.0), new Rgb(255.0, 255.0, 255.0), -1);
            Application.Run(new ImageViewer(res.ToBitmap()));
            */
         }
      }

      public void TestCompression()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg");
         DateTime t1 = DateTime.Now;
         int l1 = image.Bytes.Length;
         DateTime t2 = DateTime.Now;
         image.SerializationCompressionRatio = 9;
         int l2 = image.Bytes.Length;
         DateTime t3 = DateTime.Now;
         TimeSpan ts1 = t2.Subtract(t1);
         TimeSpan ts2 = t3.Subtract(t2);
         Trace.WriteLine(String.Format("Image Size: {0} x {1}", image.Width, image.Height));
         Trace.WriteLine(String.Format("T1: {0}, T2: {1}, Delta: {2}", ts1.TotalMilliseconds, ts2.TotalMilliseconds, ts2.TotalMilliseconds - ts1.TotalMilliseconds));

         Trace.WriteLine(
             String.Format(
             "Original size: {0}; Compressed Size: {1}, Compression Ratio: {2}%",
             l1,
             l2,
             l2 * 100.0 / l1));
      }

      public void TestMarshalIplImage()
      {
         Image<Bgr, Single> image = new Image<Bgr, float>(2041, 1023);
         DateTime timeStart = DateTime.Now;
         for (int i = 0; i < 10000; i++)
         {
            MIplImage img = image.MIplImage;
         }
         TimeSpan timeSpan = DateTime.Now.Subtract(timeStart);
         Trace.WriteLine(String.Format("Time: {0} milliseconds", timeSpan.TotalMilliseconds));
      }

      public void TestReadImage()
      {
         ImageViewer.Show(new Image<Gray, Byte>("lena.jpg"));
         ImageViewer.Show(new Image<Bgr, Byte>("lena.jpg").Convert<Gray, Byte>());
      }

      public void TestImageViewer()
      {
         System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo("zh-CN");
         Application.Run(new ImageViewer(null));
      }

      /*
      public void TestArrayChangeDimension()
      {
         float[] vec = new float[10];
         for (int i = 0; i < vec.Length; i++)
            vec[i] = (float)i;

         float[,] vec2 = new float[vec.Length, 1];

         GCHandle hdl1 = GCHandle.Alloc(vec, GCHandleType.Pinned);
         GCHandle hdl2 = GCHandle.Alloc(vec2, GCHandleType.Pinned);
         Emgu.Util.Toolbox.memcpy(hdl1.AddrOfPinnedObject(), hdl2.AddrOfPinnedObject(), vec.Length * Marshal.SizeOf(typeof(float)));
         hdl1.Free();
         hdl2.Free();
         //Array.Copy(vec, vec2, vec.Length);

         for (int i = 0; i < vec.Length; i++)
         {
            Assert.AreEqual(vec[i], vec2[i, 0]);
         }
      }*/

      
      private void TestMap()
      {
         PointF center = new PointF(-110032, -110032);
         float width = 10000, height = 12000;


         Map<Gray, Byte> map = new Map<Gray, byte>(new RectangleF(center.X - width/2, center.Y - height/2, width, height), new PointF(100, 100));
         PointF[] pts = new PointF[]
            {
                new PointF( (float)center.X + 3120,(float) center.Y + 2310),
                new PointF((float)center.X -220, (float) center.Y-4120)
            };
         map.DrawPolyline(pts, false, new Gray(255.0), 1);
         Triangle2DF tri = new Triangle2DF(
             new PointF((float)center.X - 1000.0f, (float)center.Y + 200.0f),
             new PointF((float)center.X - 3000.0f, (float)center.Y + 200.0f),
             new PointF((float)center.X - 700f, (float)center.Y + 800.0f));
         map.Draw(tri, new Gray(80), 0);
         map.Draw(tri, new Gray(255), 1);
         ImageViewer.Show(map);
      }

      private class SyntheticData
      {
         private Matrix<float> _state;
         private Matrix<float> _transitionMatrix;
         private Matrix<float> _measurementNoise;
         private Matrix<float> _processNoise;
         private Matrix<float> _errorCovariancePost;
         private Matrix<float> _measurementMatrix;
         public Matrix<float> MeasurementMatrix
         {
            get
            {
               return _measurementMatrix;
            }
         }
         public Matrix<float> TransitionMatrix
         {
            get
            {
               return _transitionMatrix;
            }
         }

         public Matrix<float> State
         {
            get
            {
               return _state;
            }
         }
         public Matrix<float> MeasurementNoise
         {
            get
            {
               return _measurementNoise;
            }
         }
         public Matrix<float> ProcessNoise
         {
            get
            {
               return _processNoise;
            }
         }
         public Matrix<float> ErrorCovariancePost
         {
            get
            {
               return _errorCovariancePost;
            }
         }

         public SyntheticData()
         {
            _state = new Matrix<float>(2, 1);
            // start with random position and velocity
            //_state.SetRandNormal(new MCvScalar(0.0), new MCvScalar(1.0));
            _state[0, 0] = 0.0f;
            _state[1, 0] = 0.05f;

            _measurementNoise = new Matrix<float>(1, 1);
            _measurementNoise.SetIdentity(new MCvScalar(1.0e-2));
            _processNoise = new Matrix<float>(2, 2);
            _processNoise.SetIdentity(new MCvScalar(1.0e-5));
            _errorCovariancePost = new Matrix<float>(2, 2);
            _errorCovariancePost.SetIdentity();
            _transitionMatrix = new Matrix<float>(new float[,] { { 1, 1 }, { 0, 1 } }); // phi_t = phi_{t-1} + delta_phi
            _measurementMatrix = new Matrix<float>(new float[,] { {1, 0}});
            _measurementMatrix.SetIdentity(); //the measurement is [ phi ]
         }

         public Matrix<float> GetMeasurement()
         {
            Matrix<float> measurementNoise = new Matrix<float>(1, 1);
            measurementNoise.SetRandNormal(new MCvScalar(), new MCvScalar(Math.Sqrt(MeasurementNoise[0, 0])));
            return MeasurementMatrix * _state + measurementNoise;
         }

         public void GoToNextState()
         {
            Matrix<float> processNoise = new Matrix<float>(2, 1);
            processNoise.SetRandNormal(new MCvScalar(), new MCvScalar(Math.Sqrt(ProcessNoise[0, 0])));
            _state = TransitionMatrix * _state + processNoise;
         }
      }

      public void TestPoint()
      {
         Point p = new Point(1, 2);
         XmlDocument d = Emgu.Util.Toolbox.XmlSerialize<Point>(p);
         Point p2 = Emgu.Util.Toolbox.XmlDeserialize<Point>(d);
         Assert.AreEqual(p, p2);

         Size s = new Size(1, 2);
         d = Emgu.Util.Toolbox.XmlSerialize<Size>(s);
         Trace.WriteLine(d.InnerXml);

         Rectangle r = new Rectangle(1, 2, 3, 4);
         d = Emgu.Util.Toolbox.XmlSerialize<Rectangle>(r);
         Trace.WriteLine(d.InnerXml);
      }

      public void TestNegate()
      {
         Assert.AreEqual(~1, -2);
         Assert.AreEqual(~(-1), 0);
         //Assert.AreEqual(~3, 2);
      }

      public void TestReflection()
      {
         MethodInfo[] members = typeof(Emgu.CV.CvInvoke).GetMethods();
         foreach (MethodInfo member in members)
         {
            object[] pinvokeAtts = member.GetCustomAttributes(typeof(DllImportAttribute), false);
            if (pinvokeAtts.Length > 0) // if this is a PInvoke call
            {
               DllImportAttribute att = pinvokeAtts[0] as DllImportAttribute;
               Type[] types = Array.ConvertAll<ParameterInfo, Type>(member.GetParameters(), delegate(ParameterInfo p) { return p.ParameterType; });
               //System.Reflection.Emit.DynamicMethod m = new System.Reflection.Emit.DynamicMethod("_"+member.Name, member.Attributes, member.CallingConvention, member.ReturnType, types, typeof(CvInvoke), true);
            }
         }
      }
      /*
      private class CustomDllImportAttribute : DllImportAttribute
      {
      }*/

      public void TestDiv2()
      {
         int total = 1000000000;
         int b = 0;
         Stopwatch watch = Stopwatch.StartNew();
         for (int i = 0; i < total; i++)
         {
            b += (i / 2) * 2;
            //b /= 2;
         }
         watch.Stop();
         Trace.WriteLine(watch.ElapsedMilliseconds);

         b = 0;
         watch.Reset(); watch.Start();
         for (int i = 0; i < total; i++)
         {
            b += (i >> 1 ) << 1;
            //b >>= 1;
         }
         Trace.WriteLine(watch.ElapsedMilliseconds);

      }

      public void TestCodeBookBGModel()
      {
         using (Capture capture = new Capture())
         using (BGCodeBookModel<Bgr> model = new BGCodeBookModel<Bgr>())
         {
            ImageViewer viewer = new ImageViewer();
            Image<Gray, byte> fgMask = capture.QueryFrame().Convert<Gray, Byte>();

            Application.Idle += delegate(Object sender, EventArgs args)
            {
               Image<Bgr, Byte> frame = capture.QueryFrame();
               model.Update(frame);
               viewer.Image = model.ForgroundMask; 
            };
            viewer.ShowDialog();
         }
      }

      public void TestBlobTracking()
      {
         Capture capture = new Capture();
        
         ImageViewer viewer = new ImageViewer();

         BlobTrackerAutoParam<Bgr> param = new BlobTrackerAutoParam<Bgr>();
         //param.BlobDetector = new BlobDetector(Emgu.CV.CvEnum.BLOB_DETECTOR_TYPE.CC);
         param.ForgroundDetector = new FGDetector<Bgr>(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
         //param.BlobTracker = new BlobTracker(Emgu.CV.CvEnum.BLOBTRACKER_TYPE.CCMSPF);
         param.FGTrainFrames = 10;
         BlobTrackerAuto<Bgr> tracker = new BlobTrackerAuto<Bgr>(param);

         MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);

         Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
         {
            tracker.Process(capture.QuerySmallFrame().PyrUp());
            
            Image<Gray, Byte> img = tracker.ForgroundMask;
            //viewer.Image = tracker.GetForgroundMask();
            foreach (MCvBlob blob in tracker)
            {
               img.Draw(Rectangle.Round(blob), new Gray(255.0), 2);
               img.Draw(blob.ID.ToString(), ref font, Point.Round(blob.Center), new Gray(255.0));
            }
            viewer.Image = img;
         });
         viewer.ShowDialog();
         
      }

      public void TestKalman()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 400);

         SyntheticData syntheticData = new SyntheticData();

         // state is (phi, delta_phi) - angle and angle increment 
         Matrix<float> state = new Matrix<float>(new float[] { 0.0f, 0.0f}); //initial guess

         #region initialize Kalman filter
         Kalman tracker = new Kalman(2, 1, 0);
         tracker.TransitionMatrix = syntheticData.TransitionMatrix;
         tracker.MeasurementMatrix = syntheticData.MeasurementMatrix;
         tracker.ProcessNoiseCovariance = syntheticData.ProcessNoise;
         tracker.MeasurementNoiseCovariance = syntheticData.MeasurementNoise;
         tracker.ErrorCovariancePost = syntheticData.ErrorCovariancePost;
         tracker.CorrectedState = state;
         #endregion 

         System.Converter<double, PointF> angleToPoint =
            delegate(double radianAngle)
            {
               return new PointF(
                  (float)(img.Width / 2 + img.Width / 3 * Math.Cos(radianAngle)),
                  (float)(img.Height / 2 - img.Width / 3 * Math.Sin(radianAngle)));
            };

         Toolbox.Action<PointF, Bgr> drawCross =
           delegate(PointF point, Bgr color)
           {
              img.Draw(new Cross2DF(point, 15, 15), color, 1);
           };

         ImageViewer viewer = new ImageViewer();
         System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
         timer.Interval = 1000;
         timer.Tick += new EventHandler(delegate(object sender, EventArgs e)
         {
            Matrix<float> measurement = syntheticData.GetMeasurement();
            // adjust Kalman filter state 
            tracker.Correct(measurement);

            tracker.Predict();

            #region draw the state, prediction and the measurement
            PointF statePoint = angleToPoint(tracker.CorrectedState[0, 0]);
            PointF predictPoint = angleToPoint(tracker.PredictedState[0, 0]);
            PointF measurementPoint = angleToPoint(measurement[0, 0]);

            img.SetZero(); //clear the image
            drawCross(statePoint, new Bgr(Color.White)); //draw current state in White
            drawCross(measurementPoint, new Bgr(Color.Red)); //draw the measurement in Red
            drawCross(predictPoint, new Bgr(Color.Green)); //draw the prediction (the next state) in green 
            img.Draw(new LineSegment2DF(statePoint, predictPoint), new Bgr(Color.Magenta), 1); //Draw a line between the current position and prediction of next position 

            //Trace.WriteLine(String.Format("Velocity: {0}", tracker.CorrectedState[1, 0]));
            #endregion

            syntheticData.GoToNextState();

            viewer.Image = img;
         });
         timer.Start();
         viewer.Disposed += delegate(Object sender, EventArgs e) { timer.Stop(); };
         viewer.Text = "Actual State: White; Measurement: Red; Prediction: Green";
         viewer.ShowDialog();
      }
   }
}
