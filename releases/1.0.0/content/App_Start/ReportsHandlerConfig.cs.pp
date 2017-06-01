using IEIT.Reports.Export.WebFramework.Resolvers;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.ReportsHandlerConfig), "PreStart")]

namespace $rootnamespace$.App_Start {
    public static class ReportsHandlerConfig {
        public static void PreStart() {
            RepositoryResolver.InitRepositories();
        }
    }
}