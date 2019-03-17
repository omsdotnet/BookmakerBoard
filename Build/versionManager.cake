public class VersionManager
{
  public string FullVersion { get; private set; }
  public string SemVersion { get; private set; }
  
  public static VersionManager Create(ICakeContext context, string workingDirPath)
  {
    context.Information($"Create {nameof(VersionManager)}");
    
    var gitVersion = context.GitVersion(new GitVersionSettings 
    {
      UpdateAssemblyInfo = false,
      NoFetch = true,
      WorkingDirectory = workingDirPath
    });
      
    var version = string.Format("{0}.{1}.{2}.{3}", gitVersion.Major, gitVersion.Minor, gitVersion.Patch, gitVersion.CommitDate); 
    var semVersion = gitVersion.FullSemVer;
    
    context.Information("Full semantic version detected: " + semVersion);
    context.Information("Assembly version: " + version);

    context.Information($"Version initialized.");
    
    return new VersionManager
    {
      FullVersion = version,
      SemVersion = semVersion
    };
  }
}