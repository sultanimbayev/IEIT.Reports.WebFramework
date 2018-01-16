using IEIT.Reports.WebFramework.Api.Resolvers;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(TestProj.ReportsHandlerConfig), "PreStart")]

namespace TestProj {
    public static class ReportsHandlerConfig {
        public static void PreStart() {
            RepositoryResolver.InitRepositories();
        }
    }
}