using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Caliburn.Micro;
using Hexenstein.Framework;
using Hexenstein.Framework.Caliburn;
using Hexenstein.UI.Shell;
using NLog.Config;
using NLog.Targets;
using ReactiveUI;
using ReactiveUI.NLog;
using CMLogManager = Caliburn.Micro.LogManager;
using LogLevel = NLog.LogLevel;
using NLogManager = NLog.LogManager;

namespace Hexenstein
{
    internal class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        protected IContainer Container { get; private set; }

        protected override void Configure()
        {
            ConfigureLogging();

            ConfigureContainer();
        }

        private void ConfigureLogging()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget();
            consoleTarget.Layout = "${level:uppercase=true} ${logger}: ${message}${onexception:inner=${newline}${exception:format=tostring}}";
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("ViewModelBinder", LogLevel.Warn, consoleTarget) { Final = true });
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
            NLogManager.Configuration = config;

            CMLogManager.GetLog = type => new CaliburnMicroNLogShim(type);

            RxApp.LoggerFactory = type => new NLogLogger(NLogManager.GetLogger(type.Name));
        }

        private void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var typesToRegister = Conventions.FindAll<AttachmentConventions>()
                .Concat(Conventions.FindAll<ViewModelConventions>())
                .Concat(Conventions.FindAll<ViewConventions>())
                ;

            foreach (var type in typesToRegister)
                builder.RegisterType(type)
                    .AsSelf()
                    .InstancePerDependency();

            foreach (var type in Conventions.FindAll<ServiceConventions>())
                builder.RegisterType(type)
                    .AsImplementedInterfaces()
                    .SingleInstance();

            builder.RegisterModule<AutoAttachmentModule>();

            builder.Register<IWindowManager>(c => new WindowManager()).InstancePerLifetimeScope();

            builder.Register<IEventAggregator>(c => new RxEventAggregator()).InstancePerLifetimeScope();

            Container = builder.Build();
        }

        protected override void PrepareApplication()
        {
            this.Application.Startup += this.OnStartup;
            if (!Debugger.IsAttached)
                this.Application.DispatcherUnhandledException += this.OnUnhandledException;
            this.Application.Exit += this.OnExit;
        }

        protected override object GetInstance(System.Type service, string key)
        {
            object instance;
            if (string.IsNullOrWhiteSpace(key))
            {
                if (Container.TryResolve(service, out instance))
                    return instance;
            }
            else
            {
                if (Container.TryResolveNamed(key, service, out instance))
                    return instance;
            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override System.Collections.Generic.IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }
    }
}