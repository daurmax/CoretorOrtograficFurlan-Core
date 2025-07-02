using CoretorOrtografic.Infrastructure.ContentReader;
using CoretorOrtografic.Infrastructure.KeyValueDatabase;
using CoretorOrtografic.Infrastructure.SpellChecker;
using CoretorOrtografic.Core.Input;
using CoretorOrtografic.Core.KeyValueDatabase;
using CoretorOrtografic.Core.SpellChecker;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CoretorOrtografic.Business
{
    public class CoretorOrtograficDependencyModule : Module
    {
        private readonly bool _isDevelopment = false;
        private CallerApplicationEnum _callerApplication;

        public CoretorOrtograficDependencyModule(bool isDevelopment, CallerApplicationEnum callerApplication)
        {
            _isDevelopment = isDevelopment;
            _callerApplication = callerApplication;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Register generic ILogger<T> for DI
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
            builder.RegisterInstance(NullLoggerFactory.Instance.CreateLogger<FurlanSpellChecker>())
                .As<ILogger<FurlanSpellChecker>>()
                .SingleInstance();

            if (_isDevelopment)
            {
                RegisterDevelopmentOnlyDependencies(builder);
            }
            else
            {
                RegisterProductionOnlyDependencies(builder);
            }

            switch (_callerApplication)
            {
                case CallerApplicationEnum.CLI:
                    RegisterCLIDependencies(builder);
                    break;
                case CallerApplicationEnum.Web:
                    RegisterWebDependencies(builder);
                    break;
            }
        }

        private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // Add development only services
            builder.RegisterType<FurlanSpellChecker>().As<ISpellChecker>();
            builder.RegisterType<SQLiteKeyValueDatabase>().As<IKeyValueDatabase>();
        }
        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // Add production only services
            builder.RegisterType<FurlanSpellChecker>().As<ISpellChecker>();
            builder.RegisterType<SQLiteKeyValueDatabase>().As<IKeyValueDatabase>();
        }

        private void RegisterCLIDependencies(ContainerBuilder builder)
        {
            // Add CLI only services
            builder.RegisterType<ConsoleContentReader>().As<IContentReader>();
        }
        private void RegisterWebDependencies(ContainerBuilder builder)
        {
            // Add web only services
        }
    }
}
