using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics.Metrics;

namespace MDKP
{
    internal class MDKPInstance
    {

        private int mNumItems;   // Number of items
        private int mNumConstraints;   // Number of constraints

        private int[] mItemValues;   // Value of each item
        // [items][constraints]
        private int[][] mItemWeights;   // Weight of each item
        private int[] mCapacities;   // Capacity of each constraint
        private int mOptimal;


        public int NumItems
        {
            get { return mNumItems; }
        }

        public int NumConstraints
        {
            get { return mNumConstraints; }
        }
        public int Optimal
        {
            get { return mOptimal; }
        }

        public int[] ItemValues
        {
            get { return mItemValues; }
        }
        public int[] Capacities
        {
            get { return mCapacities; }
        }
        public int[][] ItemWeights
        {
            get { return mItemWeights; }
        }


        public MDKPInstance()
        {

        }


        public MDKPInstance(int iNumItems, int iNumConstraints) {

            mNumConstraints = iNumConstraints;
            mNumItems = iNumItems;
            Allocate();
        }

        public void Allocate() { 
        
            mItemValues = new int[mNumItems];
            mCapacities = new int[mNumConstraints];
            mItemWeights = new int[mNumItems][];
            for (int i = 0; i < mNumItems; i++)
                mItemWeights[i] = new int[mNumConstraints];
        }

        public void Load(string FileName) {

            String[] Lines = File.ReadAllLines(FileName);
            List<int> AllValues = new List<int>();
            string[] words;
            int temp;

            for (int i = 0; i < Lines.Length; i++) { 
                
                words = Lines[i].Split(' ');
                for (int j = 0; j < words.Length; j++) {
                    if(int.TryParse(words[j], out temp))
                        AllValues.Add(int.Parse(words[j]));
                }
            }

            int counter = 0;
            
            mNumItems = AllValues[counter++];
            mNumConstraints = AllValues[counter++];

            Allocate();

            for(int i=0; i< mNumItems; i++)
                mItemValues[i] = AllValues[counter++];
            for (int i = 0; i < mNumConstraints; i++) 
                mCapacities[i] = AllValues[counter++];

            for (int j = 0; j < mNumConstraints; j++)
            {
                for (int i = 0; i < mNumItems; i++)
                {
                    mItemWeights[i][j] = AllValues[counter++];
                }
            }

            mOptimal = AllValues[counter++];






        }

        public void LoadType2(string FileName)
        {

            String[] Lines = File.ReadAllLines(FileName);
            List<int> AllValues = new List<int>();
            string[] words;
            int temp;

            for (int i = 0; i < Lines.Length; i++)
            {

                words = Lines[i].Split(' ');
                for (int j = 0; j < words.Length; j++)
                {
                    if (int.TryParse(words[j], out temp))
                        AllValues.Add(int.Parse(words[j]));
                }
            }

            int counter = 0;

            mNumItems = AllValues[counter++];
            mNumConstraints = AllValues[counter++];
            mOptimal = AllValues[counter++];

            Allocate();

            for (int i = 0; i < mNumItems; i++)
                mItemValues[i] = AllValues[counter++];

            for (int j = 0; j < mNumConstraints; j++)
            {
                for (int i = 0; i < mNumItems; i++)
                {
                    mItemWeights[i][j] = AllValues[counter++];
                }
            }

            for (int i = 0; i < mNumConstraints; i++)
                mCapacities[i] = AllValues[counter++];

//            mOptimal = AllValues[counter++];






        }
        public MDKPInstance GetSubInstance(List<int> iItems, int[] iCapacities)
        {

            MDKPInstance Result = new MDKPInstance(iItems.Count, mNumConstraints);

            for (int i = 0; i < iItems.Count; i++) {

                Result.ItemValues[i] = mItemValues[iItems[i]];
            }

            for (int i = 0; i < mNumConstraints; i++) {
                Result.mCapacities[i] = iCapacities[i];

                for (int j=0; j < iItems.Count; j++) {

                    Result.mItemWeights[j][i] = mItemWeights[iItems[j]][i];
                }
            }

            return Result;

        }


    }
}
