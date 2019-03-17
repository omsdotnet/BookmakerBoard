#load "./versionManager.cake"
#load "./pathsManager.cake"

public class BuildContext
{
  public const string DefaultBranch = "master";
  public const string DefaultConfiguration = "Release";
  public const string DefaultNugetRepositoryUrl = "";
  public const string DefaultSonarUrl = "https://sonarcloud.io";
  public const string DefaultSonarOrganization = "omsdotnet";
  public const string DefaultSonarProjectKey = "omsdotnet.bookmaker";
  public const string DefaultSonarProjectName = "BookmakerBoard";

  public string Target { get; }
  public string Configuration { get; }
  public string NugetRepositoryUrl { get; }
  public string SonarUrl { get; }
  public string SonarLogin { get; }
  public string SonarPassword { get; }
  public string SonarProjectKey { get; }
  public string SonarProjectName { get; }
  public string SonarOrganization { get; }
  public string Branch { get; }
  public string TargetBranch { get; }
  public bool IsLocalBuild { get; }
  public bool IsMasterBranch { get; }
  public VersionManager VersionManager { get; }
  public PathsManager PathsManager { get; }

  public BuildContext(ICakeContext context)
  {
    context.Information($"{nameof(BuildContext)} initialization.");

    var buildSystem = context.BuildSystem();
    var working = context.ArgumentOrEnvironmentVariable("WorkingDir", string.Empty).ThrowIfNullOrEmpty("WorkingDir");

    VersionManager = VersionManager.Create(context, working);

    Configuration = context.ArgumentOrEnvironmentVariable(nameof(Configuration), string.Empty, DefaultConfiguration);

    PathsManager = PathsManager.Create(context, this);

    Branch = context.GitBranchCurrent(PathsManager.Working).FriendlyName ?? DefaultBranch;
    TargetBranch = context.ArgumentOrEnvironmentVariable(nameof(TargetBranch), string.Empty, DefaultBranch);
    NugetRepositoryUrl = context.ArgumentOrEnvironmentVariable(nameof(NugetRepositoryUrl), string.Empty, DefaultNugetRepositoryUrl);
    SonarUrl = context.ArgumentOrEnvironmentVariable(nameof(SonarUrl), string.Empty, DefaultSonarUrl);
    SonarLogin = context.ArgumentOrEnvironmentVariable(nameof(SonarLogin), string.Empty);
    SonarPassword = context.ArgumentOrEnvironmentVariable(nameof(SonarPassword), string.Empty);
    SonarOrganization = context.ArgumentOrEnvironmentVariable(nameof(SonarOrganization), string.Empty, DefaultSonarOrganization);
    SonarProjectKey = context.ArgumentOrEnvironmentVariable(nameof(SonarProjectKey), string.Empty, DefaultSonarProjectKey);
    SonarProjectName = context.ArgumentOrEnvironmentVariable(nameof(SonarProjectName), string.Empty, DefaultSonarProjectName);
    IsLocalBuild = buildSystem.IsLocalBuild;
    IsMasterBranch = DefaultBranch.EqualsIgnoreCase(Branch);
  }

  public static BuildContext Create(ICakeContext context)
  {
    context.Information($"{nameof(BuildContext)} creation.");
    var instance = new BuildContext(context);

    context.Information($"{nameof(BuildContext)} created.");
    return instance;
  }
}