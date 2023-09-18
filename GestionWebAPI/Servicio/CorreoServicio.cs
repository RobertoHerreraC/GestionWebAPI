using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GestionWebAPI.Servicio
{
    public class CorreoServicio
    {
        ConfiguracionCorreo cc = new ConfiguracionCorreo();

        public string EnviarCorreo(CorreoRequest request)
        {
            try
            {
                //var archivosAdjuntos = new List<string>
                //{
                //  @"C:\archivos\texto.txt",
                //};

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(cc.UserName()));
                email.To.Add(MailboxAddress.Parse(request.Destinatario));
                email.Subject = request.Asunto;

                var builder = new BodyBuilder();
                builder.HtmlBody = request.Contenido;

                var multipart = new Multipart("mixed");
                multipart.Add(builder.ToMessageBody());

                if (request.Adjuntos != null)
                {

                    foreach (var archivoAdjunto in request.Adjuntos)
                    {
                        if (!string.IsNullOrEmpty(archivoAdjunto) && File.Exists(archivoAdjunto))
                        {
                            var attachment = new MimePart()
                            {
                                Content = new MimeContent(File.OpenRead(archivoAdjunto), ContentEncoding.Default),
                                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                ContentTransferEncoding = ContentEncoding.Base64,
                                FileName = Path.GetFileName(archivoAdjunto)
                            };

                            multipart.Add(attachment);
                        }
                    }
                }

                email.Body = multipart;

                using var smtp = new SmtpClient();
                smtp.Connect(
                    cc.Host(),
                    Convert.ToInt32(cc.Port()),
                    SecureSocketOptions.StartTls
                );

                smtp.Authenticate(cc.UserName(), cc.Pass());
                smtp.Send(email);
                smtp.Disconnect(true);

                return "Correo enviado exitosamente";


            }
            catch (Exception ex)
            {
                throw new Exception("Error general al enviar correo: " + ex.Message, ex);
            }


        }
    }
}
