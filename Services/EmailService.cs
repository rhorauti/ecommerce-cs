using System.Net;
using System.Net.Mail;

namespace e_commerce_cs.Services
{
  public class EmailService(IConfiguration config)
  {
    readonly string _fromEmail = config["EmailSettings:FromEmail"]
      ?? throw new ArgumentNullException(nameof(config), "Sem email definido.");
    readonly string _password = config["EmailSettings:Password"]
      ?? throw new ArgumentNullException(nameof(config), "Sem senha definida.");
    readonly string _smtpHost = config["EmailSettings:SmtpHost"]
      ?? throw new ArgumentNullException(nameof(config), "Smpt não definido.");
    readonly int _smtpPort = int.Parse(config["EmailSettings:SmtpPort"]
      ?? throw new ArgumentNullException(nameof(config), "Porta não definida."));
    readonly string? _urlFront = config["EmailSettings:UrlFront"]
      ?? throw new ArgumentNullException(nameof(config), "UrlFront não definida.");

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
      MailMessage message = new(_fromEmail, toEmail, subject, body)
      {
        IsBodyHtml = true
      };

      using SmtpClient client = new(_smtpHost, _smtpPort)
      {
        Credentials = new NetworkCredential(_fromEmail, _password),
        EnableSsl = true
      };

      await client.SendMailAsync(message);
    }

    public async Task SendEmailConfirmationSignUp(string toEmail, string name, string token)
    {
      string subject = "Confirme o seu cadastro!";
      string body = $@"
            <html>
              <body>
                <h2>Olá, <span style='font-weight: bold;'>{name}</span></h2>
                <p>Para confirmar seu cadastro na Atari, clique no link abaixo:</p><br>
                <h3><p><a href='{_urlFront}/redirect?token={token}' target='_blank'>Confirmar cadastro</a></p></h3><br>
                <p>Tenha um ótimo dia!</p><br>
                <p>Atenciosamente,</p>
                <p>Equipe Atari</p>
              </body>
            </html>";
      await SendEmailAsync(toEmail, subject, body);
    }
    
      public async Task SendEmailConfirmationResetPassword(string toEmail, string name, string token)
    {
      string subject = "Recuperação de senha!";
      string body = $@"
            <html>
              <body>
                <h2>Olá, <span style='font-weight: bold;'>{name}</span></h2>
                <p>Recebemos sua solicitação de recuperação de senha. Para criar uma nova senha, clique no link abaixo:</p><br>
                <h3><p><a href='{_urlFront}/new-password?token={ token}' target='_blank'>Recuperar senha</a></p></h3><br>
                <p>Se você não solicitou a recuperação de senha, por favor, ignore este e-mail, por motivos de segurança.</p>
                <p>Tenha um ótimo dia!</p><br>
                <p>Atenciosamente,</p>
                <p>Equipe Atari</p>
              </body>
            </html>";
      await SendEmailAsync(toEmail, subject, body);
    }
  }
};