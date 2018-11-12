using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanBayes
{
    class NaiveBayes
    {
        private List<Feature> frequencyTable;
        private WebParser WP;
        int trustedArticlesCount;
        int untrustedArticlesCount;

        public NaiveBayes()
        {
            frequencyTable = new List<Feature>();
            WP = new WebParser();
            trustedArticlesCount = 0;
            untrustedArticlesCount = 0;
        }

        public void setFrequencyTable(List<Feature> ft)
        {
            frequencyTable = ft;
        }

        public List<Feature> getFrequencyTable()
        {
            return frequencyTable;
        }

        public void setWP(WebParser wp)
        {
            WP = wp;
        }

        public WebParser getWP()
        {
            return WP;
        }

        public void buildFrequencyTable()
        {
            int i = 0;
            int j = 0;

            //create T/F ratios for each feature
            List<String> trusted = readBinaryStrings(true);
            List<String> untrusted = readBinaryStrings(false);

            //reading failure? Parse these by hand...
            if (trusted.Count == 0 || untrusted.Count == 0)
            {
                WP.parse();

                trusted = WP.getTrustedArticles();
                untrusted = WP.getUntrustedArticles();

                //write to local files so we don't have to parse by hand next time we run...
                writeBinaryStrings(true, trusted);
                writeBinaryStrings(false, untrusted);
            }

            trustedArticlesCount = trusted.Count;
            untrustedArticlesCount = untrusted.Count;

            //set frequency table to n default values
            defaultFrequencyTable(trusted);

            //translate binary strings to the frequency table
            for (i = 0; i < trustedArticlesCount; i++)
            {
                //these strings should all be the same length
                for (j = 0; j < trusted[i].Length; j++)
                {
                    //incriment frequency
                    if (trusted[i][j] == '1')
                    {
                        frequencyTable[j].increaseTrustCount();
                    }
                }
            }

            //translate untrusted strings to frequency table
            for (i = 0; i < untrustedArticlesCount; i++)
            {
                //these strings should all be the same length...
                for (j = 0; j < untrusted[i].Length; j++)
                {
                    //inc frequency
                    if (untrusted[i][j] == '1')
                    {
                        frequencyTable[j].increaseUntrustCount();
                    }
                }
            }

            return;
        }

        //fill frequency table with a bunch of default Features
        public void defaultFrequencyTable(List<String> trust)
        {
            int i = 0;

            for (i = 0; i < trust[0].Length; i++)
            {
                frequencyTable.Add(new Feature());
            }

            return;
        }

        //read in urls from "test_data.txt" - attempt to correctly classify them
        public String test()
        {
            String output = String.Empty;
            int test_cases = 0;
            int test_pass = 0;
            double percentage_passed = 0;

            //we can't train without a frequency table!
            if (frequencyTable.Count == 0)
            { buildFrequencyTable(); }

            //read lines from "test_data.txt"
            String path = "C:\\CUSTOM SOFTWARE\\NathanBayes\\test_data.txt";
            System.IO.StreamReader f = new System.IO.StreamReader(path);
            String line = String.Empty;

            //variables for each test case
            int real_output = 0;
            int expected_output = 0;
            String url = String.Empty;

            //read to end
            while ((line = f.ReadLine()) != null)
            {
                //test_cases look like [t/f]:[url]
                //expected output [t] = 1
                //expected output [f] = 0

                //examine first character
                if (line[0] == 't')
                { expected_output = 1; }
                else
                { expected_output = 0; }

                //parse URL - characters 2-n
                url = line.Substring(2);

                //classify this instance
                real_output = classify(url);

                //ignore errors - some websites may be down or unavaliable
                if (real_output == -1)
                { continue; }

                //test passed?
                if (real_output == expected_output)
                { test_pass++; }

                test_cases++;
            }

            //avoid divide by zero error
            if (test_cases != 0)
            { percentage_passed = (double)test_pass / (double)test_cases; }

            //reformat percentage
            percentage_passed = percentage_passed * 100;
            percentage_passed = Math.Round(percentage_passed, 2);
            output = "Test Cases:[" + test_cases + "] Passed:[" + test_pass + "] (" + percentage_passed + "%)";

            return output;
        }

        //outputs: 0=untrusted, 1=trusted, -1=error
        public int classify(String url)
        {
            //final verdict
            int TRUSTED = 1;
            int UNTRUSTED = 0;
            int ERROR = -1;

            //we need a frequency table to classify a new instance...
            if (frequencyTable.Count == 0)
            { buildFrequencyTable(); }

            int i = 0;

            //numbers used in calculation
            int trustedExponent = 0;
            double trustedProbability = 0;
            int untrustedExponent = 0;
            double untrustedProbability = 0;

            //calculations for a single feature
            double featureTrusted = 0;
            double featureUntrusted = 0;

            //url converted to a list of 0s and 1s
            String binaryStr = String.Empty;

            //convert incoming URL to a sequence of 1s and 0s
            binaryStr = WP.createBinaryString(url);

            //check for an error contacting the web page
            if (binaryStr.Equals("bad"))
            { return ERROR; }

            //this statement makes sure divide by zero errors are not possible
            if (trustedArticlesCount == 0 || untrustedArticlesCount == 0)
            { return ERROR; }

            //we now have a binary string with len(frequencyTable)

            //p(TRUST) =
            trustedProbability = (double)trustedArticlesCount / (double)(trustedArticlesCount + untrustedArticlesCount);
            //P(UNTRUST) =
            untrustedProbability = (double)untrustedArticlesCount / (double)(trustedArticlesCount + untrustedArticlesCount);

            for (i = 0; i < frequencyTable.Count; i++)
            {
                //we're going to skip over unused features and see what happens!
                if (frequencyTable[i].getTotalCount() <= 0)
                { continue; }
                
                
                if (binaryStr[i] == '1') //feature[i] exists in 'url'
                {
                    //we want P(f#i | trusted) and P(f#i | untrusted)
                    //calculate P(f#i | trusted)
                    featureTrusted = frequencyTable[i].getTrustedCount() / (double)trustedArticlesCount;
                    //nonzero elements are NOT allowed - set to small value 
                    if (featureTrusted == 0)
                    { featureTrusted = 1 / (double)(trustedArticlesCount + untrustedArticlesCount); }

                    //calculate P(f#i | untrusted)
                    featureUntrusted = frequencyTable[i].getUntrustedCount() / (double)untrustedArticlesCount;

                    //nonzero values are NOT allowed - set to small value 
                    if (featureUntrusted == 0)
                    { featureUntrusted = 1 / (double)(trustedArticlesCount + untrustedArticlesCount); }
                }
                else //feature[i] does not exist in 'url'
                {
                    //we want P(~f#i | trusted) and P(f#i | untrusted)
                    //calculate P(~f#i | trusted)
                    featureTrusted = (trustedArticlesCount - frequencyTable[i].getTrustedCount()) / (double)trustedArticlesCount;

                    //nonzero elements are NOT allowed - set to small value 
                    if (featureTrusted == 0)
                    { featureTrusted = 1 / (double)(trustedArticlesCount + untrustedArticlesCount); }

                    //calculate P(~f#i | untrusted)
                    featureUntrusted = (untrustedArticlesCount - frequencyTable[i].getUntrustedCount()) / (double)untrustedArticlesCount;

                    //nonzero values are NOT allowed - set to small value 
                    if (featureUntrusted == 0)
                    { featureUntrusted = 1 / (double)(trustedArticlesCount + untrustedArticlesCount); }
                }

                //add value to multiply P(trust)
                trustedProbability = trustedProbability * featureTrusted;

                //add value to multiply P(untrusted)
                untrustedProbability = untrustedProbability * featureUntrusted;

                //we have to use scientific notation since these numbers get TINY
                //real value = trustedProbability * 10^(-trustedExponent)
                while (trustedProbability < 1)
                {
                    trustedProbability = trustedProbability * 10;
                    trustedExponent++;
                }

                //we have to use scientific notation since these numbers get TINY
                //real value = untrustedProbability * 10^(-untrustedExponent)
                while (untrustedProbability < 1)
                {
                    untrustedProbability = untrustedProbability * 10;
                    untrustedExponent++;
                }

                //next feature...
            }

            //pause here so the results make sense
            String sanityCheck = "Trusted:[" + trustedProbability + " * 10^(-" + trustedExponent + ")]  ---  ";
            sanityCheck = sanityCheck + "Untrusted:[" + untrustedProbability + " * 10^(-" + untrustedExponent + ")]";
            int breakpoint = 0;
            

            //compare P(trusted) and P(untrusted)
            if (trustedExponent == untrustedExponent)
            {
                if (trustedProbability > untrustedProbability)
                { return TRUSTED; }
                else
                { return UNTRUSTED; }
            }
            else if (trustedExponent > untrustedExponent)
            {
                return UNTRUSTED;
            }
            else //trustedExponent < untrustedExponent
            {
                return TRUSTED;
            }
        }

        //flag == true means trusted, flag == false means untrusted 
        public List<String> readBinaryStrings(bool flag)
        {
            List<String> binaries = new List<String>();
            String path = String.Empty;

            if (flag)
            {
                path = "C:\\CUSTOM SOFTWARE\\NathanBayes\\trusted_binary.txt";
            }
            else
            {
                path = "C:\\CUSTOM SOFTWARE\\NathanBayes\\untrusted_binary.txt";
            }

            System.IO.StreamReader f = new System.IO.StreamReader(path);
            String line = String.Empty;

            //for each line in untrusted.txt...
            while ((line = f.ReadLine()) != null)
            {
                binaries.Add(line);
            }

            return binaries;
        }

        //flag == true means trusted, flag == false means untrusted 
        public void writeBinaryStrings(bool flag, List<String> binaries)
        {
            int i = 0;
            String path = String.Empty;

            if (flag)
            {
                path = "C:\\CUSTOM SOFTWARE\\NathanBayes\\trusted_binary.txt";
            }
            else
            {
                path = "C:\\CUSTOM SOFTWARE\\NathanBayes\\untrusted_binary.txt";
            }


            System.IO.StreamWriter f = new System.IO.StreamWriter(path);

            //set up writer
            for (i = 0; i < binaries.Count; i++)
            {
                f.WriteLine(binaries[i]);
            }

            return;
        }
    }
}
