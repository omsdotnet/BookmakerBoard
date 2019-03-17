///////////////////////////////////////////////////////////////////////////////
// AddIn
///////////////////////////////////////////////////////////////////////////////
#addin nuget:?package=Cake.Sonar&version=1.1.18
#addin nuget:?package=Cake.ArgumentHelpers&version=0.3.0
#addin nuget:?package=Cake.Git&version=0.19.0
#addin nuget:?package=Cake.Incubator&version=3.1.0
#addin nuget:?package=Cake.FileHelpers&version=3.0.0

///////////////////////////////////////////////////////////////////////////////
// Load custom classes
///////////////////////////////////////////////////////////////////////////////
#load "./BuildContext.cake"

///////////////////////////////////////////////////////////////////////////////
// Arguments Or Environment
///////////////////////////////////////////////////////////////////////////////
var target = ArgumentOrEnvironmentVariable("Target", string.Empty, "Default");

///////////////////////////////////////////////////////////////////////////////
// Variables
///////////////////////////////////////////////////////////////////////////////
BuildContext buildContext = null;
SonarBeginSettings sonarBeginSettings = null;
DotNetCoreBuildSettings coreBuildSettings = null;
DotNetCoreTestSettings  coreTestsSettings = null;
DotCoverAnalyseSettings dotCoverAnalyseSettings = null;
SonarEndSettings sonarEndSettings = null;
ReportGeneratorSettings reportGeneratorSettings = null;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup(context =>
{   
  buildContext = BuildContext.Create(context);
  
  if(!context.IsDryRun())
  { 
    Information($"Initialize { nameof(DotNetCoreBuildSettings) }");
    coreBuildSettings= new DotNetCoreBuildSettings()
    {
      Configuration = buildContext.Configuration,
      OutputDirectory = buildContext.PathsManager.BuildOutput
    };

    Information("Initialize NUnit3Settings");
    coreTestsSettings = new DotNetCoreTestSettings () 
    {
      Configuration = buildContext.Configuration,
      Filter = "TestCategory=Unit",
      NoBuild = true,
      OutputDirectory = buildContext.PathsManager.BuildOutput
    };

    Information("Initialize DotCoverAnalyseSettings");
    dotCoverAnalyseSettings = new DotCoverAnalyseSettings()
    {
      ReportType = DotCoverReportType.DetailedXML,
      TargetWorkingDir = buildContext.PathsManager.BuildOutput
    };
                        
    buildContext.PathsManager.Artifacts.Select(artifact => $"{artifact.Name}")
                                        .ToList()
                                        .ForEach(
                                          assemblyName => dotCoverAnalyseSettings.WithFilter($"+:module={assemblyName}"));

    dotCoverAnalyseSettings.WithFilter($"-:module=*AssemblyInfo")
                          .WithFilter($"-:module=*Test")
                          .WithFilter($"-:module=*Tests")
                          .WithFilter($"-:module=*Program")
                          .WithFilter($"-:module=*Console");
    
    Information("Initialize ReportGeneratorSettings");
    reportGeneratorSettings = new ReportGeneratorSettings()
    {
      ArgumentCustomization = args => args.Append("-reporttypes:HTML;SonarQube")
    };

    Information("Initialize SonarBeginSettings");
    sonarBeginSettings = new SonarBeginSettings() 
    {
      Url = buildContext.SonarUrl,
      Login = buildContext.SonarLogin,
      Password = buildContext.SonarPassword, 
      Key = buildContext.SonarProjectKey,
      Name = buildContext.SonarProjectName,
      NUnitReportsPath = buildContext.PathsManager.Reports.Combine(Directory("./TestsResultsReport.xml")).FullPath,
      Version = buildContext.VersionManager.FullVersion,
      UseCoreClr = true,
      ArgumentCustomization = args => 
        args.Append($"/d:sonar.branch.name={buildContext.Branch}")
            .Append(buildContext.IsMasterBranch ? string.Empty : $"/d:sonar.branch.target={buildContext.TargetBranch}")
            .Append($"/d:sonar.organization={buildContext.SonarOrganization}")
            .Append($"/d:sonar.coverageReportPaths=\"{buildContext.PathsManager.Reports}/SonarQube.xml\"")  
    };

    Information("Initialize SonarEndSettings");
    sonarEndSettings = sonarBeginSettings.GetEndSettings();
  }
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Build")
  .Does(() =>
{
  DotNetCoreBuild(buildContext.PathsManager.Solution.FullPath, coreBuildSettings);
});

Task("Run-Tests")
  .IsDependentOn("Build")
  .Does(() =>
{
  if(buildContext.PathsManager.Tests.Any())
  {
    DotCoverAnalyse(tool => 
    {
      var projectFiles = buildContext.PathsManager.Tests.Select(test => test.SourcePath);
      foreach(var file in projectFiles)
      {
        tool.DotNetCoreTest(
          file.FullPath, 
          coreTestsSettings);
      }
    }, 
    buildContext.PathsManager.Reports.Combine("./CoverReport.xml").FullPath, 
    dotCoverAnalyseSettings.WithFilter("+:BookmakerBoard")
                         .WithFilter("-:BookTracker.Tests"));

    ReportGenerator(
      $"{buildContext.PathsManager.Reports.FullPath}/CoverReport.xml", 
      $"{buildContext.PathsManager.Reports}", 
      reportGeneratorSettings);
  }
});
Task("InitializeSonar")
  .Does(() => 
{
  SonarBegin(sonarBeginSettings);
});

Task("Analysis")
  .IsDependentOn("InitializeSonar")
  .IsDependentOn("Run-Tests")
  .Does(() =>
{
    SonarEnd(sonarEndSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////
Task("Default")
  .IsDependentOn("Analysis");

RunTarget(target);