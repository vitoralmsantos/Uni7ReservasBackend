using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Models.Entidades
{
    public enum EntityExcCode
    {
        ERRODESCONHECIDO = 1,
        AUTENTICACAO = 1001,
        EMAILNAOCADASTRADO = 1002,
        IDUSUARIONAOCADASTRADO = 1003,
        EMAILINVALIDO = 1004,
        SENHACURTA = 1005,
        NOMEUSUARIOVAZIO = 1006,
        SENHANAOENVIADA = 1007,
        EMAILJACADASTRADO = 1008,
        SENHANAOCONFERE = 1009,
        USUARIOPOSSUIRESERVAS = 1010,
        LOCALINDISPONIVEL = 1101,
        EQUIPAMENTOINDISPONIVEL = 1102,
        EQUIPAMENTONECESSARIO = 1103,
        EQUIPAMENTOSIGUAIS = 1104,
        RESTRICALLOCALEQUIPAMENTO = 1105,
        RESERVAINEXISTENTE = 1106,
        CATEGORIAINEXISTENTE = 1201,
        NOMECATEGORIAVAZIO = 1202,
        CATEGORIAJACADASTRADA = 1203,
        CATEGORIAPOSSUIRESERVAS = 1204,
        CATEGORIAPOSSUIEQUIPAMENTOS = 1205,
        LOCALINEXISTENTE = 1301
        
    }

    public class EntidadesException : Exception
    {
        private string Detalhe { get; }
        public EntityExcCode Codigo { get; }

        public EntidadesException(EntityExcCode codigo, string detalhe)
            : base(detalhe)
        {
            this.Codigo = codigo;
            this.Detalhe = detalhe;
        }

        public override string Message
        {
            get
            {
                switch (Codigo)
                {
                    case EntityExcCode.ERRODESCONHECIDO:
                        return "Erro desconhecido: " + Detalhe;
                    case EntityExcCode.AUTENTICACAO:
                        return "Não foi possível realizar sua autenticação: " + Detalhe;
                    case EntityExcCode.EMAILNAOCADASTRADO:
                        return "E-mail não cadastrado: " + Detalhe;
                    case EntityExcCode.IDUSUARIONAOCADASTRADO:
                        return "ID de usuário não cadastrado: " + Detalhe;
                    case EntityExcCode.EMAILINVALIDO:
                        return "E-mail inválido: " + Detalhe;
                    case EntityExcCode.SENHACURTA:
                        return "Senha muito curta. " + Detalhe;
                    case EntityExcCode.NOMEUSUARIOVAZIO:
                        return "Nome de usuário não pode ser vazio.";
                    case EntityExcCode.SENHANAOENVIADA:
                        return "Erro ao tentar enviar nova senha: " + Detalhe;
                    case EntityExcCode.EMAILJACADASTRADO:
                        return "E-mail já cadastrado: " + Detalhe;
                    case EntityExcCode.SENHANAOCONFERE:
                        return "Senha antiga não confere.";
                    case EntityExcCode.USUARIOPOSSUIRESERVAS:
                        return String.Format("Usuário possui {0} reservas futuras.", Detalhe);
                    case EntityExcCode.LOCALINDISPONIVEL:
                        return "O local está indisponível.";
                    case EntityExcCode.EQUIPAMENTOINDISPONIVEL:
                        return "O equipamento está indisponível: " + Detalhe;
                    case EntityExcCode.EQUIPAMENTONECESSARIO:
                        return "Um equipamento precisa ser escolhido.";
                    case EntityExcCode.EQUIPAMENTOSIGUAIS:
                        return "Equipamentos não podem ser iguais.";
                    case EntityExcCode.RESTRICALLOCALEQUIPAMENTO:
                        return "O equipamento especificado não pode ser reservado para o local: " + Detalhe;
                    case EntityExcCode.RESERVAINEXISTENTE:
                        return "Reserva inexistente: " + Detalhe;
                    case EntityExcCode.CATEGORIAINEXISTENTE:
                        return "Categoria inexistente: " + Detalhe;
                    case EntityExcCode.NOMECATEGORIAVAZIO:
                        return "Nome da categoria não pode ser vazio.";
                    case EntityExcCode.CATEGORIAPOSSUIRESERVAS:
                        return String.Format("Categoria possui {0} reservas.", Detalhe);
                    case EntityExcCode.CATEGORIAPOSSUIEQUIPAMENTOS:
                        return String.Format("Categoria possui {0} equipamentos.", Detalhe);
                    case EntityExcCode.LOCALINEXISTENTE:
                        return "Local inexistente: " + Detalhe;
                    default: return "Erro desconhecido";
                }
            }
        }
    }
}