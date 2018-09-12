﻿using System;
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
        EMAILJACADASTRADO = 1008
    }

    public class EntityException : Exception
    {
        private string Detail { get; }
        public EntityExcCode Ecode { get; }

        public EntityException(EntityExcCode ecode, string detail)
            : base(detail)
        {
            this.Ecode = ecode;
            this.Detail = detail;
        }

        public override string Message
        {
            get
            {
                switch (Ecode)
                {
                    case EntityExcCode.ERRODESCONHECIDO:
                        return "Erro desconhecido: " + Detail;
                    case EntityExcCode.AUTENTICACAO:
                        return "Não foi possível realizar sua autenticação: " + Detail;
                    case EntityExcCode.EMAILNAOCADASTRADO:
                        return "E-mail não cadastrado: " + Detail;
                    case EntityExcCode.IDUSUARIONAOCADASTRADO:
                        return "ID de usuário não cadastrado: " + Detail;
                    case EntityExcCode.EMAILINVALIDO:
                        return "E-mail inválido: " + Detail;
                    case EntityExcCode.SENHACURTA:
                        return "Senha muito curta. " + Detail;
                    case EntityExcCode.NOMEUSUARIOVAZIO:
                        return "Nome de usuário não pode ser vazio.";
                    case EntityExcCode.SENHANAOENVIADA:
                        return "Senha não enviada.";
                    case EntityExcCode.EMAILJACADASTRADO:
                        return "E-mail já cadastrado: " + Detail;
                    default: return "Erro desconhecido";
                }
            }
        }
    }
}