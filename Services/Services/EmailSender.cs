﻿// EmailSender.cs
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

public class EmailSender : IEmailSender
{
	private readonly EmailConfiguration _emailConfiguration;

	public EmailSender(EmailConfiguration emailConfiguration)
	{
		_emailConfiguration = emailConfiguration;
	}

	public async Task SendEmailAsync(string email, string subject, string htmlMessage)
	{
		var mimeMessage = new MimeMessage();
		mimeMessage.From.Add(new MailboxAddress(_emailConfiguration.FromName, _emailConfiguration.FromAddress));
		mimeMessage.To.Add(new MailboxAddress("", email));
		mimeMessage.Subject = subject;

		var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
		mimeMessage.Body = bodyBuilder.ToMessageBody();

		using (var client = new SmtpClient())
		{
			await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
			await client.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
			await client.SendAsync(mimeMessage);
			await client.DisconnectAsync(true);
		}
	}
}
