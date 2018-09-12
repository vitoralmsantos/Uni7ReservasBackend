using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Models
{
    public partial class Usuario
    {
        public static readonly int TAMANHOSENHA = 6;

        public static Usuario ConsultarUsuarioPorEmail(string email)
        {
            Usuario usuario = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntityException(EntityExcCode.EMAILNAOCADASTRADO, email);
                else
                    usuario = usuario_.First();
            }

            return usuario;
        }

        public static Usuario ConsultarMedicoPorId(int idUsuario)
        {
            Usuario usuario = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Id == idUsuario
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntityException(EntityExcCode.IDUSUARIONAOCADASTRADO, idUsuario.ToString());
                else
                    usuario = usuario_.First();
            }

            return usuario;
        }

        public static void Cadastrar(string nome, string email, TIPOUSUARIO tipo)
        {
            if (nome == null || nome.Length == 0)
                throw new EntityException(EntityExcCode.NOMEUSUARIOVAZIO, "");

            Regex regexEmail = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                        + @"[a-zA-Z]{2,}))$");
            if (!regexEmail.IsMatch(email))
                throw new EntityException(EntityExcCode.EMAILINVALIDO, email);

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario_.Count() > 0)
                    throw new EntityException(EntityExcCode.EMAILJACADASTRADO, email);
                else
                {
                    Usuario usuario = new Usuario();
                    usuario.Nome = nome;
                    usuario.Email = email;
                    usuario.Tipo = tipo;

                    context.Usuarios.Add(usuario);
                    context.SaveChanges();
                }
            }

            try
            {
                EnviarNovaSenha(email);
            }
            catch (Exception ex)
            {
                throw new EntityException(EntityExcCode.SENHANAOENVIADA, ex.Message);
            }
        }

        public static void Atualizar(int idUsuario, string nome, string email, TIPOUSUARIO tipo)
        {
            if (nome == null || nome.Length == 0)
                throw new EntityException(EntityExcCode.NOMEUSUARIOVAZIO, "");

            Regex regexEmail = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                        + @"[a-zA-Z]{2,}))$");
            if (!regexEmail.IsMatch(email))
                throw new EntityException(EntityExcCode.EMAILINVALIDO, email);

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario1_ = from Usuario u in context.Usuarios
                               where u.Id == idUsuario
                               select u;

                if (usuario1_.Count() == 0)
                    throw new EntityException(EntityExcCode.IDUSUARIONAOCADASTRADO, idUsuario.ToString());

                var usuario2_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario2_.Count() > 0)
                    throw new EntityException(EntityExcCode.EMAILJACADASTRADO, email);

                Usuario usuario = usuario1_.First();
                usuario.Nome = nome;
                usuario.Email = email;
                usuario.Tipo = tipo;

                context.SaveChanges();
            }
        }

        public static void EnviarNovaSenha(string email)
        {
            string nomeUsuario = "";

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntityException(EntityExcCode.EMAILNAOCADASTRADO, email);

                nomeUsuario = usuario_.First().Nome;
            }

            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder pwdBuilder = new StringBuilder();
            char ch;
            for (int i = 0; i < TAMANHOSENHA; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                pwdBuilder.Append(ch);
            }
            string password = pwdBuilder.ToString();

           
            var fromAddress = new MailAddress(Util.EMAIL_CONTATO, "UNI7 Reservas");
            var toAddress = new MailAddress(email, "");
            string fromPassword = Util.EMAIL_PWD;
            string subject = "UNI7 Reservas - Senha de acesso";

            string body = "";
            body += "<p>Prezado(a) " + nomeUsuario + ",</p>";
            body += "<p>Sua nova senha de acesso do UNI7 Reservas é " + password + "</p>";
            body += "<p></p>";
            body += "<a href='http://www.uni7.edu.br/reservas'><img alt =\"\" hspace=0 src=\"cid:imageId\" align=baseline border=0 width=\"120\"></a>";
            body += "<br/><a href='http://www.uni7.edu.br/reservas'>http://www.uni7.edu.br/reservas</a>";

            var smtp = new SmtpClient
            {
                Host = "",
                Port = 25,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var message = new MailMessage(fromAddress, toAddress);

            message.Subject = subject;

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            LinkedResource imagelink = new LinkedResource(HttpContext.Current.Server.MapPath("~/Imagens/uni7reservaslogo.png"), "image/png");
            imagelink.ContentId = "imageId";
            imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            htmlView.LinkedResources.Add(imagelink);
            message.AlternateViews.Add(htmlView);

            smtp.Send(message);
        }

        public static void AtualizarSenha(int idUsuario, string oldPassword, string newPassword)
        {
            if (newPassword.Length < TAMANHOSENHA)
                throw new EntityException(EntityExcCode.SENHACURTA, "tamanho mínimo é " + TAMANHOSENHA);

            string email = "";
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Id == idUsuario
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntityException(EntityExcCode.IDUSUARIONAOCADASTRADO, idUsuario.ToString());
                else if 
                    usuario = usuario_.First();
            }

        }
    }
}