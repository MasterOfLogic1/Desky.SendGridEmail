using System;
using System.Activities;
using System.ComponentModel;
using System.Security;

namespace Desky.SendGridEmail
{
    [DisplayName("Send Grid Email")]
    [Description("Sends an email using SendGrid")]
    [Category("Desky SendGrid Email")]
    public class SendGridEmail : CodeActivity
    {
        [Category("Input")]
        [Description("Your SendGrid API key as a SecureString (if using this)")]
        public InArgument<SecureString> ApiKeySecure { get; set; }

        [Category("Input")]
        [Description("Your SendGrid API key as a plain string (if using this)")]
        public InArgument<string> ApiKey { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [Description("Sender's email address")]
        public InArgument<string> SenderEmail { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [Description("Sender's name")]
        public InArgument<string> SenderName { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [Description("Comma-separated recipient email addresses")]
        public InArgument<string> MailRecipientsTo { get; set; }

        [Category("Input")]
        [Description("Comma-separated CC email addresses")]
        public InArgument<string> MailRecipientsCc { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [Description("Email subject")]
        public InArgument<string> MailSubject { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [Description("Email body")]
        public InArgument<string> MailBody { get; set; }

        [Category("Input")]
        [Description("Is the email body in HTML format?")]
        public InArgument<bool> IsHtml { get; set; }

        [Category("Input")]
        [Description("File attachments (optional)")]
        public InArgument<string[]> MailFileAttachments { get; set; }

        [Category("Output")]
        [Description("Response message after the email is sent")]
        public OutArgument<string> ActionResponse { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string _apiKey = null;

            // Use SecureString if provided, otherwise use the regular string
            SecureString _secureApiKey = ApiKeySecure.Get(context);
            if (_secureApiKey != null)
            {
                _apiKey = ConvertSecureStringToString(_secureApiKey);
            }
            else
            {
                _apiKey = ApiKey.Get(context);
                if (string.IsNullOrEmpty(_apiKey))
                {
                    throw new ArgumentNullException("API key must be provided as either a SecureString or a normal string.");
                }
            }

            string _senderEmail = SenderEmail.Get(context);
            string _senderName = SenderName.Get(context);
            string _mailRecipientsTo = MailRecipientsTo.Get(context);
            string _mailRecipientsCc = MailRecipientsCc.Get(context);
            string _mailSubject = MailSubject.Get(context);
            string _mailBody = MailBody.Get(context);
            bool _isHtml = IsHtml.Get(context);
            string[] _mailFileAttachments = MailFileAttachments.Get(context);
            string actionResponse;

            bool success = ActionHelper.SendEmail(
                _apiKey,
                _senderEmail,
                _senderName,
                _mailRecipientsTo,
                _mailRecipientsCc,
                _mailSubject,
                _mailBody,
                _isHtml,
                _mailFileAttachments,
                out actionResponse
            );

            ActionResponse.Set(context, actionResponse);

            if (!success)
            {
                throw new Exception("Email sending failed: " + actionResponse);
            }
        }

        // Method to convert SecureString to a regular string
        private string ConvertSecureStringToString(SecureString secureString)
        {
            return new System.Net.NetworkCredential(string.Empty, secureString).Password;
        }
    }
}
