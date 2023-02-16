using MDKP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDKP
{
    internal class MDKPCplexEXT
    {
        public MDKPSolution.MDKPItemStatus[] mStates;
        public int                           mNumElements;
        public MDKPSolution                  mHotStart;
        public List<int[]>                    mPairs;



        public void InitAll() {

            mNumElements = -1;
            mStates = null;
            mHotStart = null;
            mPairs = null;

        }
        public MDKPCplexEXT()
        {

            InitAll();
        }

        public MDKPCplexEXT(MDKPSolution.MDKPItemStatus[] iStates)
        {
            InitAll();
            mStates = iStates;
        }
        public MDKPCplexEXT(int iNumElements)
        {
            InitAll();
            mNumElements = iNumElements;
        }

        public MDKPCplexEXT(MDKPSolution iHotStart)
        {
            mNumElements = -1;
            mStates = null;
            mHotStart = iHotStart;
        }
    }
}
