using IEIT.Reports.WebFramework.Api.Resolvers;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.ReportsHandlerConfig), "PreStart")]

namespace $rootnamespace$ {
    public static class ReportsHandlerConfig {
        public static void PreStart() {
            RepositoryResolver.InitRepositories();
        }
    }
}