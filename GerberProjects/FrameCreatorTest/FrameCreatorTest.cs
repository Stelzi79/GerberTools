﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GerberLibrary;
using GerberLibrary.Core.Primitives;

namespace FrameCreatorTest
{
    class FrameCreatorTest: ProgressLog
    {
        static void Main(string[] args)
        {
            string basepath = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(Path.Combine(basepath, "outline"));
            Directory.CreateDirectory(Path.Combine(basepath, "frame"));

            Directory.CreateDirectory(Path.Combine(basepath, "imagetest"));

            GerberFrameWriter.FrameSettings FS = new GerberFrameWriter.FrameSettings();
            PolyLine PL = new PolyLine();
            FS.FrameTitle = "Test Frame";
            FS.RenderSample = false;
            FS.margin = 3;

            PL.MakeRoundedRect(new PointD(10, 10), new PointD(400, 300), 7);
            FS.PositionAround(PL);
            //FS.offset = new PointD(200, 200);
            FS.RenderSample = true;

            GerberArtWriter GAW = new GerberArtWriter();
            GAW.AddPolyLine(PL);
            
            GAW.Write("outline/outtestinside.gko");
            
            Bitmap FrontPrint = (Bitmap)Bitmap.FromFile("BigJigImage.png");
            Bounds FrontBound = new Bounds() { BottomRight = new PointD(220, 230), TopLeft = new PointD(-220, -230) , Valid = true};

            PCBWriterSet s = new PCBWriterSet();
            s.TopSilk.WriteImageToBounds(FrontPrint, FrontBound, new Bounds() { TopLeft = new PointD(-10, -10), BottomRight = new PointD(10, 10),Valid = true });
            s.Write("imagetest", "test!");

//            GerberFrameWriter.WriteSideEdgeFrame(PL, FS, "frame/outtest");
  //          GerberFrameWriter.MergeFrameIntoGerberSet(Path.Combine(basepath, "frame"), Path.Combine(basepath, "outline"), Path.Combine(basepath, "mergedoutput"),FS, new FrameCreatorTest(),"testframe");

//            GerberFrameWriter.MergeFrameIntoGerberSet(Path.Combine(basepath, "SliceFrameOutline6"), Path.Combine(basepath, "Slice6"), Path.Combine(basepath, "slice6inframe"), FS, new FrameCreatorTest(),"slice6framed" );
//            PNL.SaveOutlineTo("panelized.gko", "panelcombinedgko.gko");

        }

        public override void AddString(string text, float progress = -1)
        {
            string output = "";
            foreach(var a in ActivityStack)
            {
                output += a + " -> ";
            }
            Console.WriteLine(output + text);
        }
    }
}
