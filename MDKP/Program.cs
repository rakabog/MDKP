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

            TestInstance.Load_CB("c:\\primeri\\MDKP\\mknapcb9.txt",0);
            MDKPCplex T = new MDKPCplex(TestInstance);
            T.TimeLimit = 600;
            MDKPProblem Problem = new MDKPProblem(TestInstance);

            Problem.TimeLimit = 2000 * 1000;
            Problem.InitRandom(1);
//             Problem.SolveFixSet(100,5, 10000, 100,50,0.1);

            MDKPExperiments Exp = new MDKPExperiments();
            Exp.GenerateTables("Table.txt");
//             Exp.SolveAll();
            System.Console.ReadKey();
        }
    }
}
