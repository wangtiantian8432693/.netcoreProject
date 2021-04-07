using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Domain.Dto
{
    public class TokenDto
    {
        public int expires_in { get; set; }
        public string access_token { get; set; }
    }
}
