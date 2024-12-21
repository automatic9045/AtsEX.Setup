using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Setup.ViewModels;

namespace BveEx.Setup
{
    internal static class PageViewModelConverter
    {
        private readonly static Dictionary<int, Lazy<IPageViewModel>> Dic = new Dictionary<int, Lazy<IPageViewModel>>()
        {
            { (int)Page.Preparing, new Lazy<IPageViewModel>(() => new PreparingPageViewModel()) },
            { (int)Page.Aborted, new Lazy<IPageViewModel>(() => new AbortedPageViewModel()) },
            { (int)Page.Welcome, new Lazy<IPageViewModel>(() => new WelcomePageViewModel()) },
            { (int)Page.NotLatestVersion, new Lazy<IPageViewModel>(() => new NotLatestVersionPageViewModel()) },
            { (int)Page.SelectBve6, new Lazy<IPageViewModel>(() => new SelectBve6PageViewModel()) },
            { (int)Page.SelectBve5, new Lazy<IPageViewModel>(() => new SelectBve5PageViewModel()) },
            { (int)Page.SelectScenarioDirectory, new Lazy<IPageViewModel>(() => new SelectScenarioDirectoryPageViewModel()) },
            { (int)Page.Sdk, new Lazy<IPageViewModel>(() => new SdkPageViewModel()) },
            { (int)Page.Confirm, new Lazy<IPageViewModel>(() => new ConfirmPageViewModel()) },
            { (int)Page.Installing, new Lazy<IPageViewModel>(() => new InstallingPageViewModel())  },
            { (int)Page.RequiresElevation, new Lazy<IPageViewModel>(() => new RequiresElevationPageViewModel())  },
            { (int)Page.Completed, new Lazy<IPageViewModel>(() => new CompletedPageViewModel())  },
        };

        public static IPageViewModel Convert(this Page page) => Dic[(int)page].Value;
    }
}
