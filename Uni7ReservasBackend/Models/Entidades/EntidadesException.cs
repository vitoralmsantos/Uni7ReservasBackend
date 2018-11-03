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
        USUARIOPOSSUICHAMADOS = 1011,
        USUARIOPOSSUIRESERVASBOLSISTA = 1012,
        LOCALINDISPONIVEL = 1101,
        EQUIPAMENTOINDISPONIVEL = 1102,
        EQUIPAMENTONECESSARIO = 1103,
        EQUIPAMENTOSIGUAIS = 1104,
        RESTRICAOLOCALEQUIPAMENTO = 1105,
        RESERVAINEXISTENTE = 1106,
        LOCALINDISPONIVELPROPRIOUSUARIO = 1107,
        DATAINICIALINVALIDA = 1108,
        CATEGORIAINEXISTENTE = 1201,
        NOMECATEGORIAVAZIO = 1202,
        CATEGORIAJACADASTRADA = 1203,
        CATEGORIAPOSSUIRESERVAS = 1204,
        CATEGORIAPOSSUIEQUIPAMENTOS = 1205,
        LOCALINEXISTENTE = 1301,
        NOMELOCALVAZIO = 1302,
        LOCALPOSSUIRESERVAS = 1303,
        RESTRICAOJACADASTRADA = 1304,
        RESTRICAOINEXISTENTE = 1305,
        EQUIPAMENTOINEXISTENTE = 1401,
        MODELOEQUIPAMENTOVAZIO = 1402,
        EQUIPAMENTONOLIMITEDERESERVAS = 1403,
        RECURSOINEXISTENTE = 1501,
        NOMERECURSOVAZIO = 1502,
        RECURSOPOSSUILOCAIS = 1503,
        RECURSOLOCALINEXISTENTE = 1506,
        CHAMADOINEXISTENTE = 1601,
        DESCRICAOCHAMADOVAZIO = 1602
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
                    case EntityExcCode.USUARIOPOSSUICHAMADOS:
                        return String.Format("Usuário possui {0} chamados.", Detalhe);
                    case EntityExcCode.USUARIOPOSSUIRESERVASBOLSISTA:
                        return String.Format("Bolsita possui {0} reservas vinculadas a ele(a).", Detalhe);
                    case EntityExcCode.LOCALINDISPONIVEL:
                        return String.Format("O local já possui reserva nesta data e horário por {0}.", Detalhe);
                    case EntityExcCode.LOCALINDISPONIVELPROPRIOUSUARIO:
                        return "O local já possui reserva em seu nome nesta data e horário. Se desejar incluir um equipamento, por favor, selecione a opção de edição ao lado da reserva realizada.";
                    case EntityExcCode.DATAINICIALINVALIDA:
                        return "A data inicial não coincide com nenhum dos dias da semana a serem reservados.";
                    case EntityExcCode.EQUIPAMENTOINDISPONIVEL:
                        return "O equipamento está indisponível: " + Detalhe;
                    case EntityExcCode.EQUIPAMENTONECESSARIO:
                        return "Um equipamento precisa ser escolhido.";
                    case EntityExcCode.EQUIPAMENTOSIGUAIS:
                        return "Equipamentos não podem ser iguais.";
                    case EntityExcCode.RESTRICAOLOCALEQUIPAMENTO:
                        return "O equipamento especificado não pode ser reservado para o local: " + Detalhe;
                    case EntityExcCode.EQUIPAMENTOINEXISTENTE:
                        return "Equipamento inexistente: " + Detalhe;
                    case EntityExcCode.RESERVAINEXISTENTE:
                        return "Reserva inexistente: " + Detalhe;
                    case EntityExcCode.CATEGORIAINEXISTENTE:
                        return "Categoria inexistente: " + Detalhe;
                    case EntityExcCode.NOMECATEGORIAVAZIO:
                        return "Nome da categoria não pode ser vazio.";
                    case EntityExcCode.CATEGORIAPOSSUIRESERVAS:
                        return String.Format("Categoria possui reservas: {0}", Detalhe);
                    case EntityExcCode.CATEGORIAPOSSUIEQUIPAMENTOS:
                        return String.Format("Categoria possui {0} equipamentos.", Detalhe);
                    case EntityExcCode.MODELOEQUIPAMENTOVAZIO:
                        return String.Format("Modelo do equipamento não pode ser vazio.");
                    case EntityExcCode.LOCALINEXISTENTE:
                        return "Local inexistente: " + Detalhe;
                    case EntityExcCode.NOMELOCALVAZIO:
                        return String.Format("Nome do local não pode ser vazio.");
                    case EntityExcCode.LOCALPOSSUIRESERVAS:
                        return String.Format("Local possui reservas: {0}", Detalhe);
                    case EntityExcCode.RESTRICAOJACADASTRADA:
                        return String.Format("A restrição já está cadastrada: {0}", Detalhe);
                    case EntityExcCode.RESTRICAOINEXISTENTE:
                        return String.Format("A restrição inexistente: {0}", Detalhe);
                    case EntityExcCode.EQUIPAMENTONOLIMITEDERESERVAS:
                        return String.Format("O equipamento não pode ser removido, pois a quantidade de reservas da sua categoria está no limite na(s) seguinte(s) data(s), turno(s) e horário(s): {0}", Detalhe);
                    case EntityExcCode.RECURSOINEXISTENTE:
                        return "Recurso inexistente: " + Detalhe;
                    case EntityExcCode.NOMERECURSOVAZIO:
                        return "Nome do recurso não pode ser vazio.";
                    case EntityExcCode.RECURSOPOSSUILOCAIS:
                        return String.Format("Recurso possui locais: {0}", Detalhe);
                    case EntityExcCode.RECURSOLOCALINEXISTENTE:
                        return String.Format("Recurso não existe no local especificado.");
                    case EntityExcCode.CHAMADOINEXISTENTE:
                        return String.Format("Chamado inexistente: {0}", Detalhe);
                    case EntityExcCode.DESCRICAOCHAMADOVAZIO:
                        return String.Format("Descrição do chamado não pode ser vazia.");
                    default: return "Erro desconhecido";
                }
            }
        }
    }
}