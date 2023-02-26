using ILOG.Concert;
using ILOG.CPLEX;

namespace MDKP
{
    class MDKPCplex
    {

        MDKPInstance mInstance;

        private IIntVar[] x;   // Decision variables
        Cplex cplex;
        bool mSolved;
        double mTimeLimit;
        MDKPSolution mSolution;


        public MDKPSolution Solution
        {
            get { return mSolution; }
        }

        public double TimeLimit
        {
            get { return mTimeLimit; }
            set { mTimeLimit = value; }
        }


        public MDKPCplex(MDKPInstance iInstance)
        {
            mInstance = iInstance;
            mTimeLimit = 0;
        }

        public void GenerateModel(MDKPCplexEXT Fix = null)
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

        public void GenerateFixConstraints(MDKPCplexEXT Fix) {

            ILinearIntExpr expr;

            if (Fix.mStates != null)
            {
                for (int i = 0; i < Fix.mStates.Length; i++)
                {

                    expr = cplex.LinearIntExpr();

                    switch (Fix.mStates[i])
                    {
                        case MDKPSolution.MDKPItemStatus.Unknown:
                            break;
                        case MDKPSolution.MDKPItemStatus.NotUsing:
                            expr.AddTerm(1, x[i]);
                            cplex.AddEq(0, expr);
                            break;
                        case MDKPSolution.MDKPItemStatus.Using:
                            expr.AddTerm(1, x[i]);
                            cplex.AddEq(1, expr);
                            break;
                    }
                }
            }

            if (Fix.mNumElements != -1) {

                expr = cplex.LinearIntExpr();
                for (int i = 0; i < mInstance.NumItems; i++) {

                    expr.AddTerm(x[i], 1);
                }

                cplex.AddEq(Fix.mNumElements, expr);

            }

            if (Fix.mPairs != null) {

                foreach (int[] cpair in Fix.mPairs) {

                    expr = cplex.LinearIntExpr();
                    expr.AddTerm(x[cpair[0]], 1);
                    expr.AddTerm(x[cpair[1]], -1);
                    cplex.AddEq(expr, 0);

                }
            }
        }

        private void GenerateVariables()
        {

            x = new IIntVar[mInstance.NumItems];

            int[] xlb = new int[mInstance.NumItems];
            int[] xub = new int[mInstance.NumItems];

            double[] blb = new double[mInstance.NumItems];
            double[] bub = new double[mInstance.NumItems];


            for (int i = 0; i < mInstance.NumItems; i++)
            {

                xlb[i] = 0;
                xub[i] = 1;
            }

            // Define the variables
            x = cplex.IntVarArray(mInstance.NumItems, xlb, xub);


        }


        void GenerateObjective()
        {

            IObjective objective1;
            ILinearIntExpr expr = cplex.LinearIntExpr();

            expr = cplex.LinearIntExpr();

            for (int i = 0; i < mInstance.NumItems; i++)
            {
                expr.AddTerm(mInstance.ItemValues[i], x[i]);
            }


            objective1 = cplex.Maximize(expr);
            cplex.Add(objective1);
        }

        private void GenerateConstraints()
        {

            ILinearIntExpr expr = cplex.LinearIntExpr();

            // Define the constraints
            for (int i = 0; i < mInstance.NumConstraints; i++)
            {

                expr = cplex.LinearIntExpr();
                for (int j = 0; j < mInstance.NumItems; j++)
                {
                    expr.AddTerm(mInstance.ItemWeights[j][i], x[j]);
                }

                cplex.AddLe(expr,  mInstance.Capacities[i]);
            }
        }

        public void AddFixedConstraints(int[] fix)
        {
            for (int i = 0; i < fix.Length; i++)
            {
                if (fix[i] == 1 || fix[i] == 0)
                {
                    cplex.AddEq(x[i], fix[i]);
                }
            }
        }

        public void Solve(MDKPCplexEXT Fix =null)
        {

            GenerateModel(Fix);

            if (Fix != null) {

                if (Fix.mHotStart != null) {

                    IIntVar[] startvar = new IIntVar[mInstance.NumItems];
                    double[] startval = new double[mInstance.NumItems];

                    for (int i = 0; i < mInstance.NumItems; i++) {

                        startvar[i] = x[i];
                        if (Fix.mHotStart.ItemStatuses[i] == MDKPSolution.MDKPItemStatus.Using)
                            startval[i] = 1;
                        else
                            startval[i] = 0;
                    }

                    cplex.AddMIPStart(startvar, startval, Cplex.MIPStartEffort.SolveMIP);
                }
            }

         //   cplex.ExportModel("lpex1.lp");

            if(mTimeLimit>0)
                cplex.SetParam(Cplex.Param.TimeLimit, mTimeLimit);
          cplex.SetOut(null);
                

            
            if (cplex.Solve())
            {
                mSolution = new MDKPSolution(mInstance);
                
                Console.WriteLine("Solution value: " + cplex.ObjValue);

                double[] xres = cplex.GetValues(x);

                for (int i = 0; i < mInstance.NumItems; i++)
                {

                    if ((int)Math.Round(xres[i]) == 1)
                    {
//                        System.Console.WriteLine("x" + i + " " + xres[i]);
                        mSolution.SetStatusItem(i, MDKPSolution.MDKPItemStatus.Using);
                    }
                    else {

                        mSolution.SetStatusItem(i, MDKPSolution.MDKPItemStatus.NotUsing);
                    }
                }
            }
            else
            {
                mSolved = false;
                mSolution = null;
                Console.WriteLine("Failed to find a solution.");
            }
            cplex.End();
        }

       

    }

   
}
