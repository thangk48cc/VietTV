using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using VietTV.Services;

namespace VietTV.ViewModel
{
    public class ViewModelLocator
    {
        /// <summary>
        /// This class contains static references to all the view models in the
        /// application and provides an entry point for the bindings.
        /// <para>
        /// See http://www.galasoft.ch/mvvm
        /// </para>
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<NavigationHelper>(() => new NavigationHelper(App.RootFrame));

        }
        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MenuMainVM MenuMain
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MenuMainVM>();
            }
        }
    }
}
