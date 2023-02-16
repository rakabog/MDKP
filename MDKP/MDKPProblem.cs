using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDKP
{
    internal class MDKPProblem
    {

        MDKPInstance        mInstance;
        Random              mGenerator;
        MDKPSolution        mSolution;
        MDKPSolution        mBestSolution;
        MDKPSolutionTracker mSolutionTracker;



        public MDKPSolution Solution
        {
            get { return mSolution; }
        }
        public MDKPSolution BestSolution
        {
            get { return mBestSolution; }
        }

        public bool CheckBest() {

            if (mBestSolution == null) {
                Solution.CalculateObjective();
                mBestSolution = Solution;
                return true;
            }
            if (mBestSolution.CalculateObjective() < mSolution.CalculateObjective()) {
                mBestSolution = Solution;
                return true; 
            }

            return false;
        } 

        public MDKPProblem() {


        }
        public void InitRandom(int seed) {

            mGenerator = new Random(seed);
        }
        public MDKPProblem(MDKPInstance iInstance)
        {
            mInstance = iInstance;
            InitRandom(2);

        }


        public void SolveRandom() {

            int[] TotalWeight = new int[mInstance.NumConstraints];
            List<int> Selected = new List<int>();
            for (int i = 0; i < mInstance.NumConstraints; i++) {

                TotalWeight[i] = 0;
            }

            List<int> Indexes = new List<int>();


            for (int i = 0; i < mInstance.NumItems; i++) {

                Indexes.Add(i);
            }

            shuffle<int>(Indexes, mGenerator);

            int Satisfied;
            foreach (int index in Indexes) {

                Satisfied = 0;
                for (int i = 0; i< mInstance.NumConstraints; i++) {

                    if (TotalWeight[i] + mInstance.ItemWeights[index][i] > mInstance.Capacities[i])
                        break;

                    Satisfied++;
                }

                if (Satisfied == mInstance.NumConstraints) {
                    for (int i = 0; i < mInstance.NumConstraints; i++)
                    {

                        TotalWeight[i] += mInstance.ItemWeights[index][i];
                    }

                    Selected.Add(index);
                }

            }

            mSolution = new MDKPSolution(Selected, mInstance);
        
        }

        public void GenerateInitialPopulation(int PopulationSize, int iMaxItems, double MaxCalcTime)
        {

            mSolutionTracker = new MDKPSolutionTracker(PopulationSize, mInstance);


            for (int global = 0; global < PopulationSize; global++)
            {

                SolveStepGreedy(iMaxItems, MaxCalcTime);
                mSolutionTracker.AddSolution(mSolution);
                CheckBest();


            }
        }

        

        public void SolveFixSet(int PopulationSize, int MaxIterations, int iMaxItems, double MaxCalcTime) {

            List<int> SolutionIndexes= new List<int>();
            List<int> SelIndexes = new List<int>();
            int BaseIndex;
            int k=10;
            MDKPSolution BaseSolution;
            MDKPSolution.MDKPItemStatus[] Fix;
            MDKPCplexEXT cFix = new MDKPCplexEXT();
            MDKPCplex Solver;

            //            GenerateInitialPopulation(PopulationSize, iMaxItems, MaxCalcTime);
            //            mSolutionTracker.SaveAllSolutions(".\\Solutions\\");

            mSolutionTracker = new MDKPSolutionTracker(20, mInstance);
            for (int i = 0; i < 10000; i++) {

                SolveRandom();
                mSolutionTracker.AddSolution(mSolution);
                CheckBest();
            }
            for (int i = 0; i < MaxIterations; i++) {


                SolutionIndexes.Clear();
                for (int j = 0; j < mSolutionTracker.GetNumSolutions(); j++) {
                    SolutionIndexes.Add(j);
                }

                BaseIndex = mGenerator.Next() % mSolutionTracker.GetNumSolutions();
                BaseSolution = mSolutionTracker.GetSolution(BaseIndex);

                shuffle<int>(SolutionIndexes, mGenerator);
                SelIndexes.Clear();

                for (int s = 0; s < k; s++) {
                    SelIndexes.Add(SolutionIndexes[s]);
                
                }


                 Fix = mSolutionTracker.GetFix(BaseIndex, SelIndexes, mInstance.NumItems - iMaxItems, mGenerator);


                Solver = new MDKPCplex(mInstance);
                Solver.TimeLimit = MaxCalcTime;
                cFix.mStates = Fix;
                cFix.mHotStart = BaseSolution;
                //                    cFix.mHotStart = null;
                Solver.Solve(cFix);
                mSolution = Solver.Solution;
                mSolutionTracker.AddSolution(mSolution);
                CheckBest();

            }
        
        }
        /*
            public void SolveMH(int MaxIterations, int iMaxItems, double MaxCalcTime) {


            MDKPSolution iBest;
            MDKPCplex Solver;
            MDKPCplexEXT cFix = new MDKPCplexEXT();
            MDKPSolution.MDKPItemStatus[] Fix;
            int Population =20;



            int[] Counter = new int[mInstance.NumItems];

            mSolutionTracker = new MDKPSolutionTracker(20,mInstance); 
            
            for (int i = 0; i < mInstance.NumItems; i++) {

                Counter[i] = 0;
            }

            for (int global = 0; global < Population; global++)
            {
                mBestSolution = null;

//                SolveStepGreedy(iMaxItems, MaxCalcTime);
                SolveStepGreedy(iMaxItems, MaxCalcTime/5);



                CheckBest();

                List<int> AllItems = new List<int>();
                Fix = new MDKPSolution.MDKPItemStatus[mInstance.NumItems];

                for (int ii = 0; ii < mInstance.NumItems; ii++)
                {

                    AllItems.Add(ii);
                }


                for (int i = 0; i < MaxIterations; i++)
                {


                    shuffle<int>(AllItems, mGenerator);

                    for (int j = 0; j < mInstance.NumItems; j++)
                    {

                        Fix[j] = mBestSolution.ItemStatuses[j];
                    }
                    int Inside = 0;
                    int OutSide = 0;


                    for (int f = 0; f < iMaxItems; f++)
                    {

                        Fix[AllItems[f]] = MDKPSolution.MDKPItemStatus.Unknown;

                        if (mBestSolution.ItemStatuses[AllItems[f]] == MDKPSolution.MDKPItemStatus.Using)
                            Inside++;
                        else
                            OutSide++;

                    }
                    


                    Solver = new MDKPCplex(mInstance);
                    Solver.TimeLimit = MaxCalcTime;
                    cFix.mStates = Fix;

                    cFix.mHotStart = mBestSolution;
//                    cFix.mHotStart = null;
                    Solver.Solve(cFix);
                    mSolution = Solver.Solution;
                    CheckBest();

                }

                mSolutionTracker.AddSolution(mSolution);

                for (int y = 0; y < mInstance.NumItems; y++) {
                    if (mBestSolution.ItemStatuses[y] == MDKPSolution.MDKPItemStatus.Using)
                        Counter[y]++;
                }
            }

            mSolutionTracker.SaveAllSolutions(".\\Solutions\\");
            mSolutionTracker.CalculateTrackers();
            /*
             Fix = new MDKPSolution.MDKPItemStatus[mInstance.NumItems];


            int FreeCounter = 0;
            for (int y = 0; y < mInstance.NumItems; y++)
            {
                if (Counter[y] == 0)
                    Fix[y] = MDKPSolution.MDKPItemStatus.NotUsing;
                if(Counter[y] == Population)
                Fix[y] = MDKPSolution.MDKPItemStatus.Using;

                if ((Counter[y] > 0) && (Counter[y] < Population))
                {
                    Fix[y] = MDKPSolution.MDKPItemStatus.Unknown;
                    FreeCounter++;
                }
            }
            */
            /*
            Solver = new MDKPCplex(mInstance);
            Solver.TimeLimit = MaxCalcTime*50;
            cFix.mStates = mSolutionTracker.GetStatusFix();
//            cFix.mPairs = mSolutionTracker.GetPairs();
            Solver.Solve(cFix);
            mSolution = Solver.Solution;
            CheckBest();

        }
*/

        public void SolveStepGreedy(int iMaxItems, double MaxCalcTime) {

            List<int> shuffleItems = new List<int>();
            for (int i = 0; i < mInstance.NumItems; i++) {

                shuffleItems.Add(i);
            }

            shuffle<int>(shuffleItems, mGenerator);
            int[] ScaledCapacities = new int[mInstance.NumConstraints];
            double CoefSub = (double) iMaxItems / mInstance.NumItems;

            for (int i = 0; i < mInstance.NumConstraints; i++)
            {
                ScaledCapacities[i] = (int)Math.Floor(mInstance.Capacities[i] * CoefSub);
            }

            int Done = 0;
            List<int> SubList;
            MDKPCplex Solver ;
            MDKPInstance cInstance;

            mSolution = new MDKPSolution(mInstance);
            mSolution.ResetSolution();

            int a, b;

            while (Done + iMaxItems < mInstance.NumItems) {


                SubList = shuffleItems.GetRange(Done, iMaxItems);
                cInstance = mInstance.GetSubInstance(SubList, ScaledCapacities);

                Solver = new MDKPCplex(cInstance);
                Solver.TimeLimit = MaxCalcTime;
                Solver.Solve();
                
                mSolution.AddInfoFromSolution(Solver.Solution, SubList);
                b = Solver.Solution.GetTotalWeightForConstraint(0);
                a = mSolution.GetTotalWeightForConstraint(0);

                Done = Done + iMaxItems;
            }


            for (int i = 0; i < mInstance.NumConstraints; i++) {

                ScaledCapacities[i] = mInstance.Capacities[i] - mSolution.GetTotalWeightForConstraint(i);
            }

            SubList = shuffleItems.GetRange(Done, shuffleItems.Count - Done);
            cInstance = mInstance.GetSubInstance(SubList, ScaledCapacities);

            Solver = new MDKPCplex(cInstance);
            Solver.TimeLimit = MaxCalcTime;
            Solver.Solve();
            mSolution.AddInfoFromSolution(Solver.Solution, SubList);
        }

        static public void shuffle<T>(List<T> list, Random nGenerator)
        {
            //            Random rng = new Random();   // i.e., java.util.Random.
            int n = list.Count;        // The number of items left to shuffle (loop invariant).
            while (n > 1)
            {
                int k = nGenerator.Next(n);  // 0 <= k < n.
                n--;                     // n is now the last pertinent index;
                T temp = list[n];     // swap array[n] with array[k] (does nothing if k == n).
                list[n] = list[k];
                list[k] = temp;
            }
        }

    }

}
