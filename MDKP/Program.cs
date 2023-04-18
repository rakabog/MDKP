using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDKP
{
    class Program
    {
        static void Main(string[] args)
        {


            MDKPInstance TestInstance = new MDKPInstance();

            /*
            TestInstance.Load_CB("c:\\primeri\\MDKP\\mknapcb8.txt",0);
            MDKPCplex T = new MDKPCplex(TestInstance);
            T.TimeLimit = 600;
            MDKPProblem Problem = new MDKPProblem(TestInstance);
            Problem.TimeLimit = 2000 * 1000;
            Problem.InitRandom(1);
            Problem.TimeLimit = 600 * 1000;
            Problem.SolveFixSet(25,5, 10000, 25,50,0.1);
            Problem.SaveIntermediate("mknapcb8_P25_MI25_TLF0.1_K5_M50.txt");


            TestInstance.Load_CB("c:\\primeri\\MDKP\\mknapcb9.txt", 0);
            T = new MDKPCplex(TestInstance);
            T.TimeLimit = 600;
            Problem = new MDKPProblem(TestInstance);
            Problem.TimeLimit = 2000 * 1000;
            Problem.InitRandom(1);
            Problem.TimeLimit = 600 * 1000;
            Problem.SolveFixSet(25, 5, 10000, 25, 50, 0.1);
            Problem.SaveIntermediate("mknapcb9_P25_MI25_TLF0.1_K5_M50.txt");
            */


            MDKPExperiments Exp = new MDKPExperiments();
            //                        Exp.GenerateTables("Table.txt");
            //             Exp.SolveAll();

            //                        Exp.SolveParamTest();
            //   Exp.GenerateTimeTable("TableTime.txt");
            //            Exp.GenerateParamResultFile("ResFile.txt",3,1);

            /*
            Exp.LoadAllRuns("Resmknapcb9_0_25_", "c:\\primeri\\MDKP\\Res_P100_MI25_TLF0.1_K5_M25\\", ".\\Res\\", "P100_MI25_TLF0.1_K5_M25", 10);
            Exp.LoadAllRuns("Resmknapcb9_0_25_", "c:\\primeri\\MDKP\\Res_P50_MI25_TLF0.1_K5_M25\\", ".\\Res\\", "P50_MI25_TLF0.1_K5_M25", 10);
            Exp.LoadAllRuns("Resmknapcb9_0_25_", "c:\\primeri\\MDKP\\Res_P200_MI25_TLF0.1_K5_M25\\", ".\\Res\\", "P200_MI25_TLF0.1_K5_M25", 10);
            */
            /*
            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P100_MI25_TLF0.1_K5_M50", ".\\Res\\", 10);
            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P100_MI50_TLF0.1_K5_M50", ".\\Res\\", 10);
            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P100_MI100_TLF0.1_K5_M50", ".\\Res\\", 10);


            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P50_MI100_TLF0.1_K5_M50", ".\\Res\\", 10);
            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P100_MI100_TLF0.1_K5_M50", ".\\Res\\", 10);
            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P200_MI100_TLF0.1_K5_M50", ".\\Res\\", 10);

            */
            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P100_MI25_TLF0.1_K5_M50", ".\\Res\\", 10, 116056);
            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P100_MI50_TLF0.1_K5_M50", ".\\Res\\", 10, 116056);
            Exp.LoadAllRuns("Resmknapcb9_0_50_", "c:\\primeri\\MDKP\\", "P100_MI100_TLF0.1_K5_M50", ".\\Res\\", 10, 116056);



            Exp.LoadAllRuns("Resmknapcb5_0_50_", "c:\\primeri\\MDKP\\", "P25_MI25_TLF0.1_K5_M50", ".\\Res\\", 10, 59187);
            Exp.LoadAllRuns("Resmknapcb5_0_50_", "c:\\primeri\\MDKP\\", "P50_MI25_TLF0.1_K5_M50", ".\\Res\\", 10, 59187);
            Exp.LoadAllRuns("Resmknapcb5_0_50_", "c:\\primeri\\MDKP\\", "P100_MI25_TLF0.1_K5_M50", ".\\Res\\", 10, 59187);
            Exp.LoadAllRuns("Resmknapcb5_0_50_", "c:\\primeri\\MDKP\\", "P200_MI25_TLF0.1_K5_M50", ".\\Res\\", 10, 59187);


            





            System.Console.ReadKey();
        }
    }
}
