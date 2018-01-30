using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace GetData
{
    class Program
    {
        private static Logger log= NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {            
            try
            {
                HeadersFromAndSubject(@"pop3server", 995, true, "username", "password");
            }
            catch (Exception e)
            {
                log.Info(e.Message);
            }            
        }
        
        public static void HeadersFromAndSubject(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                
                log.Info("Try to connect server...");
                client.Connect(hostname, port, useSsl);  // Connect to the server                
                client.Authenticate(username, password); // Authenticate ourselves towards the server
                log.Info("Server connected.");
                // We want to check the headers of the message before we download the full message
                int messageCount = client.GetMessageCount();
                log.Info($"You have {messageCount} messages.");
                int messageNumber ;
                for (messageNumber = 1; messageNumber <= messageCount; messageNumber++)
                {
                    MessageHeader headers = client.GetMessageHeaders(messageNumber);
                    RfcMailAddress from = headers.From;
                    string subject = headers.Subject;
                    
                    // Only want to download message if: is from test@xample.com and has subject "Some subject"
                    if (from.HasValidMailAddress && from.Address.Equals("a@b.com") && ("a".Equals(subject) || "b".Equals(subject)))
                    {
                        log.Info($"The subject you are looking for is: {subject}");
                        // Download the full message
                        Message message = client.GetMessage(messageNumber);
                        log.Info($"The message sent date is: {message.Headers.DateSent}");
                        if (message.Headers.DateSent>=DateTime.Today) {
                            log.Info("Try to get the mail...");
                            foreach (MessagePart attachment in message.FindAllAttachments())
                            {
                                if (attachment.FileName.Equals("c.zip"))
                                {
                                    // Save the raw bytes to a file
                                    var oldfilename = attachment.FileName;
                                    var newfilename = oldfilename.Substring(0, oldfilename.Length - 4) +"_"+ DateTime.Now.ToString("yyyyMMdd") + ".zip";
                                    File.WriteAllBytes(newfilename, attachment.Body);
                                }
                            }
                            log.Info("Done.");
                        }
                    }
                }
            }
        }
    }
}
