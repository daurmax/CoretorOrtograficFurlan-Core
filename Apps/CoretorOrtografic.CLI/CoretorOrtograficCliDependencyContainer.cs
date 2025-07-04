﻿using CoretorOrtografic.Business;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoretorOrtografic.CLI
{
    class CoretorOrtograficCliDependencyContainer
    {
        public static IContainer Configure(bool isDevelopment)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoretorOrtograficDependencyModule(isDevelopment, CallerApplicationEnum.CLI));

            return builder.Build();
        }
    }
}
