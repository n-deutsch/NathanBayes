using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanBayes
{
    class Feature
    {
        private int trustedCount;
        private int untrustedCount;
        private int totalCount;

        public Feature()
        {
            trustedCount = 0;
            untrustedCount = 0;
            totalCount = 0;
        }

        public Feature(int trust, int untrust, int total)
        {
            trustedCount = trust;
            untrustedCount = untrust;
            totalCount = total;
        }

        public void setTrustedCount(int t)
        { trustedCount = t; }

        public int getTrustedCount()
        { return trustedCount; }

        public void setUntrustedCount(int u)
        { untrustedCount = u; }

        public int getUntrustedCount()
        { return untrustedCount; }

        public void setTotalCount(int t)
        { totalCount = t; }

        public int getTotalCount()
        { return totalCount; }

        public void increaseTrustCount()
        {
            trustedCount++;

            //adjust total
            totalCount = trustedCount + untrustedCount;
        }

        public void increaseUntrustCount()
        {
            untrustedCount++;

            //adjust total
            totalCount = trustedCount + untrustedCount;
        }
    }
}
