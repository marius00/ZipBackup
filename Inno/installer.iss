#define ApplicationVersion GetFileVersion('..\installer\ZipBackup.exe')
#define ProductVersion GetStringFileInfo('..\installer\ZipBackup.exe', 'ProductVersion')
#define FindFolder(Path) \
    Local[0] = FindFirst(Path, faDirectory), \
    Local[0] ? AddBackslash(ExtractFileDir(Path)) + FindGetFileName(Local[0]) : Path
	
	
[Setup]
AppVerName=ZipBackup
AppName=ZipBackup (c) EvilSoft
VersionInfoVersion={#ApplicationVersion}
AppId=zipbackup
DefaultDirName={code:DefDirRoot}\ZipBackup
Uninstallable=Yes
OutputDir=..\Installer
SetupIconFile=installer.ico


[Tasks]
Name: desktopicon; Description: "Create a &desktop icon"; GroupDescription: "Icons:"
Name: starticon; Description: "Create a &startmenu icon"; GroupDescription: "Icons:"


[Icons]
Name: "{commonprograms}\ZipBackup"; Filename: "{app}\\ZipBackup.exe"; Tasks: starticon
Name: "{commondesktop}\ZipBackup"; Filename: "{app}\\ZipBackup.exe"; Tasks: desktopicon


[Files]
Source: "{#FindFolder("..\ZipBackup\bin\Release\net*")}\*.*"; Excludes: "*.pdb"; DestDir: "{app}"; Flags: overwritereadonly replacesameversion recursesubdirs createallsubdirs touch ignoreversion
Source: "readme.txt"; DestDir: "{app}";

[Run]
Filename: "{app}\ZipBackup.exe"; Description: "Launch ZipBackup"; Flags: postinstall nowait




[Setup]
UseSetupLdr=yes
DisableProgramGroupPage=yes
DiskSpanning=no
AppVersion={#ApplicationVersion}
VersionInfoProductTextVersion={#ApplicationVersion}
PrivilegesRequired=admin
DisableWelcomePage=Yes
ArchitecturesInstallIn64BitMode=x64
AlwaysShowDirOnReadyPage=Yes
DisableDirPage=No
OutputBaseFilename=ZipBackupInstaller
InfoAfterFile=readme.txt


[UninstallDelete]
Type: filesandordirs; Name: {app}

[Languages]
Name: eng; MessagesFile: compiler:Default.isl

[Code]
function IsRegularUser(): Boolean;
begin
Result := not (IsAdminLoggedOn or IsPowerUserLoggedOn);
end;

function DefDirRoot(Param: String): String;
begin
if IsRegularUser then
Result := ExpandConstant('{localappdata}')
else
Result := ExpandConstant('{pf}')
end;

