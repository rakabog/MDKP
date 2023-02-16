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
            TestInstance.LoadType2("c:\\primeri\\MDKP\\250_30_0.dat");
                        MDKPCplex T = new MDKPCplex(TestInstance);
                      T.TimeLimit = 400;
     //       MDKPFix Fix = new MDKPFix(63);
  //          T.Solve();
            
     //       int a = T.Solution.GetNumberOfUsedItems();
     //       int b = T.Solution.CalculateObjective();

            MDKPProblem Problem = new MDKPProblem(TestInstance);

            Problem.SolveFixSet(20, 1000, 100, 3);
            //          Problem.SolveStepGreedy(50, 1);
//            Problem.GenerateInitialPopulation(10, 50, 1);
       //     Problem.SolveMH(40,100, 2);


//            System.Console.WriteLine(Problem.Solution.CalculateObjective());
            System.Console.ReadKey();
        }
    }
}
