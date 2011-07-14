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
  xunit.assembly = "src/SampleWebsite.Mvc3.Tests/bin/debug/SampleWebsite.Mvc3.Tests.dll"
end

task :test => [:test_library, :test_sampleWebsite] do
end

msbuild :release => :test do |msb|
  msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Release
  msb.targets :Clean, :Rebuild
  msb.solution = "src/MvcMembership.sln"
end

########## PACKAGING ##########

task :prepare_package_mvcmembership => :release do
  require 'fileutils'
  build_directory = './src/MvcMembership/bin/Release/'
  output_directory = './packages/MvcMembership/lib/40/'
  FileUtils.mkdir_p output_directory
  FileUtils.cp build_directory + 'MvcMembership.dll', output_directory + 'MvcMembership.dll'
  FileUtils.cp build_directory + 'MvcMembership.pdb', output_directory + 'MvcMembership.pdb'
  FileUtils.cp build_directory + 'MvcMembership.xml', output_directory + 'MvcMembership.xml'
end

task :prepare_package_mvcmembershipmvc => :release do
  require 'fileutils'

  root = './src/SampleWebsite.Mvc3/'
  appstart_directory = root + 'App_Start/'
  content_directory = root + 'Content/'
  area_directory = root + 'Areas/MvcMembership/'
  controllers_directory = area_directory + 'Controllers/'
  models_directory = area_directory + 'Models/UserAdministration/'
  views_directory = area_directory + 'Views/'
  views_useradministration_directory = views_directory + 'UserAdministration'

  output_root = './packages/MvcMembership.Mvc/content/'
  output_appstart_directory = output_root + 'App_Start/'
  output_content_directory = output_root + 'Content/'
  output_area_directory = output_root + 'Areas/MvcMembership'
  output_controllers_directory = output_area_directory + 'Controllers/'
  output_models_directory = output_area_directory + 'Models/UserAdministration/'
  output_views_directory = output_area_directory + 'Views/'
  output_views_useradministration_directory = output_views_directory + 'UserAdministration/'

  FileUtils.mkdir_p output_appstart_directory
  FileUtils.mkdir_p output_content_directory
  FileUtils.mkdir_p output_area_directory
  FileUtils.mkdir_p output_controllers_directory
  FileUtils.mkdir_p output_models_directory
  FileUtils.mkdir_p output_views_directory
  FileUtils.mkdir_p output_views_useradministration_directory

  FileUtils.cp appstart_directory + 'MvcMembership.cs', output_appstart_directory + 'MvcMembership.cs'
  FileUtils.cp content_directory + 'MvcMembership.css', output_content_directory + 'MvcMembership.css'
  FileUtils.cp area_directory + 'MvcMembershipAreaRegistration.cs', output_area_directory + 'MvcMembershipAreaRegistration.cs'
  FileUtils.cp controllers_directory + 'UserAdministrationController.cs', output_controllers_directory + 'UserAdministrationController.cs'
  FileUtils.cp models_directory + 'CreateUserViewModel.cs', output_models_directory + 'CreateUserViewModel.cs'
  FileUtils.cp models_directory + 'DetailsViewModel.cs', output_models_directory + 'DetailsViewModel.cs'
  FileUtils.cp models_directory + 'IndexViewModel.cs', output_models_directory + 'IndexViewModel.cs'
  FileUtils.cp models_directory + 'RoleViewModel.cs', output_models_directory + 'RoleViewModel.cs'
  FileUtils.cp views_directory + 'Web.config', output_views_directory + 'Web.config'
  FileUtils.cp views_useradministration_directory + 'CreateUser.cshtml', output_views_useradministration_directory + 'CreateUser.cshtml'
  FileUtils.cp views_useradministration_directory + 'Details.cshtml', output_views_useradministration_directory + 'Details.cshtml'
  FileUtils.cp views_useradministration_directory + 'Index.cshtml', output_views_useradministration_directory + 'Index.cshtml'
  FileUtils.cp views_useradministration_directory + 'Password.cshtml', output_views_useradministration_directory + 'Password.cshtml'
  FileUtils.cp views_useradministration_directory + 'Role.cshtml', output_views_useradministration_directory + 'Role.cshtml'
  FileUtils.cp views_useradministration_directory + 'UsersRoles.cshtml', output_views_useradministration_directory + 'UsersRoles.cshtml'
end

exec :package_mvcmembership => :prepare_package_mvcmembership do |cmd|
	cmd.path_to_command = 'nuget'
	cmd.parameters [
		'pack',
		'./packages/MvcMembership/MvcMembership.nuspec',
		'-OutputDirectory',
		'./packages/'
	]
end

exec :package_mvcmembershipmvc => :prepare_package_mvcmembershipmvc do |cmd|
	cmd.path_to_command = 'nuget'
	cmd.parameters [
		'pack',
		'./packages/MvcMembership.Mvc/MvcMembership.Mvc.nuspec',
		'-OutputDirectory',
		'./packages/'
	]
end

task :package => [:package_mvcmembership, :package_mvcmembershipmvc] do
end