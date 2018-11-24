using System;
using System.Collections.Generic;
using System.Text;

namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1.Authentication
{
    public class ValidateAuthenticationRequest : OperationRequestBase
    {
        public string HeaderAuthorizationContent { get; set; }
    }
}
