﻿using System.Collections.Generic;

namespace Braspag.Sdk.Contracts.CartaoProtegido
{
    public class GetExtraDataRequest
    {
        public string JustClickKey { get; set; }

        public string JustClickAlias { get; set; }

        public List<string> FieldCollection { get; set; }
    }
}