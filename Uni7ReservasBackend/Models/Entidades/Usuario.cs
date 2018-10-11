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
        public static readonly int TEMPOSESSAO = 20;

        public static List<Usuario> ConsultarUsuarios()
        {
            List<Usuario> Usuarios = new List<Usuario>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuarios_ = from Usuario u in context.Usuarios
                                select u;

                Usuarios = usuarios_.ToList();
            }

            return Usuarios;
        }

        public static Usuario ConsultarUsuarioPorEmail(string email)
        {
            Usuario usuario = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.EMAILNAOCADASTRADO, email);
                else
                    usuario = usuario_.First();
            }

            return usuario;
        }

        public static bool ValidarToken(string email, string token)
        {
            bool tokenValido = false;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.EMAILNAOCADASTRADO, email);
                else
                {
                    Usuario usuario = usuario_.First();
                    byte[] data = Convert.FromBase64String(token);
                    DateTime hora = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

                    //Token coincide e está não expirou
                    if (usuario.Token.Equals(token) && hora < DateTime.UtcNow.AddMinutes(-1 * TEMPOSESSAO))
                    {
                        tokenValido = true;
                    }
                }
            }

            return tokenValido;
        }

        /// <summary>
        /// Retorna o token da sessão.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        public static string Autenticar(string email, string senha)
        {
            string token = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.EMAILNAOCADASTRADO, email);
                else
                {
                    string senhaBD = usuario_.First().Senha;
                    if (senhaBD.Equals(Util.GerarHashMd5(senha)))
                    {
                        byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                        byte[] key = Guid.NewGuid().ToByteArray();
                        token = Convert.ToBase64String(time.Concat(key).ToArray());

                        usuario_.First().Token = token;
                        context.SaveChanges();
                    }
                    else
                        throw new EntidadesException(EntityExcCode.AUTENTICACAO, email);
                }
            }

            return token;
        }

        public static Usuario ConsultarUsuarioPorId(int idUsuario)
        {
            Usuario usuario = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Id == idUsuario
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.IDUSUARIONAOCADASTRADO, idUsuario.ToString());
                else
                    usuario = usuario_.First();
            }

            return usuario;
        }

        public static int Cadastrar(string nome, string email, TIPOUSUARIO tipo)
        {
            Usuario usuario = null;

            if (nome == null || nome.Length == 0)
                throw new EntidadesException(EntityExcCode.NOMEUSUARIOVAZIO, "");

            Regex regexEmail = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                        + @"[a-zA-Z]{2,}))$");
            if (!regexEmail.IsMatch(email))
                throw new EntidadesException(EntityExcCode.EMAILINVALIDO, email);

            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder pwdBuilder = new StringBuilder();
            char ch;
            for (int i = 0; i < TAMANHOSENHA; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                pwdBuilder.Append(ch);
            }
            string senhaTemp = pwdBuilder.ToString(); //Senha temporária pois o campo não pode ser nulo.

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario_.Count() > 0)
                    throw new EntidadesException(EntityExcCode.EMAILJACADASTRADO, email);
                else
                {
                    usuario = new Usuario();
                    usuario.Nome = nome;
                    usuario.Email = email;
                    usuario.Tipo = tipo;
                    usuario.Senha = senhaTemp;

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
                throw new EntidadesException(EntityExcCode.SENHANAOENVIADA, ex.Message);
            }

            return usuario.Id;
        }

        public static void Atualizar(int idUsuario, string nome, string email, TIPOUSUARIO tipo)
        {
            if (nome == null || nome.Length == 0)
                throw new EntidadesException(EntityExcCode.NOMEUSUARIOVAZIO, "");

            Regex regexEmail = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                        + @"[a-zA-Z]{2,}))$");
            if (!regexEmail.IsMatch(email))
                throw new EntidadesException(EntityExcCode.EMAILINVALIDO, email);

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario1_ = from Usuario u in context.Usuarios
                               where u.Id == idUsuario
                               select u;

                if (usuario1_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.IDUSUARIONAOCADASTRADO, idUsuario.ToString());

                var usuario2_ = from Usuario u in context.Usuarios
                               where u.Email == email
                               select u;

                if (usuario2_.Count() > 0 && !usuario1_.First().Email.Equals(email))
                    throw new EntidadesException(EntityExcCode.EMAILJACADASTRADO, email);

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
                    throw new EntidadesException(EntityExcCode.EMAILNAOCADASTRADO, email);

                nomeUsuario = usuario_.First().Nome;
            }

            try
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                StringBuilder pwdBuilder = new StringBuilder();
                char ch;
                for (int i = 0; i < TAMANHOSENHA; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    pwdBuilder.Append(ch);
                }
                string senha = pwdBuilder.ToString();

                var fromAddress = new MailAddress(Util.EMAIL_CONTATO, "UNI7 Reservas");
                var toAddress = new MailAddress(email, "");
                string fromPassword = Util.EMAIL_PWD;
                string subject = "UNI7 Reservas - Senha de acesso";

                string body = "";
                body += "<p>Prezado(a) " + nomeUsuario + ",</p>";
                body += "<p>Sua nova senha de acesso do UNI7 Reservas é " + senha + "</p>";
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

                using (Uni7ReservasEntities context = new Uni7ReservasEntities())
                {
                    var usuario_ = from Usuario u in context.Usuarios
                                   where u.Email == email
                                   select u;

                    if (usuario_.Count() == 0)
                        throw new EntidadesException(EntityExcCode.EMAILNAOCADASTRADO, email);

                    usuario_.First().Senha = Util.GerarHashMd5(senha);
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw new EntidadesException(EntityExcCode.SENHANAOENVIADA, ex.Message);
            }
        }

        public static void AtualizarSenha(int idUsuario, string oldPassword, string newPassword)
        {
            if (newPassword.Length < TAMANHOSENHA)
                throw new EntidadesException(EntityExcCode.SENHACURTA, "tamanho mínimo é " + TAMANHOSENHA);

            
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Id == idUsuario
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.IDUSUARIONAOCADASTRADO, idUsuario.ToString());
                
                if (Util.GerarHashMd5(oldPassword) != Util.GerarHashMd5(usuario_.First().Senha))
                {
                    throw new EntidadesException(EntityExcCode.SENHANAOCONFERE, "");
                }

                usuario_.First().Senha = Util.GerarHashMd5(newPassword);
                context.SaveChanges();
            }

        }

        public static void Remover(int idUsuario)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var usuario_ = from Usuario u in context.Usuarios.Include("Reservas").Include("ReservasBolsista").Include("Chamados")
                               where u.Id == idUsuario
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.IDUSUARIONAOCADASTRADO, idUsuario.ToString());

                if (usuario_.First().Chamados.Count() > 0)
                    throw new EntidadesException(EntityExcCode.USUARIOPOSSUICHAMADOS, usuario_.First().Chamados.Count().ToString());

                if (usuario_.First().ReservasBolsista.Count() > 0)
                    throw new EntidadesException(EntityExcCode.USUARIOPOSSUIRESERVASBOLSISTA, usuario_.First().ReservasBolsista.Count().ToString());

                //Consulta reservas futuras
                List<Reserva> reservas = usuario_.First().Reservas.Where(r => r.Data > DateTime.Now.AddDays(-1)).ToList();
                if (reservas.Count() > 0)
                    throw new EntidadesException(EntityExcCode.USUARIOPOSSUIRESERVAS, reservas.Count().ToString());

                //Remove as reservas antigas
                context.Reservas.RemoveRange(usuario_.First().Reservas);
                context.Usuarios.Remove(usuario_.First());
                context.SaveChanges();
            }
        }
    }
}