#tool "Google.Protobuf.Tools"


var target = Argument("target", "Default");
var configuration = "Release";
var buildDir = "./build";


Task("Default")    
	.IsDependentOn("Proto")
    .IsDependentOn("Restore")    
	.IsDependentOn("Build")
	.IsDependentOn("Test");
    

Task("Proto")
    .Does(() =>
    {
        var executable = GetFiles("./tools/**/windows_x64/protoc.exe").First().FullPath;
        var protoTools = System.IO.Directory.GetParent(executable).Parent.FullName;

        var importProtos = GetFiles("proto/**/Common.proto")
            .Union(GetFiles("proto/**/ProtoActor.proto"))
            .Select(x => System.IO.Path.GetDirectoryName(x.FullPath))
            .ToArray();

        foreach(var file in GetFiles("proto/**/*.proto"))
        {            
            var protoPaths = new List<string>();
            protoPaths.Add(protoTools);
            protoPaths.Add(System.IO.Path.GetDirectoryName(file.FullPath));
            protoPaths.AddRange(importProtos);            

            var argsProtoPath = string.Join(" ", protoPaths.Distinct().Select(x => string.Format("--proto_path={0}", x)));
            var argsOut = string.Format("--csharp_out={0}", System.IO.Path.GetDirectoryName(file.FullPath));
            var argsOpt = " --csharp_opt=file_extension=.g.cs ";

            StartProcess(executable, new ProcessSettings {
                Arguments = new ProcessArgumentBuilder()
                    .Append(argsProtoPath)
                    .Append(argsOut)
                    .Append(argsOpt)
                    .Append(file.FullPath)
                });
        }
    });


Task("Clean")
  .Does(()=>{   
    var directoriesToClean = GetDirectories("./**/bin/Debug")
      .Union(GetDirectories("./**/bin/Release"))     
      .Union(GetDirectories(buildDir));
      
    CleanDirectories(directoriesToClean);
  });

Task("Restore")
    .Does(() => 
    {
        DotNetCoreRestore();
    });

Task("Build")
    .Does(() => 
    {
        DotNetCoreBuild("Vodamep.sln", new DotNetCoreBuildSettings 
        {
            Configuration = configuration			
        });
    });


Task("Test")	
    .Does(() => 
    {	
		var settings = new DotNetCoreTestSettings
		{
			Configuration = "Release",
			NoBuild = true
		};

        foreach(var file in GetFiles("./tests/**/*.csproj")) 
		{
			Information("{0}", file);
			DotNetCoreTest(file.FullPath, settings);
		}  
        
    });


RunTarget(target);
