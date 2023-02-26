using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDKP
{

    
    internal class MDKPSolutionTracker
    {
        MDKPInstance       mInstance;
        List<MDKPSolution> mSolutions;
        int[]              mUsedCounter;
        int[]              mNotUsedCounter;
        int[,]             mPairTracking; 
        int                mMaxSolutions;


        public int NumberOfSolutions
        {
            get { return mSolutions.Count; }
        }

        public MDKPSolution GetBestFit(MDKPSolution.MDKPItemStatus[] iStates) {
            bool Fit;

            for (int i = 0; i < mSolutions.Count; i++) {

                Fit = true;
                for (int j = 0; j < mInstance.NumItems; j++) {

                    if ((iStates[j] != MDKPSolution.MDKPItemStatus.Unknown) && (iStates[j] != mSolutions[i].ItemStatuses[j]))
                    {
                        Fit = false;
                        break;
                    }
                }

                if(Fit)
                    return mSolutions[i];
            }

            return null;
        }


        public MDKPSolutionTracker(int iMaxSolutions, MDKPInstance iInstance) { 
        
            mInstance = iInstance;
            mSolutions = new List<MDKPSolution>();
            mMaxSolutions = iMaxSolutions;
            Allocate();

        }

        public int GetNumSolutions() { 
            return mSolutions.Count;
        }
        public void SaveAllSolutions(string Folder) {

            for (int i = 0; i < mSolutions.Count; i++) {

                mSolutions[i].SaveSolution(Folder + "Sol_" + i + ".txt");
            }
        }
        public MDKPSolution GetSolution(int Index) { 
        
            return mSolutions[Index];
        }
        public MDKPSolution.MDKPItemStatus[] GetStatusFix() {

            MDKPSolution.MDKPItemStatus[] Result = new MDKPSolution.MDKPItemStatus[mInstance.NumItems];
            int UsedCounter = 0;
            int NotUsedCounter = 0;
            for (int i = 0; i < mInstance.NumItems; i++) {

                if (mUsedCounter[i] == mSolutions.Count)
                {
                    Result[i] = MDKPSolution.MDKPItemStatus.Using;
                    UsedCounter++;
                    continue;
                }
                if (mNotUsedCounter[i] == mSolutions.Count)
                {
                    Result[i] = MDKPSolution.MDKPItemStatus.NotUsing;
                    NotUsedCounter++;
                    continue;
                }
                Result[i] = MDKPSolution.MDKPItemStatus.Unknown;
            }

            return Result;
        }

        public List<int[]> GetPairs() {

            List<int[]> Res = new List<int[]>();
            int[] temp;
            for (int i = 0; i < mInstance.NumItems; i++) {
                for (int j = i+1; j < mInstance.NumItems; j++)
                {
                    if ((mPairTracking[i, j] == mSolutions.Count) && (mUsedCounter[i] != mSolutions.Count) && (mNotUsedCounter[i] != mSolutions.Count)) {

                        temp = new int[2];
                        temp[0] = i;
                        temp[1] = j;

                        Res.Add(temp);

                    }
                    
                }

            }

            return Res;
        }
        public void ResetTracking() {


            Allocate();
            for (int i = 0; i < mInstance.NumItems; i++)
            {
                mUsedCounter[i] = 0;
                mNotUsedCounter[i] = 0;

                for (int j = 0; j < mSolutions.Count; j++)
                {
                    mPairTracking[i,j] = 0;
                }

            }


        }


        public MDKPSolution.MDKPItemStatus[] GetFix(int BaseIndex, List<int> TestIndexes, int Size, Random iGenerator) {

            MDKPSolution Base = mSolutions[BaseIndex];
            MDKPSolution.MDKPItemStatus[] Fix = new MDKPSolution.MDKPItemStatus[mInstance.NumItems];
            List<int[]>      Tracker = new List<int[]>();
            int[] temp;
            for (int i = 0; i < mInstance.NumItems; i++) {

                temp = new int[2];
                temp[0] = i;
                temp[1] = 0;
                Tracker.Add(temp);
            }


            foreach (int t in TestIndexes) {

                for (int i = 0; i < mInstance.NumItems; i++) {

                    if (Base.ItemStatuses[i] == mSolutions[t].ItemStatuses[i])
                        Tracker[i][1]++;
                }    
            }

            Tracker = Tracker.OrderByDescending(o => o[1]).ToList();
            List<int[]> RandTracker = new List<int[]>();

            int cFreq = TestIndexes.Count;
            while (RandTracker.Count < Size) {

                for (int i = 0; i < Tracker.Count; i++) {
                    if (Tracker[i][1] == cFreq)
                        RandTracker.Add(Tracker[i]);
                    
                }
                cFreq--;
            }

            MDKPProblem.shuffle<int[]>(RandTracker, iGenerator);

            for (int i = 0; i < mInstance.NumItems; i++)
            {

                Fix[i] = MDKPSolution.MDKPItemStatus.Unknown;
            }

            for (int i = 0; i < Size; i++) {

                Fix[RandTracker[i][0]] = Base.ItemStatuses[RandTracker[i][0]];
            }


            return Fix;
        }


        public void CalculateTrackers(List<int> SolutonIndex) {


            ResetTracking();
            foreach (MDKPSolution Sol in mSolutions) {

                for (int i = 0; i < mInstance.NumItems; i++) {

                    if (Sol.ItemStatuses[i] == MDKPSolution.MDKPItemStatus.Using)
                        mUsedCounter[i]++;
                    else
                        mNotUsedCounter[i]++;


                    for (int j = i+1; j < mInstance.NumItems; j++) {

                        if (Sol.ItemStatuses[i] == Sol.ItemStatuses[j]) {
                            mPairTracking[i, j]++;                         
                        }
                    }
                }
            }
        }

        public void RemoveSolution(int Index) {

            if (Index < mSolutions.Count)
            {
                mSolutions.RemoveAt(Index);
            }
        }

        public int AddSolution(MDKPSolution iSolution) {

            int tObjective = iSolution.CalculateObjective();


            for (int i = 0; i < mSolutions.Count; i++)
            {
                if (mSolutions[i].IsSame(iSolution))
                {
                    return -1;
                }
            }

            for (int i = 0; i < mSolutions.Count; i++)
            {
                        if (mSolutions[i].IsSame(iSolution))
                        {
                            return -1;
                        }

                        if (mSolutions[i].CalculateObjective() <= tObjective) {

                            mSolutions.Insert(i, iSolution);
                            if (mSolutions.Count > mMaxSolutions) {
                                mSolutions.RemoveAt(mSolutions.Count - 1);
                            }
                            return i;
                        }
            }

            if (mSolutions.Count < mMaxSolutions)
            {
                mSolutions.Add(iSolution);
                return mSolutions.Count-1;
            }
            

            return -1;
        }

        public void Allocate() {
            mUsedCounter = new int[mInstance.NumItems];
            mNotUsedCounter = new int[mInstance.NumItems];
            mPairTracking = new int[mInstance.NumItems, mInstance.NumItems];

        }

    }
}
