#tool "Google.Protobuf.Tools"


var target = Argument("target", "Default");
var configuration = "Release";
var publishDir = MakeAbsolute(Directory("./publish")).FullPath;


Task("Default")    
	.IsDependentOn("Proto")
	.IsDependentOn("Clean")
    .IsDependentOn("Restore")    
	.IsDependentOn("Build")
	.IsDependentOn("Test");
	

Task("Appveyor")    
	.IsDependentOn("Default")    
	.IsDependentOn("Publish");
    

Task("Proto")
    .Does(() =>
    {
        var executable = GetFiles("./tools/**/windows_x64/protoc.exe").First().FullPath;
        var protoTools = System.IO.Directory.GetParent(executable).Parent.FullName;

        foreach(var file in GetFiles("proto/**/*.proto"))
        {            
            var protoPaths = new List<string>();
            protoPaths.Add(protoTools);
            protoPaths.Add(System.IO.Path.GetDirectoryName(file.FullPath));
            
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
      .Union(GetDirectories(publishDir));
      
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
		Func<IFileSystemInfo, bool> exclude_legacy =
			fileSystemInfo => !fileSystemInfo.Path.FullPath.EndsWith(
				".Legacy", StringComparison.OrdinalIgnoreCase);


		var files = GetFiles("./**/*.csproj", exclude_legacy);
		foreach (var file in files)
		{
			Information(file);
			DotNetCoreBuild(file.FullPath, new DotNetCoreBuildSettings 
			{
				Configuration = configuration			
			});
		}

		var msBuildSettings = new MSBuildSettings {
    		Verbosity = Verbosity.Minimal,
    		ToolVersion = MSBuildToolVersion.VS2017,
    		Configuration = configuration
    	};

		MSBuild("./src/Vodamep.Legacy/Vodamep.Legacy.csproj",msBuildSettings);
		
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

Task("Publish")	
	.Does(() =>
	{	

		EnsureDirectoryExists(publishDir);
		
		var msBuildSettings = new MSBuildSettings {
    		Verbosity = Verbosity.Minimal,
    		ToolVersion = MSBuildToolVersion.VS2017,
    		Configuration = configuration
    	};

		MSBuild("./src/Vodamep.Legacy/Vodamep.Legacy.csproj",msBuildSettings.WithProperty("OutDir", publishDir + "/dml"));
	
		Zip(publishDir, publishDir + "/dml.zip", publishDir + "/dml/dml.exe");

		var ms = new DotNetCoreMSBuildSettings();			

		var settings = new DotNetCorePublishSettings
		{         
			Configuration = "Release",
			MSBuildSettings = ms,
			OutputDirectory = publishDir + "/dmc",
			Runtime = "win-x64"
		};

		DotNetCorePublish("./src/Vodamep.Client/Vodamep.Client.csproj", settings); 
		
		if (bool.Parse(EnvironmentVariable("vodamepnative") ?? "false"))
		{
			MoveFile(publishDir + "/dmc/Vodamep.Client.exe", publishDir + "/dmc/dmc.exe");

			Zip(publishDir, publishDir + "/dmc.zip", publishDir + "/dmc/dmc.exe");
		}

		
	});



RunTarget(target);
