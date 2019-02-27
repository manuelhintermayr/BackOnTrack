; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{1C936F13-8A8D-4D98-A42F-99FB392BF58F}
AppName=Back On Track
AppVersion=1.0
AppPublisher=Manuelweb
DefaultDirName={pf}\BackOnTrack 
DisableDirPage=yes
DefaultGroupName=Back On Track 
AllowNoIcons=yes
OutputBaseFilename=backOnTrack_v1.0_setup
SetupIconFile=logo.ico
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
CloseApplications=force

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"


[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Types]
Name: "full"; Description: "Full installation"
Name: "compact"; Description: "Compact installation"
Name: "custom"; Description: "Custom installation"; Flags: iscustom

[Components]
Name: "program"; Description: "Program Files"; Types: full compact custom; Flags: fixed
Name: "help"; Description: "Desktop.ini"; Types: full   


[Files]
Source: "BackOnTrack.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "logo.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "BackOnTrack.SharedResources.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "BackOnTrack.SystemLevelModification.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "BackOnTrack.WebProxy.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "BouncyCastle.Crypto.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FirstFloor.ModernUI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FluentAssertions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Microsoft.VisualStudio.CodeCoverage.Shim.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices.Interface.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Microsoft.VisualStudio.TestPlatform.TestFramework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Microsoft.VisualStudio.TestPlatform.TestFramework.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "rootCert.pfx"; DestDir: "{app}"; Flags: ignoreversion
Source: "StreamExtended.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "System.ValueTuple.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "System.Windows.Controls.DataVisualization.Toolkit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Titanium.Web.Proxy.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WPFToolkit.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Back On Track"; Filename: "{app}\BackOnTrack.exe"
Name: "{group}\{cm:UninstallProgram,Back On Track}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\Back On Track"; Filename: "{app}\BackOnTrack.exe"; Tasks: desktopicon; IconFilename: "{app}\logo.ico"

[Run]
Filename: "{app}\BackOnTrack.exe"; Description: "{cm:LaunchProgram,Back On Track}"; Flags: nowait postinstall skipifsilent

[Code]
function IsAppRunning(const FileName : string): Boolean;
//got code from https://stackoverflow.com/questions/9941293/how-to-check-with-inno-setup-if-a-process-is-running-at-a-windows-2008-r2-64bit
var
    FSWbemLocator: Variant;
    FWMIService   : Variant;
    FWbemObjectSet: Variant;
begin
    Result := false;
    FSWbemLocator := CreateOleObject('WBEMScripting.SWBEMLocator');
    FWMIService := FSWbemLocator.ConnectServer('', 'root\CIMV2', '', '');
    FWbemObjectSet :=
      FWMIService.ExecQuery(
        Format('SELECT Name FROM Win32_Process Where Name="%s"', [FileName]));
    Result := (FWbemObjectSet.Count > 0);
    FWbemObjectSet := Unassigned;
    FWMIService := Unassigned;
    FSWbemLocator := Unassigned;
end;

procedure TaskKill(FileName: String);
//got code from https://stackoverflow.com/questions/33776405/kill-process-before-reinstall-using-taskkill-f-im-in-inno-setup
var
  ResultCode: Integer;
begin
    Exec(ExpandConstant('taskkill.exe'), '/f /im ' + '"' + FileName + '"', '', SW_HIDE,
     ewWaitUntilTerminated, ResultCode);
end;

function InitializeUninstall(): Boolean;
begin
  
  if(IsAppRunning('BackOnTrack.exe')) then
    begin
      Result := MsgBox('Back On Track is still running. Should the uninstaller close the process?' #13#13 'Please make sure that the WebProxy is not enabled before you press ok.', mbConfirmation, MB_YESNO) = idYes;
      if Result = True then
        begin
          TaskKill('BackOnTrack.exe')
        end;
      if Result = False then
        MsgBox('Uninstall aborted.', mbInformation, MB_OK);
    end
  else
    begin
      Result := true
    end;

end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  foundProgramData: Boolean;
  userDirPath: String;

begin
  case CurUninstallStep of
    usPostUninstall:
      begin
        userDirPath := ExpandConstant('{userdocs}\..\.backOnTrack'); 
        if DirExists(userDirPath) then
        begin
          foundProgramData := MsgBox('Found Back On Track Settings, do you wish them to delete?', mbConfirmation, MB_YESNO) = idYes;
            if foundProgramData = True then
              begin 
                DelTree(userDirPath, True, True, True);
                MsgBox('Back on Track Settings deleted!', mbInformation, MB_OK);
              end;
        end;
      end;
  end;
end;