#load "./artifact.cake"

public class PathsManager
{
  public const string DefaultCredentialStore = "CredentialStore";
  public const string DefaultWorking = "./../";
  public const string DefaultBuildOutputPath = "Bin";
  public const string DefaultSourcePath = "Src";

  private readonly ICakeContext context;
  private readonly BuildContext buildContext;

  public DirectoryPath Working { get; }  
  public DirectoryPath Reports { get; }
  public DirectoryPath BuildOutput { get; }
  public DirectoryPath ArtifactsOutput { get; }
  public DirectoryPath Packages { get; }
  public FilePath Solution { get; }
  public DirectoryPath Source { get; }
  public IReadOnlyCollection<Artifact> Artifacts { get; set; }
  public IReadOnlyCollection<(string Name, FilePath OutputPath, FilePath SourcePath)> Tests { get; set; }

  private void GetArtifacts()
  {
    context.Information($"Getting artifacts.");
    context.Information($"Parse solution.");
    var projects = context.ParseSolution(Solution).GetProjects();
    
    if(projects.Any())
    {
      context.Information($"Parse projects.");
      Artifacts = 
      (
        from project in projects
        where !project.Name.Contains("Test")
        let projectInfo = context.ParseProject(project.Path, buildContext.Configuration)
        let name = projectInfo.AssemblyName
        let sourcePath = project.Path
        let outputPath = new DirectoryPath($"{project.Path.GetDirectory()}/{projectInfo.OutputPath}")
        let outputType = projectInfo.OutputType.EqualsIgnoreCase("Exe") ? ".exe" : ".dll"
        let version = buildContext.VersionManager.FullVersion
        let dependencies = projectInfo.References.Where(@ref => @ref.HintPath is object).Select(@ref =>
        {
            var dependencyName = @ref.Include.Substring(0, @ref.Include.IndexOf(','));
            var dependencyVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(
              sourcePath.GetDirectory().CombineWithFilePath(@ref.HintPath).FullPath).FileVersion;

            return (dependencyName, dependencyVersion);

        }).Union(projectInfo.ProjectReferences.Select(@ref => (@ref.Name, version)))
        select new Artifact(name, outputPath, sourcePath, outputType, version, dependencies)

      ).ToList().AsReadOnly();

      context.Information($"Found {Artifacts.Count()} artifacts:{Environment.NewLine}{string.Join(Environment.NewLine, Artifacts)}.");
    }
    else
    {
       throw new Exception($"Artifacts in solution {Solution} not found.");
    }
  }

  private void GetTests()
  {
    context.Information($"Getting artifacts.");
    context.Information($"Parse solution.");
    var projects = context.ParseSolution(Solution).GetProjects();
    
    if(projects.Any(project => project.Name.Contains("Tests")))
    {
      context.Information($"Parse projects.");
      Tests = 
      (
        from project in projects
        where project.Name.Contains("Tests")
        let projectInfo = context.ParseProject(project.Path, buildContext.Configuration)
        let name = projectInfo.AssemblyName
        let sourcePath = project.Path
        let outputPath = new DirectoryPath($"{project.Path.GetDirectory()}/{projectInfo.OutputPath}")
        select ($"{name}.dll", new FilePath($"{outputPath}/{name}.dll"), sourcePath)
      ).ToList().AsReadOnly();

      context.Information($"Found {Tests.Count()} tests:{Environment.NewLine}{string.Join(Environment.NewLine, Tests)}.");
    }
    else
    {
      context.Information($"Tests in solution {Solution} not found.");
    }
  }

  public PathsManager(ICakeContext context, BuildContext buildContext)
  {
    context.Information($"{nameof(PathsManager)} initialization.");
    this.context = context.ThrowIfNull(nameof(context));
    this.buildContext = buildContext.ThrowIfNull(nameof(buildContext));

    var working = context.ArgumentOrEnvironmentVariable("WorkingDir", string.Empty, DefaultWorking).ThrowIfNullOrEmpty("WorkingDir");
    var buildOutputPath = context.ArgumentOrEnvironmentVariable(nameof(BuildOutput), string.Empty, DefaultBuildOutputPath);
    var solution = context.ArgumentOrEnvironmentVariable(nameof(Solution), string.Empty).ThrowIfNullOrEmpty(nameof(Solution));

    context.Information($"Search working directory: {working}.");
    Working = new DirectoryPath(working);

    context.EnsureDirectoryExists($"{Working}/Artifacts");
    context.Information($"Search artifacts output directory: {Working}/Artifacts.");
    ArtifactsOutput = context.GetDirectories($"{Working}/**/Artifacts").First();

    if(!context.IsDryRun())
    {
      context.EnsureDirectoryExists($"{Working}/Reports");
      context.Information($"Search reports directory: {Working}/Reports.");
      Reports = context.GetDirectories($"{Working}/**/Reports").First();
      
      context.Information($"Search build output directory: {Working}/**/{buildOutputPath}.");
      BuildOutput = context.GetDirectories($"{Working}/**/{buildOutputPath}").First();

      context.Information($"Search solution directory: {Working}/**/{solution}.sln.");
      Solution = context.GetFiles($"{Working}/**/{solution}.sln").First();
      
      context.Information($"Search source directory.");
      Source = Solution.GetDirectory();
      GetArtifacts();
      GetTests();
    }

    context.Information($"PathsManager initialized.");
  }

  public static PathsManager Create(ICakeContext context, BuildContext buildContext)
  {
    context.Information($"{nameof(PathsManager)} creation.");

    var instance = new PathsManager(context, buildContext);

    context.Information($"{nameof(BuildContext)} created.");
    return instance;
  }
}