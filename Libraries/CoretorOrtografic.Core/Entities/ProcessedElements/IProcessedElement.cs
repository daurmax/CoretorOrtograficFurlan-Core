﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.CoretorOrtografic.Entities.ProcessedElements
{
    public interface IProcessedElement
    {
        string Original { get; }
        string Current { get; set; }
    }
}
