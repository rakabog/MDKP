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

            int           PopupationSize        = 100;
            int           MaxItems              =100;
            double           LimitPerFix           = 0.1;
            int           MaxGeneratedSolutions = 500000;
            long          TimeLimit             = 600 ;
            int             K = 8;


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
            string configString = "Res_P" + PopupationSize + "_MI" + MaxItems + "_TLF" + LimitPerFix + "_K" + K;
            string OptFile;

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
                        TestProblem.SolveFixSet(PopupationSize, K, MaxGeneratedSolutions, MaxItems, LimitPerFix);
                        TestProblem.SaveIntermediate(cResDirectory + "Res" + iFile + "_" + i +" "+s+".txt");

                    }
                }
            }
            
        }

    }
}
