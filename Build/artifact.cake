public class Artifact
{
  public string Name { get; }
  public DirectoryPath OutputPath { get; }
  public FilePath SourcePath { get; }
  public string OutputType { get; }
  public string Version { get; }
  public IReadOnlyCollection<(string Name, string Version)> Dependencies { get; }

  public Artifact(string name, DirectoryPath outputPath, FilePath sourcePath, string outputType, string version,
   IEnumerable<(string Name, string Version)> dependencies)
  {    
    Name = name.ThrowIfNullOrEmpty(nameof(name));
    OutputPath = outputPath.ThrowIfNull(nameof(outputPath));
    SourcePath = sourcePath.ThrowIfNull(nameof(sourcePath));
    OutputType = outputType.ThrowIfNullOrEmpty(nameof(outputType));
    Version = version.ThrowIfNullOrEmpty(nameof(version));
    Dependencies = dependencies.ThrowIfNull(nameof(dependencies)).ToList().AsReadOnly();
  }

  public override string ToString()
  {
    return $"The artifact {Name}.{OutputType} from {SourcePath}";
  }
}