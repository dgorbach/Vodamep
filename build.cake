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
	.IsDependentOn("PublishLegacy")
	.IsDependentOn("PublishClient")
	.IsDependentOn("PublishApi");
    

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

Task("PublishLegacy")	
	.Does(() =>
	{
		EnsureDirectoryExists(publishDir);
		CleanDirectory(publishDir + "/dml");
		if (FileExists(publishDir + "dml.zip"))
		{
			DeleteFile(publishDir + "dml.zip");
		}

		var msBuildSettings = new MSBuildSettings {
    		Verbosity = Verbosity.Minimal,
    		ToolVersion = MSBuildToolVersion.VS2017,
    		Configuration = configuration
    	};

		MSBuild("./src/Vodamep.Legacy/Vodamep.Legacy.csproj",msBuildSettings.WithProperty("OutDir", publishDir + "/dml"));
	
		Zip(publishDir + "/dml", publishDir + "/dml.zip", publishDir + "/dml/dml.exe");
	});

Task("PublishClient")	
	.Does(() =>
	{	

		EnsureDirectoryExists(publishDir);
		CleanDirectory(publishDir + "/dmc");
		if (FileExists(publishDir + "/dmc.zip"))
		{
			DeleteFile(publishDir + "/dmc.zip");
		}

		var ms = new DotNetCoreMSBuildSettings();			

		var settings = new DotNetCorePublishSettings
		{         
			Configuration = "Release",			
			OutputDirectory = publishDir + "/dmc",
			MSBuildSettings = ms,
			Runtime = "win-x64"
		};	
		
		settings.MSBuildSettings = settings.MSBuildSettings.WithProperty("CoreRT", "True");

		// compression wird derzeit noch nicht unterstützt: workaround
		// https://github.com/dotnet/corert/issues/5496
		settings.MSBuildSettings = settings.MSBuildSettings.WithProperty("NativeCompilationDuringPublish", "False");		

		DotNetCorePublish("./src/Vodamep.Client/Vodamep.Client.csproj", settings); 

		settings.MSBuildSettings = settings.MSBuildSettings.WithProperty("NativeCompilationDuringPublish", "True");		

		DotNetCorePublish("./src/Vodamep.Client/Vodamep.Client.csproj", settings); 
		
		var files = new [] {
			publishDir + "/dmc/dmc.exe",
			publishDir + "/dmc/clrcompression.dll"
		};

		Zip(publishDir + "/dmc", publishDir + "/dmc.zip", files);
	});

Task("PublishApi")	
	.Does(() =>
	{	

		EnsureDirectoryExists(publishDir);
		CleanDirectory(publishDir + "/dms");
		if (FileExists(publishDir + "/dms.zip"))
		{
			DeleteFile(publishDir + "/dms.zip");
		}

		var ms = new DotNetCoreMSBuildSettings();			

		var settings = new DotNetCorePublishSettings
		{         
			Configuration = "Release",			
			OutputDirectory = publishDir + "/dms",
			MSBuildSettings = ms,
			Runtime = "win-x64"
		};	
		
		settings.MSBuildSettings = settings.MSBuildSettings.WithProperty("CoreRT", "True");

		// compression wird derzeit noch nicht unterstützt: workaround
		// https://github.com/dotnet/corert/issues/5496
		settings.MSBuildSettings = settings.MSBuildSettings.WithProperty("NativeCompilationDuringPublish", "False");		

		DotNetCorePublish("./src/Vodamep.Api/Vodamep.Api.csproj", settings); 

		settings.MSBuildSettings = settings.MSBuildSettings.WithProperty("NativeCompilationDuringPublish", "True");		

		DotNetCorePublish("./src/Vodamep.Api/Vodamep.Api.csproj", settings); 
		
		var files = new [] {
			publishDir + "/dms/dms.exe",
			publishDir + "/dms/libuv.dll",
			publishDir + "/dms/clrcompression.dll",
			publishDir + "/dms/sni.dll",
			publishDir + "/dms/web.config",
			publishDir + "/dms/appsettings.json"
		};

		Zip(publishDir + "/dms", publishDir + "/dms.zip", files);
	});

RunTarget(target);


