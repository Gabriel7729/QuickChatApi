using ApiTemplate.Core.Interfaces;
using ApiTemplate.Core.Services;
using Autofac;

namespace ApiTemplate.Core;
public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ChatService>().As<IChatService>()
      .InstancePerLifetimeScope();
  }
}
