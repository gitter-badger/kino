﻿using Autofac;

namespace kino.Rendezvous.Consensus
{
    public class ConsensusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LeaseProvider>()
                   .As<ILeaseProvider>()
                   .SingleInstance();

            builder.RegisterType<RoundBasedRegister>()
                   .As<IRoundBasedRegister>()
                   .SingleInstance();

            builder.RegisterType<BallotGenerator>()
                   .As<IBallotGenerator>()
                   .SingleInstance();

            builder.RegisterType<SynodConfiguration>()
                   .As<ISynodConfiguration>()
                   .SingleInstance();

            builder.RegisterType<IntercomMessageHub>()
                   .As<IIntercomMessageHub>()
                   .SingleInstance();
        }
    }
}