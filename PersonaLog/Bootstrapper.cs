using System.Windows;
using Caliburn.Micro;
using PersonaLog.ViewModels;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Metro.Dialogs;

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
            //Define the catalog ...
            var catalog = new AggregateCatalog(
                AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>());

            _container = new CompositionContainer(catalog); 

            var batch = new CompositionBatch();

            var windowManager = new WindowManager();
            batch.AddExportedValue<IWindowManager>(windowManager);
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            //Added export for the customized dialogs so we can easily import at runtime.
            batch.AddExportedValue<IWindowsDialogs>(new WindowsDialogs(windowManager));
            batch.AddExportedValue(_container);

            _container.Compose(batch);

        }

        /*  Used to get the customized Dialogs assembly via subsequent call.
         *  Update [2019/03/18] We cannot use this, because Metro.Dialogs has an
         *  assembly dependency to an older version of MahApps.Metro.dll [1.2.4.0].
         *  We are better of instantiating this ourselves and dealing with the problems
         *  as they come ...
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            yield return typeof(MEFBootstrapper).Assembly;
            yield return typeof(IWindowsDialogs).Assembly;
            
        }
        */

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
