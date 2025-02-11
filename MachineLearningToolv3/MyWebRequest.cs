﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace MachineLearningToolv3
{
    public class MyWebRequest
    {
        private WebRequest request;
        private Stream dataStream;
        public string  responseFromServer;

        private string status;

        public String Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public MyWebRequest(string url)
        {
            // Create a request using a URL that can receive a post.

            request = WebRequest.Create(url);
        }

        public MyWebRequest(string url, string method)
            : this(url)
        {

            if (method.Equals("GET") || method.Equals("POST"))
            {
                // Set the Method property of the request to POST.
                request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        public MyWebRequest(string url, string method, string data)
            : this(url, method)
        {

            // Create POST data and convert it to a byte array.
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();

        }

        public HttpStatusCode GetResponse()
        {
            // Get the original response.
            WebResponse response = request.GetResponse();

            this.Status = ((HttpWebResponse)response).StatusDescription;
            var code = ((HttpWebResponse)response).StatusCode;
            // Get the stream containing all content returned by the requested server.
            dataStream = response.GetResponseStream();
           

            // Open the stream using a StreamReader for easy access.

            //using (StreamWriter writer = new StreamWriter(dataStream))
            //{
            //    writer.Write(dataStream.ToString());
            //}
           // try
           // {
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    responseFromServer = reader.ReadToEnd();

                    FileReadWrite.WriteFile(Environment.GetEnvironmentVariable("windir") + @"/locred.dat", responseFromServer);

                    dataStream.Close();
                    response.Close();
                    return code;
                }
            //}
            //catch(System.ArgumentException e)
           // {
            //    Console.WriteLine("error is normal");
             //   return code;
           // }
           

                // Read the content fully up to the end.
               

            // Clean up the streams.
           // reader.Close();
           // reader.Dispose();
            

            //return responseFromServer;
        }

    }
}
