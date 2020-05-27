using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Uni7ReservasBackend.Models.Entidades
{
    public class Util
    {
        public const string EMAIL_ADMIN = "reservas@uni7.edu.br";
        public const string EMAIL_CONTATO = "reservas-naoresponda@uni7.edu.br";
        public const string EMAIL_PWD = "F@h63Ie#";

        public static string GerarHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static void AtualizarDiasReservas(int coord, int prof, int apoio)
        {
            string jsonString = JsonSerializer.Serialize(new DiasReservas() 
                { MaxDiasCoord = coord, 
                  MaxDiasProf = prof, 
                  MaxDiasApoio = apoio });
            File.WriteAllText("configlimite.json", jsonString);
        }

        public static DiasReservas ConsultarDiasReservas()
        {
            string jsonString = File.ReadAllText(HttpContext.Current.Request.MapPath("~//Models/configlimite.json"));
            DiasReservas dr = JsonSerializer.Deserialize<DiasReservas>(jsonString);
            return dr;
        }

        public class DiasReservas
        {
            public int MaxDiasCoord { get; set; }
            public int MaxDiasProf { get; set; }
            public int MaxDiasApoio { get; set; }
        }

        public class HorarioPorHora
        {
            public string Horario { get; set; }
            public string Turno { get; set; }
            public int Minutos { get; set; }
        }

        public static int HorarioParaHora (string horario, string turno)
        {
            string jsonString = File.ReadAllText(HttpContext.Current.Request.MapPath("~//Models/confighorario.json"));
            List<HorarioPorHora> horarios = JsonSerializer.Deserialize<List<HorarioPorHora>>(jsonString);
            HorarioPorHora hh = horarios.Find(h => h.Horario == horario && h.Turno == turno);
            return hh == null ? 0 : hh.Minutos;
        }

    }
}