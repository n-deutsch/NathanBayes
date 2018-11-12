using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace NathanBayes
{
    //class exists to input some URL and turn it into a string of 1s and 0s
    class WebParser
    {
        private List<String> commonWords;
        private List<String> trustedArticles;
        private List<String> untrustedArticles;

        public WebParser()
        {
            commonWords = new List<String>();
            trustedArticles = new List<String>();
            untrustedArticles = new List<String>();
        }

        //called once during setup...
        private void populateCommonWords()
        {
            System.IO.StreamReader f = new System.IO.StreamReader("C:\\CUSTOM SOFTWARE\\NathanBayes\\common_words.txt");
            String line = String.Empty;

            while ((line = f.ReadLine()) != null)
            {
                commonWords.Add(line);
            }

            return;
        }

        public void setCommonWords(List<String> cw)
        {
            commonWords = cw;
        }

        public List<String> getCommonWords()
        {
            return commonWords;
        }

        public void setTrustedArticles(List<String> ta)
        {
            trustedArticles = ta;
        }

        public List<String> getTrustedArticles()
        {
            return trustedArticles;
        }

        public void setUntrustedArticles(List<String> ua)
        {
            untrustedArticles = ua;
        }

        public List<String> getUntrustedArticles()
        {
            return untrustedArticles;
        }

        public void parse()
        {
            populateCommonWords();
            buildTrustedArticles();
            buildUntrustedArticles();
        }
        
        public void buildTrustedArticles()
        {
            System.IO.StreamReader f = new System.IO.StreamReader("C:\\CUSTOM SOFTWARE\\NathanBayes\\trusted.txt");
            String line = String.Empty;
            String freq = String.Empty;
            

            //for each line in trusted.txt...
            while ((line = f.ReadLine()) != null)
            {
                freq = createBinaryString(line);

                //ignore invalid inputs
                if (!freq.Equals("bad"))
                {
                    trustedArticles.Add(freq);
                }
            }

            return;
        }

        public void buildUntrustedArticles()
        {
            System.IO.StreamReader f = new System.IO.StreamReader("C:\\CUSTOM SOFTWARE\\NathanBayes\\untrusted.txt");
            String line = String.Empty;
            String freq = String.Empty;

            //for each line in untrusted.txt...
            while ((line = f.ReadLine()) != null)
            {
                freq = createBinaryString(line);

                //ignore invalid inputs
                if (!freq.Equals("bad"))
                {
                    untrustedArticles.Add(freq);
                }
            }

            return;
        }

        public String createBinaryString(String url)
        {
            //we need to read common_words.txt for this to work
            if (commonWords.Count == 0)
            { populateCommonWords(); }

            String err = "bad";
            String output = String.Empty;
            char[] array = new char[commonWords.Count];
            int i = 0;
            int index = 0;

            //variables for parsing a web page....
            WebRequest request = null;
            WebResponse response = null;
            Stream data = null;
            String html = String.Empty;

            //check for invalid input
            try
            {
                //attempt to populate data with the url's html
                request = WebRequest.Create(url);
                response = request.GetResponse();
                data = response.GetResponseStream();
            }
            catch (Exception E)
            {
                return err;
            }; //return "bad"
            

            //set all values to ZERO...
            for (i = 0; i < commonWords.Count; i++)
            {
                array[i] = '0';
            }

            //examine the URL
            using (StreamReader sr = new StreamReader(data))
            {
                //now we have the page's html
                html = sr.ReadToEnd();
                html = html.ToLower();

                //split special characters so we can split() by whitespace...
                html = html.Replace(">", " ");
                html = html.Replace("<", " ");
                html = html.Replace(".", " ");
                html = html.Replace("?", " ");
                html = html.Replace("!", " ");
                html = html.Replace("'", " ");
                html = html.Replace("\"", " ");
                html = html.Replace(",", " ");

                //array separated by whitespace
                var splitHTML = html.Split(' ');

                //binary search each word we split by whitespace
                foreach (String word in splitHTML)
                {
                    //we only need to lookup words that are lower case a-z
                    if (containsIllegalChars(word))
                    { continue; }


                    index = binarySearch(word);
                    //positive number?
                    if (index >= 0)
                    {
                        array[index] = '1';
                    }
                }
            }
            output = new string(array);

            return output;
        }

        public bool containsIllegalChars(String input)
        {
            int i = 0;

            //empty strings are ignored
            if (input.Length == 0)
            { return true; }

            //commonWords has only lower case words, so we only care about letters a-z [97-122]
            for (i = 0; i < input.Length; i++)
            {
                //special character found, remove this
                if (input[i] < 97 || input[i] > 122)
                { return true; }
            }

            //all clear!
            return false;
        }


        public int binarySearch(String query)
        {
            int i = 0;
            int left = 0;
            int right = commonWords.Count -1;
            int middle = 0;
            
            String selected = String.Empty;

            int minLength = 0;

            while (left <= right)
            {
                middle = (int)(left + right) / 2;
                selected = commonWords[middle];

                //get the shortest string
                if (query.Length < selected.Length)
                { minLength = query.Length; }
                else
                { minLength = selected.Length; }

                //did we find our query?
                if (query.Equals(selected))
                {
                    return middle;
                }

                //compare characters...
                for (i = 0; i < minLength; i++)
                {
                    //keep iterating...
                    if (query[i] == selected[i])
                    {
                        continue;
                    }
                    else if (query[i] < selected[i]) //compare ASCII values
                    {
                        //move LEFT
                        right = middle - 1;
                        break;
                    }
                    else //compare ASCII values
                    {
                        //move RIGHT
                        left = middle + 1;
                        break;
                    }
                }

                //broke out of loop??? we decide based on shortest string
                if (i == minLength)
                {
                    if (query.Length < selected.Length)//query is shorter than index? move LEFT
                    {
                        right = middle - 1;
                    }
                    else //index is shorter than query? move RIGHT
                    {
                        left = middle + 1;
                    }
                }
            }

            return -1;
        }
    }
}
