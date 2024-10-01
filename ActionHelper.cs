using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Desky.SendGridEmail
{
    internal static class ActionHelper
    {
        public static bool SendEmail(string apiKey, string senderEmail, string senderName, string mailRecipientsTo, string mailRecipientsCc, string mailSubject, string mailBody, bool isHtml, string[] mailFileAttachments , out string actionResponse)
        {
            try
            {
                Console.WriteLine("Sendgrid : sending mails...");
                var sendGridClient = new SendGrid.SendGridClient(apiKey);
                var fromEmail = new SendGrid.Helpers.Mail.EmailAddress(senderEmail, senderName);
                var toEmails = new List<SendGrid.Helpers.Mail.EmailAddress>();
                var ccEmails = new List<SendGrid.Helpers.Mail.EmailAddress>();
                var attachments = new List<SendGrid.Helpers.Mail.Attachment>();
                string[] cc = new string[] { };

                // Add recipient mails
                if (!string.IsNullOrEmpty(mailRecipientsTo))
                {
                    mailRecipientsTo = mailRecipientsTo.Replace(";", ",");
                    var recipientsTo = mailRecipientsTo.Split(',');

                    // Loop through all the "To" recipients and add them to the list
                    foreach (var recipient in recipientsTo)
                    {
                        toEmails.Add(new SendGrid.Helpers.Mail.EmailAddress(recipient.Trim(), null));
                    }
                }
                else
                {
                    throw new SystemException("Recipient address not provided");
                }


                // Add specified Cc recipients
                if (!string.IsNullOrEmpty(mailRecipientsCc))
                {
                    mailRecipientsCc = mailRecipientsCc.Replace(";", ",");
                    var recipientsCc = mailRecipientsCc.Split(',');

                    foreach (var recipient in recipientsCc)
                    {
                        ccEmails.Add(new SendGrid.Helpers.Mail.EmailAddress(recipient.Trim(), null));
                    }
                }


                SendGrid.Helpers.Mail.SendGridMessage msg;

                // Create the message
                if (isHtml)
                {
                    msg = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(fromEmail, toEmails.First(), mailSubject, null, mailBody);
                }
                else
                {
                    msg = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(fromEmail, toEmails.First(), mailSubject, mailBody + Environment.NewLine, null);
                }

                // Add all "To" recipients
                if (toEmails != null && toEmails.Count > 1)
                {
                    msg.AddTos(toEmails); 
                }

                // Add "Cc" recipients
                if (ccEmails != null && ccEmails.Count > 0)
                {
                    msg.AddCcs(ccEmails);
                }

                // Add attachments if any
                if (mailFileAttachments != null && mailFileAttachments.Length > 0)
                {
                    foreach (var filePath in mailFileAttachments)
                    {
                        if (File.Exists(filePath))
                        {
                            attachments.Add(new SendGrid.Helpers.Mail.Attachment
                            {
                                Content = Convert.ToBase64String(File.ReadAllBytes(filePath)),
                                Filename = Path.GetFileName(filePath)
                            });
                        }
                    }
                }

                // Add all attachments
                if (attachments != null && attachments.Count > 0)
                {
                    msg.AddAttachments(attachments);
                }

                // Send the email
                var response = sendGridClient.SendEmailAsync(msg).Result; // Use SendEmailAsync for asynchronous sending
                Console.WriteLine("Sendgrid : mail sent!");

                actionResponse = "Success";
                return true;
            }
            catch (Exception ex)
            {
                actionResponse = ex.Message;
                return false;
            }
        }

    }
}
