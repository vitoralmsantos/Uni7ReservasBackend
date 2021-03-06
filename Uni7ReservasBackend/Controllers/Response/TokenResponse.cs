﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Controllers.TransferObjects;

namespace Uni7ReservasBackend.Controllers.Response
{
    public class TokenResponse : BaseResponse
    {
        public string Token { get; set; }
        public int UserID { get; set; }
        public UsuarioTO Usuario { get; set; }
    }
}