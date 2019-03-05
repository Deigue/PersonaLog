using System.Windows;
using Caliburn.Micro;
using PersonaLog.ViewModels;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonaLog
{
    class MEFBootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;

        public MEFBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void Configure()
        {
            _container = new CompositionContainer(new AggregateCatalog
                                                    (AssemblySource.Instance
                                                        .Select(x => new AssemblyCatalog(x))
                                                        .OfType<ComposablePartCatalog>())); 

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);

            _container.Compose(batch);
        }

        protected override object GetInstance(Type service, string key)
        {
            return base.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return base.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            base.BuildUp(instance);
        }


        
    }
}
