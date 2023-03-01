using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MDKP
{
    internal class MDKPExperiments
    {
        string   mInstancesDirectory;
        string   mOutputDirectory;
        List<string> mFilenames;
        int      mNumInstances;


        public MDKPExperiments() {
        
        
        }

        void InitFiles() {
             mInstancesDirectory = "c:\\\\primeri\\\\MDKP\\\\";
            mOutputDirectory = "c:\\\\primeri\\\\MDKP\\\\Results\\\\";
             mFilenames = new List<string>();

                        mFilenames.Add("mknapcb2");
                        mFilenames.Add("mknapcb3");

            mFilenames.Add("mknapcb5");
            mFilenames.Add("mknapcb6");


            mFilenames.Add("mknapcb8");
            mFilenames.Add("mknapcb9");

            mNumInstances = 30;

        }


        public void SolveAll() {

            MDKPProblem TestProblem;
            MDKPInstance TestInstance = new MDKPInstance();

            int             PopupationSize        = 100;
            int             MaxItems              =100;
            double          LimitPerFix           = 0.1;
            int             MaxGeneratedSolutions = 500000;
            int             MaxStag = 50;
            long            TimeLimit             = 600 ;
            int             K = 5;


            //            TestInstance.Load_CB("c:\\primeri\\MDKP\\mknapcb9.txt", 20);
            //            MDKPCplex T = new MDKPCplex(TestInstance);
            //            T.TimeLimit = 400;
            //       MDKPFix Fix = new MDKPFix(63);
            //          T.Solve();

            //       int a = T.Solution.GetNumberOfUsedItems();
            //       int b = T.Solution.CalculateObjective();

            //            MDKPProblem Problem = new MDKPProblem(TestInstance);

            InitFiles();
            string cResDirectory;
            string configString = "Res_P" + PopupationSize + "_MI" + MaxItems + "_TLF" + LimitPerFix + "_K" + K+"_M"+ MaxStag;
            string OptFile;
            string tFileName;

            for (int s = 0; s < 10; s++)
            {
                foreach (string iFile in mFilenames)
                {

                    cResDirectory = mInstancesDirectory + configString + "\\\\";
                    if (!System.IO.Directory.Exists(cResDirectory))
                    {
                        System.IO.Directory.CreateDirectory(cResDirectory);
                    }

                    for (int i = 0; i < mNumInstances; i++)
                    {

                        TestInstance = new MDKPInstance();
                        TestInstance.Load_CB(mInstancesDirectory + iFile + ".txt", i);
                        TestProblem = new MDKPProblem(TestInstance);
                        TestProblem.InitRandom(s);
                        OptFile = mInstancesDirectory + "Res_" + TestInstance.NumItems + "_" + TestInstance.NumConstraints + ".csv";
                        TestInstance.LoadOptimum(OptFile, i);
                        TestProblem.TimeLimit = TimeLimit * 1000;

                        

                        tFileName = cResDirectory + "Res" + iFile + "_" + i + "_" + MaxStag + "_" + s + ".txt";

                        if (File.Exists(tFileName))
                            continue;
                        TestProblem.SolveFixSet(PopupationSize, K, MaxGeneratedSolutions, MaxItems, MaxStag, LimitPerFix);
                        TestProblem.SaveIntermediate(tFileName);

                    }
                }
            }
            
        }


        public void LoadAllResults(string FileName, int NumInstances,out double[][] Results, out string[] InstanceNames) {

            string[] Lines = File.ReadAllLines(FileName);
            string[] words;
            double cValue;

            Results = new double[NumInstances][];
            InstanceNames = new string[NumInstances];

            for (int i = 0; i < NumInstances; i++) {

                words = Lines[i].Split(',');

                InstanceNames[i] = words[0];

                Results[i] = new double[ words.Length - 1];
                double.TryParse(words[1], out Results[i][0]);
                for (int j = 2; j < words.Length; j++) {

                    double.TryParse(words[j], out cValue);
                    Results[i][j - 1] =  Results[i][0] - cValue;
                }

            }

        }

        public int GetBestSolution(string FileName) {

            double tResult = 0;
            int Result = -1;
            string[] Lines = File.ReadAllLines(FileName);
            string[] words = Lines[Lines.Length - 1].Split(' ');


            if (double.TryParse(words[0], out tResult))
            {
                Result = (int)Math.Round(tResult);
            }

            return Result;
        }

        public void GenerateTables(string OutputFile)
        {

            MDKPProblem TestProblem;
            MDKPInstance TestInstance = new MDKPInstance();

            int PopupationSize = 100;
            int MaxItems = 100;
            double LimitPerFix = 0.1;
            int MaxGeneratedSolutions = 500000;
            int MaxStag = 50;
            long TimeLimit = 600;
            int K = 5;

            StreamWriter F = new StreamWriter(OutputFile);
         


            //            TestInstance.Load_CB("c:\\primeri\\MDKP\\mknapcb9.txt", 20);
            //            MDKPCplex T = new MDKPCplex(TestInstance);
            //            T.TimeLimit = 400;
            //       MDKPFix Fix = new MDKPFix(63);
            //          T.Solve();

            //       int a = T.Solution.GetNumberOfUsedItems();
            //       int b = T.Solution.CalculateObjective();

            //            MDKPProblem Problem = new MDKPProblem(TestInstance);

            InitFiles();
            string cResDirectory;
            string configString = "Res_P" + PopupationSize + "_MI" + MaxItems + "_TLF" + LimitPerFix + "_K" + K + "_M" + MaxStag;
            string MethodsFile;
            string tFileName;
            double[][] MethodValues;
            double[] cValues;
            int cSolution;
            int Sum = 0;
            int Best = int.MinValue;
            int counter;
            double Avg;
            string[] InstanceNames;
            string Line;
            int NumBest;
            int NumAvg;


            foreach (string iFile in mFilenames)
                {

                     F.WriteLine(iFile);

                TestInstance = new MDKPInstance();
                TestInstance.Load_CB(mInstancesDirectory + iFile + ".txt", 0);

                MethodsFile = mInstancesDirectory + "DQPSORes_" + TestInstance.NumItems + "_" + TestInstance.NumConstraints + ".csv";
                LoadAllResults(MethodsFile, 30, out MethodValues, out InstanceNames);    
                for (int i = 0; i < mNumInstances; i++)
                    {
                        Sum = 0;
                        Best = int.MinValue;
                        counter = 0;

                        TestInstance = new MDKPInstance();
                        TestInstance.Load_CB(mInstancesDirectory + iFile + ".txt", i);

                        

                        for (int s = 0; s < 10; s++)
                            {
                                cResDirectory = mInstancesDirectory + configString + "\\\\";
                                tFileName = cResDirectory + "Res" + iFile + "_" + i + "_" + MaxStag + "_" + s + ".txt";

                                if (File.Exists(tFileName))
                                {

                                    cSolution = GetBestSolution(tFileName);

                                    Sum += cSolution;
                                    if (cSolution > Best)
                                        Best = cSolution;
                                    counter++;
                                }
                            
                        }

                        Avg = Sum / counter;
                        Avg = MethodValues[i][0] - Avg;
                        Best = (int)MethodValues[i][0] - Best;

//                        Line = iInstanceNames[i] + "&";
                    Line = i + "&";

                    if (TestInstance.NumItems == 500)
                    {

                        if (TestInstance.NumConstraints < 30)
                        {
                            NumBest = 7;
                            NumAvg = 4;
                        }
                        else {

                            NumBest = 6;
                            NumAvg = 3;
                        }
                    }
                    else {

                        NumBest = 5;
                        NumAvg = 3;

                    } 
                            
                        for (int k = 0; k < NumBest+1; k++) {
                            Line += (int)(MethodValues[i][k]) + "&";
                        }
                        //Line += Best + "&";
                        Line += Best + "\\\\";
                    /*
                                            for (int k = NumBest; k < NumBest+NumAvg; k++)
                                            {
                                                Line += MethodValues[i][k].ToString("0.00") + "&";
                                            }
                                            Line += Avg.ToString("0.00")+"\\\\";
                    */
                    F.WriteLine(Line);
                    }

             
            }
            F.Close();
        }


    }
}
