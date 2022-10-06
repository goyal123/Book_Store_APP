using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQModel
    {
        MessageQueue messagequeue = new MessageQueue();
        public void sendData2Queue(string token)
        {
            messagequeue.Path = @".\private$\Token";
            if(!MessageQueue.Exists(messagequeue.Path))
                MessageQueue.Create(messagequeue.Path);
            

            messagequeue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messagequeue.ReceiveCompleted += Messagequeue_ReceiveCompleted;
            messagequeue.Send(token);
            messagequeue.BeginReceive();
            messagequeue.Close();
        }

        private void Messagequeue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                var msg = messagequeue.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
                string subject = "FundooNote App Password Reset Link";
                string body = token;
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("happy642world@gmail.com", "jvtaozxxrdtrgyep"),
                    EnableSsl = true
                };
                smtp.Send("happy642world@gmail.com", "happy642world@gmail.com", subject, body);

                messagequeue.BeginReceive();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
