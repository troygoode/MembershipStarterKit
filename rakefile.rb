require 'albacore'

task :default => [:build]

msbuild :build do |msb|
  msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.solution = "src/MvcMembership.sln"
end

xunit :test_library => :build do |xunit|
  xunit.path_to_command = "src/packages-manual/xunit-1.8/xunit.console.clr4.exe"
  xunit.assembly = "src/MvcMembership.Tests/bin/debug/MvcMembership.Tests.dll"
end

xunit :test_sampleWebsite => :build do |xunit|
  xunit.path_to_command = "src/packages-manual/xunit-1.8/xunit.console.clr4.exe"
  xunit.assembly = "src/SampleWebsite.Tests/bin/debug/SampleWebsite.Tests.dll"
end

task :test => [:test_library, :test_sampleWebsite] do
end

msbuild :release => :test do |msb|
  msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Release
  msb.targets :Clean, :Rebuild
  msb.solution = "src/MvcMembership.sln"
end