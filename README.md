# Desky SendGrid Email UiPath Library

## Overview

**Desky SendGrid Email** is a custom UiPath activity that enables sending emails using the SendGrid API. This activity allows developers to automate the process of sending emails by configuring key details such as the sender, recipients, subject, and body, while optionally including attachments and setting whether the content should be in HTML format.

## Features

- Send emails via the SendGrid API.
- Use either a secure API key (`SecureString`) or a plain API key.
- Send emails to multiple recipients (To, CC).
- Include email subject, body, and attachments.
- Send emails in plain text or HTML format.
- Receive a response indicating the status of the email operation.

## Properties

### Inputs:

1. **ApiKeySecure** (SecureString)  
   - The SendGrid API key in a secure format.
   
2. **ApiKey** (string)  
   - The SendGrid API key in plain text.

3. **SenderEmail** (string, Required)  
   - The email address of the sender.

4. **SenderName** (string, Required)  
   - The name of the sender.

5. **MailRecipientsTo** (string, Required)  
   - Comma-separated email addresses for the recipients.

6. **MailRecipientsCc** (string)  
   - Comma-separated email addresses for CC (carbon copy) recipients.

7. **MailSubject** (string, Required)  
   - The subject of the email.

8. **MailBody** (string, Required)  
   - The body of the email.

9. **IsHtml** (bool)  
   - Indicates if the email body is in HTML format.

10. **MailFileAttachments** (string[])  
    - Array of file paths to include as attachments (optional).

### Outputs:

1. **ActionResponse** (string)  
   - The response message after the email is sent.

## Usage

1. **Add the activity to your workflow**:  
   Drag and drop the **SendGridEmail** activity into your UiPath workflow.

2. **Configure the inputs**:  
   Set up the required properties such as the API key (either `ApiKeySecure` or `ApiKey`), sender information, recipient list, subject, and body.

3. **Handle response**:  
   The output `ActionResponse` will provide a message indicating whether the email was sent successfully or if there were any errors.

### Example Workflow

To send an email:

1. Configure your API key securely by using `ApiKeySecure` or use the plain `ApiKey` property.
2. Specify the sender’s email and name.
3. Add the recipients’ email addresses in `MailRecipientsTo`.
4. Optionally, specify CC recipients in `MailRecipientsCc`.
5. Provide the subject in `MailSubject` and the body in `MailBody`.
6. If required, set `IsHtml` to `True` to format the email body as HTML.
7. Optionally, add file paths to `MailFileAttachments` for attachments.
8. Use `ActionResponse` to capture the response message.

## Exception Handling

If the API key is missing or email sending fails, the activity will throw an exception, and the error message will be provided in the response.

## Dependencies

This activity depends on SendGrid’s API for sending emails. You will need a valid API key from SendGrid to use this library.

## License



## Support

For any issues or feature requests, please reach out to the project maintainer at masteroflogic.mol@gmail.com.
