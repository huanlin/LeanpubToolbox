using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FlubuCore.Context;
using FlubuCore.Context.Attributes.BuildProperties;
using FlubuCore.IO;
using FlubuCore.Scripting;

namespace Build
{
    public class BuildScript : DefaultBuildScript
    {
        [SolutionFileName]
        public string SolutionFileName => "LeanpubApiClient.sln";

        [BuildConfiguration]
        public string BuildConfiguration { get; set; } = "Release"; // Debug or Release

        protected override void ConfigureBuildProperties(IBuildPropertiesContext context)
        {
        }

        protected override void ConfigureTargets(ITaskContext context)
        {
            var compile = context.CreateTarget("compile")
                .SetDescription("Compile the solution.")
                .AddCoreTask(
                    x => x.Build());
                //.Do(CopyAdditionalFiles);
        }

        private void CopyAdditionalFiles(ITaskContext context)
        {
            const string ApiKeyFileName = ".apikey";

            //File.Copy(RootDirectory.CombineWith(ApiKeyFileName), OutputDir);
        }
    }
}
