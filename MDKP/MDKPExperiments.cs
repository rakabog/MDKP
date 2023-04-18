using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public void GenerateParamResultFile(string OutputFolder, int iInstance, int iItems  ) {

            int[] PopulationSize = { 50, 100, 200 };
            int[] MaxItems = { 25, 50, 100 };
            double LimitPerFix = 0.1;
            int MaxGeneratedSolutions = 500000;
            int[] MaxStag = { 25, 50, 100 };
            long TimeLimit = 600;
            string[] TestFiles = { "mknapcb8", "mknapcb9", "mknapcb5", "mknapcb6" };
            int[] InstanceIndex = { 0, 0, 0, 0 };
            int NumRuns = 10;
            int K = 5;
            InitFiles();
            string cResDirectory;
            string configString;
            string OptFile;
            string tFileName;
            MDKPProblem TestProblem;
            MDKPInstance TestInstance = new MDKPInstance();
            StreamWriter F = new StreamWriter(OutputFolder);
            int Res;



            //            for (int iInstance = 0; iInstance < InstanceIndex.Length; iInstance++)
            //            {
            for (int iPop = 0; iPop < PopulationSize.Length; iPop++)
            {

                for (int iMaxStag = 0; iMaxStag < MaxStag.Length; iMaxStag++)
                    {

//                            for (int iItems = 0; iItems < MaxItems.Length; iItems++)
//                            {

                            configString = "Res_P" + PopulationSize[iPop] + "_MI" + MaxItems[iItems] + "_TLF" + LimitPerFix + "_K" + K + "_M" + MaxStag[iMaxStag];
//                            F.WriteLine(configString);
                                for (int s = 0; s < 10; s++)
                                {


                                    
                                    cResDirectory = mInstancesDirectory + configString + "\\\\";
                                    if (!System.IO.Directory.Exists(cResDirectory))
                                    {
                                        System.IO.Directory.CreateDirectory(cResDirectory);
                                    }


                                    TestInstance = new MDKPInstance();
                                    TestInstance.Load_CB(mInstancesDirectory + TestFiles[iInstance] + ".txt", InstanceIndex[iInstance]);
                                    TestProblem = new MDKPProblem(TestInstance);
                                    TestProblem.InitRandom(s);
                                    OptFile = mInstancesDirectory + "Res_" + TestInstance.NumItems + "_" + TestInstance.NumConstraints + ".csv";
                                    if (iInstance > 1)
                                    {
                                        TestInstance.LoadOptimum(OptFile, InstanceIndex[iInstance]);
                                    }
                                    TestProblem.TimeLimit = TimeLimit * 1000;




                                tFileName = cResDirectory + "Res" + TestFiles[iInstance] + "_" + InstanceIndex[iInstance] + "_" + MaxStag[iMaxStag] + "_" + s + ".txt";

                                Res = GetBestSolution(tFileName);

                                if(s<9)
                                    F.Write(Res + ",");
                                else
                                    F.Write(Res+"\n");


                                if (File.Exists(tFileName))
                                    continue;

//                                TestProblem.SolveFixSet(PopulationSize[iPop], K, MaxGeneratedSolutions, MaxItems[iItems], MaxStag[iMaxStag], LimitPerFix);
//                                TestProblem.SaveIntermediate(tFileName);
//                            }

                        }
                    }
                }
        //    }

            F.Close();

        }

        public void SolveParamTest()
        {

            MDKPProblem TestProblem;
            MDKPInstance TestInstance = new MDKPInstance();

//            int[]    PopulationSize = {50, 100, 200 };
            int[] PopulationSize = {25};
//            int[]    MaxItems = { 25, 50, 100 };
            int[] MaxItems = { 25};
            double LimitPerFix = 0.1;
            int      MaxGeneratedSolutions = 500000;
//            int[]    MaxStag = { 25, 50, 100 };
            int[] MaxStag = {  50};
            long TimeLimit = 600;
            string[] TestFiles = { "mknapcb8", "mknapcb9", "mknapcb5", "mknapcb6" };
            int[]    InstanceIndex = {0, 0, 0, 0 };
            int      NumRuns = 10; 
            int K = 5;


            InitFiles();
            string cResDirectory;
            string configString;
            string OptFile;
            string tFileName;

            for (int iInstance = 0; iInstance < InstanceIndex.Length; iInstance++)
                {
                for (int s = 0; s < 10; s++)
                {
                    for (int iMaxStag = 0; iMaxStag < MaxStag.Length; iMaxStag++)
                    {

                        for (int iPop = 0; iPop < PopulationSize.Length; iPop++)
                        {
                            for (int iItems = 0; iItems < MaxItems.Length; iItems++)
                            {

                                configString = "Res_P" + PopulationSize[iPop] + "_MI" + MaxItems[iItems] + "_TLF" + LimitPerFix + "_K" + K + "_M" + MaxStag[iMaxStag];
                                cResDirectory = mInstancesDirectory + configString + "\\\\";
                                if (!System.IO.Directory.Exists(cResDirectory))
                                {
                                    System.IO.Directory.CreateDirectory(cResDirectory);
                                }


                                TestInstance = new MDKPInstance();
                                TestInstance.Load_CB(mInstancesDirectory +  TestFiles[iInstance] + ".txt", InstanceIndex[iInstance]);
                                TestProblem = new MDKPProblem(TestInstance);
                                TestProblem.InitRandom(s);
                                OptFile = mInstancesDirectory + "Res_" + TestInstance.NumItems + "_" + TestInstance.NumConstraints + ".csv";
                                if (iInstance > 1)
                                {
                                    TestInstance.LoadOptimum(OptFile, InstanceIndex[iInstance]);
                                }
                                TestProblem.TimeLimit = TimeLimit * 1000;




                                tFileName = cResDirectory + "Res" + TestFiles[iInstance] + "_" + InstanceIndex[iInstance] + "_" + MaxStag[iMaxStag] + "_" + s + ".txt";

                                if (File.Exists(tFileName))
                                    continue;

                                TestProblem.SolveFixSet(PopulationSize[iPop], K, MaxGeneratedSolutions, MaxItems[iItems], MaxStag[iMaxStag], LimitPerFix);
                                TestProblem.SaveIntermediate(tFileName);
                            }

                        }
                    }
                }
            }

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

//                    Results[i][j - 1] = cValue;

                    
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

        public double GetTimeBestSolution(string FileName)
        {

            double Result = -1;
            string[] Lines = File.ReadAllLines(FileName);
            string[] words = Lines[Lines.Length - 1].Split(' ');


            if (!double.TryParse(words[2], out Result))
            {
                Result = double.MinValue;
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




            InitFiles();
            string cResDirectory;
            string configString = "Res_P" + PopupationSize + "_MI" + MaxItems + "_TLF" + LimitPerFix + "_K" + K + "_M" + MaxStag;
            string MethodsFile;
            string tFileName;
            double[][] MethodValues;
            double[] cValues;
            int cSolution;
            double Sum = 0;
            int[] Best = new int[mNumInstances];
            int counter;
            double[] Avg = new double[mNumInstances];
            string[] InstanceNames;
            string Line;
            int NumBest = -1;
            int NumAvg = -1;
            double[] AvgAvgMethod = new double[mNumInstances];
            double[] AvgBestMethod = new double[mNumInstances];

            int[] CounterAvgBetter = new int[mNumInstances];
            int[] CounterAvgWorse = new int[mNumInstances];
            int[] CounterAvgEqual = new int[mNumInstances];


            int[] CounterBestBetter = new int[mNumInstances];
            int[] CounterBestWorse = new int[mNumInstances];
            int[] CounterBestEqual = new int[mNumInstances];
            double[] AvgAllMethods;
            double AVGBestAll;
            double AVGAvgAll;

            int StartTableTime = 8;





            foreach (string iFile in mFilenames)
            {

                F.WriteLine(iFile);

                TestInstance = new MDKPInstance();
                TestInstance.Load_CB(mInstancesDirectory + iFile + ".txt", 0);

                MethodsFile = mInstancesDirectory + "DQPSORes_" + TestInstance.NumItems + "_" + TestInstance.NumConstraints + ".csv";
                LoadAllResults(MethodsFile, 30, out MethodValues, out InstanceNames);

                AVGBestAll = 0;
                AVGAvgAll = 0;



                AvgAllMethods = new double[MethodValues[0].Length];

                for (int i = 0; i < AvgAllMethods.Length; i++) {
                    AvgAllMethods[i] = 0;
                }

                for (int i = 0; i < mNumInstances; i++)
                {
                    Sum = 0;
                    Best[i] = int.MinValue;
                    counter = 0;

                    TestInstance = new MDKPInstance();
                    TestInstance.Load_CB(mInstancesDirectory + iFile + ".txt", i);

                    for (int j = 0; j < AvgAllMethods.Length; j++)
                    {
                        AvgAllMethods[j] += MethodValues[i][j];
                    }
                    /*
                    if (TestInstance.NumItems == 500)
                    {

                        if (TestInstance.NumConstraints < 30)
                        {
                            NumBest = 7;
                            NumAvg = 4;
                        }
                        else
                        {

                            NumBest = 6;
                            NumAvg = 3;
                        }
                    }
                    else
                    {
                    */
                        NumBest = 5;
                        NumAvg = 3;

                    //}



                    for (int s = 0; s < 10; s++)
                    {
                        cResDirectory = mInstancesDirectory + configString + "\\\\";
                        tFileName = cResDirectory + "Res" + iFile + "_" + i + "_" + MaxStag + "_" + s + ".txt";

                        if (File.Exists(tFileName))
                        {

                            cSolution = GetBestSolution(tFileName);

                            Sum += cSolution;
                            if (cSolution > Best[i])
                                Best[i] = cSolution;
                            counter++;
                        }

                    }

                    Avg[i] = Sum / counter;
                    Avg[i] = MethodValues[i][0] - Avg[i];
                    Best[i] = (int)MethodValues[i][0] - Best[i];

                    AVGBestAll += Best[i];
                    AVGAvgAll += Avg[i];

                    //                        Line = iInstanceNames[i] + "&";
                    Line = i + "&";

                    string AddS;

                    for (int k = 0; k < NumBest + 1; k++) {

                        Line += (int)(MethodValues[i][k]) + "&";
                    }
                        Line += Best[i] + "&";
//                                Line += Best[i] + "\\\\";
                        for (int k = NumBest+1; k < NumBest+NumAvg+1; k++)
                        {

                        Line += MethodValues[i][k].ToString("0.00") + "&";
                        }
                        Line += Avg[i].ToString("0.00")+"\\\\";
                    
                        F.WriteLine(Line);
                    }

                    Line = "\\midrule";
                    F.WriteLine(Line);
        //            Avg.Average();



                CounterAvgBetter = new int[NumAvg];
                CounterAvgWorse = new int[NumAvg];
                CounterAvgEqual = new int[NumAvg];


                CounterBestBetter = new int[NumBest];
                CounterBestWorse = new int[NumBest];
                CounterBestEqual = new int[NumBest];


                for (int k = 0; k < NumBest; k++)

                {
                    CounterBestBetter[k] =0;
                    CounterBestWorse[k] = 0;
                    CounterBestEqual[k] = 0;

                }


                for (int k = 0; k < NumAvg; k++)

                {
                    CounterAvgBetter[k] = 0;
                    CounterAvgWorse[k] = 0;
                    CounterAvgEqual[k] = 0;

                }


                for (int k = 1; k < NumBest + 1; k++)
                    {
                        for (int kk = 0; kk < mNumInstances; kk++) { 
                    
                            if(MethodValues[kk][k] == Best[kk])
                                CounterBestEqual[k-1]++;

                            if (MethodValues[kk][k] < Best[kk])
                                CounterBestBetter[k-1]++;

                            if (MethodValues[kk][k] > Best[kk])
                                CounterBestWorse[k-1]++;


                    }

                }

                for (int k = 0; k < NumAvg; k++)
                {
                    for (int kk = 0; kk < mNumInstances; kk++)
                    {

                        if (MethodValues[kk][k+ NumBest + 1] ==Avg[kk])
                            CounterAvgEqual[k]++;

                        if (MethodValues[kk][k+NumBest + 1] < Avg[kk])
                            CounterAvgBetter[k]++;

                        if (MethodValues[kk][k+ NumBest + 1] > Avg[kk])
                            CounterAvgWorse[k]++;


                    }

                }

                AVGBestAll /= mNumInstances;
                AVGAvgAll /= mNumInstances;

                for (int j = 0; j < AvgAllMethods.Length; j++) {

                    AvgAllMethods[j] /= mNumInstances;
                }

                Line = "Avg. &  &";
                for (int k = 1; k < NumBest + 1; k++)
                {
                    Line += AvgAllMethods[k].ToString("0.00") + "&";

                }

                Line += AVGBestAll.ToString("0.00") + " & ";

                for (int k = NumBest + 1; k < NumBest + NumAvg+1; k++)
                {
                    Line += AvgAllMethods[k].ToString("0.00") + "&";

                }

                Line += AVGAvgAll.ToString("0.00") ;


                Line += " \\\\ ";
                F.WriteLine(Line);



                Line = "\\#worse &  &";
                for (int k = 1; k < NumBest + 1; k++)
                {
                    Line += CounterBestWorse[k - 1] + "&";

                }
                Line += " & ";

                for (int k = 0; k < NumAvg; k++)
                {
                    Line += CounterAvgWorse[k] + "&";

                }


                Line += "\\\\";
                F.WriteLine(Line);

                Line = "\\#equal &  &";
                for (int k = 1; k < NumBest + 1; k++)
                {
                    Line += CounterBestEqual[k - 1] + "&";

                }

                Line += " & ";
                for (int k = 0; k < NumAvg; k++)
                {
                    Line += CounterAvgEqual[k] + "&";

                }


                Line += "\\\\";
                F.WriteLine(Line);



                Line = "\\#better &  &";
                for (int k = 1; k < NumBest + 1; k++)
                {
                    Line += CounterBestBetter[k - 1] + "&";

                }

                Line += "  &";
                for (int k = 0; k < NumAvg; k++)
                {
                    Line += CounterAvgBetter[k] + "&";

                }


                Line += "\\\\";
                F.WriteLine(Line);

            }
            F.Close();
        }

        public void GenerateTimeTable(string OutputFile)
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



            
            InitFiles();
            string cResDirectory;
            string configString = "Res_P" + PopupationSize + "_MI" + MaxItems + "_TLF" + LimitPerFix + "_K" + K + "_M" + MaxStag;
            string MethodsFile;
            string tFileName;
            double[][] MethodValues;
            double[] cValues;
            int cSolution;
            double Sum = 0;
            int[] Best = new int[mNumInstances];
            int counter;
            double[] Avg = new double[mNumInstances];
            string[] InstanceNames;
            string Line;
            int NumBest = -1;
            int NumAvg = -1;
            double[] AvgAvgMethod = new double[mNumInstances];
            double[] AvgBestMethod = new double[mNumInstances];

            int[] CounterAvgBetter = new int[mNumInstances];
            int[] CounterAvgWorse = new int[mNumInstances];
            int[] CounterAvgEqual = new int[mNumInstances];


            int[] CounterBestBetter = new int[mNumInstances];
            int[] CounterBestWorse = new int[mNumInstances];
            int[] CounterBestEqual = new int[mNumInstances];
            double[] AvgAllMethods;
            double AVGBestAll;
            double AVGAvgAll;

            double[] cMFSSTimes = new double[10];
            double[] MFSSTimes = new double[mNumInstances];
            double[][] MethodTimes = new double[3][];
            
            double average;
            double sumOfSquaresOfDifferences;
            double sd;


            for (int i = 0; i < 3; i++)
                MethodTimes[i] = new double[mNumInstances];



            foreach (string iFile in mFilenames)
            {

//                F.WriteLine(iFile);

                TestInstance = new MDKPInstance();
                TestInstance.Load_CB(mInstancesDirectory + iFile + ".txt", 0);

                MethodsFile = mInstancesDirectory + "DQPSORes_" + TestInstance.NumItems + "_" + TestInstance.NumConstraints + ".csv";
                LoadAllResults(MethodsFile, 30, out MethodValues, out InstanceNames);

                AVGBestAll = 0;
                AVGAvgAll = 0;

                AvgAllMethods = new double[MethodValues[0].Length];

                for (int i = 0; i < AvgAllMethods.Length; i++)
                {
                    AvgAllMethods[i] = 0;
                }

                for (int i = 0; i < mNumInstances; i++)
                {
                    Sum = 0;
                    Best[i] = int.MinValue;
                    counter = 0;

                    TestInstance = new MDKPInstance();
                    TestInstance.Load_CB(mInstancesDirectory + iFile + ".txt", i);

                    for (int j = 9; j < 11; j++)
                    {
                        //                        AvgAllMethods[j-8] += 
                        MethodTimes[j - 9][i] = MethodValues[i][j] / 2;
                    }



                    for (int s = 0; s < 10; s++)
                    {
                        cResDirectory = mInstancesDirectory + configString + "\\\\";
                        tFileName = cResDirectory + "Res" + iFile + "_" + i + "_" + MaxStag + "_" + s + ".txt";

                        if (File.Exists(tFileName))
                        {
                            cMFSSTimes[s] = GetTimeBestSolution(tFileName) / 1000;
                        }

                    }
                    average = cMFSSTimes.Average();
                    sumOfSquaresOfDifferences = MFSSTimes.Select(val => (val - average) * (val - average)).Sum();
                    sd = Math.Sqrt(sumOfSquaresOfDifferences / MFSSTimes.Length);
                    MethodTimes[2][i] = cMFSSTimes.Average();
                    MFSSTimes[i] = cMFSSTimes.Average();



                }

//                Line = "\\midrule";
//                F.WriteLine(Line);
                //            Avg.Average();


                Line ="$" + TestInstance.NumItems + " \\times " + TestInstance.NumConstraints + "$ &";

                for (int i = 0; i < 3; i++) {

                    average = MethodTimes[i].Average();
                    sumOfSquaresOfDifferences = MethodTimes[i].Select(val => (val - average) * (val - average)).Sum();
                    sd = Math.Sqrt(sumOfSquaresOfDifferences / MethodTimes[i].Length);


                    Line += average.ToString("0.0") + " & ";


                }

                for (int i = 0; i < 3; i++)
                {

                    average = MethodTimes[i].Average();
                    sumOfSquaresOfDifferences = MethodTimes[i].Select(val => (val - average) * (val - average)).Sum();
                    sd = Math.Sqrt(sumOfSquaresOfDifferences / MethodTimes[i].Length);


                    Line += sd.ToString("0.0");

                    if (i < 2)
                        Line += " & ";
                    else
                        Line += "\\\\";

                }



                F.WriteLine(Line);

            }
            F.Close();
        }

        long GetValueForTime(long iTime, List<long[]> TimeValues)
        {

            long Result = TimeValues[0][1];

            for (int i = 0; i < TimeValues.Count; i++)
                if (TimeValues[i][0] <= iTime)
                    Result = TimeValues[i][1];

            return Result;
        }


        public void LoadAllRuns(string FileBase, string Directory, string ParamsString, string OutputDir, int NumInstanaces, int BestKnown)
        {
            string[] lines;
            string FileName;
            string[] words;
            string temp;
            List<long[]>[] Solutions = new List<long[]>[10];
            long[] tempData;
            List<long> AllTimes = new List<long>();
            List<double> AllValuesForTime = new List<double>();

            for (int i = 0; i < NumInstanaces; i++)
                Solutions[i] = new List<long[]>();

            for (int i = 0; i < NumInstanaces; i++)
            {


                FileName = Directory + "Res_" +ParamsString + "\\\\"+ FileBase + i + ".txt";
                lines = System.IO.File.ReadAllLines(FileName);

                for (int j = 0; j < lines.Length; j++)
                {
                    temp = lines[j];
                    words = temp.Split(' ');

                    tempData = new long[2];

                    tempData[0] = Convert.ToInt64(words[2]) ;
                    tempData[1] = BestKnown - Convert.ToInt64(words[0]);

                    if (Solutions[i].Count == 0)
                        Solutions[i].Add( tempData);
                    else
                    {
                        if (Solutions[i][Solutions[i].Count - 1][1] != tempData[1])
                            Solutions[i].Add(tempData);
                    }

                    if (!AllTimes.Contains(tempData[0]))
                        AllTimes.Add(tempData[0]);
                }
            }

            AllTimes.Sort();

            for (int i = 0; i < AllTimes.Count; i++)
            {

                AllValuesForTime.Add(0);

                for (int j = 0; j < 10; j++)
                {

                    AllValuesForTime[i] += GetValueForTime(AllTimes[i], Solutions[j]);
                }

                AllValuesForTime[i] /= NumInstanaces;
            }


            StreamWriter tFile = new StreamWriter(OutputDir + FileBase + "avg_" + ParamsString+ ".txt");

            for (int i = 0; i < AllTimes.Count; i++)
            {

                tFile.WriteLine((AllTimes[i] / (float)1000) + " " + AllValuesForTime[i]);
            }
            tFile.Close();


        }


    }
}
