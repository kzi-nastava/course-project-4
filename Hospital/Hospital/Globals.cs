using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Hospital
{
    public static class Globals
    {
        public static IContainer container;

        public static void Load()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<ProgramModule>();
            container = containerBuilder.Build();
        }
    }
}
