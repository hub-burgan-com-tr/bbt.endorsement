﻿using Worker.App.Application.Internals.Models;

namespace Worker.App.Application.Workers.Commands.LoadContactInfos
{
    public class LoadContactInfoResponse
    {
        public CustomerList Customer { get; set; }
    }
}
