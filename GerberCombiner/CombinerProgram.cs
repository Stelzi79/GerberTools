﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GerberLibrary;
using GerberLibrary.Core;

namespace GerberCombiner
{
    class CombinerProgram
    {
        static void Main(string[] args)
        {
            

            //GerberSplitter GS = new GerberSplitter();
            //GS.Split("G03*", new GerberLibrary.Core.Primitives.GerberNumberFormat());
            //GS.Split("%SRX1Y1I0.00000J1.76100*%", new GerberLibrary.Core.Primitives.GerberNumberFormat(),true);
            //Console.WriteLine("{0}", GS.Pairs.Count);
            Gerber.ShowProgress = true;
            Gerber.DumpSanitizedOutput = true;
            Gerber.WriteSanitized = true;

            if (args.Count() < 2)
            {
                Console.WriteLine("usage: ");
                Console.WriteLine("GerberCombiner <outputfile> <inputfile1> <inputfile2> <inputfileN> ");
                Console.WriteLine("<outputfile>: file to write - if the outputfile extension is .txt, all input files will be treated as excellon");
                Console.WriteLine("<inputfileN>: files to load");

                return;
            }
            for (int j = 1; j < args.Count(); j++)
            {
                if (File.Exists(args[j]) == false)
                {
                    Console.WriteLine("file not found! {0}", args[j]);
                    return;
                }
            }
            BoardSide S;
            BoardLayer L;

            var FileType = Gerber.FindFileType(args[1]);
            Gerber.DetermineBoardSideAndLayer(args[1], out S, out L);

            for (int j = 2; j < args.Count(); j++)
            {
                var FileType2 = Gerber.FindFileType(args[1]);

                Gerber.DetermineBoardSideAndLayer(args[1], out S, out L);
                if (FileType2 != FileType)
                {
                    Console.WriteLine("Warning! Filetypes seem to be mismatched! First file ({0}) is a {1}, but {2} is a {3}", args[1], FileType, args[j], FileType2);
                }
            }

            if (FileType == BoardFileType.Drill)
            {
                List<string> ExcellonFiles = new List<string>();
                ExcellonFiles.AddRange(args.Skip(1));
                ExcellonFile.MergeAll(ExcellonFiles, args[0], new StandardConsoleLog());
            }
            else
            {
                List<string> GerberFiles = new List<string>();
                GerberFiles.AddRange(args.Skip(1));
                GerberMerger.MergeAll(GerberFiles, args[0], new StandardConsoleLog());
            }
          //  Console.WriteLine("Press any key to continue..");
         //  Console.ReadKey();
        }
                
    }
}
