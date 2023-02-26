using MDKP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDKP
{
    internal class MDKPSolution
    {
        public enum MDKPItemStatus { Using, NotUsing, Unknown};
        MDKPInstance     mInstanace;
        MDKPItemStatus[] mItemStatuses;
        int              mObjective;
        public int       mNumCreatedDuplicate;
               
        List<int>           mSelectedItems;

        public int[] mAvailableCapacity; 
        public MDKPItemStatus[] ItemStatuses
        {
            get { return mItemStatuses; }
        }

        public int Objective { 
            get { return mObjective; }
        }
        public void CalculateAvaillableCapacity() {


            mAvailableCapacity = new int[mInstanace.NumConstraints];

            for (int j = 0; j < mInstanace.NumConstraints; j++)
            {
                mAvailableCapacity[j] = mInstanace.Capacities[j];
            }

            for (int i = 0; i < mInstanace.NumItems; i++)
            {
                if (mItemStatuses[i] == MDKPItemStatus.Using)
                {
                    for (int j = 0; j < mInstanace.NumConstraints; j++)
                    {

                        mAvailableCapacity[j] -= mInstanace.ItemWeights[i][j];
                    }
                }
            }

        }
        public bool CanSwap(int GoingOut, int GoingIn) {

            for (int i = 0; i < mInstanace.NumConstraints; i++) {

                if (mAvailableCapacity[i] + mInstanace.ItemWeights[GoingOut][i] < mInstanace.ItemWeights[GoingIn][i])
                    return false;
            }

            return true;
        }

        public MDKPSolution(MDKPInstance iInstanace) {


            mInstanace = iInstanace;
            Allocate();

        }
        public bool Improve(Random iGenerator) {

            List<int> Indexes = new List<int>();

            for (int i = 0; i < mInstanace.NumItems; i++)
                Indexes.Add(i);

            MDKPProblem.shuffle<int>(Indexes, iGenerator);
            CalculateAvaillableCapacity();

            int GoingOut;
            int GoingIn;
            bool Improve = true ;

            while (Improve)
            {
                Improve = false;
                for (int i = 0; i < mInstanace.NumItems; i++)
                {
                    for (int j = i + 1; j < mInstanace.NumItems; j++)
                    {
                        if (ItemStatuses[Indexes[i]] != ItemStatuses[Indexes[j]])
                        {

                            if (ItemStatuses[Indexes[i]] == MDKPItemStatus.Using)
                            {
                                GoingOut = Indexes[i];
                                GoingIn = Indexes[j];
                            }
                            else
                            {
                                GoingIn = Indexes[i];
                                GoingOut = Indexes[j];
                            }

                            if (mInstanace.ItemValues[GoingIn] <= mInstanace.ItemValues[GoingOut])
                                continue;

                            if (CanSwap(GoingOut, GoingIn))
                            {
                                Improve = true;
                                ItemStatuses[GoingOut] = MDKPItemStatus.NotUsing;
                                ItemStatuses[GoingIn] = MDKPItemStatus.Using;
                                for (int k = 0; k < mInstanace.NumConstraints; k++)
                                {

                                    mAvailableCapacity[k] += mInstanace.ItemWeights[GoingIn][k] - mInstanace.ItemWeights[GoingOut][k];
                                }

                            }
                        }
                    }
                }
            }

            return false;
        }
        public MDKPSolution(List<int> indexes, MDKPInstance iInstanace)
        {


            mInstanace = iInstanace;
            Allocate();

            for (int i = 0; i < mInstanace.NumItems; i++)
            {
                mItemStatuses[i] = MDKPItemStatus.NotUsing;
            }

            foreach (int index in indexes) {
                mItemStatuses[index] = MDKPItemStatus.Using;
            }
            mSelectedItems = indexes;


        }




        public void Allocate() {

            mItemStatuses = new MDKPItemStatus[mInstanace.NumItems];
            mSelectedItems = new List<int>();
        }
        public void ResetSolution() {

            for (int i = 0; i < mInstanace.NumItems; i++) {
                mItemStatuses[i] = MDKPItemStatus.Unknown;
            }
            mSelectedItems.Clear();
        }
        public int GetNumberOfUsedItems() {

            int Result = 0;

            for (int i = 0; i < mInstanace.NumItems; i++) {
                if (mItemStatuses[i] == MDKPItemStatus.Using)
                    Result++;
            }

            return Result;
        
        }

        public void SaveSolution(string FileName) {

            StreamWriter T = new StreamWriter(FileName);


            
                
            T.WriteLine(GetNumberOfUsedItems());

            for (int i = 0; i < mInstanace.NumItems; i++) {

                if (ItemStatuses[i] == MDKPItemStatus.Using)
                    T.WriteLine(i);
            }
        
            T.Close();
        }

        public void LoadSolution(string FileName)
        {

            string[] Lines = File.ReadAllLines(FileName);
            int line = 0;
            int NumSelected = Convert.ToInt16(Lines[line++]);
            int Index;

            for (int i = 0; i < mInstanace.NumItems; i++) {
                ItemStatuses[i] = MDKPSolution.MDKPItemStatus.NotUsing;
            }

            for (int i = 0; i < NumSelected; i++)
            {
                Index = Convert.ToInt16(Lines[line++]);
                ItemStatuses[Index] = MDKPSolution.MDKPItemStatus.NotUsing;
                    
            }

            
        }



        public void SetStatusItem(int ItemIndex, MDKPItemStatus iStatus) {
            mItemStatuses[ItemIndex] = iStatus;
        }
        public bool IsSame(MDKPSolution iSolution) {

            for (int i = 0; i < mInstanace.NumItems; i++) {

                if (ItemStatuses[i] != iSolution.ItemStatuses[i])
                    return false;
            }

            return true;
        }
        public bool CheckConstraints() {

            for (int i = 0; i < mInstanace.NumConstraints; i++) {
                if (GetTotalWeightForConstraint(i) > mInstanace.Capacities[i])
                    return false;
            }


                return true;
        }

        public int GetTotalWeightForConstraint(int ConstraintIndex) {

            int Result = 0;

            for (int i = 0; i < mInstanace.NumItems; i++)
            {
                if (mItemStatuses[i] == MDKPItemStatus.Using)
                    Result += mInstanace.ItemWeights[i][ConstraintIndex];
            }

            return Result;
        
        }


        public int CalculateObjective() {


            int Sum = 0;

            for (int i = 0; i < mInstanace.NumItems; i++)
            {
                if (mItemStatuses[i] == MDKPItemStatus.Using)
                    Sum += mInstanace.ItemValues[i];
            }

            mObjective = Sum;

            return Sum;
        }

        public void AddInfoFromSolution(MDKPSolution iSolution, List<int> Translate) {

            for (int i = 0; i < iSolution.ItemStatuses.Length; i++) {

                mItemStatuses[Translate[i]] = iSolution.ItemStatuses[i];
            }
        }



    }
}
