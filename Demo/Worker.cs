using Microsoft.Extensions.Options;
using System.IO;
using System.Security.Principal;
using Veriket.WinService.Extensions;
using Veriket.WinService.Services;

namespace Veriket.WinService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private FileSystemWatcher? _folderWatcher;
        private readonly string _inputFolder;
        private readonly string _logFolder;
        private readonly IServiceProvider _services;

        FileInfo logFileInfo;
        string logFilePath = string.Empty;
        public Worker(ILogger<Worker> logger, IOptions<AppSettings> settings, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
            //_inputFolder = settings.Value.InputFolder;
            _logFolder = settings.Value.LogFolder;

            SeedPath();
        }

        private void SeedPath()
        {
            logFilePath = _logFolder;

            if (!Directory.Exists(logFilePath))
                Directory.CreateDirectory(logFilePath);

            logFilePath = _logFolder + "Log" + ".txt";

            logFileInfo = new FileInfo(logFilePath);

            if (!logFileInfo.Exists)
                logFileInfo.Create().Close();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now.AddMinutes(1));


                string time = string.Concat(DateTime.Now.ToShortDateString(), " - ", DateTime.Now.ToShortTimeString());
                string userName = WindowsIdentity.GetCurrent().Name;
                string computerName = Environment.MachineName;


                var strLog = string.Format("[{0} : {1} : {2}]", time, computerName, userName);

                WriteLog(strLog);
                await Task.Delay(1000 * 60, stoppingToken);
            }

            await Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Starting");


            _logger.LogInformation("Binding Events from Input Folder: {logFolder}", _logFolder);
            _folderWatcher = new FileSystemWatcher(_logFolder, "*.TXT")
            {
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName |
                                  NotifyFilters.DirectoryName
            };
            _folderWatcher.Created += Input_OnChanged;
            _folderWatcher.EnableRaisingEvents = true;

            return base.StartAsync(cancellationToken);
        }

        protected void Input_OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                _logger.LogInformation("InBound Change Event Triggered by [{0}]", e.FullPath);

                // do some work
                using (var scope = _services.CreateScope())
                {
                    var serviceA = scope.ServiceProvider.GetRequiredService<IServiceA>();
                    serviceA.Run();
                }

                _logger.LogInformation("Done with Inbound Change Event");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Service");
            if (_folderWatcher != null)
            {
                _folderWatcher.EnableRaisingEvents = false;
            }
            await base.StopAsync(cancellationToken);
        }

        public void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;


            char parentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows)[0];

            var IsSameLocation = _logFolder[0].CompareTo(parentFolder);
            if (IsSameLocation < 0)
                _logFolder.ReplaceAtIndex(0, parentFolder);


            logFilePath = _logFolder + "Log" + ".txt";

            logFileInfo = new FileInfo(logFilePath);

            try
            {
                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }

                fileStream = new FileStream(logFilePath, FileMode.Append);

            }
            catch (UnauthorizedAccessException)
            {
                FileAttributes attributes = File.GetAttributes(logFilePath);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    attributes &= ~FileAttributes.ReadOnly;
                    File.SetAttributes(logFilePath, attributes);
                    File.Create(logFilePath);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                log = new StreamWriter(fileStream);
                log.WriteLine(strLog);
                log.Close();
            }

        }
        public override void Dispose()
        {
            _logger.LogInformation("Disposing Service");
            _folderWatcher?.Dispose();
            base.Dispose();
        }
    }
}
