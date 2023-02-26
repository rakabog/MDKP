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

            /*
            List<int> l = new List<int>();
            int[] lcapacities = { 30, 40, 50 };

            l.Add(1);
            l.Add(2);


            T.Solve();
            */
            //            T.SolveSubproblem(l, lcapacities);

            MDKPInstance TestInstance = new MDKPInstance();

//            TestInstance.LoadType2("c:\\primeri\\MDKP\\cb1.DAT");
            TestInstance.Load_CB("c:\\primeri\\MDKP\\mknapcb9.txt",25);
                        MDKPCplex T = new MDKPCplex(TestInstance);
                      T.TimeLimit = 600;
     //       MDKPFix Fix = new MDKPFix(63);
//            T.Solve();
            
     //       int a = T.Solution.GetNumberOfUsedItems();
     //       int b = T.Solution.CalculateObjective();

            MDKPProblem Problem = new MDKPProblem(TestInstance);

            Problem.TimeLimit = 2000 * 1000;
            Problem.InitRandom(2);
             Problem.SolveFixSet(100,8, 10000, 100,0.1);
            //          Problem.SolveStepGreedy(50, 1);
            //            Problem.GenerateInitialPopulation(10, 50, 1);
            //     Problem.SolveMH(40,100, 2);

            MDKPExperiments Exp = new MDKPExperiments();

      //     Exp.SolveAll();
//            System.Console.WriteLine(Problem.Solution.CalculateObjective());
            System.Console.ReadKey();
        }
    }
}
