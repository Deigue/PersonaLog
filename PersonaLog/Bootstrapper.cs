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
            DisplayRootViewFor<IShell>();
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

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key)
                               ? AttributedModelServices.GetContractName(serviceType)
                               : key;
            var exports = _container.GetExportedValues<object>(contract).ToList();

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }


        
    }
}
