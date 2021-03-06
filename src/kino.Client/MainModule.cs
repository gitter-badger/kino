﻿using Autofac;

namespace kino.Client
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new kino.MainModule());

            builder.RegisterType<MessageHub>()
                   .As<IMessageHub>()
                   .SingleInstance();

            builder.RegisterType<CallbackHandlerStack>()
                   .As<ICallbackHandlerStack>()
                   .SingleInstance();

            builder.RegisterType<MessageTracer>()
                   .As<IMessageTracer>()
                   .SingleInstance();
        }
    }
}