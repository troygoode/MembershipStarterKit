require 'albacore'

task :default => [:build]

msbuild :build do |msb|
  msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.solution = "src/MvcMembership.sln"
end

# xunit :test_library => :build do |xunit|
#   xunit.path_to_command = "src/MvcMembership.Tests/bin/debug/xunit.console.exe"
#   xunit.assembly = "src/MvcMembership.Tests/bin/debug/MvcMembership.Tests.dll"
# end

# msbuild :release => :test_library do |msb|
#   msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
#   msb.properties :configuration => :Release
#   msb.targets :Clean, :Rebuild
#   msb.solution = "src/MvcMembership.sln"
# end