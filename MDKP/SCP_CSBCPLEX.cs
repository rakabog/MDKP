using ILOG.Concert;
using ILOG.CPLEX;
using MDKP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP
{
    public class SCP_CSBCPLEX
    {
        private IIntVar[] x;   // Decision variables
        private IIntVar[][] y;   // Decision variables
        Cplex cplex;
        bool mSolved;
        double mTimeLimit;
        MDKPSolution mSolution;

/*        public void GenerateModel(MDKPCplexEXT Fix = null)
        {

            cplex = new Cplex();
            GenerateVariables();
            GenerateConstraints();
            GenerateObjective();
            if (Fix != null)
            {
                GenerateFixConstraints(Fix);
            }

        }
*/
    }
}
